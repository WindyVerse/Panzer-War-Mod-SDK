using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using ShanghaiWindy.Core;
using ShanghaiWindy.Core.AssetSystem.Cache;
using ShanghaiWindy.Core.Config;
using ShanghaiWindy.Core.Data;
using ShanghaiWindy.Core.Delegate;
using ShanghaiWindy.Core.ResourceManager;
using UnityEngine;

namespace Tools.RuntimeDebugger.Scripts
{
    public class EditorModeStartup : GamePreLoadRoot
    {
        public static System.Action OnInit;
        public GameCoreConfig Config;
        public EDamageMode DamageMode = EDamageMode.ModuleBased;

        public override async UniTask EnterGame()
        {
            DontDestroyOnLoad(gameObject.transform.root);

            GameDataManager.DamageMode = DamageMode;
            GameDataManager.isNonMultiPlayerLib = true;

            // No loading panel here — bump to HIGH concurrency + disable
            // FrameBudget so editor boot doesn't crawl at the LOW default.
            AssetLoadConcurrencyPolicy.Instance.AddHighSpeedTask(this);

            SimpleResourceManager.Instance = new SimpleResourceManager(AssetSys.Scheduler);
            AssetCacheBootstrap.Init();

            var externalDirs = new List<DirectoryInfo>
            {
                new("Packages/com.shanghaiwindy.middlelayer/RuntimeRes/BuildPipline-RuntimeSupport/packages/"),
                new("Packages/com.shanghaiwindy.middlelayer/RuntimeRes/BuildPipline-Official-SoundBank/packages/"),
                new("Packages/com.shanghaiwindy.middlelayer/RuntimeRes/BuildPipline-Solider/packages/"),
            };

            var buildDir = new DirectoryInfo(Application.dataPath + "/../Build/TemporarilyBuild");
            if (buildDir.Exists)
            {
                externalDirs.Add(buildDir);
            }

            await AssetSys.RegisterExternalPackagesAsync(externalDirs);

            foreach (var packageInfo in AssetSys.GetResolvedViews())
            {
                ResourceLog.Log($"{packageInfo.PackageName} is added to package manager.");
            }

            // Atlas bundles are not manifest dependencies; they must be mounted
            // before the first bundle whose sprites reference them, and
            // RuntimeSupport itself is one.
            await SpriteAtlasBundleMounter.MountAllAsync();

            var runtimeSupportGo =
                await SimpleResourceManager.Instance.InstantiatePersistentAsync(AssetConst.RUNTIME_SUPPORT);
            DontDestroyOnLoad(runtimeSupportGo);

            // Workshop installs sit one level below the mod root, so the root
            // scan never sees them; the workshop pipeline registers their dirs.
            // Must run before the scan or they miss the single visible pass.
            if (NetworkDelegateUtil.WorkshopInitializeDelegate != null)
            {
                await NetworkDelegateUtil.WorkshopInitializeDelegate();
            }

            AssetBundleManager.OnQueryed += InitializeAsync;
            AssetBundleManager.RunInitialScanAsync().Forget();
        }

        private async void InitializeAsync()
        {
            await AssetBundleManager.AsyncHotFix();
            await AssetBundleManager.LoadLuaEnvResources();

            await UniTask.Delay(100);
            await UniTask.WaitWhile(AssetBundleManager.IsLoadingAB);
            AssetBundleManager.RunLuaEnvs();

            CommonDataManager.Instance.ApplySettings();
            UserDIYDataManager.Instance.LoadFiles();
            PoolManager.Initialize();

            AssetLoadConcurrencyPolicy.Instance.RemoveHighSpeedTask(this);

            OnInit?.Invoke();
            Debug.Log("<color=green>Enter game completed. 游戏加载已完毕</color>");
        }
    }
}
