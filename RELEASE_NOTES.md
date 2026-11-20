装甲纷争用户模组工程，使用 Unity **2022.3.62f1**。

- 下载 `Panzer-War-Mod-SDK-sdk-*.zip`，完整解压后打开其中的 `UnityProject`。
- ZIP 包含实际 DLL 和全部本地依赖；Git 克隆用户需要安装 Git LFS 并运行 `git lfs pull`。
- 保留载具、地图、瞄具、涂装、贴花等用户示例，SDK 与官方模组工程同步。
- `SHA256SUMS.txt` 提供下载校验；`release.json` 记录发行提交、SDK 来源与 Unity 版本。
- 升级：删除旧 SDK 文件夹里的 `Packages`，再复制新版 ZIP 里的 `Packages`；`UnityProject` 里是你的模组和设置，保留不动（新版若改动了 `UnityProject` 的文件，对照 ZIP 里的同名文件合并）。不要把 ZIP 直接解压覆盖旧文件夹：新版删除的旧文件会残留，可能导致工程编译失败。
- Upgrade: delete your SDK's `Packages` folder and copy in the one from the new ZIP; keep your `UnityProject` (merge any of its files the release changed). Do not extract the ZIP over the old folder: files the new SDK removed stay behind and can break compilation.
