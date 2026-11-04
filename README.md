# Panzer War Mod SDK

**English** | [简体中文](README.zh-CN.md) | [Русский](README.ru.md)

Create your own vehicles, maps, sights, camouflage, decals and more for **Panzer War**. This Unity project includes the modding tools, runtime libraries and example assets you need to get started.

[Download the project](https://github.com/WindyVerse/Panzer-War-Mod-SDK/releases/latest) · [Modding documentation](https://doc.game.windyverse.net/)

## 1. Install Unity

Use **Unity 2022.3.62f1**, installed through Unity Hub. Use this exact version when opening the project.

To build Android mods, also install **Android Build Support**, including the Android SDK & NDK Tools and OpenJDK modules. You will need the game to test your finished mod.

## 2. Download and open the project

1. Open the [latest release](https://github.com/WindyVerse/Panzer-War-Mod-SDK/releases/latest).
2. Download **`Panzer-War-Mod-SDK-sdk-*.zip`** from Assets and extract the entire archive.
3. In Unity Hub, add the extracted **`UnityProject`** folder and open it with Unity 2022.3.62f1.
4. Let Unity finish downloading packages, importing assets and compiling scripts. The first import takes longer than later launches.

Keep `UnityProject` and `Packages` together in their original layout. The project references the neighboring `Packages` folder; copying only `UnityProject` will leave dependencies missing.

The release ZIP contains the actual DLLs and local dependencies, so **Git and Git LFS are not required** for this download. Choose the named project ZIP rather than GitHub's automatically generated **Source code (zip)**, which may contain LFS pointers instead of binary files.

## 3. Build your first mod

Start by building the included vehicle template before changing its assets. Paths beginning with `Assets/` below are relative to `UnityProject`.

1. Open **File → Build Settings**. For a Windows mod, select **PC, Mac & Linux Standalone**, choose Windows and the x86_64 architecture, then switch platform if necessary. For Android, select Android and switch platform.
2. In the Project window, select `Assets/ModManager/BuildPipline-Template-Vehicle.asset`.
3. In its Inspector, click **Build Linked Objects to AssetBundle** and wait for the resource build to finish.
4. Select `Assets/ModManager/Template-Blank-Vehicle.asset`, the mod package linked to that pipeline.
5. Click **Package Mod Now**. The editor opens the output folder when packaging finishes.
6. Install the generated `.modpack` in the game using the [mod installation guide](https://doc.game.windyverse.net/manual/gameplay/64310317e9947d97.html), then test it in the garage and in battle.

For the Windows vehicle example, the output is under:

```text
UnityProject/Build/Mod-Package/StandaloneWindows64/Template-Blank-Vehicle/
```

**Build separately for each target platform.** After changing assets or switching platforms, rebuild the linked objects before packaging again. A Windows mod package cannot be used as an Android package.

To try the map example, use `BuildPipline-Template-Blank.asset` and its linked package `Template-Blank-Scene.asset` in the same `Assets/ModManager` folder.

## 4. Create your own content

Use the [modding documentation](https://doc.game.windyverse.net/) for the full creation and publishing workflows. Start with the matching example:

| Content | Example assets |
| --- | --- |
| Vehicles | `Assets/Template-Vehicle` |
| Maps | `Assets/Template-Scene` |
| Sights | `Assets/Template-UserSight` |
| Camouflage | `Assets/Template-Camo` |
| Decals | `Assets/Template-Decal` |
| Voice packs | `Assets/Template-VoicePack` |

Keep your own content in a separate folder under `Assets`. Create your own mod package and build pipeline using the examples as a reference, assign unique mod identifiers, and update the name, author, description and linked assets before sharing your mod. Keep those identifiers stable when releasing updates.

Set personal game paths and optional output-copy destinations in the editor's configuration tools. Personal settings are saved in `UnityProject/Assets/EditorStorage.json`; they do not need to be shared with other creators.

The bundled `Tutorials` folder contains older reference material. Some screenshots, download links and Unity versions there are outdated; use this README for installing this project and the documentation website for current guides.

## Get the project with Git instead

Install Git and Git LFS, then run:

```bash
git lfs install
git clone https://github.com/WindyVerse/Panzer-War-Mod-SDK.git
cd Panzer-War-Mod-SDK
git lfs pull
```

Then open `UnityProject` in Unity Hub. DLLs and other binary assets use **Git LFS**; the LFS download must finish before opening Unity.

## Troubleshooting

| Problem | What to check |
| --- | --- |
| Invalid DLL, missing plugin or a tiny text file where a DLL should be | For a Git checkout, run `git lfs pull`. For a ZIP download, use the named release ZIP, not Source code (zip). |
| Local packages cannot be found | Keep the full extracted directory and its original `UnityProject` / `Packages` layout. |
| Missing build cache when packaging | Build the pipeline's linked objects first, for the currently selected platform. |
| The game cannot load the mod | Check that the package targets your game's platform, then follow the installation guide above. |
| Errors on first opening the project | Confirm Unity 2022.3.62f1, let imports finish, then check the first relevant error in the Console. |

For an unresolved issue, include your SDK release version, Unity version, target platform and relevant Console error when [reporting it](https://github.com/WindyVerse/Panzer-War-Mod-SDK/issues).

## Updating the SDK

Back up your work, extract the new release into a **new folder**, and migrate your own assets together with their `.meta` files, mod packages and build pipelines. Preserve existing asset references and mod identifiers. Reapply your personal settings as needed, then rebuild and test your mods. Avoid copying the old SDK's `Packages` folder over the new one.

## License

See [LICENSE](LICENSE). Third-party plugins and assets retain their own licenses and notices.

