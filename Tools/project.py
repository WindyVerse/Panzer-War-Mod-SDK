"""Validate, synchronize and package the public mod project. No Unity installation required."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import tarfile
import tempfile
import zipfile

ROOT = Path(__file__).resolve().parents[1]
POINTER = b'version https://git-lfs.github.com/spec/v1\n'
TAG = re.compile(r'sdk-[0-9]{4}\.[0-9]{2}\.[0-9]{2}\.[0-9]+\Z')
MIRRORS = ['Packages/Sdk'] + [f'Packages/Tools/{name}' for name in (
    'com.shanghaiwindy.publicresources', 'com.shanghaiwindy.renderpipline.publicshared',
    'com.shanghaiwindy.sge', 'com.windyverse.modbuild')]
FORBIDDEN_PARTS = {'Library', 'Temp', 'Logs', 'UserSettings', '.git', '.idea', 'mod.io', 'saves'}


def git(*args, root=None):
    root = ROOT if root is None else root
    return subprocess.check_output(['git', '-C', str(root), *args])


def tracked():
    return [p for p in git('ls-files', '-z').decode().split('\0') if p]


def attributes(paths):
    output = subprocess.check_output(['git', '-C', str(ROOT), 'check-attr', '-z', '--stdin', 'filter'],
                                     input='\0'.join(paths).encode()).decode().split('\0')
    return dict(zip(output[::3], output[2::3]))


def checked_path(root, relative):
    path = root / relative
    if path.is_symlink() or not path.resolve().is_relative_to(root.resolve()) or path.resolve() == root.resolve():
        raise ValueError(f'Unsafe path: {relative}')
    return path


def contents(root, ref, paths=()):
    """Read committed blobs, expanding LFS from the checkout only after checking the hash."""
    environment = {**os.environ, 'GIT_LFS_SKIP_SMUDGE': '1'}
    process = subprocess.Popen(['git', '-C', str(root), 'archive', ref, *paths], stdout=subprocess.PIPE,
                               env=environment)
    try:
        with tarfile.open(fileobj=process.stdout, mode='r|') as archive:
            for member in archive:
                if member.isdir():
                    continue
                if not member.isfile():
                    raise ValueError(f'Unsupported archive entry: {member.name}')
                data = archive.extractfile(member).read()
                if data.startswith(POINTER):
                    oid = re.search(rb'oid sha256:([0-9a-f]{64})\n', data)
                    size = re.search(rb'size ([0-9]+)\n', data)
                    if not oid or not size:
                        raise ValueError(f'Invalid LFS pointer: {member.name}')
                    data = checked_path(root, member.name).read_bytes()
                    if len(data) != int(size[1]) or hashlib.sha256(data).hexdigest() != oid[1].decode():
                        raise ValueError(f'Missing or modified LFS content: {member.name}; run git lfs pull')
                yield member.name, data
        if process.wait():
            raise RuntimeError('git archive failed')
    finally:
        process.stdout.close()
        if process.poll() is None:
            process.terminate()
            process.wait()


def check(pointers=False):
    paths = tracked()
    if not paths:
        raise ValueError('No tracked project files')
    filters = attributes(paths)
    errors = []
    for relative in paths:
        path = checked_path(ROOT, relative)
        if FORBIDDEN_PARTS.intersection(path.relative_to(ROOT).parts) or path.name == 'EditorStorage.json':
            errors.append(f'Personal/generated file tracked: {relative}')
        if path.suffix.lower() in {'.dll', '.exe', '.so', '.dylib'} and filters.get(relative) != 'lfs':
            errors.append(f'Binary is not managed by LFS: {relative}')
        if not path.is_file():
            errors.append(f'Missing file: {relative}')
            continue
        with path.open('rb') as stream:
            prefix = stream.read(200)
        if prefix.startswith(POINTER):
            if not pointers:
                errors.append(f'LFS content not downloaded: {relative}')
        elif path.stat().st_size >= 100 * 1024**2 and filters.get(relative) != 'lfs':
            errors.append(f'Large file is not managed by LFS: {relative}')

    manifest = json.loads((ROOT/'UnityProject/Packages/manifest.json').read_text())
    for name, version in manifest['dependencies'].items():
        if version.startswith('file:'):
            folder = (ROOT/'UnityProject/Packages'/version[5:]).resolve()
            if not folder.is_relative_to(ROOT.resolve()) or not (folder/'package.json').is_file():
                errors.append(f'Package outside the distribution or missing: {name}')
            elif json.loads((folder/'package.json').read_text(encoding='utf-8-sig'))['name'] != name:
                errors.append(f'Package name mismatch: {name}')
    source = json.loads((ROOT/'sdk-source.json').read_text())
    if not re.fullmatch('[0-9a-f]{40}', source['modsCommit']):
        errors.append('SDK source must be pinned to a commit')
    editor = (ROOT/'UnityProject/ProjectSettings/ProjectVersion.txt').read_text().splitlines()[0]
    if editor != 'm_EditorVersion: ' + source['unity']:
        errors.append('Unity version does not match sdk-source.json')
    storage = json.loads((ROOT/'UnityProject/EditorStorage.template.json').read_text())
    if storage['accountInfo'] or storage['luaProjectFolder'] or storage['exportDllPaths']:
        errors.append('Editor storage template contains personal configuration')
    if errors:
        raise ValueError('\n'.join(errors[:50]))
    print(f'Checked {len(paths)} tracked files; DLLs use Git LFS; local packages are self-contained.')


def sync(source):
    source = Path(source).resolve()
    sha = git('rev-parse', 'HEAD', root=source).decode().strip()
    # Materialize the complete snapshot before touching the destination packages.
    with tempfile.TemporaryDirectory(prefix='mod-sdk-') as temporary:
        staging = Path(temporary)
        for relative, data in contents(source, sha, MIRRORS):
            path = checked_path(staging, relative)
            path.parent.mkdir(parents=True, exist_ok=True)
            path.write_bytes(data)
        for relative in MIRRORS:
            incoming = checked_path(staging, relative)
            if not incoming.is_dir():
                raise ValueError(f'Missing SDK source: {relative}')
        for relative in MIRRORS:
            destination = checked_path(ROOT, relative)
            if destination.exists():
                shutil.rmtree(destination)
            shutil.copytree(staging/relative, destination)
    metadata_path = ROOT/'sdk-source.json'
    metadata = json.loads(metadata_path.read_text())
    metadata['modsCommit'] = sha
    metadata_path.write_text(json.dumps(metadata, indent=2)+'\n', encoding='utf-8', newline='\n')
    print(f'Synchronized SDK from {sha}')


def package(tag, output):
    if not TAG.fullmatch(tag):
        raise ValueError('Version must look like sdk-2026.11.04.1')
    check()
    output = Path(output).resolve()
    output.mkdir(parents=True, exist_ok=True)
    name = f'Panzer-War-Mod-SDK-{tag}'
    archive_path = output/(name + '.zip')
    if archive_path.exists():
        raise ValueError(f'Archive already exists: {archive_path}')
    sha = git('rev-parse', 'HEAD').decode().strip()
    metadata = None
    count = 0
    with zipfile.ZipFile(archive_path, 'w', zipfile.ZIP_DEFLATED, compresslevel=6) as archive:
        for relative, data in contents(ROOT, sha):
            if relative == 'sdk-source.json':
                metadata = json.loads(data)
            if relative.startswith('.github/') or relative in {'.gitignore', '.gitattributes'}:
                continue
            info = zipfile.ZipInfo(f'{name}/{relative}', (2026, 1, 1, 0, 0, 0))
            info.compress_type = zipfile.ZIP_DEFLATED
            info.external_attr = 0o100644 << 16
            archive.writestr(info, data)
            count += 1
        release = {'tag':tag, 'commit':sha, 'sdk':metadata, 'files':count}
        archive.writestr(f'{name}/release.json', json.dumps(release, indent=2)+'\n')
    if archive_path.stat().st_size >= 2 * 1024**3:
        raise ValueError('Archive exceeds the GitHub Release asset size limit')
    digest = hashlib.sha256()
    with archive_path.open('rb') as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b''):
            digest.update(chunk)
    (output/'SHA256SUMS.txt').write_text(f'{digest.hexdigest()}  {archive_path.name}\n', encoding='utf-8')
    (output/'release.json').write_text(json.dumps(release, indent=2)+'\n', encoding='utf-8')
    print(f'Packaged {count} files: {archive_path.name} ({archive_path.stat().st_size // 1024**2} MiB)')


def verify_modpacks(folder):
    packs = sorted(Path(folder).glob('*.modpack'))
    if len(packs) != 2:
        raise ValueError(f'Expected vehicle and map smoke builds, found {len(packs)}')
    for path in packs:
        with zipfile.ZipFile(path) as archive:
            bad = archive.testzip()
            if bad:
                raise ValueError(f'Corrupt modpack: {path.name}/{bad}')
            names = archive.namelist()
            manifests = [n for n in names if n == 'package.json' or n.endswith('/package.json')]
            if not manifests:
                raise ValueError(f'Missing package.json: {path.name}')
            if 'Template-Blank-Scene' in path.name:
                for suffix in ('.mapdata', '.scene', '.sprite'):
                    if not any(name.endswith(suffix) for name in names):
                        raise ValueError(f'Map template is missing {suffix}: {path.name}')
            for name in manifests:
                json.loads(archive.read(name))
        print(f'Verified {path.name}: {path.stat().st_size} bytes')


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    commands = parser.add_subparsers(dest='command', required=True)
    validation = commands.add_parser('check')
    validation.add_argument('--pointers', action='store_true', help='Static PR validation without fetching LFS')
    synchronize = commands.add_parser('sync')
    synchronize.add_argument('source', help='Clean, hydrated official Mods checkout')
    packing = commands.add_parser('package')
    packing.add_argument('tag')
    packing.add_argument('--output', default=str(ROOT/'Artifacts/release'))
    verification = commands.add_parser('verify-modpacks')
    verification.add_argument('folder')
    args = parser.parse_args()
    if args.command == 'check':
        check(args.pointers)
    elif args.command == 'sync':
        sync(args.source)
    elif args.command == 'package':
        package(args.tag, args.output)
    elif args.command == 'verify-modpacks':
        verify_modpacks(args.folder)


if __name__ == '__main__':
    main()
