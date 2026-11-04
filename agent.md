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
