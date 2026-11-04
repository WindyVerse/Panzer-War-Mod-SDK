using System;
using System.Collections.Generic;
using System.Threading;
using InstaLOD;
using ShanghaiWindy.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShanghaiWindy.Core
{
    // Cook-time reduction through InstaLOD. Output is cached under CookOutputScope.LodGroupRoot keyed by
    // source mesh content + recipe, so identical parts (wheels, mirrored items) resolve to one asset.
    public class InstaLodMeshProvider : ILodMeshProvider
    {
        private const string PlaceholderShader = "Hidden/InternalErrorShader";
        private const int OperationTimeoutMs = 10 * 60 * 1000;
        private const int PumpIntervalMs = 20;

        public Mesh Reduce(GameObject go, string meshName, in LodLevelRecipe recipe)
        {
            try
            {
                return ReduceInternal(go, meshName, recipe);
            }
            catch (Exception e)
            {
                Debug.LogError($"[InstaLOD] {meshName}: {e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        private static Mesh ReduceInternal(GameObject go, string meshName, in LodLevelRecipe recipe)
        {
            var sourceMesh = go.GetComponent<MeshFilter>().sharedMesh;
            var key = LodMeshCache.ContentKey(sourceMesh, recipe.CacheKey);

            var cached = LodMeshCache.TryGet(sourceMesh, key);
            if (cached != null)
            {
                return cached;
            }

            // InstaLOD saves its result under "InstaLOD Data" on its own; the cook runs inside an
            // AssetDatabase batch where that asset cannot be moved or reloaded yet, so the mesh is copied
            // into the cook's container and the transient asset dropped.
            var result = RunOptimize(go, sourceMesh, recipe, meshName);
            var copy = Object.Instantiate(result);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(result));
            return LodMeshCache.Store(sourceMesh, copy, key, meshName);
        }

        // The InstaLOD windows are the only place the plugin boots its native SDK; without this every
        // native call (ClearErrorLog is the first one in Optimize) dereferences a null context and crashes.
        private static void EnsureNativeReady()
        {
            if (!InstaLODNative.IsInitialized() && !InstaLODNative.Initialize())
            {
                throw new Exception("Failed to initialize the InstaLOD SDK");
            }

            InstaLODNative.SetRenderPipelineAssetActive(InstaLODNative.IsRenderPipelineAssetEnabled());

            if (!InstaLODNative.IsAuthorizedCached())
            {
                throw new Exception("InstaLOD is not authorized on this machine; sign in through Window > InstaLOD");
            }
        }

        private static Mesh RunOptimize(GameObject go, Mesh sourceMesh, in LodLevelRecipe recipe, string meshName)
        {
            EnsureNativeReady();
            EditorPrefs.SetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, true);
            InstaLODNative.EnsureAssetDataFolder();

            var instance = Object.Instantiate(go);
            instance.name = meshName;
            instance.transform.SetParent(null);
            instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            instance.transform.localScale = Vector3.one;

            for (var i = instance.transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(instance.transform.GetChild(i).gameObject);
            }

            var renderer = instance.GetComponent<Renderer>();
            var placeholders = CreatePlaceholderMaterials(renderer.sharedMaterials.Length);
            renderer.sharedMaterials = placeholders;

            // Shells too small to read at this level are removed before the optimizer sees the mesh.
            var extent = Mathf.Max(sourceMesh.bounds.size.x, sourceMesh.bounds.size.y, sourceMesh.bounds.size.z);
            var pruned = LodShellPruner.Prune(sourceMesh, recipe.minShellShare * extent);
            var inputMesh = pruned != null ? pruned : sourceMesh;
            instance.GetComponent<MeshFilter>().sharedMesh = inputMesh;

            try
            {
                var settings = BuildSettings(recipe, sourceMesh);
                InstaLODNative.Optimize(new List<Renderer> { renderer }, settings, default,
                    OperationResultUsage.Replace, instance);
                PumpUntilFinished();

                var result = instance.GetComponent<MeshFilter>().sharedMesh;
                if (result == null || result == inputMesh || string.IsNullOrEmpty(AssetDatabase.GetAssetPath(result)))
                {
                    throw new Exception("InstaLOD produced no mesh; check the license and the native error log");
                }

                Debug.Log($"[InstaLOD] {meshName}: {CountTriangles(sourceMesh)} -> {CountTriangles(inputMesh)} pruned -> " +
                          $"{CountTriangles(result)} tri, {sourceMesh.vertexCount} -> {result.vertexCount} vert, " +
                          $"target {settings.AbsoluteTriangles}");
                return result;
            }
            finally
            {
                Object.DestroyImmediate(instance);
                if (pruned != null)
                {
                    Object.DestroyImmediate(pruned);
                }

                foreach (var material in placeholders)
                {
                    Object.DestroyImmediate(material);
                }
            }
        }

        // One distinct material per slot: InstaLOD keys submeshes by material, so shared slots would merge.
        private static Material[] CreatePlaceholderMaterials(int count)
        {
            var shader = Shader.Find(PlaceholderShader);
            var materials = new Material[count];
            for (var i = 0; i < count; i++)
            {
                materials[i] = new Material(shader);
            }

            return materials;
        }

        // NormalizeMeshScale fits the mesh to a 10000u sphere and turns MaxDeviation into a percentage;
        // without it a 0.08u source (models scaled x100 by their parent) starves the optimizer of precision.
        // WeldingThreshold is a pre-pass in mesh units, so it stays derived from the source diagonal.
        private static InstaLODOptimizeSettings BuildSettings(in LodLevelRecipe recipe, Mesh sourceMesh)
        {
            var diagonal = sourceMesh.bounds.size.magnitude;
            var settings = new InstaLODOptimizeSettings(1f)
            {
                PercentTriangles = 1f,
                AbsoluteTriangles = recipe.ResolveTargetTriangles(CountTriangles(sourceMesh)),
                MaxDeviation = recipe.maxDeviation * 100f,
                WeldingThreshold = recipe.weldThreshold * diagonal,
                WeldingProtectDistinctUVShells = true,
                RecalculateNormals = recipe.recalculateNormals,
                HardAngleThreshold = recipe.hardAngle,
                LockBoundaries = false,
                LockSplits = false,
                ProtectBoundaries = recipe.protectBoundaries,
                ProtectSplits = recipe.protectSplits,
                NormalizeMeshScale = true,
                OptimalPlacement = true,
                Deterministic = true,
                SilhouetteImportance = (MeshFeatureImportance)recipe.silhouette,
                BoundaryImportance = (MeshFeatureImportance)recipe.boundary,
                ShadingImportance = (MeshFeatureImportance)recipe.shading,
                TextureImportance = (MeshFeatureImportance)recipe.texture,
            };

            return settings;
        }

        // The worker thread enqueues one main-thread action that swaps the mesh, creates the asset and
        // clears currentMeshOperationState, so a null state means the asset already exists.
        private static void PumpUntilFinished()
        {
            var started = Environment.TickCount;
            while (InstaLODNative.currentMeshOperationState != null)
            {
                InstaLODMainThreadAction.RunMainThreadActions();

                if (Environment.TickCount - started > OperationTimeoutMs)
                {
                    throw new TimeoutException("InstaLOD operation timed out");
                }

                Thread.Sleep(PumpIntervalMs);
            }

            InstaLODMainThreadAction.RunMainThreadActions();
        }

        private static int CountTriangles(Mesh mesh)
        {
            var count = 0;
            for (var i = 0; i < mesh.subMeshCount; i++)
            {
                count += (int)(mesh.GetIndexCount(i) / 3);
            }

            return count;
        }
    }
}
