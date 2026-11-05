# 模组工程维护说明

本文件面向维护本仓库的开发者与自动化代理。用户入口为英文 `README.md`，顶部链接到 `README.zh-CN.md` 和 `README.ru.md`；三份使用指南保持内容一致。当前模组文档站为 <https://doc.game.windyverse.net/>，不要再把旧语雀地址作为主入口。

## 工程边界

- 本仓库是面向模组作者的公开 Unity 工程，Unity 版本为 **2022.3.62f1**。Unity 的 projectPath 是 `UnityProject`，不是仓库根目录。
- `UnityProject/Assets/Template-*` 和 `UnityProject/Assets/ModManager` 保存示例资源、模组包配置与打包流程。
- `Packages/Sdk` 和 `Tools/project.py` 中 `MIRRORS` 列出的共享工具包来自官方 `WindyVerse/Panzer-War-Mods` 仓库。以该列表为同步范围，不从个人工作区直接复制未提交的改动。
- `sdk-source.json` 固定官方 SDK 的源提交与 Unity 版本，用于追溯发行内容。模板与用户指南在本仓库维护。
- 修改前检查 `git status`，保留其他任务的未提交改动。不要将 Unity 缓存、个人配置、账号、密钥或本地游戏路径提交到仓库。

## Git LFS 与发行内容

- 所有 DLL、原生插件和大型二进制资源遵循 `.gitattributes`，使用 Git LFS。不要把 LFS 指针当作可用 DLL，也不要绕过 LFS 提交二进制文件。
- 克隆或同步后运行 `git lfs pull`，再打开 Unity。校验与打包使用 `Tools/project.py`；它会核对发行内容与 Git 提交中的 LFS 哈希。
- 保留 `.meta` 文件与已有 GUID。用户资源迁移和模板更新都必须保持正确引用；不要把 SDK 导入产生的临时变更混入发行候选。
- Release 的命名工程 ZIP 必须包含真实 LFS 文件及完整的相邻 `UnityProject` / `Packages` 目录结构。GitHub 自动生成的源码 ZIP 不作为完整发行包。
- `Tutorials` 是旧版参考资料，可能包含旧版本号和旧入口。README 中明确提示其适用范围，新使用说明以当前工程和文档站为准。

## 正式发行流程

1. 客户端仓库 `WindyVerse/Panzer-War-Client` 的 **Export mod SDK** 导出 SDK。
2. 导出内容在官方模组仓库通过 **SDK smoke** 并合入后，选择经过验证的提交作为发行来源。
3. 在客户端仓库的 `main` 分支运行 **Publish mod SDK project**（`.github/workflows/sdk-project.yml`），填写未使用的版本标签 `sdk-YYYY.MM.DD.N` 和 `mods_ref`。优先填写完整提交 SHA；填写 `main` 时工作流也会记录实际检出的提交。
4. 工作流同步官方 SDK、记录候选提交，并用 Unity 分别构建用户工程的载具和地图模板。两个 `.modpack` 通过校验后，才原子推送候选提交与发行标签。
5. 标签触发公开仓库的 **Release SDK project**（`.github/workflows/release.yml`）：获取完整 LFS 文件、执行完整性测试、生成工程 ZIP、`SHA256SUMS.txt` 和 `release.json`，再创建 GitHub Release。
6. 确认两个工作流成功、Release 非草稿、下载地址可用，且校验文件与附件摘要一致。

不要绕过验证直接创建正式发行标签，也不要覆盖已有标签或替换已发布的附件。修复通过新版本发行；仅修改 README 时正常提交并运行静态检查即可，无需重跑 Unity 或重发旧版 ZIP。新文档会进入之后的发行包。

私有工作流通过 `PUBLIC_MOD_SDK_DEPLOY_KEY` 写入本公开仓库，通过 `MODS_SDK_DEPLOY_KEY` 访问官方 SDK。Unity 授权与构建凭据只保存在私有客户端仓库的 Actions secrets 中，不写入公开文件。

## 模组的多平台打包策略

### 平台与版本

同一模组使用同一份源资源、SDK 提交、模组 GUID 和 `modVersion`，分别为目标平台构建 AssetBundle 并生成 `.modpack`。同一版本的平台包属于同一个模组，不要为了区分平台更换 GUID，也不能通过修改文件名将一个平台的包变成另一个平台的包。

| 游戏平台 | Unity BuildTarget / CI `platform` | Unity 命令行 `-buildTarget` | 产物前缀 |
| --- | --- | --- | --- |
| Windows 64 位 | `StandaloneWindows64` | `win64` | `StandaloneWindows64_` |
| Android | `Android` | `android` | `Android_` |
| iOS | `iOS` | `ios` | `iOS_` |

面向 Windows、Android 玩家发布时，应分别提供这两个平台的包；需要支持 iOS 时再加入 iOS 构建。三个平台都固定使用 Unity **2022.3.62f1**，并安装对应 Build Support。Android 还需要 SDK、NDK 和 OpenJDK；自动构建环境的镜像与 Android SDK 配置以官方模组仓库的 `Tools/images.json`、`Tools/android_image.sh` 为准。

