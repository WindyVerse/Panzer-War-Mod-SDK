/**
 * InstaLODToolkitWindow.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODToolkitWindow.cs 
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODToolkitWindow class manages an editor window that provides
	/// direct access to mesh processing functionality outside of any LOD mechanism.
	/// </summary>
	[System.Serializable]
	public class InstaLODToolkitWindow : InstaLODEditorWindow
	{
		public enum MeshOperationType
		{
			Remesh = 0,
			Imposterize = 1,
			MaterialMerge = 2,
			OcclusionCull = 3,
			Optimize = 4,
			MeshToolKit = 5,
			Unwrap = 6,
			IsotropicRemesh = 7,
			License = 8
		}

		public MeshOperationType currentMeshOperationType;
		public InstaLODMeshMergeSettings meshMergeSettings;
		public InstaLODImposterizeSettings imposterizeSettings;
		public InstaLODRemeshingSettings remeshSettings;
		public InstaLODOcclusionCullSettings occlusionCullSettings;
		public InstaLODMeshToolKitSettings mtkSettings;
		public InstaLODNativeMeshOperationSettings meshOperationSettings;
		public InstaLODOptimizeSettings optimizeSettings;
		public InstaLODUnwrapSettings unwrapSettings;
		public InstaLODIsotropicRemeshSettings isotropicRemeshingSettings;
		public string mainComboBoxText;
		public string operationInformation;

		public const string optimizeOperationInformation = "The Optimize operation carefully removes polygons and relocates vertices until your polygon target has been met.Mesh attributes, such as normals or UV coordinates, are adjusted to preserve the appearence of the mesh.";
		public const string remeshOperationInformation = "The Remesh operation reconstructs the mesh from scratch. In the process, it reduces the topological complexity of the mesh while preserving the visual outcome.\n\nUV coordinates are automatically created and the original mesh appearance is transferred onto the reconstructed mesh using InstaLOD's texture baker.";
		public const string occlusionCullingOperationInformation = "The Occlusion Culling operation is capable of removing  polygons that are not visible to the observer. Occlusion Culling is an excellent tool to reduce polygon count of complex assemblies without impacting the quality of visible polygons. InstaLOD supports automatic detection and culling of interior polygons, alternatively, manually placed cameras can be used.";
		public const string meshToolkitOperationInformation = "The Mesh Toolkit operation is a swiss-army knife for geometry processing. From welding vertices to conforming normals to point into the same direction.";
		public const string materialMergeOperationInformation = "The Material Merge operation reduces draw calls by combining all material textures in your scene into a single material texture. In the process, the operation recalculates the UV layout for all objects in your scene while efficiently grouping identical meshes.";
		public const string isotropicRemeshOperationInformation = "The Isotropic Remesh operation restructures the mesh topology to ensure that edges have consistent sizes. Mesh attributes, such as normals or UV coordinates , can be optionally preserved through the process.";
		public const string imposterizeOperationInformation = "The Imposterize operation generates a stand-in mesh for foliage rendering. Foliage meshes are typically not well suited for optimization techniques that would reduce the polygon count as that would ultimately lead to the removal of elements like leaves.\n\nThe operation supports the creation of a wide variety of different imposter types.";
		public const string unwrapOperationInformation = "The UV Unwrapping operation automatically generates a UV parameterization and layout for the input mesh. The resulting UV layout is guaranteed to be free of overlaps. Multiple modes that provide the targeted results depending on the input mesh are available.";
		public const string setupInformation = "";

		public const string optimizeOperationButtonLabel = " Optimize ";
		public const string remeshOperationButtonLabel = " Remesh ";
		public const string occlusionCullingOperationButtonLabel = " Occlusion Cull ";
		public const string meshToolkitOperationButtonLabel = " Mesh Toolkit ";
		public const string materialMergeOperationButtonLabel = " Material Merge ";
		public const string isotropicRemeshOperationButtonLabel = " Isotropic Remesh ";
		public const string imposterizeOperationButtonLabel = " Imposterize ";
		public const string unwrapOperationButtonLabel = " Unwrap ";
		public const string setupButtonLabel = " Setup ";

		const UInt32 kInstaLODToolkitWindowDefaultResolution = 2048u;

		[MenuItem("Window/InstaLOD Toolkit")]
		static void Init()
		{
			GetWindow<InstaLODToolkitWindow>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODToolkitWindow"/> class.
		/// </summary>
		public InstaLODToolkitWindow()
		{
			currentMeshOperationType = MeshOperationType.Remesh;
			mainComboBoxText = new string(" Remesh ");
			operationInformation = remeshOperationInformation;
			meshMergeSettings = new InstaLODMeshMergeSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			imposterizeSettings = new InstaLODImposterizeSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			remeshSettings = new InstaLODRemeshingSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			occlusionCullSettings = new InstaLODOcclusionCullSettings(/*Resolution*/1024);
			mtkSettings = new InstaLODMeshToolKitSettings(0.0f);
			meshOperationSettings = new InstaLODNativeMeshOperationSettings(true);
			optimizeSettings = new InstaLODOptimizeSettings(1.0f);
			unwrapSettings = new InstaLODUnwrapSettings( (uint) Resolution._1024, (uint) Resolution._1024);
			isotropicRemeshingSettings = new InstaLODIsotropicRemeshSettings(1.0f);
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{
			if (InstaLODNative.currentMeshOperationState == null)
				return;

			InstaLODMainThreadAction.RunMainThreadActions();

			Repaint();
		}

		/// <summary>
		/// Notify this instance.
		/// </summary>
		/// <returns>The notify.</returns>
		/// <param name="message">Message.</param>
		public override void Notify(string message)
		{
			if (message != "ResetSettings")
				return;

			meshMergeSettings = new InstaLODMeshMergeSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			imposterizeSettings = new InstaLODImposterizeSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			remeshSettings = new InstaLODRemeshingSettings(kInstaLODToolkitWindowDefaultResolution, kInstaLODToolkitWindowDefaultResolution);
			occlusionCullSettings = new InstaLODOcclusionCullSettings(/*resolution:*/1024u);
			mtkSettings = new InstaLODMeshToolKitSettings(/*weldingThreshold:*/0.0f);
			meshOperationSettings = new InstaLODNativeMeshOperationSettings(/*initDefaults:*/true);
			optimizeSettings = new InstaLODOptimizeSettings(/*percentTriangles:*/1.0f);
			unwrapSettings = new InstaLODUnwrapSettings( (uint) Resolution._1024, (uint) Resolution._1024);
			isotropicRemeshingSettings = new InstaLODIsotropicRemeshSettings(/*targetEdgeSize:*/1.0f);
		}

		void OnEnable()
		{
			titleContent = new GUIContent("Toolkit", InstaLODStyle.headerIcon);
			minSize = new Vector2(400, 620);

			if (!InstaLODNative.IsInitialized())
			{
				if (InstaLODNative.Initialize())
				{
					InstaLODNative.SetRenderPipelineAssetActive(InstaLODNative.IsRenderPipelineAssetEnabled());
					Debug.Log("InstaLOD initialized (" + InstaLODNative.GetSDKVersionString() + ")");
				}
				else
				{
					throw new Exception("Failed to initialized InstaLOD SDK");
				}
			}
		}

		void OnSelectionChange()
		{
			Repaint();
		}
		
		void OnDestroy()
		{
			// NOTE: always respawn the window if a async operation is in progress
			// this is necessary as the main thread events are handled in our windows
			if (InstaLODNative.currentMeshOperationState != null)
			{
				InstaLODToolkitWindow newWin = Instantiate<InstaLODToolkitWindow>(this);
				newWin.Show();
			}
		}

		void OnGUI()
		{
			InstaLODStyle.EnsureStyleTextures();

			// NOTE: we need to set the content as the header icon might be null after a scene play event
			if (titleContent == null || titleContent.image == null)
			{
				titleContent = new GUIContent("InstaLOD", InstaLODStyle.headerIcon);
			}

			// draw window background fill
			{
				GUI.color = InstaLODStyle.GetWindowBackgroundColor();
				GUI.DrawTexture(new Rect(0, 0, position.width, position.height), Texture2D.whiteTexture);
				GUI.color = Color.white;
			}

			{
				if (InstaLODStyle.MainComboBoxButton(mainComboBoxText))
				{
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Optimize"), false, () => { currentMeshOperationType = MeshOperationType.Optimize; mainComboBoxText = optimizeOperationButtonLabel; operationInformation = optimizeOperationInformation; });
					menu.AddItem(new GUIContent("Remesh"), false, () => { currentMeshOperationType = MeshOperationType.Remesh; mainComboBoxText = remeshOperationButtonLabel; operationInformation = remeshOperationInformation; });
					menu.AddItem(new GUIContent("Imposterize"), false, () => { currentMeshOperationType = MeshOperationType.Imposterize; mainComboBoxText = imposterizeOperationButtonLabel; operationInformation = imposterizeOperationInformation; });
					menu.AddItem(new GUIContent("Material Merge"), false, () => { currentMeshOperationType = MeshOperationType.MaterialMerge; mainComboBoxText = materialMergeOperationButtonLabel; operationInformation = materialMergeOperationInformation; });
					menu.AddItem(new GUIContent("Occlusion Cull"), false, () => { currentMeshOperationType = MeshOperationType.OcclusionCull; mainComboBoxText = occlusionCullingOperationButtonLabel; operationInformation = occlusionCullingOperationInformation; });
					menu.AddItem(new GUIContent("MTK"), false, () => { currentMeshOperationType = MeshOperationType.MeshToolKit; mainComboBoxText = meshToolkitOperationButtonLabel; operationInformation = meshToolkitOperationInformation; });
					menu.AddItem(new GUIContent("Unwrap"), false, () => { currentMeshOperationType = MeshOperationType.Unwrap; mainComboBoxText = unwrapOperationButtonLabel; operationInformation = unwrapOperationInformation; });
					menu.AddItem(new GUIContent("Isotropic Remesh"), false, () => { currentMeshOperationType = MeshOperationType.IsotropicRemesh; mainComboBoxText = isotropicRemeshOperationButtonLabel; operationInformation = isotropicRemeshOperationInformation; });
					menu.AddItem(new GUIContent("Setup"), false, () => { currentMeshOperationType = MeshOperationType.License; mainComboBoxText = setupButtonLabel; operationInformation = setupInformation; });
					menu.ShowAsContext();
				}
			}

			using (new InstaLODStyle.FooterContent(delegate() { } ))
			{
				using (new InstaLODStyle.VerticalLayout(5))
				{
					if (!InstaLODNative.IsAuthorizedCached())
					{
						bool value = true;
						InstaLODStyle.CollapsiblePanel("Authorize", ref value, delegate()
							{
								InstaLODWindowShared.OnGUIForNotAuthorized();
							});
						InstaLODWindowShared.OnGUIForFooter(this);
						return;
					}

					if (InstaLODNative.currentMeshOperationState != null)
					{
						InstaLODWindowShared.OnGUIForAsyncMeshOperationInProgress(this);
						InstaLODWindowShared.OnGUIForFooter(this);
						return;
					}

					GUI.color = Color.white;

					_mainScrollViewPosition = EditorGUILayout.BeginScrollView(_mainScrollViewPosition);

					if (currentMeshOperationType != MeshOperationType.License)
					{
						GUILayout.Space(5.0f);

						GUILayout.TextArea(operationInformation, InstaLODStyle.informationTextBoxStyle);
					}

					GUILayout.Space(5.0f);

					using (new InstaLODStyle.ComboBoxTabLayout())
					{
						GUILayout.Space(10.0f);

						if (currentMeshOperationType == MeshOperationType.License)
						{
							InstaLODWindowShared.OnGUIForAbout(this);
						}
						else
						{
							bool spawnWarningsForMixedRenderers = currentMeshOperationType == MeshOperationType.Imposterize || currentMeshOperationType == MeshOperationType.Remesh ||
																  meshOperationSettings.combineOutputMeshes;
							List<Renderer> renderers = InstaLODWindowShared.OnGUIForSharedOperationViews(this, currentMeshOperationType != MeshOperationType.OcclusionCull && currentMeshOperationType != MeshOperationType.Imposterize, spawnWarningsForMixedRenderers, currentMeshOperationType == MeshOperationType.Optimize);

							if (Selection.activeGameObject == null || renderers.Count == 0)
							{
								InstaLODWindowShared.HelpBox("Select at least one gameobject with a 'Renderer' component.", MessageType.Info);
							}
							else
							{
								switch (currentMeshOperationType)
								{
									case MeshOperationType.Optimize:
										OnGUIForOptimize(renderers);
										break;
									case MeshOperationType.OcclusionCull:
										OnGUIForOcclusionCull(renderers);
										break;
									case MeshOperationType.MaterialMerge:
										OnGUIForMaterialMerge(renderers);
										break;
									case MeshOperationType.Imposterize:
										OnGUIForImposterize(renderers);
										break;
									case MeshOperationType.MeshToolKit:
										OnGUIForMeshToolKit(renderers);
										break;
									case MeshOperationType.Unwrap:
										OnGUIForUnwrap(renderers);
										break;
									case MeshOperationType.Remesh:
										OnGUIForRemesh(renderers);
										break;
									case MeshOperationType.IsotropicRemesh:
										OnGUIForIsotropicRemeshing(renderers);
										break;
									default:
										break;
								}
							}
						}
					}
					InstaLODWindowShared.OnGUIForFooter(this);

					EditorGUILayout.EndScrollView();
				}
			}
		}

		void OnGuiForMeshOperationSettings()
		{
			InstaLODStyle.CollapsiblePanel("Operation Settings", ref _materialOperationSettingsUIState_OperationSettingsFoldOut, delegate()
				{
					bool wasGUIEnabled = GUI.enabled;
					bool useDefaultSavePath = EditorPrefs.GetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, false);
					bool useDefaultSavePathEdited = EditorGUILayout.Toggle(new GUIContent("Create Save File Path", "Creates a save path automatically."), useDefaultSavePath);

					if (useDefaultSavePathEdited != useDefaultSavePath)
					{
						EditorPrefs.SetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, useDefaultSavePathEdited);
					}

					string hideMode = "Clone Source Game Objects";

					// NOTE: operations that only generate a single mesh cannot "clone"
					if (currentMeshOperationType == MeshOperationType.Remesh ||
						currentMeshOperationType == MeshOperationType.Imposterize)
					{
						hideMode = "Hide Source Game Objects";
					}

					meshOperationSettings.hideSourceGameObjects = EditorGUILayout.Toggle(new GUIContent(hideMode, "Hide all input meshes after the mesh operation."), meshOperationSettings.hideSourceGameObjects);

					if (currentMeshOperationType == MeshOperationType.OcclusionCull ||
						currentMeshOperationType == MeshOperationType.Optimize)
					{
						GUI.enabled = false;
						meshOperationSettings.combineOutputMeshes = false;
					}
					else if (currentMeshOperationType != MeshOperationType.MaterialMerge && 
						currentMeshOperationType != MeshOperationType.Optimize)
					{
						GUI.enabled = false;
						meshOperationSettings.combineOutputMeshes = true;
					}
					meshOperationSettings.combineOutputMeshes = EditorGUILayout.Toggle(new GUIContent("Combine Output Meshes", "Combines all input meshes into a single output mesh and parents it into the root node."), meshOperationSettings.combineOutputMeshes);
					GUI.enabled = wasGUIEnabled;

					if (currentMeshOperationType == MeshOperationType.Remesh)
					{
						meshOperationSettings.skinnedMeshEnforceBindPose = EditorGUILayout.Toggle(new GUIContent("Enforce Bindpose", "Transforms all skinned meshes back into the bindpose before remeshing the selected meshes."), meshOperationSettings.skinnedMeshEnforceBindPose);
					}
				});
		}

		private static bool IsOptimizationGoingToBreakSkinning(List<Renderer> renderers)
		{
			// NOTE: we can accept a single skeletal mesh
			if (renderers.Count <= 1)
				return false;

			// NOTE: if the selection contains multiple objects we can't convert the rig
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null && renderer is SkinnedMeshRenderer)
					return true;
			}

			return false;
		}

		void OnGUIForOptimize(List<Renderer> renderers)
		{
			bool isSkeletalMeshSelected = renderers.Find(item => item.GetComponent<Renderer>() is SkinnedMeshRenderer) != null ? true : false;

			if (IsOptimizationGoingToBreakSkinning(renderers))
			{
				InstaLODWindowShared.HelpBox("InstaLOD supports only to optimize a single skinned mesh at a time, continuing will remove the rig and generate static meshes.", MessageType.Warning);
			}

			InstaLODWindowShared.OnGUIForOptimizeSettings(this, ref optimizeSettings, isSkeletalMeshSelected);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Optimize {0} game objects", renderers.Count)))
			{
				InstaLODNative.Optimize(renderers, optimizeSettings, meshOperationSettings, resultUsage, rootObject);
			}
		}

		void OnGUIForRemesh(List<Renderer> renderers)
		{
			bool isSkeletalMeshSelected = renderers.Find(item => item.GetComponent<Renderer>() is SkinnedMeshRenderer) != null ? true : false;

			if (isSkeletalMeshSelected)
			{
				if (!meshOperationSettings.skinnedMeshEnforceBindPose)
				{
					InstaLODWindowShared.HelpBox("To avoid close mesh parts from being fused together, it recommended to remesh 'SkinnedMeshRenderer' components in the bindpose.\nSelect 'Enforce Bindpose' in the 'Operation Settings' below.", MessageType.Warning);
				}
			}

			EditorGUILayout.BeginVertical();

			InstaLODWindowShared.OnGUIForRemeshSettings(this, ref remeshSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Remesh {0} game objects", renderers.Count)))
			{
				InstaLODNative.Remesh(renderers, remeshSettings, meshOperationSettings, resultUsage, rootObject);
			}

			EditorGUILayout.EndVertical();
		}

		void OnGUIForImposterize(List<Renderer> renderers)
		{
			EditorGUILayout.BeginVertical();

			Camera camera = InstaLODWindowShared.OnGUIForImposterizeSettings(this, ref imposterizeSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			bool wasEnabled = GUI.enabled;

			if (imposterizeSettings.Type == ImposterType.Vista)
			{
				GUI.enabled = camera != null;
			}

			if (InstaLODStyle.PrimaryButton(string.Format("Imposterize {0} game objects", renderers.Count)))
			{
				InstaLODNative.Imposterize(renderers, camera, imposterizeSettings, meshOperationSettings, resultUsage, rootObject);
			}

			GUI.enabled = wasEnabled;
			EditorGUILayout.EndVertical();
		}
 
		void OnGUIForOcclusionCull(List<Renderer> renderers)
		{
			List<Camera> cameras = InstaLODWindowShared.OnGUIForOcclusionCullSettings(this, ref occlusionCullSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			bool wasEnabled = GUI.enabled;
			GUI.enabled = occlusionCullSettings.Mode == OcclusionCullMode.AutomaticInterior || cameras.Count > 0;
			if (InstaLODStyle.PrimaryButton(string.Format("Occlusion Cull {0} game objects", renderers.Count)))
			{
				InstaLODNative.OcclusionCull(renderers, cameras, occlusionCullSettings, meshOperationSettings, resultUsage, rootObject);
			}
			GUI.enabled = wasEnabled;
		}

		void OnGUIForMaterialMerge(List<Renderer> renderers)
		{
			InstaLODWindowShared.OnGUIForMeshMergeSettings(this, ref meshMergeSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Merge {0} game objects", renderers.Count)))
			{
				InstaLODNative.MeshMerge(renderers, meshMergeSettings, meshOperationSettings, resultUsage, rootObject);
			}
		}

		void OnGUIForMeshToolKit(List<Renderer> renderers)
		{
			InstaLODWindowShared.OnGUIForMeshToolKitSettings(this, ref mtkSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton("MTK"))
			{
				InstaLODNative.MeshToolKit(renderers, mtkSettings, meshOperationSettings, resultUsage, rootObject);
			}
		}

		void OnGUIForUnwrap(List<Renderer> renderers)
		{
			InstaLODWindowShared.OnGUIForUnwrapSettings(this, ref unwrapSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Unwrap {0} game objects", renderers.Count)))
			{
				InstaLODNative.Unwrap(renderers, unwrapSettings, meshOperationSettings, resultUsage, rootObject);
			}
		}

		void OnGUIForIsotropicRemeshing(List<Renderer> renderers)
		{
			InstaLODWindowShared.OnGUIForIsotropicRemeshSettings(this, ref isotropicRemeshingSettings);
			OperationResultUsage resultUsage = InstaLODWindowShared.GetResultUsageSetting(this);
			GameObject rootObject = InstaLODWindowShared.GetRootObject(this);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Isotropic Remesh {0} game objects", renderers.Count)))
			{
				InstaLODNative.IsotropicRemesh(renderers, isotropicRemeshingSettings, meshOperationSettings, resultUsage, rootObject);
			}
		}

		private Vector2 _mainScrollViewPosition;
		private bool _materialOperationSettingsUIState_OperationSettingsFoldOut = true;
	}
}