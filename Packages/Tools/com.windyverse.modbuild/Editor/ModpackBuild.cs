using System;
using System.IO;
using System.Linq;
using Frontend.Editor;
using ShanghaiWindy.Editor;
using UnityEditor;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;

namespace WindyVerse.ModBuild
{
    // Batch-mode entry that builds one modpack for the active build target:
    //   -executeMethod WindyVerse.ModBuild.ModpackBuild.Build -modPipeline <asset path> -modOutput <folder>
    // The SDK builds the pack and says what went wrong (ModPackageBuilder). What is left here is the process:
    // arguments in, the archive with its workshop description out, and an exit code.
    public static class ModpackBuild
    {
        public static void Build()
        {
            try
            {
                var pipelinePath = Arg("-modPipeline");
                var output = Arg("-modOutput");
                if (Environment.GetCommandLineArgs().Contains("-modFreshCache"))
                {
                    ClearBundleBuildCache();
                }
                var result = ModPackageBuilder.Build(pipelinePath);

                if (result.engineErrors.Count > 0)
                {
                    Debug.LogWarning($"Engine errors during the build ({result.engineErrors.Count}, not from the SDK):\n" +
                                     string.Join("\n", result.engineErrors.Distinct().Take(20)));
                }

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Build logged errors:\n" + string.Join("\n", result.errors.Take(20)));
                }

                Directory.CreateDirectory(output);
                var destination = Path.Combine(output, Path.GetFileName(result.archivePath));
                File.Copy(result.archivePath, destination, true);
                // What Tools/ci.py publish tells the workshop about the pack, read back from the archive the
                // way the SDK's upload panel reads it.
                var sidecar = ModPackageWorkshopMeta.WriteSidecar(destination);
                Debug.Log("Modpack built: " + destination + "\n" + File.ReadAllText(sidecar));
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }

        // The SBP cache can keep shaders compiled with earlier quality/stripping settings.
        // An explicit fresh build keeps imported assets and the shader compiler cache;
        // only the current project's SBP bundle cache is purged through its native API.
        private static void ClearBundleBuildCache()
        {
            var project = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            var library = Path.Combine(project, "Library");
            var cache = Path.Combine(library, "BuildCache");
            foreach (var folder in new[] { library, cache })
            {
                if (Directory.Exists(folder) && (File.GetAttributes(folder) & FileAttributes.ReparsePoint) != 0)
                {
                    throw new InvalidOperationException("Refuse clearing a linked build cache: " + folder);
                }
            }
            var previousDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(project);
                Debug.Log("Fresh SDK bundle cache: " + Path.GetFullPath("Library/BuildCache"));
                BuildCache.PurgeCache(false);
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }
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