这里构建的是供已有游戏加载的模组资源包。iOS `.modpack` 不经过游戏客户端 IPA 的 Xcode 导出、证书签名或 TestFlight 流程；官方模组 CI 使用带 iOS 模块的 Unity Linux 镜像生成该平台资源。

### 本地工程、资源与缓存

- 偶尔打包多个平台时，可以在同一个 `UnityProject` 中按平台顺序构建。每次先切换目标平台、等待导入完成，再构建资源和模组包；同一工程同一时间只运行一个 Unity 实例。
- 频繁进行 Windows / Android 开发时，沿用[多平台打包（GameHub）教程](https://doc.game.windyverse.net/manual/mod/3e869c43f14fbf8c.html)：在 GameHub 添加工程、选择目标平台生成软链接工程，再把对应路径加入 Unity Hub，并选择匹配的 Current Platform。目的是同步源资源并保留各平台自己的导入缓存。
- 创建平台工程后，必须检查 `Packages/manifest.json` 中的本地 `file:` 依赖。本仓库使用相邻的 `UnityProject` / `Packages` 布局；不能只复制 `UnityProject` 到其他位置而不处理依赖路径，也不要把不同平台的整个工程根目录都链接到同一个目录。
- 每个平台分别保留工程配置与 `Library`、`Temp`、`Obj`、`Logs`、`Build`，不要跨平台共用这些可写目录。源资源和 `.meta` 保持同步；SDK 依赖保持相同提交。
- **资源软链接不等于允许并行构建。** 构建会更新 cooked 资源、ModPackage 的 `buildCaches` 与清单等内容。共享这些可写资源的工程必须串行打包；需要并行时，为每个平台使用独立检出，并固定到相同提交。CI 也按独立 job 隔离工作目录。
- AI 工具启动构建前，核对实际 Unity 路径、`projectPath`、目标平台、LFS 是否完整和工程是否已被编辑器占用。不要在构建期间同时修改被共享的源资源。

### 每个平台的执行顺序

1. 固定本次使用的资源提交、SDK 版本、模组 GUID 与 `modVersion`，准备相应平台的工作目录和 Unity 模块。
2. 在启动 Unity 时指定目标平台，或在 Build Settings 中切换平台并等待导入、编译完成。
3. 对该平台执行 BuildPipeline 的 **Build Linked Objects to AssetBundle**，再对关联的 ModPackage 执行 **Package Mod Now**。修改资源或切换平台后，都必须重新执行这两个步骤，不能直接复用其他平台的构建缓存。
4. 保存 `<Unity平台>_<ModPackage名>.modpack`。通过批处理入口构建时，还会生成同名的 `.modpack.json` 工坊元数据文件，应与模组包一起保留。
5. 核对包内平台、Unity 版本、资源清单与元数据，在对应平台的游戏中测试，再发布该平台产物。

编辑器的默认构建目录已包含平台名：

```text
UnityProject/Build/Mod-BuildPipline/<Unity平台>/<BuildPipeline名>/packages/
UnityProject/Build/Mod-Package/<Unity平台>/<ModPackage名>/
```

自动化工具统一使用本工程已有的 `WindyVerse.ModBuild.ModpackBuild.Build`。它一次处理一条 pipeline、一个活动平台，并依次构建资源和模组包，校验 SDK 报错，输出 `.modpack` 与 sidecar。成功退出码为 0，失败为 1；不要另造只改文件名或只压缩旧缓存的替代流程。

下面是从仓库根目录执行 Windows 载具模板构建的 PowerShell 示例。按本机安装位置修改 `$unityEditor`，并先关闭占用同一工程的 Unity 编辑器：

```powershell
$sdkRoot = (Get-Location).Path
$unityEditor = 'C:\Program Files\Unity\Hub\Editor\2022.3.62f1\Editor\Unity.exe'
$sdkOutput = Join-Path $sdkRoot 'Artifacts/mods/StandaloneWindows64'
New-Item -ItemType Directory -Path $sdkOutput -Force | Out-Null
$sdkBuildArgs = @(
    '-batchmode', '-nographics', '-buildTarget', 'win64',
    '-projectPath', ('"{0}"' -f (Join-Path $sdkRoot 'UnityProject')),
    '-executeMethod', 'WindyVerse.ModBuild.ModpackBuild.Build',
    '-modPipeline', 'Assets/ModManager/BuildPipline-Template-Vehicle.asset',
    '-modOutput', ('"{0}"' -f $sdkOutput),
    '-logFile', ('"{0}"' -f (Join-Path $sdkOutput 'vehicle.log'))
)
$sdkBuild = Start-Process -FilePath $unityEditor -ArgumentList $sdkBuildArgs `
    -WindowStyle Hidden -Wait -PassThru
if ($sdkBuild.ExitCode -ne 0) { throw "Mod build failed: $($sdkBuild.ExitCode)" }
```

Android 和 iOS 分别把 `-buildTarget` 改为 `android`、`ios`，并把输出目录改为 `Artifacts/mods/Android`、`Artifacts/mods/iOS`。地图模板使用 `Assets/ModManager/BuildPipline-Template-Blank.asset`，日志另存为 `map.log`。脚本路径参数见 [Unity 2022.3 命令行文档](https://docs.unity3d.com/2022.3/Documentation/Manual/EditorCommandLineArguments.html)；批处理入口会自行退出 Unity。`-nographics` 适用于无图形设备的构建，地图需要的光照烘焙应提前完成。

### 官方 CI 与复用规则

- 官方模组仓库 `WindyVerse/Panzer-War-Mods` 的 **Build modpack** 接受 `platform=all`、`StandaloneWindows64`、`Android`、`iOS`。`all` 展开为“每条选中 BuildPipeline × 三个平台”的构建矩阵，各 job 使用独立检出与对应平台镜像，`fail-fast: false`。
- 每个 job 只导入所选包及其声明的依赖闭包。Library 缓存按工程、平台、包和源内容 tree 隔离；不能把 Windows 的 Library 缓存恢复给 Android 或 iOS。
- `modpack-store` 的成品复用 key 包含包、pipeline、平台、源资源与 SDK / 工具 tree、编辑器镜像等输入。只复用相同输入的同平台成品；构建逻辑变化而 key 输入未变化时，调整官方 `Tools/ci.py` 的 `KEY_VERSION`。
- 部分平台失败时，可以保存已经成功的构建供重试复用，但不能宣称所有平台都已通过，也不能把未完成的一组产物作为完整多平台版本发布。官方工作流在构建失败时会阻止本次正式 Release / 工坊发布。
- **当前用户工程的 Publish mod SDK project 只验证 `StandaloneWindows64` 的载具和地图模板。** 官方 **SDK smoke** 同样是 Windows 冒烟验证。工程 ZIP 本身是共用源码与依赖；发行成功不代表用户模板已经通过 Android / iOS 构建。需要这些平台的验证时，应实际运行对应构建并分别记录结果。

### 产物检查与发布

- `.modpack` 必须能完整解压。读取包内 `package.json`，核对 `buildTarget` 等于本次目标、`buildEngineVersion` 为 `2022.3.62f1`，并确认清单引用的文件都存在。
- 核对 sidecar 的 `guid`、`version` 与包内 `guid`、`modVersion` 一致；sidecar 平台值分别是 Windows `1`、Android `2`、iOS `3`。同一组多平台发行的模组 GUID 和版本号应一致。
- 本仓库的 `python Tools/project.py verify-modpacks <目录>` 用于检查载具与地图两个模板包的 ZIP / JSON 完整性及地图的 `.mapdata`、`.scene`、`.sprite`。每个平台使用独立目录，目录内恰好放这两个 `.modpack`；该命令不代替平台、Unity 版本或 sidecar 一致性检查，也不是通用的任意模组验证器。
- 官方仓库的 `python Tools/ci.py verify <Unity平台> <modpack路径...>` 会检查平台、Unity 版本、资源清单与 sidecar。**该命令仅存在于官方模组仓库**，本公开用户工程没有 `Tools/ci.py`。
- 更新已发布内容时先提高 `modVersion`，再构建并校验所有计划支持的平台。同平台、同版本、相同字节可跳过重复上传；同版本不同内容应作为错误处理，不能覆盖旧包。

## 校验命令

从本仓库根目录运行：

```bash
python Tools/project.py check
python -m unittest discover -s Tools/tests -v
```

仅做不下载 LFS 内容的静态检查时，可用：

```bash
python Tools/project.py check --pointers
```

在验证用的独立检出中，从干净且已还原 LFS 的官方模组仓库同步：

```bash
python Tools/project.py sync /path/to/Panzer-War-Mods
```

`sync` 会替换 `MIRRORS` 列出的本地包目录并更新 `sdk-source.json`。先检查工作区，不要覆盖这些目录里尚未保存的修改。

模板构建使用 `WindyVerse.ModBuild.ModpackBuild.Build`，分别传入以下 `-modPipeline`：

- `Assets/ModManager/BuildPipline-Template-Vehicle.asset`
- `Assets/ModManager/BuildPipline-Template-Blank.asset`

通过 `-modOutput` 指定产物目录，再运行 `python Tools/project.py verify-modpacks <产物目录>`。地图包需要包含场景、地图数据和缩略图；正式发行工作流会执行这些检查。

## 文档修改

- README 面向模组作者：安装、下载、打开、打包、测试、故障排查和升级。不在用户流程中展开 CI、密钥或仓库维护细节。
- 英文为默认首页，中文与俄文为独立 Markdown 文件；三份文件顶部互相链接。
- 修改路径、版本、打包按钮或命令时，核对当前模板与工具实现，并同步三种语言。
- 文档改动检查相对链接、示例路径和 `git diff --check`；无需为纯文字改动启动 Unity 构建。
