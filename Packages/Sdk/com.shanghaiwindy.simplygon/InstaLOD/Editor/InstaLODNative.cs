/**
 * InstaLODNative.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODNative.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEditor.SceneManagement;

namespace InstaLOD
{
	public enum InstaLODNativeMeshOperationType
	{
		Optimize,
		Remesh,
		Imposterize,
		MeshMerge,
		OcclusionCull,
		MTK,
		UVUnwrap,
		UVPack,
		Bake,
		IsotropicRemesh
	}

	public enum OperationResultUsage
	{
		Replace,
		AppendToObjectLODChain,
		AppendToRootLODChain
	}

	/// <summary>
	/// The InstaLODNativeMeshOperationSettings struct represents settings that are
	/// common to most mesh operations.
	/// </summary>
	[Serializable]
	public struct InstaLODNativeMeshOperationSettings
	{
		public InstaLODNativeMeshOperationSettings(bool initDefaults)
		{
			hideSourceGameObjects = true;
			combineOutputMeshes = false;
			skinnedMeshEnforceBindPose = false;
		}

		public bool hideSourceGameObjects;
		public bool combineOutputMeshes;
		public bool skinnedMeshEnforceBindPose;
	}

	internal struct InstaLODNativeConstants
	{
		public static UInt32 MaxMeshTextureCoords = 8;
		public static UInt32 MaxMeshWedgeColors = 4;
		public static UInt32 BoneIndexInvalid = ~0u;
		public static UInt32 JointIndexInvalid = ~0u;
	}

	/// <summary>
	/// The InstaLODNative class provides C# access to the C++ SDK.
	/// </summary>
	public class InstaLODNative
	{
		public static readonly string PluginVersion = "SDK2023";

		#region native methods

		[DllImport("InstaLODUnityNative", EntryPoint = "Initialize")]
		public static extern bool Initialize();

		[DllImport("InstaLODUnityNative", EntryPoint = "Initialize")]
		public static extern bool IsInitialized();

		[DllImport("InstaLODUnityNative", EntryPoint = "Dispose")]
		public static extern bool Dispose();

		[DllImport("InstaLODUnityNative", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetSDKVersion();

		public static string GetSDKVersionString()
		{
			return Marshal.PtrToStringAnsi(GetSDKVersion());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetBuildDate();

		public static string GetBuildDateString()
		{
			return Marshal.PtrToStringAnsi(GetBuildDate());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetAuthorizationInformation();

		public static string GetAuthorizationInformationString()
		{
			return Marshal.PtrToStringAnsi(GetAuthorizationInformation());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool AuthorizeMachine(string username, string password);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool DeauthorizeMachine(string username, string password);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool IsAuthorizedCached();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool IsAuthorized();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClearErrorLog();

		[DllImport("InstaLODUnityNative", EntryPoint = "GetErrorLog", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr GetErrorLogInt();

		public static string GetErrorLog()
		{
			return Marshal.PtrToStringAnsi(GetErrorLogInt());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetRenderPipelineAssetActive(bool isActive);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Append(ref InstaLODMesh mesh);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Get(ref InstaLODMesh mesh);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Extract(ref InstaLODMesh mesh, UInt32 index, UInt32 count);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_InitializeAuxiliary();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_DeallocAuxiliary();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_AppendAuxiliary(ref InstaLODMesh mesh);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODSkeleton_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODSkeleton_Dealloc();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern UInt32 InstaLODSkeleton_AddJoint(UInt32 parentIndex, UInt32 jointIndex, ref Vector3 position, ref Quaternion orientation, ref Vector3 scale, string name);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void OptimizeMesh(ref InstaLODOptimizeSettings optimizeSettings, ref OptimizeResult outResult);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void OptimizeMesh2(ref InstaLODMesh inputMesh, ref InstaLODMesh outputMesh, ref InstaLODOptimizeSettings optimizeSettings, ref OptimizeResult outResult);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterialData_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterialData_Finalize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_Begin(int materialID);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_SetDefaultColor(ref Color color);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_AddTexturePage(ref InstaLODTexturePage page);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern UInt32 InstaLODMaterial_GetTexturePageCount();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_GetTexturePageAtIndex(uint index, ref InstaLODTexturePage page);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_WriteTexturePageAtIndex(uint index, string path, UInt32 compressionQuality);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool MeshMerge(ref InstaLODMeshMergeSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool OcclusionCull(ref InstaLODOcclusionCullSettings settings, IntPtr occlusionCullCameras, int cameraCount);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool Imposterize(ref InstaLODImposterizeSettings settings, ref InstaLODMesh customGeometry);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool Remesh(ref InstaLODRemeshingSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool MeshToolKit(ref InstaLODMeshToolKitSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool Unwrap(ref InstaLODUnwrapSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool IsotropicRemesh(ref InstaLODIsotropicRemeshSettings settings);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MeshOperationProgressDelegate(float progress);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void SetProgressDelegate(MeshOperationProgressDelegate callback);

		#endregion

		/// <summary>
		/// Ensures that the asset data folder exists.
		/// </summary>
		public static void EnsureAssetDataFolder()
		{
			if (!AssetDatabase.IsValidFolder("Assets/InstaLOD Data"))
			{
				AssetDatabase.CreateFolder("Assets", "InstaLOD Data");
			}
		}

		/// <summary>
		/// Gets the asset save path for object.
		/// </summary>
		/// <returns>The asset path for object.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="initialPath">Initial save path.</param>
		public static string GetAssetSavePathForObject(UnityEngine.Object obj, string initialPath)
		{
			if (EditorPrefs.GetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, false))
			{
				string objectID = obj.GetInstanceID().ToString("x4");
				string assetPath = "Assets/InstaLOD Data/" + objectID + (obj is Material ? ".mat" : ".asset");
				return assetPath;
			}

			string title = "Save New Mesh";
			string extension = "asset";

			if (obj is Material)
			{
				title = "Save New Material";
				extension = "mat";
			}

			string path = Path.GetDirectoryName(initialPath);
			string fileName = Path.GetFileName(initialPath);
			string savePath = EditorUtility.SaveFilePanelInProject(title, fileName, extension, "Save asset in directory", path);

			return savePath.Length == 0 ? initialPath : savePath;
		}

		/// <summary>
		/// Determines whether this is a programmable pipeline.
		/// </summary>
		/// <returns>True if URP or HDRP.</returns>
		public static bool IsRenderPipelineAssetEnabled()
		{
			return UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset != null;
		}

		/// <summary>
		/// Dumps the InstaLOD native error log.
		/// </summary>
		/// <returns><c>true</c>, if native error log was dumped, <c>false</c> otherwise.</returns>
		internal static bool DumpNativeErrorLog()
		{
			string instaErrorLog = GetErrorLog();

			if (string.IsNullOrEmpty(instaErrorLog))
				return false;


			UnityEngine.Debug.LogWarningFormat("InstaLOD {0} operation generated a error/warning log:\n{1}", currentMeshOperationState.type, instaErrorLog);
			return true;
		}

		/// <summary>
		/// The InstaLODMeshOperationState manages state for a mesh operation.
		/// </summary>
		public class InstaLODMeshOperationState
		{
			public InstaLODMeshOperationState(List<Renderer> renderers, InstaLODNativeMeshOperationType meshOperationType)
			{
				renderers = new List<Renderer>();
				startTime = DateTime.Now;
				progress = 0.0f;
				type = meshOperationType;
				owningWindow = null;
				repaintOwningWindow = false;
			}

			public static void NativeProgressCallback(float progress)
			{
				if (currentMeshOperationState == null)
					return;

				currentMeshOperationState.progress = progress;

				if (currentMeshOperationState.owningWindow != null &&
					currentMeshOperationState.repaintOwningWindow == true)
				{
					if (Thread.CurrentThread.IsBackground)
						throw new Exception("Cannot repaint from background thead");
				}
			}

			public List<Renderer> renderers;
			public DateTime startTime;
			public float progress;
			public InstaLODNativeMeshOperationType type;
			public EditorWindow owningWindow;
			public bool repaintOwningWindow;

			public TimeSpan elapsedTimeSpan
			{
				get
				{
					return DateTime.Now - startTime;
				}
			}
		}

		public static InstaLODMeshOperationState currentMeshOperationState;

		/// <summary>
		/// Begins the native mesh operation.
		/// </summary>
		/// <remarks>This method is guaranteed to be invoked on the mainthread</remarks>
		/// <returns><c>true</c>, if native mesh operation was begun, <c>false</c> otherwise.</returns>
		/// <param name="meshOperationState">Mesh operation state.</param>
		public static bool BeginNativeMeshOperation(InstaLODMeshOperationState meshOperationState)
		{
			lock ("CurrentMeshOperationState")
			{
				if (currentMeshOperationState != null)
					return false;

				currentMeshOperationState = meshOperationState;

				SetProgressDelegate(InstaLODMeshOperationState.NativeProgressCallback);
				return true;
			}
		}

		/// <summary>
		/// Finishes the native mesh operation.
		/// </summary>
		/// <remarks>This method is guaranteed to be invoked on the mainthread</remarks>
		/// <returns><c>true</c>, if native mesh operation was finished, <c>false</c> otherwise.</returns>
		public static bool FinishNativeMeshOperation()
		{
			lock ("CurrentMeshOperationState")
			{
				DumpNativeErrorLog();

				currentMeshOperationState = null;
				return true;
			}
		}

		/// <summary>
		/// Helper method to build the native skeleton.
		/// </summary>
		/// <returns><c>true</c>, if skeleton add joint recursive was natived, <c>false</c> otherwise.</returns>
		/// <param name="meshOperation">Mesh operation.</param>
		/// <param name="renderer">Renderer.</param>
		/// <param name="joint">Joint.</param>
		/// <param name="parentJointIndex">Parent joint index.</param>
		internal static bool NativeSkeleton_AddJointRecursive(InstaLODNativeMeshOperationWrapper meshOperation, SkinnedMeshRenderer renderer, Transform joint, UInt32 parentJointIndex)
		{	
			int jointIndex = Array.FindIndex(renderer.bones, transform => transform == joint);

			// FIXME: we need to setup a global bone ID for all meshes in the operation no matter what if we're combining or not
			// once done we can use 'meshOperation.GetGlobalBoneID(joint);'

			if (jointIndex == -1)
				return false;

			Vector3 position = joint.position;
			Quaternion rotation = joint.rotation;
			Vector3 localScale = joint.localScale;

			UInt32 finalJointIndex = InstaLODSkeleton_AddJoint(parentJointIndex, (uint)jointIndex, ref position, ref rotation, ref localScale, joint.name);

			if (finalJointIndex == InstaLODNativeConstants.JointIndexInvalid)
				return false;

			for (int i = 0; i < joint.childCount; i++)
			{
				NativeSkeleton_AddJointRecursive(meshOperation, renderer, joint.GetChild(i), finalJointIndex);
			}

			return true;
		}

		/// <summary>
		/// Helper method to build the native skeleton.
		/// </summary>
		/// <param name="meshOperation">Mesh operation.</param>
		/// <param name="renderers">Renderers.</param>
		internal static void NativeSkeleton_Initialize(InstaLODNativeMeshOperationWrapper meshOperation, List<Renderer> renderers)
		{
			// NOTE: ensure the previous skeleton is deallocated
			InstaLODSkeleton_Dealloc();

			if (renderers == null)
				return;

			if (meshOperation == null)
				throw new NullReferenceException("meshOperation");

			if (renderers.Count > 1)
			{
				// FIXME: we need to fully implement this to allow optimizing multiple meshes via the toolkit
				// what's missing here is remapping weights to a global space and undoing the mapping post optimization
				// when we do not combine the output mesh
				// NOTE: we don't throw here anymore. The user is informed about the issue but can continue if loosing skinning
				// is acceptable.
				return;
			}

			bool didInitializeSkeleton = false;

			foreach(Renderer baseRenderer in renderers)
			{
				if (!(baseRenderer is SkinnedMeshRenderer))
					continue;

				SkinnedMeshRenderer renderer = baseRenderer as SkinnedMeshRenderer;

				if (renderer.rootBone == null)
					continue;

				if (!didInitializeSkeleton)
				{
					InstaLODSkeleton_Initialize();
				}

				NativeSkeleton_AddJointRecursive(meshOperation, renderer, renderer.rootBone, InstaLODNativeConstants.JointIndexInvalid);
			}
		}

		/// <summary>
		/// The InstaLODNativeMeshOperationWrapper class manages execution of a mesh operation on the specified
		/// Unity renderer instances. It is responsible for conversion of the meshes to InstaLOD meshes, materials as well as post processing.
		/// </summary>
		internal class InstaLODNativeMeshOperationWrapper : IDisposable
		{
			public bool combineOutputMeshes;
			public bool hideSourceGameObjects;
			public bool extractMaterials;
			public bool bakeSkinnedMeshes;
			public bool extractSubmeshes;
			public bool postProcessingRequired;

			public string auxiliaryGameObjectSuffix;
			public bool auxiliaryMeshInitialized;

			public Shader primaryShader;
			public Transform rootBone;
			public List<Matrix4x4> worldSpaceBindPoses;
			public List<InstaLODMeshWrapper> meshes;
			public List<GCHandle> textureHandles;
			public Dictionary<InstaLODMeshWrapper, UInt32> wrapperSubmeshOffsets;
			public Dictionary<Material, int> globalMaterialMap;
			public List<Transform> uniqueBones;
			public List<GameObject> owners;
			public GameObject rootObject;
			public OperationResultUsage resultUsage;
			public Matrix4x4 postProcessTransformMatrix;

			/// <summary>
			/// Initializes a new instance of the
			/// <see cref="T:InstaLOD.InstaLODNative.InstaLODNativeMeshOperationWrapper"/> class.
			/// </summary>
			public InstaLODNativeMeshOperationWrapper()
			{
				meshes = new List<InstaLODMeshWrapper>();
				wrapperSubmeshOffsets = new Dictionary<InstaLODMeshWrapper, uint>();
				textureHandles = new List<GCHandle>();
				globalMaterialMap = new Dictionary<Material, int>();
				primaryShader = null;
				uniqueBones = new List<Transform>();
				worldSpaceBindPoses = new List<Matrix4x4>();
				owners = new List<GameObject>();
				rootBone = null;
				rootObject = null;
				combineOutputMeshes = false;
				hideSourceGameObjects = true;
				extractMaterials = true;
				bakeSkinnedMeshes = false;
				auxiliaryGameObjectSuffix = string.Empty;
				auxiliaryMeshInitialized = false;
				extractSubmeshes = false;
				resultUsage = OperationResultUsage.Replace;
				postProcessingRequired = false;
				postProcessTransformMatrix = Matrix4x4.identity;

				InstaLODMesh_Initialize();
			}

			#region IDisposable implementation

			public void Dispose()
			{
				foreach (InstaLODMeshWrapper wrapper in meshes)
				{
					wrapper.Dispose();
				}
				foreach (GCHandle handle in textureHandles)
				{
					handle.Free();
				}

				meshes.Clear();
				textureHandles.Clear();
			}

			#endregion

			/// <summary>
			/// Gets the global bone identifier.
			/// </summary>
			/// <returns>The global bone identifier.</returns>
			/// <param name="bone">Bone.</param>
			public int GetGlobalBoneID(Transform bone)
			{
				return uniqueBones.IndexOf(bone);
			}

			/// <summary>
			/// Adds the mesh.
			/// </summary>
			/// <param name="owner">Owner.</param>
			/// <param name="inputMesh">Input mesh.</param>
			/// <param name="materials">Materials.</param>
			/// <param name="transform">Transform.</param>
			/// <param name="skinnedMeshRenderer">Skinned mesh renderer.</param>
			public void AddMesh(GameObject owner, Mesh inputMesh, Material[] materials, Transform transform, SkinnedMeshRenderer skinnedMeshRenderer)
			{
				if (materials != null)
				{
					foreach (Material material in materials)
					{
						if (globalMaterialMap.ContainsKey(material))
							continue;

						if (primaryShader == null)
						{
							primaryShader = material.shader;
						}

						if (material.shader != primaryShader)
							throw new Exception("All materials must use the same shader");

						globalMaterialMap.Add(material, globalMaterialMap.Count);
					}
				}

				Vector3[] transformedVertices = null;
				Dictionary<int, int> localToGlobalBoneMap = null;

				if (skinnedMeshRenderer != null && combineOutputMeshes)
				{
					if (rootBone == null)
					{
						rootBone = skinnedMeshRenderer.rootBone;
					}
					
					// take a snapshot of the mesh as our new bindpose
					Mesh tempMesh = new Mesh();
					skinnedMeshRenderer.BakeMesh(tempMesh);
					transformedVertices = tempMesh.vertices;

					// NOTE: vertices for combined meshes will be in world space 
					// with a bindpose base on the current bone state
					// so to do the bone transform we just need to do the world to local for each bone
					foreach (Transform bone in skinnedMeshRenderer.bones)
					{
						if (uniqueBones.Contains(bone))
							continue;

						uniqueBones.Add(bone);
						worldSpaceBindPoses.Add(bone.worldToLocalMatrix);
					}

					// create local to global bone map
					localToGlobalBoneMap = new Dictionary<int, int>();
					for (int i = 0; i < skinnedMeshRenderer.bones.Length; i++)
					{
						localToGlobalBoneMap[i] = GetGlobalBoneID(skinnedMeshRenderer.bones[i]);
					}
				}
				else if (skinnedMeshRenderer != null && bakeSkinnedMeshes)
				{
					Mesh tempMesh = new Mesh();
					skinnedMeshRenderer.BakeMesh(tempMesh);
					transformedVertices = tempMesh.vertices;
				}

				UInt32 submeshID = (UInt32) meshes.Count;
				InstaLODMeshWrapper wrapper = new InstaLODMeshWrapper(owner, inputMesh, materials, globalMaterialMap, transform, localToGlobalBoneMap, transformedVertices, submeshID);
				owners.Add(owner);

				if (meshes.Count > 0)
				{
					InstaLODMeshWrapper lastMesh = meshes[meshes.Count - 1];
					// NOTE: submesh indices are now discared. each mesh is now a submesh itself if used in a single operation with multiple game objects
					wrapperSubmeshOffsets[wrapper] = wrapperSubmeshOffsets[lastMesh] + 1;
				}
				else
				{
					wrapperSubmeshOffsets[wrapper] = 0;
				}

				meshes.Add(wrapper);
			
				// append to aux mesh
				if (!string.IsNullOrEmpty(this.auxiliaryGameObjectSuffix) && owner.name.EndsWith(this.auxiliaryGameObjectSuffix))
				{
					if (!this.auxiliaryMeshInitialized)
					{
						InstaLODMesh_InitializeAuxiliary();
						this.auxiliaryMeshInitialized = true;
					}

					InstaLODMesh_AppendAuxiliary(ref wrapper.nativeMesh);
					wrapperSubmeshOffsets[wrapper] = 0;
				}
				else
				{
					InstaLODMesh_Append(ref wrapper.nativeMesh);
				}
			}

			/// <summary>
			/// Extracts the combined mesh.
			/// </summary>
			/// <returns>The combined mesh.</returns>
			public Mesh ExtractCombinedMesh()
			{
				using (InstaLODMeshWrapper wrapper = new InstaLODMeshWrapper(null, null))
				{
					InstaLODMesh nativeMesh = new InstaLODMesh();
					InstaLODMesh_Get(ref nativeMesh);

					return wrapper.ConvertNativeMesh(nativeMesh);
				}
			}

			/// <summary>
			/// Extracts the mesh.
			/// </summary>
			/// <returns>The mesh.</returns>
			/// <param name="owningObject">Owning object.</param>
			public Mesh ExtractMesh(GameObject owningObject)
			{
				InstaLODMeshWrapper wrapper = meshes.Find(item => item.owningObject == owningObject);
			
				if (wrapper == null)
					return null;
				
				InstaLODMesh nativeMesh = new InstaLODMesh();
				int submeshIndex = owners.IndexOf(owningObject);

				InstaLODMesh_Extract(ref nativeMesh, (UInt32) submeshIndex, (UInt32)1);

				bool applyInverseWorldTransform = true;

				if (nativeMesh.BoneWeights != IntPtr.Zero && bakeSkinnedMeshes)
				{
					applyInverseWorldTransform = false;
				}

				int submeshOffset = extractSubmeshes ? (int) wrapperSubmeshOffsets[wrapper] - submeshIndex : 0;
				return wrapper.ConvertNativeMesh(nativeMesh, applyInverseWorldTransform, /*isNewMaterial:*/ false);
			}

			private static Shader _normalMapConversionShader;
			static Shader normalMapConversionShader
			{
				get
				{
					if (_normalMapConversionShader == null)
					{
						_normalMapConversionShader = Shader.Find("InstaLOD/UnpackNormal");

						if (_normalMapConversionShader == null)
							throw new Exception("Failed to load 'UnpackNormal' shader"); 
					}
					return _normalMapConversionShader;
				}
			}

			private static Material _normalMapConversionMaterial;
			static Material normalMapConversionMaterial
			{
				get
				{
					if (_normalMapConversionMaterial == null)
					{
						_normalMapConversionMaterial = new Material(normalMapConversionShader);
					}
					return _normalMapConversionMaterial;
				}
			}

			/// <summary>
			/// Gets the texture data in a safe manner. This means that it is guaranteed to return valid image data.
			/// If a normal map is being processed, it converted back from a Unity packed normal map, to a regular tangent-space normal mpa.
			/// </summary>
			/// <returns>The texture data safe.</returns>
			/// <param name="texturePageData">Texture page data.</param>
			public static Color32[] GetTextureDataSafe(InstaLODWindowShared.TexturePageData texturePageData)
			{
				Texture texture = texturePageData.texture;
				RenderTextureReadWrite colorSpace = texturePageData.isSRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear;

				RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, colorSpace);
				rt.filterMode = FilterMode.Point;
				RenderTexture.active = rt;

				if (texturePageData.isNormalMap)
				{
					Graphics.Blit(texture, rt, normalMapConversionMaterial);
				}
				else
				{
					Graphics.Blit(texture, rt);
				}

				Texture2D img2 = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);
				img2.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
				img2.Apply();

				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(rt);

				return img2.GetPixels32();
			}

			/// <summary>
			/// Builds the native material data object.
			/// </summary>
			public void BuildMaterialData()
			{
				InstaLODMaterialData_Initialize();

				foreach (Material material in globalMaterialMap.Keys)
				{
					int materialID = globalMaterialMap[material];
					List<InstaLODWindowShared.TexturePageData> textures = InstaLODWindowShared.GetMaterialTextures(material);
				
					InstaLODMaterial_Begin(materialID);

					Color materialColor = material.color; 
					InstaLODMaterial_SetDefaultColor(ref materialColor);

					foreach (InstaLODWindowShared.TexturePageData texturePageData in textures)
					{
						if (texturePageData.texture is Texture2D == false)
							continue;

						Color32[] textureData = GetTextureDataSafe(texturePageData);

						GCHandle dataHandle = GCHandle.Alloc(textureData, GCHandleType.Pinned);
						textureHandles.Add(dataHandle);

						InstaLODTexturePage instaPage = new InstaLODTexturePage();

						instaPage.Width = (UInt32)texturePageData.texture.width;
						instaPage.Height = (UInt32)texturePageData.texture.height;
						instaPage.Name = Marshal.StringToHGlobalAnsi(texturePageData.texturePageName);
						instaPage.IsNormalMap = texturePageData.isNormalMap;
						instaPage.Data = dataHandle.AddrOfPinnedObject();
						instaPage.ComponentType = TexturePageComponentType.ComponentTypeUInt8;
						instaPage.PixelType = TexturePagePixelType.PixelTypeRGBA;
						instaPage.DefaultColor = material.color;

						InstaLODMaterial_AddTexturePage(ref instaPage);
					}
				}

				InstaLODMaterialData_Finalize();
			}

			/// <summary>
			/// Gets the texture format for texture page.
			/// </summary>
			/// <returns>The texture format for texture page.</returns>
			/// <param name="page">Page.</param>
			public TextureFormat GetTextureFormatForTexturePage(InstaLODTexturePage page)
			{
				if (page.ComponentType == TexturePageComponentType.ComponentTypeUInt8)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.R8;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RG16; 
						case TexturePagePixelType.PixelTypeRGB:
							return TextureFormat.RGB24; 
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBA32; 
					}
				}
				else if (page.ComponentType == TexturePageComponentType.ComponentTypeUInt16)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.RHalf;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RGHalf; 
						case TexturePagePixelType.PixelTypeRGB:
							return TextureFormat.RGBAHalf; 
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBAHalf; 
					}
				}
				else if (page.ComponentType == TexturePageComponentType.ComponentTypeFloat32)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.RFloat;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RGFloat; 
						case TexturePagePixelType.PixelTypeRGB:
							throw new Exception("PixelTypeRGB for Float32 not supported by Unity");
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBAFloat; 
					}
				}

				throw new Exception("Invalid texture page format");
			}

			/// <summary>
			/// Creates a unity material based on InstaLOD material data.
			/// </summary>
			/// <returns>The material.</returns>
			public Material ExtractMaterial(string materialName = null, ExportFormats exportFormat = ExportFormats.PNG8Dithered, UInt32 compressionQuality = 100u)
			{
				string fileExtension;
				switch (exportFormat)
				{
					case ExportFormats.PNG8Dithered:
						fileExtension = "png";
						break;
					case ExportFormats.TGA8Dithered:
						fileExtension = "tga";
						break;
					case ExportFormats.JPEG:
						fileExtension = "jpeg";
						break;
					case ExportFormats.EXR16:
						fileExtension = "exr";
						break;
					case ExportFormats.HDR:
						fileExtension = "hdr";
						break;
					default:
						fileExtension = "png";
						break;
				}
				Material material = new Material(primaryShader);

				material.name = "InstaLOD Material";
				string initialFilePath = "Assets/InstaLOD Data/";

				if (!String.IsNullOrEmpty(materialName))
				{
					initialFilePath = Path.Combine(initialFilePath, materialName);
				}
				else
				{
					initialFilePath = Path.Combine(initialFilePath, "M_NewMaterial_INSTALOD");
				}

				string materialAssetPath = GetAssetSavePathForObject(material, initialFilePath);

				if (String.IsNullOrEmpty(materialAssetPath))
				{
					UnityEngine.Debug.LogError("No save path selected for material.");
					return material;
				}

				if (!AssetDatabase.IsValidFolder("Assets/InstaLOD Data"))
					AssetDatabase.CreateFolder("Assets", "InstaLOD Data");
				
				AssetDatabase.CreateAsset(material, materialAssetPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				string saveFilePath = Path.GetDirectoryName(materialAssetPath);
				string saveFileName = Path.GetFileNameWithoutExtension(materialAssetPath);

				uint count = InstaLODMaterial_GetTexturePageCount();

				for (uint i = 0; i < count; i++)
				{
					InstaLODTexturePage page = new InstaLODTexturePage();
					InstaLODMaterial_GetTexturePageAtIndex(i, ref page);

					string pageName = Marshal.PtrToStringAnsi(page.Name);
					bool isSRGB = true;

					if (pageName == "NormalTangentSpace")
					{
						if (primaryShader.name == "HDRP/Lit")
						{
							pageName = "_NormalMap";
						}
						else
						{
							pageName = "_BumpMap";
						}
					}
					else if (pageName == "AmbientOcclusion")
					{
						pageName = "_OcclusionMap";
					}
					else if (pageName == "Displacement")
					{
						pageName = "_ParallaxMap";
					}
					else if (pageName == "Opacity")
					{
						pageName = "_TranslucencyMap";
					}
					else if (pageName == "_MaskMap")
					{
						isSRGB = false;
					}

					// write texture to disk using InstaLOD SDK
					string finalTextureAssetPath = Path.Combine(saveFilePath, string.Format("{0}-{1}.{2}", saveFileName, pageName, fileExtension));
					string writeTexturePath = Path.GetFullPath(finalTextureAssetPath);
					InstaLODMaterial_WriteTexturePageAtIndex(i, writeTexturePath, compressionQuality);

					// reimport PNG
					AssetDatabase.ImportAsset(finalTextureAssetPath);

					// get texture importer
					AssetImporter baseImporter = TextureImporter.GetAtPath(finalTextureAssetPath);
					TextureImporter importer = baseImporter as TextureImporter;
					importer.textureType = TextureImporterType.Default;
					importer.mipmapEnabled = true;
					importer.sRGBTexture = isSRGB;

					// if it was a bumpmap, set the type to normalmap
					if (page.IsNormalMap)
					{
						importer.textureType = TextureImporterType.NormalMap;
					}
					importer.SaveAndReimport();

					// get the imported texture handle
					Texture importedTexture = AssetDatabase.LoadAssetAtPath<Texture>(finalTextureAssetPath);

					// assign it to the final material
					material.SetTexture(pageName, importedTexture);

					// enable material permutations

					if (primaryShader.name == "HDRP/Lit")
					{
						if (page.IsNormalMap)
						{
							material.EnableKeyword("_NORMALMAP_TANGENT_SPACE");
						}
						else if (pageName == "_MaskMap")
						{
							material.EnableKeyword("_MASKMAP");
						}
						else if (pageName == "_DetailMap")
						{
							material.EnableKeyword("_DETAIL_MAP");
						}
					}
					else
					{
						if (page.IsNormalMap)
						{
							material.EnableKeyword("_NORMALMAP");
						}
						else if (pageName == "_ParallaxMap")
						{
							material.EnableKeyword("_PARALLAXMAP");
						}
					}
				}

				AssetDatabase.Refresh();
				return material;
			}
		}

		/// <summary>
		/// This function deactivates all objects in mesh if they are not added to the LODgroup
		/// of the owner game object.
		/// </summary>
		/// <param name="owner">The game object.</param>
		/// <param name="meshes">The meshes to check.</param>
		public static void DeactivateGameObjectsIfNotInLODGroup(GameObject owner, List<Renderer> meshes)
		{
			if (meshes == null || owner == null)
				return;

			Renderer[] markedForDeactivation = null;
			markedForDeactivation = owner.GetComponent<LODGroup>() == null ? meshes.ToArray() : null;

			// check if meshes are in the last lod group entry
			if (markedForDeactivation == null)
			{
				LODGroup lodGroup = owner.GetComponent<LODGroup>();
				LOD[] lods = lodGroup.GetLODs();

				if (lods.Length != 0)
				{
					LOD lastLOD = lods[lods.Length - 1];

					List<Renderer> marked = new List<Renderer>();
					
					foreach (Renderer renderer in meshes)
					{
						if (renderer == null)
							continue;

						if (!Array.Exists(lastLOD.renderers, item => item == renderer))
						{
							marked.Add(renderer);
						}
					}
				}
				else
				{
					markedForDeactivation = meshes.ToArray();
				}
			}

			if (markedForDeactivation == null || markedForDeactivation.Length == 0)
				return;

			foreach (Renderer renderer in markedForDeactivation)
			{
				if (renderer == null || renderer.gameObject == null)
					continue;

				renderer.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Adds the specified mesh to the owner game object.
		/// </summary>
		/// <param name="owner">The gameobject receiving the mesh as an LOD group entity.</param>
		/// <param name="meshes">The renderer objects.</param>
		public static void AddToLODGroup(GameObject owner, List<Renderer> meshes)
		{
			if (owner == null || meshes == null)
				throw new Exception("Objects must not be null");

			if (owner.GetComponent<LODGroup>() == null)
			{
				LODGroup newLodGroup = owner.AddComponent<LODGroup>();
				newLodGroup.SetLODs(null);

				// add current renderer to LOD group
				Renderer renderer = owner.GetComponent<Renderer>();
				if (renderer != null)
				{
					LOD[] lod = new LOD[1];
					lod[0].renderers = new Renderer[1];
					lod[0].renderers[0] = renderer;
					lod[0].screenRelativeTransitionHeight = 0.4f;
					newLodGroup.SetLODs(lod);
				}
			}

			LODGroup lodGroup = owner.GetComponent<LODGroup>();
			LOD[] oldLODS = lodGroup.GetLODs();

			// check if an empty entry exists, NOTE: if we generate 
			int emptyLODIndex = -1;
			for (int i = 0; i < oldLODS.Length; i++)
			{
				if (oldLODS[i].renderers == null || oldLODS[i].renderers.Length == 0)
				{
					emptyLODIndex = i;
					break;
				}
			}

			if (emptyLODIndex != -1)
			{
				oldLODS[emptyLODIndex].renderers = meshes.ToArray();
				lodGroup.RecalculateBounds();
				return;
			}

			LOD[] newLODS = new LOD[oldLODS.Length + 1];

			float lastLODScreenSize = 1.0f;
			for (int i = 0; i < oldLODS.Length; i++)
			{
				newLODS[i] = oldLODS[i];
				lastLODScreenSize = newLODS[i].screenRelativeTransitionHeight;
			}
			lastLODScreenSize *= 0.5f;

			// Note: search for empty LOD entry first
			LOD newLOD = new LOD();
			newLOD.screenRelativeTransitionHeight = lastLODScreenSize;
			newLOD.renderers = meshes.ToArray();
			newLODS[oldLODS.Length] = newLOD;

			lodGroup.SetLODs(newLODS);
			lodGroup.RecalculateBounds();
		}

		/// <summary>
		/// Merges the specified mesh materials.
		/// </summary>
		/// <returns><c>true</c>, if merge was meshed, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		internal static bool ExecuteMeshOperation(List<Renderer> renderers, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> initalizeOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> willExecuteOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> executeOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, Renderer, bool> finalizeOperation,
												  Func<InstaLODNativeMeshOperationWrapper, bool> exitOperation, ExportFormats exportFormat = ExportFormats.PNG8Dithered, UInt32 compressionQuality = 100u)
		{
			Shader primaryShader = null;

			foreach (Renderer renderer in renderers)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (primaryShader != null && primaryShader != material.shader)
						throw new Exception("All renderers must make use of the same shader");

					primaryShader = material.shader;
				}

				// ensure it has a mesh filter
				if (renderer is MeshRenderer)
				{
					if (renderer.gameObject.GetComponent<MeshFilter>() == null)
						throw new Exception("MeshRenderer without MeshFilter");
				}
			}

			ClearErrorLog();

			InstaLODNative.EnsureAssetDataFolder();

			InstaLODNativeMeshOperationWrapper operation = new InstaLODNativeMeshOperationWrapper();

			try
			{
				initalizeOperation(operation);

				foreach (Renderer renderer in renderers)
				{
					GameObject gameObject = renderer.gameObject;
					Mesh mesh = (renderer is SkinnedMeshRenderer) ? (renderer as SkinnedMeshRenderer).sharedMesh : gameObject.GetComponent<MeshFilter>().sharedMesh;

					Transform transform = gameObject.transform;
					operation.AddMesh(gameObject, mesh, renderer.sharedMaterials, transform, renderer as SkinnedMeshRenderer);
				}

				operation.BuildMaterialData();

				willExecuteOperation(operation);
			}
			catch (Exception exception)
			{
				operation.Dispose();
				operation = null;
				throw exception;
			}

			// the mesh operation is ready to go,
			// start a child thread and execute the action
			Thread thread = new Thread(delegate()
				{
					bool result = executeOperation(operation);

					if (!result)
					{
						InstaLODMainThreadAction.EnqueueMainThreadAction(
							delegate()
							{
								exitOperation(operation);
								operation.Dispose();
							});
						return;
					}

					InstaLODMainThreadAction.EnqueueMainThreadAction(
						delegate()
						{
							try
							{
								string saveFileName = renderers[0].name + "_INSTALOD";
								Material mergeMaterial = operation.extractMaterials ? operation.ExtractMaterial("M_" + saveFileName, exportFormat, compressionQuality) : null;

								if (operation.combineOutputMeshes)
								{
									GameObject combinedMesh = new GameObject(string.Format("InstaLOD combined {0} meshes", renderers.Count));
									Mesh newMesh = operation.ExtractCombinedMesh();

									if (operation.rootBone != null)
									{
										SkinnedMeshRenderer skinnedMeshRenderer = combinedMesh.AddComponent<SkinnedMeshRenderer>();
										skinnedMeshRenderer.bones = operation.uniqueBones.ToArray();
										skinnedMeshRenderer.rootBone = operation.rootBone;
										skinnedMeshRenderer.sharedMesh = newMesh;
										skinnedMeshRenderer.sharedMesh.bindposes = operation.worldSpaceBindPoses.ToArray();
										skinnedMeshRenderer.quality = SkinQuality.Bone4;
									}
									else
									{
										MeshFilter filter = combinedMesh.AddComponent<MeshFilter>();
										combinedMesh.AddComponent<MeshRenderer>();

										filter.sharedMesh = newMesh;
										filter.sharedMesh.bindposes = null;
									}

									Renderer renderer = combinedMesh.GetComponent<Renderer>();
									renderer.sharedMaterials = new Material[] { mergeMaterial };

									if(operation.postProcessingRequired)
									{
										Vector3 boundingBoxCenter = renderer.bounds.center;
										InstaLODMeshUtilites.ApplyPostTransform(ref newMesh, operation.postProcessTransformMatrix, boundingBoxCenter);
										combinedMesh.GetComponent<MeshFilter>().sharedMesh = newMesh;
									}

									// disable all original game objects in the operation
									if (operation.hideSourceGameObjects)
									{
										foreach (Renderer originalRenderer in renderers)
										{
											//  NOTE: it is possible that the original gameObject has been destroyed
											try
											{
												originalRenderer.gameObject.SetActive(false);
											}
											catch (Exception)
											{
												UnityEngine.Debug.LogWarning("Failed to hide original game object. It is recommended to not remove game objects from the scene while a mesh operation is being executed.");
											}
										}
									}

									string savePath = GetAssetSavePathForObject(newMesh, "Assets/InstaLOD Data/MESH_" + saveFileName);

									if (String.IsNullOrEmpty(savePath))
									{
										UnityEngine.Debug.LogError("No save path selected for mesh");
										return;
									}

									AssetDatabase.CreateAsset(newMesh, savePath);
									finalizeOperation(operation, renderer);

									// handle if root should receive lod chain
									List<Renderer> newRenderers = new List<Renderer>();
									newRenderers.Add(renderer);

									if (operation.rootObject != null && operation.resultUsage == OperationResultUsage.AppendToRootLODChain)
									{
										DeactivateGameObjectsIfNotInLODGroup(operation.rootObject, renderers);
										AddToLODGroup(operation.rootObject, newRenderers);

										renderer.gameObject.transform.SetParent(operation.rootObject.transform, /*worldPositionStays:*/ true);
									}
								}
								else
								{
									List<Renderer> newRenderers = new List<Renderer>();

									foreach (Renderer owningRenderer in renderers)
									{
										GameObject owningGameObject = owningRenderer.gameObject;
										Mesh newMesh = operation.ExtractMesh(owningGameObject);
										Renderer renderer = owningRenderer;

										if (operation.hideSourceGameObjects)
										{
											GameObject clone = UnityEngine.Object.Instantiate<GameObject>(renderer.gameObject, renderer.gameObject.transform.parent);

											clone.name = renderer.gameObject.name + " (InstaLOD)";

											if (operation.resultUsage == OperationResultUsage.Replace)
											{
												renderer.gameObject.SetActive(false);
											}
											else if (operation.resultUsage == OperationResultUsage.AppendToObjectLODChain)
											{
												clone.transform.SetParent(renderer.gameObject.transform, true);
											}

											{
												Transform cloneTransform = renderer.gameObject.transform;
												renderer = clone.GetComponent<Renderer>();
												newRenderers.Add(renderer);
											}
										}

										if (operation.extractMaterials)
										{
											// remove all submesh data after a material OP
											int[] triangles = newMesh.triangles;
											newMesh.subMeshCount = 1;
											newMesh.SetTriangles(triangles, 0);

											renderer.sharedMaterials = new Material[] { mergeMaterial }; 
										}

										if (renderer is SkinnedMeshRenderer)
										{
											SkinnedMeshRenderer skinnedMeshRenderer = (renderer as SkinnedMeshRenderer);
											skinnedMeshRenderer.sharedMesh = newMesh;
											skinnedMeshRenderer.quality = SkinQuality.Bone4;

											if (operation.bakeSkinnedMeshes)
											{
												// NOTE: reset the bone transforms to the current rig as we've baked the pose into the vertices
												List<Matrix4x4> newBindPoses = new List<Matrix4x4>();

												foreach (Transform bone in skinnedMeshRenderer.bones)
												{
													newBindPoses.Add(bone.worldToLocalMatrix);
												}

												skinnedMeshRenderer.sharedMesh.bindposes = newBindPoses.ToArray();
											}
										}
										else
										{
											MeshFilter filter = (renderer as MeshRenderer).gameObject.GetComponent<MeshFilter>();
											filter.mesh = newMesh;
										}

										// NOTE: only DataUsage replace will trigger a save file dialog
										string directory = "Assets/InstaLOD Data/";
										string newFileName = "MESH_" + owningRenderer.gameObject.name + "_INSTALOD.mesh";
										string savePath = Path.Combine(directory, newFileName);

										if (operation.resultUsage == OperationResultUsage.Replace)
										{
											savePath = GetAssetSavePathForObject(newMesh, savePath);
										}
										else
										{
											savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);
										}

										if (String.IsNullOrEmpty(savePath))
										{
											UnityEngine.Debug.LogError("No save path selected for mesh");
											return;
										}

										AssetDatabase.CreateAsset(newMesh, savePath);
										finalizeOperation(operation, renderer);

										// add object to local lod chain
										if (operation.resultUsage == OperationResultUsage.AppendToObjectLODChain)
										{
											List<Renderer> lodObjects = new List<Renderer>();
											lodObjects.Add(renderer);
											GameObject ownedObject = renderer.gameObject;

											// remove own lod group
											if (ownedObject.GetComponent<LODGroup>() != null)
											{
												GameObject.DestroyImmediate(ownedObject.GetComponent<LODGroup>());
											}

											AddToLODGroup(owningGameObject, lodObjects);

											List<GameObject> childrenToDestroy = new List<GameObject>();
											for (int i = 0; i < ownedObject.transform.childCount; i++)
											{
												childrenToDestroy.Add(ownedObject.transform.GetChild(i).gameObject);
											}

											// destroy children
											foreach (GameObject childObject in childrenToDestroy)
											{
												GameObject.DestroyImmediate(childObject);
											}
										}
									}

									// handle if root should receive lod chain
									if (operation.rootObject != null && operation.resultUsage == OperationResultUsage.AppendToRootLODChain)
									{
										DeactivateGameObjectsIfNotInLODGroup(operation.rootObject, renderers);
										AddToLODGroup(operation.rootObject, newRenderers);
									}
								}

								AssetDatabase.SaveAssets();
								AssetDatabase.Refresh();
								EditorSceneManager.MarkAllScenesDirty();
							}
							finally
							{
								exitOperation(operation);

								// NOTE: force repaint, without refocusing UI windows
								if (EditorWindow.HasOpenInstances<InstaLODGroupWindow>())
								{
									if (InstaLODGroupWindow.GetWindow<InstaLODGroupWindow>("", false).isVisible)
									{
										InstaLODGroupWindow.GetWindow<InstaLODGroupWindow>().Repaint();
									}
								}

								if (EditorWindow.HasOpenInstances<InstaLODToolkitWindow>())
								{
									if (InstaLODToolkitWindow.GetWindow<InstaLODToolkitWindow>("", false).isVisible)
									{
										InstaLODToolkitWindow.GetWindow<InstaLODToolkitWindow>().Repaint();
									}
								}

								operation.Dispose();
							}
						});
				});
			
			thread.Start();
			return true;
		}

		/// <summary>
		/// Occlusion culls the specified renderers.
		/// </summary>
		/// <returns><c>true</c>, if cull was occlusioned, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="unityCameras">Unity cameras.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static bool OcclusionCull(List<Renderer> renderers, List<Camera> unityCameras, InstaLODOcclusionCullSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			List<InstaLODOcclusionCullCamera> nativeCameras = new List<InstaLODOcclusionCullCamera>();

			foreach (Camera unityCamera in unityCameras)
			{
				InstaLODOcclusionCullCamera nativeCamera = new InstaLODOcclusionCullCamera();

				nativeCamera.Position = unityCamera.transform.position;
				nativeCamera.Forward = unityCamera.transform.forward;
				nativeCamera.Right = unityCamera.transform.right;
				nativeCamera.Up = unityCamera.transform.up;
				nativeCamera.NearPlane = unityCamera.nearClipPlane;
				nativeCamera.FarPlane = unityCamera.farClipPlane;
				nativeCamera.Aspect = unityCamera.aspect;
				nativeCamera.IsOrthogonal = unityCamera.orthographic;
				nativeCamera.OrthogonalScale = unityCamera.orthographicSize * 2.0f;

				// NOTE: unity fov is vertical
				float verticalFovInRadians = unityCamera.fieldOfView * Mathf.Deg2Rad;
				float horizontalFovInRadians = 2 * Mathf.Atan(Mathf.Tan(verticalFovInRadians / 2) * unityCamera.aspect);
				nativeCamera.FieldOfViewInDegrees = horizontalFovInRadians * Mathf.Rad2Deg;

				nativeCameras.Add(nativeCamera);
			}

			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = false;
					wrapper.extractMaterials = false;
					wrapper.bakeSkinnedMeshes = true;
					wrapper.resultUsage = resultUsage;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.OcclusionCull)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					InstaLODOcclusionCullCamera[] nativeCamerasArray = nativeCameras.ToArray();
					GCHandle camerasHandle = GCHandle.Alloc(nativeCamerasArray, GCHandleType.Pinned);

					bool occlusionResult = OcclusionCull(ref settings, nativeCameras.Count == 0 ? IntPtr.Zero : camerasHandle.AddrOfPinnedObject(), nativeCameras.Count);
				
					camerasHandle.Free();

					return occlusionResult;
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Mesh merges the specified renderers.
		/// </summary>
		/// <returns><c>true</c>, if merge was meshed, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static bool MeshMerge(List<Renderer> renderers, InstaLODMeshMergeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					wrapper.resultUsage = resultUsage;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.MeshMerge)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return MeshMerge(ref settings);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				},
				settings.BakeOutput.ExportFormat,
				settings.BakeOutput.CompressionQuality
			);

			return result;
		}

		/// <summary>
		/// Remeshes the specified renderers.
		/// </summary>
		/// <returns>The remesh.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static bool Remesh(List<Renderer> renderers, InstaLODRemeshingSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object.");

			if (meshOperationSettings.skinnedMeshEnforceBindPose)
			{
				ResetSkinnedRendererToBindPose(renderers);
			}
			
			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					wrapper.resultUsage = resultUsage;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Remesh)))
						throw new Exception("Mesh operation already in progress.");
					
					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return Remesh(ref settings);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				},
				settings.BakeOutput.ExportFormat,
				settings.BakeOutput.CompressionQuality
			);

			return result;
		}

		/// <summary>
		/// Called on vista imposterize.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		/// <param name="camera">Camera</param>
		/// <param name="vistaDistance">Plane distance from camera</param>
		/// <param name="parallelImposterPlane">Parallels imposter plane with camera</param>
		/// <param name="planeMesh">Plane mesh</param>
		/// <param name="planePosition">Plane position</param>
		/// <param name="planeRotation">Plane rotation</param>
		/// <param name="postProcessTransformMatrix">Post process transformation matrix</param>
		public static bool OnVistaImposter(ref List<Renderer> renderers, Camera camera, float vistaDistance, bool parallelImposterPlane, ref Mesh planeMesh, ref Vector3 planePosition, ref Quaternion planeRotation, ref Matrix4x4 postProcessTransformMatrix)
		{

			Transform cameraTransform = camera.GetComponent<Transform>();

			Bounds minimumBoundingBox = new Bounds();
			Vector3 minimumPoint = Vector3.positiveInfinity;
			Vector3 maximumPoint = Vector3.negativeInfinity;
			foreach (Renderer renderer in renderers)
			{
				minimumPoint = Vector3.Min(minimumPoint, renderer.bounds.min);
				maximumPoint = Vector3.Max(maximumPoint, renderer.bounds.max);
			}

			if (camera == null || renderers.Count < 1)
			{
				UnityEngine.Debug.LogError("Select at least two game objects, one of which with a 'Camera' component.");
				return false;
			}

			if (!Mathf.Approximately(cameraTransform.rotation.eulerAngles.z, 0.0f))
			{
				UnityEngine.Debug.LogWarning("Rotating the camera around the forward axis (Roll), causes the result to be inaccurate.");
			}

			minimumBoundingBox.SetMinMax(minimumPoint, maximumPoint);

			Vector3[] minimumBoundingBoxVertices = new Vector3[]{
														minimumBoundingBox.min,
														new Vector3(minimumBoundingBox.center.x - minimumBoundingBox.extents.x, minimumBoundingBox.center.y + minimumBoundingBox.extents.y, minimumBoundingBox.center.z - minimumBoundingBox.extents.z),
														new Vector3(minimumBoundingBox.center.x + minimumBoundingBox.extents.x, minimumBoundingBox.center.y - minimumBoundingBox.extents.y, minimumBoundingBox.center.z - minimumBoundingBox.extents.z),
														new Vector3(minimumBoundingBox.center.x - minimumBoundingBox.extents.x, minimumBoundingBox.center.y - minimumBoundingBox.extents.y, minimumBoundingBox.center.z + minimumBoundingBox.extents.z),
														new Vector3(minimumBoundingBox.center.x + minimumBoundingBox.extents.x, minimumBoundingBox.center.y + minimumBoundingBox.extents.y, minimumBoundingBox.center.z - minimumBoundingBox.extents.z),
														new Vector3(minimumBoundingBox.center.x + minimumBoundingBox.extents.x, minimumBoundingBox.center.y - minimumBoundingBox.extents.y, minimumBoundingBox.center.z + minimumBoundingBox.extents.z),
														new Vector3(minimumBoundingBox.center.x - minimumBoundingBox.extents.x, minimumBoundingBox.center.y + minimumBoundingBox.extents.y, minimumBoundingBox.center.z + minimumBoundingBox.extents.z),
														minimumBoundingBox.max,
														};

			Vector3[] minimumBoundingBoxVerticesProjected = new Vector3[8];

			Vector3 minimumProjectedPoint = Vector3.positiveInfinity;
			Vector3 maximumProjectedPoint = Vector3.negativeInfinity;

			List<KeyValuePair<uint, float>> verticesDistanceFromCamera = new List<KeyValuePair<uint, float>>();
			Vector3 cameraObjectVector = minimumBoundingBox.center - cameraTransform.position;

			Vector3 normalizedForwardVector = cameraTransform.forward;
			if(!parallelImposterPlane)
			{
				normalizedForwardVector = cameraObjectVector.normalized;
			}
			planeRotation = Quaternion.LookRotation(normalizedForwardVector);

			for (uint index=0u; index<minimumBoundingBoxVertices.Length; index++)
			{
				verticesDistanceFromCamera.Add(new KeyValuePair<uint, float>(index, Vector3.Project(minimumBoundingBoxVertices[index] - cameraTransform.position, normalizedForwardVector).magnitude));

				minimumBoundingBoxVerticesProjected[index] = camera.WorldToViewportPoint(minimumBoundingBoxVertices[index]);
				minimumProjectedPoint = Vector3.Min(minimumBoundingBoxVerticesProjected[index], minimumProjectedPoint);
				maximumProjectedPoint = Vector3.Max(minimumBoundingBoxVerticesProjected[index], maximumProjectedPoint);
			}

			verticesDistanceFromCamera.Sort((vertexDistance1, vertexDistanc2) => vertexDistance1.Value.CompareTo(vertexDistanc2.Value));
			Vector3 nearestVertexFromCamera = minimumBoundingBoxVertices[verticesDistanceFromCamera[0].Key];
			Vector3 farthestVertexFromCamera = minimumBoundingBoxVertices[verticesDistanceFromCamera[verticesDistanceFromCamera.Count - 1].Key];

			Plane farIntersectionPlane = new Plane(planeRotation * Vector3.back, cameraTransform.position + Vector3.Project(farthestVertexFromCamera - cameraTransform.position, normalizedForwardVector));
			Plane nearIntersectionPlane = new Plane(planeRotation * Vector3.back, cameraTransform.position + Vector3.Project(nearestVertexFromCamera - cameraTransform.position, normalizedForwardVector));

			Vector3[] viewportRectVertices = new Vector3[]{ minimumProjectedPoint,
														new Vector3(minimumProjectedPoint.x , maximumProjectedPoint.y, maximumProjectedPoint.z),
														new Vector3(maximumProjectedPoint.x, minimumProjectedPoint.y, minimumProjectedPoint.z),
														maximumProjectedPoint,
														};

			Vector3 boundingBoxProjectionCenter = minimumProjectedPoint + (maximumProjectedPoint - minimumProjectedPoint) / 2.0f;

			float farCenterDistance;
			float nearCenterDistance;

			float defaultPlaneMaximumDistanceFromCamera = Vector3.Dot(farthestVertexFromCamera - cameraTransform.position, normalizedForwardVector);
			float defaultPlaneMinimumDistanceFromCamera = Vector3.Dot(nearestVertexFromCamera - cameraTransform.position, normalizedForwardVector);
			float desiredPlaneDistanceFromCamera = defaultPlaneMinimumDistanceFromCamera + (defaultPlaneMaximumDistanceFromCamera - defaultPlaneMinimumDistanceFromCamera) * vistaDistance;
			Plane desiredIntersectionPlane = new Plane(planeRotation * Vector3.back, cameraTransform.position + normalizedForwardVector * desiredPlaneDistanceFromCamera);

			Ray boundingBoxCenterDeprojectionRay = camera.ViewportPointToRay(boundingBoxProjectionCenter);
			Vector3 farPlaneCenterVertex = Vector3.zero;
			Vector3 nearPlaneCenterVertex = Vector3.zero;
			if (farIntersectionPlane.Raycast(boundingBoxCenterDeprojectionRay, out farCenterDistance) && nearIntersectionPlane.Raycast(boundingBoxCenterDeprojectionRay, out nearCenterDistance))
			{
				farPlaneCenterVertex = boundingBoxCenterDeprojectionRay.GetPoint(farCenterDistance);
				nearPlaneCenterVertex = boundingBoxCenterDeprojectionRay.GetPoint(nearCenterDistance);
			}
			else
			{
				UnityEngine.Debug.LogError("Failed to generate deprojection plane.");
				return false;
			}
			planePosition = farPlaneCenterVertex;

			Vector3[] deprojectedVertices = new Vector3[4];
			Vector3[] desiredDeprojectedVertices = new Vector3[4];
			float distanceAlongRay;
			float desiredDistanceAlongRay;
			Ray deprojectionRay;

			for (uint index=0u; index<viewportRectVertices.Length; index++)
			{
				deprojectionRay = camera.ViewportPointToRay(viewportRectVertices[index]);
				if (farIntersectionPlane.Raycast(deprojectionRay, out distanceAlongRay) && desiredIntersectionPlane.Raycast(deprojectionRay, out desiredDistanceAlongRay))
				{
					deprojectedVertices[index] = deprojectionRay.GetPoint(distanceAlongRay);
					desiredDeprojectedVertices[index] = deprojectionRay.GetPoint(desiredDistanceAlongRay);
				}
				else
				{
					UnityEngine.Debug.LogError("The deprojected points do not hit the planes.");
					return false;
				}
			}

			float farDistanceDimension1 = Vector3.Distance(deprojectedVertices[0], deprojectedVertices[1]);
			float nearDistanceDimension1 = Vector3.Distance(desiredDeprojectedVertices[0], desiredDeprojectedVertices[1]);

			float farDistanceDimension2 = Vector3.Distance(deprojectedVertices[0], deprojectedVertices[2]);
			float nearDistanceDimension2 = Vector3.Distance(desiredDeprojectedVertices[0], desiredDeprojectedVertices[2]);

			if (Mathf.Approximately(farDistanceDimension1, 0.0f) || Mathf.Approximately(farDistanceDimension2, 0.0f))
			{
				UnityEngine.Debug.LogError("The selected objects have no width, or height.");
				return false;
			}

			float newScaleDimension1 = nearDistanceDimension1 / farDistanceDimension1;
			float newScaleDimension2 = nearDistanceDimension2 / farDistanceDimension2;

			Vector3 translationVector = (defaultPlaneMaximumDistanceFromCamera - desiredPlaneDistanceFromCamera) * (nearPlaneCenterVertex - farPlaneCenterVertex).normalized;
			Vector3 scalingVector = new Vector3(newScaleDimension1, newScaleDimension1, newScaleDimension2);

			postProcessTransformMatrix.SetTRS(translationVector, Quaternion.identity, scalingVector);

			float planeHeight = Vector3.Distance(deprojectedVertices[0], deprojectedVertices[1]);
			float planeWidth = Vector3.Distance(deprojectedVertices[0], deprojectedVertices[2]);

			planeMesh = InstaLOD.InstaLODMeshUtilites.CreatePlane(new Vector2(planeWidth / 2.0f, planeHeight / 2.0f));

			Quaternion planeInverseRotation = Quaternion.Inverse(planeRotation);
			planeMesh.normals = new Vector3[] { planeInverseRotation * (cameraTransform.position - deprojectedVertices[1]).normalized,
													planeInverseRotation * (cameraTransform.position - deprojectedVertices[3]).normalized,
													planeInverseRotation * (cameraTransform.position - deprojectedVertices[0]).normalized,
													planeInverseRotation * (cameraTransform.position - deprojectedVertices[2]).normalized};

			return true;
		}

		/// <summary>
		/// Imposterizes the specified renderers.
		/// </summary>
		/// <returns>The imposterize.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="camera">Camera</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static bool Imposterize(List<Renderer> renderers, Camera camera, InstaLODImposterizeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{		
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			GameObject planeObject = null;
			InstaLODMesh planeNativeMesh = new InstaLODMesh();
			Matrix4x4 postProcessTransformMatrix = Matrix4x4.identity;
			if (settings.Type == ImposterType.Vista)
			{
				Mesh planeMesh = new Mesh();
				Vector3 planePosition = Vector3.zero;
				Quaternion planeRotation = Quaternion.identity;
				bool vistaImposterReady = OnVistaImposter(ref renderers, camera, settings.VistaDistance, settings.ParallelImposterPlane, ref planeMesh, ref planePosition, ref planeRotation, ref postProcessTransformMatrix);

				if (!vistaImposterReady)
					return false;

				planeObject = new GameObject("Plane_imposter", typeof(MeshFilter), typeof(MeshRenderer));
				planeObject.GetComponent<MeshFilter>().mesh = planeMesh;
				planeObject.GetComponent<Transform>().transform.position = planePosition;
				planeObject.GetComponent<MeshFilter>().transform.rotation = planeRotation;

				InstaLODMeshWrapper meshWrapper = new InstaLODMeshWrapper(planeObject, planeMesh, planeObject.GetComponent<Renderer>().sharedMaterials, transform: planeObject.transform);
				planeNativeMesh = meshWrapper.nativeMesh;
			}

			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (settings.Type == ImposterType.HybridBillboardCloud)
						wrapper.auxiliaryGameObjectSuffix = "_cloudpoly";
					
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					wrapper.resultUsage = resultUsage;
					wrapper.rootObject = rootObject;
					wrapper.postProcessingRequired = settings.Type == ImposterType.Vista && settings.VistaDistance < 1.0f;
					wrapper.postProcessTransformMatrix = postProcessTransformMatrix;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Imposterize)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return Imposterize(ref settings, ref planeNativeMesh);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					try
					{
						Material standardShaderMaterial = renderer.sharedMaterial;

						standardShaderMaterial.SetInt("_Mode", 1);
						standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						standardShaderMaterial.SetInt("_ZWrite", 1);
						standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
						standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
						standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						standardShaderMaterial.SetFloat("_Cutoff", 0.25f);
						standardShaderMaterial.renderQueue = 2450;

						renderer.sharedMaterial = standardShaderMaterial;
					}
					catch (Exception)
					{
						UnityEngine.Debug.LogErrorFormat("Failed to set material '{0}' blend mode to to 'CutOut'. Material is probably not a standard material.", renderer.sharedMaterial.name);
					}

					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					if (planeObject != null)
					{ 
						GameObject.DestroyImmediate(planeObject);
						planeObject = null;
					}
					return true;
				},
				settings.BakeOutput.ExportFormat,
				settings.BakeOutput.CompressionQuality
			);

			return result;
		}

		/// <summary>
		/// Perform polygon optimization on the specified renderers.
		/// </summary>
		/// <returns>The optimize.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static void Optimize(List<Renderer> renderers, InstaLODOptimizeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{			
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			// dealloc previous skeleton
			NativeSkeleton_Initialize(null, null);

			ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = false;
					wrapper.extractMaterials = false;
					wrapper.extractSubmeshes = true;
					wrapper.resultUsage = resultUsage;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					NativeSkeleton_Initialize(wrapper, renderers);

					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Optimize)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					OptimizeResult result = new OptimizeResult();
					OptimizeMesh(ref settings, ref result);
					return result.Success;
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer render)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);
		}

		/// <summary>
		/// Perform mesh tool kit operation on the specified renderers.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		public static bool MeshToolKit(List<Renderer> renderers, InstaLODMeshToolKitSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			bool result = ExecuteMeshOperation(renderers,
				/* initialize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.resultUsage = resultUsage;
					wrapper.extractMaterials = false;
					wrapper.extractSubmeshes = !settings.RemoveSubmeshIndices && (settings.CalculateSubmeshIndicesByAdjacency || settings.CalculateSubmeshIndicesByMaterial);
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.MTK)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					return MeshToolKit(ref settings);
				},
				/* finalize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Perform unwrap operation on the specified renderers.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		/// <returns>True upon success.</returns>
		public static bool Unwrap(List<Renderer> renderers, InstaLODUnwrapSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			bool result = ExecuteMeshOperation(renderers,
				/* initialize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.resultUsage = resultUsage;
					wrapper.extractMaterials = false;
					wrapper.extractSubmeshes = false;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.UVUnwrap)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					return Unwrap(ref settings);
				},
				/* finalize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Perform isotropic remesh operation on the specified renderers.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		/// <param name="resultUsage">How the result should be handled.</param>
		/// <param name="rootObject">The root object of the hierarchy.</param>
		/// <returns>True upon success.</returns>
		public static bool IsotropicRemesh(List<Renderer> renderers, InstaLODIsotropicRemeshSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings, OperationResultUsage resultUsage = OperationResultUsage.Replace, GameObject rootObject = null)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (rootObject == null && resultUsage == OperationResultUsage.AppendToRootLODChain)
				throw new Exception("Can't add to root LOD without root object");

			bool result = ExecuteMeshOperation(renderers,
				/* initialize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.resultUsage = resultUsage;
					wrapper.extractMaterials = false;
					wrapper.extractSubmeshes = false;
					wrapper.rootObject = rootObject;
					return true;
				},
				/* will execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.IsotropicRemesh)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					return IsotropicRemesh(ref settings);
				},
				/* finalize */
				delegate (InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */
				delegate (InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Resets the skinned renderer to bind pose.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		public static void ResetSkinnedRendererToBindPose(List<Renderer> renderers)
		{
			renderers.ForEach(delegate(Renderer render)
				{
					ReflectionRestoreToBindPose(render as SkinnedMeshRenderer);
				});
		}

		/// <summary>
		/// Uses C# reflection to restore the mesh to the bind pose.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		public static void ReflectionRestoreToBindPose(SkinnedMeshRenderer renderer)
		{
			if (renderer == null)
				return;

			try
			{
				Type type = Type.GetType("UnityEditor.AvatarSetupTool, UnityEditor");
				if (type == null)
					return;
				
				MethodInfo info = type.GetMethod("SampleBindPose", BindingFlags.Static | BindingFlags.Public);
				if (info == null)
					return;
			
				info.Invoke(null, new object[] { renderer.gameObject });
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogErrorFormat("InstaLOD: failed to reset SkinnedRenderer of '{0}' to bind pose", renderer.gameObject);
			}
		}

		/// <summary>
		/// Selects all game objects that fall within the bounds of the selected objects, expanded by the specified expansion radius.
		/// </summary>
		/// <param name="selection">The initial selection</param>
		/// <param name="expandRadius">The expand radius</param>
		/// <param name="sceneObjects">All the objects that are in the active scene</param>
		/// <returns>True upon success.</returns>
		public static bool ExpandSelection(ref GameObject [] gameObjectsSelection, float expandRadius, GameObject[] sceneObjects)
		{
			List<GameObject> newObjectSelection = new List<GameObject>();

			if (gameObjectsSelection.Length < 1)
				return true;

			Bounds minimumBounds = new Bounds();
			for(int i = 0; i < gameObjectsSelection.Length; i++)
			{
				Renderer objectRenderer = gameObjectsSelection[i].GetComponent<Renderer>();
				if (objectRenderer == null)
				{
					uint iteration = 0u;
					List<GameObject> objectChildren = new List<GameObject>();
					if(InstaLODWindowShared.CollectChildrenRecursive(gameObjectsSelection[i], objectChildren, ref iteration))
					{
						newObjectSelection.AddRange(objectChildren);
					}
				}
				else
				{
					newObjectSelection.Add(gameObjectsSelection[i]);
				}
			}

			if(newObjectSelection.Count < 1)
				return false;

			Renderer renderer = newObjectSelection[0].GetComponent<Renderer>();
			minimumBounds.center = renderer.bounds.center;

			for (int i = 0; i < newObjectSelection.Count; i++)
			{
				Bounds objectBound= newObjectSelection[i].GetComponent<Renderer>().bounds;
				minimumBounds.Encapsulate(objectBound.min);
				minimumBounds.Encapsulate(objectBound.max);
			}

			newObjectSelection.Clear();

			minimumBounds.Expand(expandRadius * 2);

			for (int i = 0; i < sceneObjects.Length; i++)
			{
				Renderer sceneObjectsRenderer = sceneObjects[i].GetComponent<Renderer>();
				if (sceneObjectsRenderer != null)
				{
					if (minimumBounds.Intersects(sceneObjectsRenderer.bounds))
					{
						newObjectSelection.Add(sceneObjects[i]);
					}
				}
				else
				{
					uint iteration = 0u;
					List<GameObject> objectChildren = new List<GameObject>();
					if (InstaLODWindowShared.CollectChildrenRecursive(sceneObjects[i], objectChildren, ref iteration))
					{
						foreach(GameObject child in objectChildren)
						{
							Renderer childRenderer = child.GetComponent<Renderer>();
							if (minimumBounds.Intersects(childRenderer.bounds))
							{
								newObjectSelection.Add(child);
							}
						}
					}
				}
			}
			gameObjectsSelection = newObjectSelection.ToArray();
			return true;
		}
	}
}