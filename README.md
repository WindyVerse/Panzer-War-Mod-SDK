# Panzer War Mod SDK · 装甲纷争模组工程

给模组作者使用的 Unity 工程，包含载具、地图、瞄具、涂装、贴花等示例，以及配套编辑器与运行库。

## 下载和打开

1. 安装 **Unity 2022.3.62f1**。制作 Android 模组时，在 Unity Hub 中额外安装 Android Build Support（含 SDK/NDK 和 OpenJDK）。
2. 从 [Releases](https://github.com/WindyVerse/Panzer-War-Mod-SDK/releases) 下载 `Panzer-War-Mod-SDK-sdk-*.zip` 并完整解压。
3. 在 Unity Hub 中添加解压后的 **`UnityProject`** 文件夹。首次打开会下载 Unity 包并导入资源。
4. 示例位于 `Assets/Template-*`；打包配置位于 `Assets/ModManager`。从示例复制自己的资源后，在模组编辑器里配置并打包。

整个目录需要一起保留：`UnityProject` 通过相对路径引用旁边的 `Packages`。
发行 ZIP 已包含完整 DLL，不需要额外安装 Git LFS。`SHA256SUMS.txt` 可用于核对下载文件。

## 用 Git 获取工程

本仓库的 **DLL、原生插件和其他大型二进制资源均使用 Git LFS**。

```bash
git lfs install
git clone https://github.com/WindyVerse/Panzer-War-Mod-SDK.git
cd Panzer-War-Mod-SDK
git lfs pull
python Tools/project.py check
```

若 Unity 提示 DLL 格式无效，先确认已安装 Git LFS 并完成 `git lfs pull`。
网页默认生成的 `Source code.zip` 可能只有 LFS 指针；请优先使用 Release 中命名完整的发行 ZIP。

## 个人设置和升级

首次运行从 `UnityProject/EditorStorage.template.json` 读取默认设置。游戏路径、账号和复制目的地在编辑器中设置，个人配置写入被 Git 忽略的 `Assets/EditorStorage.json`。

升级时下载并解压到新目录，再迁移自己的模组资源和对应 `.meta` 文件。保留资源 GUID；不要把旧版 SDK 覆盖到新版目录上。

教程：[装甲纷争模组开发文档](https://www.yuque.com/chaojiduolajiang/panzerwar/abep3y)。`Tutorials` 中保留了原工程的参考资料；旧截图与当前编辑器可能存在差异。

## 维护与发行

- `sdk-source.json` 记录工程所用 SDK 的源提交与 Unity 版本。
- `Packages/Sdk` 与列入 `Tools/project.py` 的共享工具由官方 SDK 同步；用户模板在本仓库维护。
- 私有客户端仓库的 **Publish mod SDK project** 工作流同步选定的官方 SDK、导入工程并实际打包示例载具和地图。成功后推送工程提交和 `sdk-YYYY.MM.DD.N` 标签。
- 标签触发本仓库的 **Release SDK project**：下载 LFS 原文件，校验内容，生成完整 ZIP、SHA-256 校验文件和版本清单，再发布 Release。
- 常规提交和 PR 只运行静态检查，不接触 Unity 授权。只有通过私有构建流程的版本才应打发行标签。

本地检查：`python Tools/project.py check`。发行脚本测试：`python -m unittest discover -s Tools/tests -v`。

## 许可

保留原工程的 `LICENSE`。第三方插件和资源保留各自随附的许可与声明，仓库许可证不替代它们的使用条款。

