/**
 * InstaLODGroupWindow.cs (InstaLODUnity)
 *
 * Copyright © 2018 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODGroupWindow.cs
 * @copyright 2017-2018 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace InstaLOD
{
	using UnityEditor;

	/// <summary>
	/// The InstaLODGroupWindow editor window provides convenient access 
	/// to manage a game object's LOD hierarchy.
	/// </summary>
	[System.Serializable]
	public class InstaLODGroupWindow : InstaLODEditorWindow
	{
		public const String kInstaLODDefaultSavePathEnabled = "kInstaLODDefaultSavePathEnabled";

		protected enum WindowTabType
		{
			LODGroup,
			License
		}

		[MenuItem("Window/InstaLOD")]
		static void Init()
		{
			GetWindow<InstaLODGroupWindow>();
		}

		/// <summary>
		/// Notify this instance.
		/// </summary>
		/// <returns>The notify.</returns>
		/// <param name="message">Message.</param>
		public override void Notify(string message)
		{
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
		/// Gets the name of the LOD entry at the specified LOD index.
		/// </summary>
		/// <returns>The LOD Name for index.</returns>
		/// <param name="i">The index.</param>
		public static string GetLODNameForIndex(int i)
		{
			return string.Concat("LOD ", i);
		}

		void OnEnable()
		{
			minSize = new Vector2(400, 620);
			_currentTab = WindowTabType.LODGroup;
			_mainComboBoxText = " LODGroup Settings ";

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

		void OnDisable()
		{
		}

		void OnSelectionChange()
		{
			Repaint();
		}

		void OnDestroy()
		{
			// NOTE: always respawn the window if an async operation is in progress
			// this is necessary as the main thread events are handled in our windows
			if (InstaLODNative.currentMeshOperationState != null)
			{
				InstaLODGroupWindow newWin = Instantiate<InstaLODGroupWindow>(this);
				newWin.Show();
			}
		}

		void OnClickCreateLODEntry(object data)
		{
			Dictionary<string, object> menuData = data as Dictionary<string, object>;

			if (menuData == null)
				throw new NullReferenceException("menuData");
			
			CreateLODGroupEntryAtIndex(menuData["hierarchy"] as InstaLODHierarchy, (int)menuData["lodIndex"]);
		}

		void OnGuiForHierarchyListItem(InstaLODHierarchy hierarchy, InstaLODHierarchyItem item, GUIStyle style, GUIStyle errorStyle, bool foldout = false, string alternateText = "", string alternateWarning = "")
		{
			bool isSelected = hierarchy.selected == item;

			GUIContent content = new GUIContent();
			content.text = !string.IsNullOrEmpty(alternateText) || item == null ? alternateText : item.gameObject.name;
			content.tooltip = !string.IsNullOrEmpty(alternateWarning) || item == null ? alternateWarning : item.warnings;

			if (foldout == true)
			{
				content.image = EditorStyles.foldout.onNormal.background;
			}

			if (content.tooltip != null)
			{
				content.image = InstaLODStyle.alertIcon;
			}

			if (item == null)
			{
				GUILayout.Toggle(false, content, errorStyle);
				return;
			}

			if (GUILayout.Toggle(isSelected, content, item.warnings != null ? errorStyle : style) == true && isSelected == false)
			{
				hierarchy.selected = item;
				EditorGUIUtility.PingObject(item.gameObject);
				Selection.activeGameObject = item.gameObject;
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

			// draw logo
			{
				GUILayout.Space(10.0f);
				GUILayout.Label(InstaLODStyle.logoForCurrentStyleType, GUIStyle.none);
				GUILayout.Space(5.0f);
			}

			InstaLODHierarchy hierarchy = new InstaLODHierarchy(Selection.activeGameObject);
			bool isHierarchyValid = hierarchy != null && hierarchy.IsValid();
			int selectedGameObjectCount = Selection.gameObjects.Length;

			using (new InstaLODStyle.VerticalLayout(10))
			{
				if (!InstaLODNative.IsAuthorizedCached())
				{
					bool value = true;
					InstaLODStyle.CollapsiblePanel("Authorize", ref value, delegate()
						{
							InstaLODWindowShared.OnGUIForNotAuthorized();
						});
					InstaLODWindowShared.OnGUIForFooter(this);
				}
				else
				{
					if (InstaLODNative.currentMeshOperationState != null)
					{
						InstaLODWindowShared.OnGUIForAsyncMeshOperationInProgress(this);
						InstaLODWindowShared.OnGUIForFooter(this);
						return;
					}

					_mainScrollViewPosition = EditorGUILayout.BeginScrollView(_mainScrollViewPosition);
					if (InstaLODStyle.MainComboBoxButton(_mainComboBoxText))
					{
						GenericMenu menu = new GenericMenu();
						menu.AddItem(new GUIContent("LODGroup Settings"), false, () => { _currentTab = WindowTabType.LODGroup; _mainComboBoxText = " LODGroup Settings "; });
						menu.AddItem(new GUIContent("Setup"), false, () => { _currentTab = WindowTabType.License; _mainComboBoxText = " Setup "; });
						menu.ShowAsContext();
					}
					{
						if (_currentTab == WindowTabType.License)
						{
							InstaLODWindowShared.OnGUIForAbout(this);
						}
						else
						{
							if (selectedGameObjectCount > 1)
							{
								InstaLODWindowShared.HelpBox("Multiple game objects selected.\n\nTo manage per game object LOD, selected only a single game object that has a 'MeshFilter' or 'SkinnedMeshRenderer' component attached.\n", MessageType.Warning);
							}
							else if (!isHierarchyValid)
							{
								// nothing selected
								if (Selection.activeGameObject == null)
								{
									InstaLODWindowShared.HelpBox("No game object selected.", MessageType.Info);
								}
								else
								{
									if (InstaLODHierarchy.IsGameObjectImmutablePrefab(Selection.activeGameObject))
									{
										InstaLODWindowShared.HelpBox("Please setup LOD before creating the prefab or drag the prefab into the viewport and setup the LOD and update the original prefab.\n" +
											"However, existing LOD Group entries can be modified by selecting the original prefab.", MessageType.Warning);
									}

									// find any components with LOD groups
									LODGroup[] lodGroups = Selection.activeGameObject.GetComponentsInChildren<LODGroup>();

									if (InstaLODHierarchy.IsGameObjectCompatibleForLOD(Selection.activeGameObject))
									{
										InstaLODWindowShared.HelpBox("The selected game object does not have a 'LOD Group' component attached.\n" +
											"\n" +
											"A 'LOD Group' component is required to create and render LOD in Unity.\n" +
											"Click the button below to automatically add a 'LOD Group' component to the selected game object.", MessageType.Info);

										if (GUILayout.Button(string.Format("Add 'LOD Group' component to '{0}'", Selection.activeGameObject.name)))
										{
											Selection.activeGameObject.AddComponent<LODGroup>();
										}
									}
									else
									{
										InstaLODWindowShared.HelpBox("The selected game object does not have a 'MeshFilter' or 'SkinnedMeshRenderer' component attached.\n", MessageType.Warning);
									}

									if (lodGroups.Length > 0)
									{
										InstaLODWindowShared.HelpBox("'LOD Group' components have been found that are attached to children of the selected game object.\n" +
											"Please select a gameobject with a 'LOD Group' below.", MessageType.Info);

										EditorGUILayout.BeginVertical();
										foreach (LODGroup lodGroup in lodGroups)
										{ 
											if (GUILayout.Button(string.Format("Select '{0}'", lodGroup.gameObject.name)))
											{
												EditorGUIUtility.PingObject(lodGroup.gameObject);
												Selection.activeGameObject = lodGroup.gameObject;
											}
										}
										EditorGUILayout.EndVertical();
									}
								}
							}
							else
							{
								OnGUIForHierarchy(hierarchy);
							}
						}
					}

					InstaLODWindowShared.OnGUIForFooter(this);
					EditorGUILayout.EndScrollView();
				}
			}
		}

		void OnGUIForHierarchy(InstaLODHierarchy hierarchy)
		{
			if (hierarchy == null)
				return;
			
			bool isImmutablePrefab = hierarchy.IsImmutablePrefab();
			bool isEditablePrefab = hierarchy.IsEditablePrefab();

			// LOD Toolbar
			InstaLODStyle.CollapsiblePanel("LOD Group Settings", ref _hierarchyUIState_LODGroup, delegate()
				{
					if (isImmutablePrefab)
					{
						InstaLODWindowShared.HelpBox("Please setup LOD before creating the prefab or drag the prefab into the viewport and setup the LOD and update the original prefab.\n" +
							"In case of an imported 3D Model please unpack the model first.\n" +
							"However, existing LOD Group entries can be modified by selecting the original prefab.", MessageType.Warning);
					}
					else if (isEditablePrefab)
					{
						InstaLODWindowShared.HelpBox("Prefab LOD Group entries cannot be created or removed, as game objects cannot be added to prefabs.\n" +
							"To modify the LOD Group entries, drag the prefab into the viewport then setup the LOD and update the original prefab.\n" +
							"However, existing LOD Group entries can be modified.", MessageType.Warning);
					}

					// NOTE: this would draw the lod group component editor UI
					// but it doesn't seem to work properly
					#if false 
					{
					LODGroup group = hierarchy.main.gameObject.GetComponent<LODGroup>();
					var editor = Editor.CreateEditor(group);
					editor.OnInspectorGUI();     
					}
					#endif

					GUILayout.Space(5.0f);

					GUI.enabled = hierarchy != null && hierarchy.selected != hierarchy.main && !isImmutablePrefab && !isEditablePrefab;
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Delete LODGroup entry"))
					{
						DeleteSelectedLODEntry(hierarchy);
					}

					GUI.enabled = hierarchy.IsLODEntryMissing() && !isImmutablePrefab && !isEditablePrefab;
					if (GUILayout.Button("Create missing LODGroup entry"))
					{
						GenericMenu menu = new GenericMenu();

						for (int i = 1; i < hierarchy.numLODs; i++)
						{
							InstaLODHierarchyItem hierarchyItem = hierarchy.items.Find(item => item.lod == i);

							if (hierarchyItem != null)
								continue;

							Dictionary<string, object> data = new Dictionary<string, object>();
							data["hierarchy"] = hierarchy;
							data["lodIndex"] = i;
							menu.AddItem(new GUIContent(GetLODNameForIndex(i)), false, OnClickCreateLODEntry, data);
						}

						menu.DropDown(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0, 0));
						EditorGUIUtility.ExitGUI();
					}
					GUI.enabled = !isImmutablePrefab;

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(5.0f);

					GUI.enabled = true;
					// LOD Hierarchy
					_lodHierarchyScrollPosition = GUILayout.BeginScrollView(_lodHierarchyScrollPosition, false, false, GUILayout.MinHeight(100));

					if (hierarchy != null && hierarchy.main.gameObject != null)
					{
						OnGuiForHierarchyListItem(hierarchy, hierarchy.main, InstaLODStyle.listItemStyle, InstaLODStyle.listItemStyleWarning, true);

						for (int i = 1; i < hierarchy.numLODs; i++)
						{
							InstaLODHierarchyItem hierarchyItem = hierarchy.items.Find(item => item.lod == i);

							if (hierarchyItem != null)
							{
								OnGuiForHierarchyListItem(hierarchy, hierarchyItem, InstaLODStyle.listItemStyleIndentend, InstaLODStyle.listItemStyleIndentendWarning);
							}
							else
							{
								OnGuiForHierarchyListItem(hierarchy, null, InstaLODStyle.listItemStyleIndentend, InstaLODStyle.listItemStyleIndentendWarning, false, GetLODNameForIndex(i) + " missing", "Missing LOD");
							}
						}
					}

					GUILayout.EndScrollView();
				});

			if (hierarchy.IsValid() == true)
			{
				InstaLODSettingsComponent.SettingsContainer lodSettings = new InstaLODSettingsComponent.SettingsContainer(true);
				int lodIndex = 0;

				// Take saved settings from hierarchy context.
				if (hierarchy != null && hierarchy.selected != null && hierarchy.selected.gameObject != null)
				{
					lodSettings = hierarchy.selected.lodSettingsComponent.settings;
					lodIndex = hierarchy.selected.lod;
				} 

				GUI.enabled = !isImmutablePrefab;
				{
					InstaLODStyle.CollapsiblePanel(string.Format("LOD {0} Settings", lodIndex), ref _hierarchyUIState_LODSettingsroup, delegate()
						{
							lodSettings.operationType = (InstaLODSettingsComponent.OperationType)EditorGUILayout.EnumPopup(new GUIContent("Mesh Operation", "The mesh operation used to create the LOD mesh."), lodSettings.operationType);
						});

					switch (lodSettings.operationType)
					{
						case InstaLODSettingsComponent.OperationType.Optimize:
							InstaLODWindowShared.OnGUIForOptimizeSettings(this, ref lodSettings.optimize, hierarchy.selected.skinnedRenderer != null);
							break;
						case InstaLODSettingsComponent.OperationType.Remesh:
							InstaLODWindowShared.OnGUIForRemeshSettings(this, ref lodSettings.remeshing);
							break;
						case InstaLODSettingsComponent.OperationType.Imposterize:
							InstaLODWindowShared.OnGUIForImposterizeSettings(this, ref lodSettings.imposterize);
							break;
						default:
							break;
					}
				}

				// copy back
				if (hierarchy != null && hierarchy.selected != null && hierarchy.selected.gameObject != null)
				{
					if (hierarchy.selected.lodSettingsComponent.settings.Equals(lodSettings) == false)
					{
						hierarchy.selected.lodSettingsComponent.settings = lodSettings;
						EditorUtility.SetDirty(hierarchy.selected.gameObject);
						EditorSceneManager.MarkAllScenesDirty();
					}
				}

				InstaLODStyle.CollapsiblePanel("Operation Settings", ref _operationSettingsState, delegate ()
				{
					bool wasGUIEnabled = GUI.enabled;
					bool useDefaultSavePath = EditorPrefs.GetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, false);
					bool useDefaultSavePathEdited = EditorGUILayout.Toggle(new GUIContent("Create Save File Path", "Creates a save path automatically."), useDefaultSavePath);

					if (useDefaultSavePathEdited != useDefaultSavePath)
					{
						EditorPrefs.SetBool(InstaLODGroupWindow.kInstaLODDefaultSavePathEnabled, useDefaultSavePathEdited);
					}
				});

				// Display warning, that main mesh is beeing changed.
				GUIContent btnContent = new GUIContent(string.Format("Rebuild LOD {0}", lodIndex));
				if (hierarchy.selected == hierarchy.main)
				{
					btnContent.image = InstaLODStyle.alertIcon;
					btnContent.tooltip = "If the base mesh is optimized, all LODs will be optimized based on this mesh. Reverting the base mesh is only possible via reimport!";
				}

				if (InstaLODStyle.PrimaryButton(btnContent))
				{
					ExecuteLODEntryMeshOperationAsync(hierarchy, hierarchy.selected);
				}
			}
		}

		/// <summary>
		/// Executes the mesh operation in asynchronous fashion.
		/// </summary>
		/// <param name="hierarchy">Hierarchy.</param>
		/// <param name="item">Item.</param>
		public void ExecuteLODEntryMeshOperationAsync(InstaLODHierarchy hierarchy, InstaLODHierarchyItem item)
		{
			if (hierarchy == null || !hierarchy.IsValid())
				return;

			if (!hierarchy.CopyMeshAttributesFromHierarchyMain(item))
				return;

			InstaLODNativeMeshOperationSettings meshOperationSettings = new InstaLODNativeMeshOperationSettings()
			{
				combineOutputMeshes = false,
				hideSourceGameObjects = false,
				skinnedMeshEnforceBindPose = false
			};

			switch (hierarchy.selected.lodSettingsComponent.settings.operationType)
			{
				case InstaLODSettingsComponent.OperationType.Optimize:
					InstaLODNative.Optimize(new List<Renderer>() { hierarchy.selected.renderer }, hierarchy.selected.lodSettingsComponent.settings.optimize, meshOperationSettings);
					break;
				case InstaLODSettingsComponent.OperationType.Imposterize:
					InstaLODNative.Imposterize(new List<Renderer>() { hierarchy.selected.renderer }, new Camera(), hierarchy.selected.lodSettingsComponent.settings.imposterize, meshOperationSettings);
					break;
				case InstaLODSettingsComponent.OperationType.Remesh:
					InstaLODNative.Remesh(new List<Renderer>() { hierarchy.selected.renderer }, hierarchy.selected.lodSettingsComponent.settings.remeshing, meshOperationSettings);
					break;
			}
		}

		/// <summary>
		/// Deletes the selected LOD entry.
		/// </summary>
		/// <returns><c>true</c>, if selected LOD entry was deleted, <c>false</c> otherwise.</returns>
		/// <param name="hierarchy">Hierarchy.</param>
		bool DeleteSelectedLODEntry(InstaLODHierarchy hierarchy)
		{
			if (!hierarchy.IsValid())
				return false;
			
			if (hierarchy.selected == hierarchy.main)
				return false;
		
			// find previous item in list to select after deletion
			InstaLODHierarchyItem last = hierarchy.main;
			foreach (InstaLODHierarchyItem item in hierarchy.items)
			{
				if (item == hierarchy.selected)
				{
					hierarchy.items.Remove(item);
					break;
				}

				last = item;
			}

			Selection.activeGameObject = last.gameObject;
			DestroyImmediate(hierarchy.selected.gameObject);
			return true;
		}

		/// <summary>
		/// Creates the game object and template mesh data for the LOD group at the specified index.
		/// </summary>
		/// <param name="hierarchy">Hierarchy.</param>
		/// <param name="lodIndex">Lod index.</param>
		void CreateLODGroupEntryAtIndex(InstaLODHierarchy hierarchy, int lodIndex = 1)
		{
			if (!hierarchy.IsValid())
				return;
			
			// create new game object and parent it to main
			GameObject newGameObject = new GameObject();
			newGameObject.name = hierarchy.main.gameObject.name + "_LOD_" + lodIndex;
			newGameObject.transform.SetParent(hierarchy.main.gameObject.transform, false);

			// copy mesh data
			hierarchy.CopyMeshAttributesFromHierarchyMain(new InstaLODHierarchyItem() { gameObject = newGameObject, lod = lodIndex });
	
			// select it
			EditorGUIUtility.PingObject(newGameObject);
			Selection.activeGameObject = newGameObject;

			// rebuilt the hierarchy and the actual InstaLODHierarchyItem instance
			hierarchy.Update(Selection.activeGameObject);

			InstaLODHierarchyItem newItem = hierarchy.GetItemForLODIndex(lodIndex);

			if (newItem == null)
				throw new Exception("Rebuild hierarchy with new lod group entry, but newItem is null");
			
			// check if there is a parent lod index that we can use to copy settings from
			int parentLODIndex = lodIndex - 1;
			while (parentLODIndex >= 0)
			{
				// grab the previous lod entry
				InstaLODHierarchyItem item = hierarchy.GetItemForLODIndex(parentLODIndex);

				if (item != null)
				{
					newItem.lodSettingsComponent.settings.optimize.PercentTriangles = item.lodSettingsComponent.settings.optimize.PercentTriangles / 2;
					newItem.lodSettingsComponent.settings.optimize.AbsoluteTriangles = item.lodSettingsComponent.settings.optimize.AbsoluteTriangles / 2;
					newItem.lodSettingsComponent.settings.optimize.ScreenSizeInPixels = item.lodSettingsComponent.settings.optimize.ScreenSizeInPixels / 2;
					break;
				}
				parentLODIndex--;
			}

			ExecuteLODEntryMeshOperationAsync(hierarchy, hierarchy.selected);
		}

		protected Vector2 _lodHierarchyScrollPosition;
		protected WindowTabType _currentTab;
		protected Vector2 _mainScrollViewPosition;
		protected bool _hierarchyUIState_LODGroup = true;
		protected bool _hierarchyUIState_LODSettingsroup = true;
		protected bool _operationSettingsState = true;
		protected string _mainComboBoxText;
	}
}