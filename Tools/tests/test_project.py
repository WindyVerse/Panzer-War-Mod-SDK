import importlib.util
import json
from pathlib import Path
import subprocess
import tempfile
import unittest
import zipfile

spec = importlib.util.spec_from_file_location('project', Path(__file__).parents[1]/'project.py')
project = importlib.util.module_from_spec(spec)
spec.loader.exec_module(project)


class DistributionTests(unittest.TestCase):
    def setUp(self):
        self.temporary = tempfile.TemporaryDirectory()
        self.root = Path(self.temporary.name)
        self.previous_root = project.ROOT
        project.ROOT = self.root
        self.git('init', '-q')
        self.git('config', 'user.email', 'test@example.invalid')
        self.git('config', 'user.name', 'Test')
        self.git('lfs', 'install', '--local')
        self.write('.gitattributes', '*.dll filter=lfs diff=lfs merge=lfs -text\n')
        self.write('UnityProject/Packages/manifest.json', json.dumps({'dependencies':{
            'sdk':'file:../../Packages/sdk'}}))
        self.write('Packages/sdk/package.json', '{"name":"sdk"}')
        self.write('Packages/sdk/example.dll', b'MZ-test-library\0' * 20)
        self.write('UnityProject/ProjectSettings/ProjectVersion.txt', 'm_EditorVersion: 2022.3.62f1\n')
        self.write('UnityProject/EditorStorage.template.json', json.dumps({
            'accountInfo':[], 'luaProjectFolder':'', 'exportDllPaths':[]}))
        self.write('sdk-source.json', json.dumps({'modsCommit':'a'*40, 'unity':'2022.3.62f1'}))
        self.git('add', '.')
        self.git('commit', '-qm', 'fixture')

    def tearDown(self):
        project.ROOT = self.previous_root
        # Windows marks Git object files read-only; the fixture is scoped to our temporary directory.
        import os
        import stat
        import shutil
        def writable(function, path, _error):
            os.chmod(path, stat.S_IWRITE)
            function(path)
        shutil.rmtree(self.root, onerror=writable)
        self.temporary.cleanup()

    def git(self, *args):
        return subprocess.check_output(['git', '-C', str(self.root), *args], stderr=subprocess.STDOUT)

    def write(self, relative, value):
        path = self.root/relative
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(value if isinstance(value, bytes) else value.encode())

    def test_archive_expands_lfs_and_uses_committed_metadata(self):
        self.write('sdk-source.json', json.dumps({'modsCommit':'b'*40, 'unity':'2022.3.62f1'}))
        project.package('sdk-2026.11.04.1', self.root/'out')
        with zipfile.ZipFile(next((self.root/'out').glob('*.zip'))) as archive:
            binary = next(n for n in archive.namelist() if n.endswith('.dll'))
            self.assertTrue(archive.read(binary).startswith(b'MZ-test-library'))
            release = json.loads(archive.read(next(n for n in archive.namelist() if n.endswith('/release.json'))))
            self.assertEqual(release['sdk']['modsCommit'], 'a'*40)

    def test_pointer_checkout_cannot_be_released(self):
        self.write('Packages/sdk/example.dll', self.git('show', 'HEAD:Packages/sdk/example.dll'))
        project.check(pointers=True)
        with self.assertRaisesRegex(ValueError, 'not downloaded'):
            project.package('sdk-2026.11.04.1', self.root/'out')

    def test_changed_lfs_bytes_cannot_be_released(self):
        self.write('Packages/sdk/example.dll', b'MZ-corrupt')
        with self.assertRaisesRegex(ValueError, 'modified LFS'):
            project.package('sdk-2026.11.04.1', self.root/'out')

    def test_dependency_outside_distribution_is_rejected(self):
        self.write('UnityProject/Packages/manifest.json', json.dumps({'dependencies':{'sdk':'file:../../../private'}}))
        with self.assertRaisesRegex(ValueError, 'outside the distribution'):
            project.check()

    def test_personal_settings_are_rejected(self):
        self.write('UnityProject/Assets/EditorStorage.json', '{}')
        self.git('add', '.')
        with self.assertRaisesRegex(ValueError, 'Personal/generated'):
            project.check()


if __name__ == '__main__':
    unittest.main()
