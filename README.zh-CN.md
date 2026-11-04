# 装甲纷争模组工程

[English](README.md) | **简体中文** | [Русский](README.ru.md)

为 **Panzer War（装甲纷争）** 制作自己的载具、地图、瞄具、涂装、贴花等内容。本 Unity 工程包含模组制作工具、运行库和示例资源，帮助你从第一个模组开始。

[下载模组工程](https://github.com/WindyVerse/Panzer-War-Mod-SDK/releases/latest) · [模组开发文档](https://doc.game.windyverse.net/)

## 1. 安装 Unity

通过 Unity Hub 安装 **Unity 2022.3.62f1**，并使用这一版本打开工程。

如果要制作 Android 模组，请同时安装 **Android Build Support**，包含 Android SDK & NDK Tools 和 OpenJDK 模块。你还需要安装游戏，用来测试制作完成的模组。

## 2. 下载并打开工程

1. 打开[最新发行版](https://github.com/WindyVerse/Panzer-War-Mod-SDK/releases/latest)。
2. 在 Assets 中下载 **`Panzer-War-Mod-SDK-sdk-*.zip`**，完整解压。
3. 在 Unity Hub 中添加解压后的 **`UnityProject`** 文件夹，使用 Unity 2022.3.62f1 打开。
4. 等待 Unity 下载依赖包、导入资源并完成脚本编译。首次导入会比之后启动耗时更长。

请保持 `UnityProject` 与 `Packages` 原有的相邻目录结构。工程通过相对路径引用 `Packages`，只复制 `UnityProject` 会导致依赖丢失。

发行 ZIP 已包含真实 DLL 和本地依赖，**无需安装 Git 或 Git LFS**。请选择上面命名的工程 ZIP；GitHub 自动生成的 **Source code (zip)** 可能只包含 LFS 指针，缺少实际二进制文件。

## 3. 打包你的第一个模组

建议先打包自带的载具模板，确认流程正常后再修改资源。下文的 `Assets/` 路径都位于 `UnityProject` 内。

1. 打开 **File → Build Settings**。制作 Windows 模组时，选择 **PC, Mac & Linux Standalone**，目标设为 Windows、架构设为 x86_64，必要时切换平台。制作 Android 模组时，选择 Android 并切换平台。
2. 在 Project 窗口中选中 `Assets/ModManager/BuildPipline-Template-Vehicle.asset`。
3. 在 Inspector 中点击 **编译相关资源（Build Linked Objects to AssetBundle）**，等待资源构建完成。
4. 选中该流程关联的模组包配置 `Assets/ModManager/Template-Blank-Vehicle.asset`。
5. 点击 **打包模组包（Package Mod Now）**。打包结束后，编辑器会打开输出目录。
6. 按照[模组安装教程](https://doc.game.windyverse.net/manual/gameplay/64310317e9947d97.html)将生成的 `.modpack` 安装进游戏，在车库和战斗中测试。

Windows 载具示例的输出目录为：

```text
UnityProject/Build/Mod-Package/StandaloneWindows64/Template-Blank-Vehicle/
```

**不同平台需要分别打包。** 修改资源或切换平台后，应先重新编译相关资源，再生成模组包。Windows 模组包不能直接用于 Android。

要尝试地图示例，请使用同一 `Assets/ModManager` 目录下的 `BuildPipline-Template-Blank.asset`，以及它关联的 `Template-Blank-Scene.asset`。

## 4. 制作自己的内容

完整制作与发布流程请查阅[模组开发文档](https://doc.game.windyverse.net/)，并参考对应的示例资源：

| 内容 | 示例目录 |
| --- | --- |
| 载具 | `Assets/Template-Vehicle` |
| 地图 | `Assets/Template-Scene` |
| 瞄具 | `Assets/Template-UserSight` |
| 涂装 | `Assets/Template-Camo` |
| 贴花 | `Assets/Template-Decal` |
| 语音包 | `Assets/Template-VoicePack` |

将自己的内容放在 `Assets` 下的独立文件夹中。参考示例创建自己的模组包和打包流程，为新模组设置独立标识，并在分享前填写名称、作者、说明和资源引用。后续更新同一模组时，请保持这些标识不变。

游戏路径和可选的打包后自动复制目录，在编辑器的配置工具中设置。个人配置保存在 `UnityProject/Assets/EditorStorage.json`，无需随作品分享给其他作者。

工程中的 `Tutorials` 文件夹保留了旧版参考资料，其中部分截图、下载链接和 Unity 版本已经过时。安装当前工程请以本 README 为准，制作教程请优先查阅新的文档站。

## 通过 Git 获取工程

安装 Git 和 Git LFS 后运行：

```bash
git lfs install
git clone https://github.com/WindyVerse/Panzer-War-Mod-SDK.git
cd Panzer-War-Mod-SDK
git lfs pull
```

完成后，在 Unity Hub 中打开 `UnityProject`。DLL 和其他二进制资源通过 **Git LFS** 保存，必须等 LFS 文件下载完成后再打开 Unity。

## 常见问题

| 问题 | 检查方法 |
| --- | --- |
| DLL 无效、插件缺失，或 DLL 实际上是很小的文本文件 | Git 克隆用户运行 `git lfs pull`；ZIP 用户重新下载正式发行的工程 ZIP，避免使用 Source code (zip)。 |
| 找不到本地依赖包 | 保留完整解压目录，确认 `UnityProject` 与 `Packages` 的相对位置没有改变。 |
| 打包时提示缺少构建缓存 | 先为当前平台执行打包流程中的“编译相关资源”，再打包模组包。 |
| 游戏无法加载模组 | 确认模组包平台与游戏运行平台一致，并按上面的安装教程操作。 |
| 首次打开出现错误 | 确认使用 Unity 2022.3.62f1，等待导入结束，再检查 Console 中首条相关错误。 |

若仍无法解决，请在[反馈问题](https://github.com/WindyVerse/Panzer-War-Mod-SDK/issues)时附上 SDK 发行版本、Unity 版本、目标平台和相关 Console 错误。

## 升级 SDK

先备份自己的作品，将新版解压到**新目录**，再迁移自己的资源、对应的 `.meta` 文件、模组包配置和打包流程。保留已有资源引用与模组标识，按需重新设置个人配置，然后重新构建并测试。不要用旧版 SDK 的 `Packages` 覆盖新版目录。

## 许可

请参阅 [LICENSE](LICENSE)。第三方插件和资源保留各自的许可证与声明。
