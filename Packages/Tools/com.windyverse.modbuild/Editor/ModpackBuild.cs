using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frontend.Editor;
using ShanghaiWindy.Core;
using ShanghaiWindy.Editor;
using UnityEditor;
using UnityEngine;

namespace WindyVerse.ModBuild
{
    // Batch-mode entry that builds one modpack for the active build target:
    //   -executeMethod WindyVerse.ModBuild.ModpackBuild.Build -modPipeline <asset path> -modOutput <folder>
    public static class ModpackBuild
    {
        public static void Build()
        {
            // The SDK reports a broken pack by logging an error itself, so those (and exceptions) fail
            // the build. Errors the engine raises while the SDK builds (physics mesh cooking, no GPU for
            // the lightmapper) carry the SDK's stack too but do not stop a local build either; they are
            // listed at the end instead.
            var errors = new List<string>();
            var engine = new List<string>();
            Application.LogCallback capture = (message, stackTrace, type) =>
            {
                if (type == LogType.Exception)
                {
                    errors.Add(message);
                }
                else if (type == LogType.Error || type == LogType.Assert)
                {
                    var caller = Caller(stackTrace);
                    (caller.StartsWith("ShanghaiWindy.") || caller.StartsWith("WindyVerse.") ? errors : engine)
                        .Add(message);
                }
            };
            Application.logMessageReceived += capture;
            try
            {
                var pipelinePath = Arg("-modPipeline");
                var output = Arg("-modOutput");
                var target = EditorUserBuildSettings.activeBuildTarget.ToString();
                var pipeline = LoadPipeline(pipelinePath);

                // A fresh checkout has no personal copy destinations, and batch mode has no game
                // managers to load a language table for the SDK's localized log messages.
                EditorStorage.Info.copyModPackageToDstWinFolders = Array.Empty<string>();
                EditorStorage.Info.copyModPackageToDstAndroidFolders = Array.Empty<string>();
                EditorStorage.Info.copyModPackageToDstIosFolders = Array.Empty<string>();
                var language = Resources.Load<TextAsset>("Lang_EN");
                if (language != null)
                {
                    LocalizationManager.LanguageKey = TextHelper.TextAssetToDictionary(language);
                    LocalizationManager.lang = "EN";
                }

                // The SDK leaves archive metadata in its bundle cache and reports it as an error on the
                // next incremental build; CI restores that cache between runs.
                var stale = Path.Combine("Build", "Mod-BuildPipline", target, pipeline.name, "packages", "package.json");
                if (File.Exists(stale))
                {
                    File.Delete(stale);
                }

                ModPackageBuildPiplineDataEditor.BuildPipline(pipeline);
                pipeline = LoadPipeline(pipelinePath); // bundle builds reimport and invalidate the instance
                var archive = ModPackageDataEditor.BuildPipline(pipeline.linkedModPackage);
                if (string.IsNullOrEmpty(archive) || !File.Exists(archive) || new FileInfo(archive).Length == 0)
                {
                    throw new InvalidOperationException("The SDK produced no modpack");
                }

                Application.logMessageReceived -= capture;
                if (engine.Count > 0)
                {
                    Debug.LogWarning($"Engine errors during the build ({engine.Count}, not from the SDK):\n" +
                                     string.Join("\n", engine.Distinct().Take(20)));
                }

                if (errors.Count > 0)
                {
                    throw new InvalidOperationException("Build logged errors:\n" + string.Join("\n", errors.Take(20)));
                }

                Directory.CreateDirectory(output);
                var destination = Path.Combine(output, $"{target}_{pipeline.linkedModPackage.name}.modpack");
                File.Copy(archive, destination, true);
                // What Tools/ci.py publish tells the workshop about the pack, read back from the archive the
                // way the SDK's upload panel reads it.
                var sidecar = ModPackageWorkshopMeta.WriteSidecar(destination);
                Debug.Log("Modpack built: " + destination + "\n" + File.ReadAllText(sidecar));
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Application.logMessageReceived -= capture;
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }

        // The first frame below Unity's logging plumbing: whoever called Debug.LogError, or the engine
        // API that raised the error natively.
        private static string Caller(string stackTrace)
        {
            foreach (var line in (stackTrace ?? "").Split('\n'))
            {
                if (line.Length > 0 && !line.StartsWith("UnityEngine.StackTraceUtility") &&
                    !line.StartsWith("UnityEngine.DebugLogHandler") && !line.StartsWith("UnityEngine.Logger") &&
                    !line.StartsWith("UnityEngine.Debug:"))
                {
                    return line;
                }
            }

            return "";
        }

        private static ModPackageBuildPiplineData LoadPipeline(string path)
        {
            var pipeline = AssetDatabase.LoadAssetAtPath<ModPackageBuildPiplineData>(path);
            if (pipeline == null || pipeline.linkedModPackage == null)
            {
                throw new FileNotFoundException("No build pipeline with a linked mod package at " + path);
            }

            return pipeline;
        }

        private static string Arg(string name)
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, name);
            if (index < 0 || index + 1 >= args.Length || string.IsNullOrWhiteSpace(args[index + 1]))
            {
                throw new ArgumentException("Missing build argument " + name);
            }

            return args[index + 1];
        }
    }
}
