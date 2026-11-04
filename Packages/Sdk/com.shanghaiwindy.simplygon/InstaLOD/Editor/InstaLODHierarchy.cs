/**
 * InstaLODHierarchy.cs (InstaLODUnity)
 *
 * Copyright © 2018 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODHierarchy.cs
 * @copyright 2017-2018 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODHierarchy class manages the LOD hierarchy for the currently selected game object.
	/// </summary>
	[System.Serializable]
	public class InstaLODHierarchy
	{
		public InstaLODHierarchyItem main;
		public LODGroup mainLODGroup;
		public List<InstaLODHierarchyItem> items;
		public InstaLODHierarchyItem selected;
		public int numLODs;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODHierarchy"/> class.
		/// </summary>
		private InstaLODHierarchy()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODHierarchy"/> class.
		/// </summary>
		/// <param name="gameObject">Game object.</param>
		public InstaLODHierarchy(GameObject gameObject)
		{
			main = new InstaLODHierarchyItem();
			mainLODGroup = null;
			items = new List<InstaLODHierarchyItem>();
			selected = null;
			numLODs = -1;

			Update(gameObject);
		}

		/// <summary>
		/// Gets the item for LOD Index.
		/// </summary>
		/// <returns>The item for LOD Index.</returns>
		/// <param name="lodIndex">Lod index.</param>
		public InstaLODHierarchyItem GetItemForLODIndex(int lodIndex)
		{
			if (lodIndex == 0)
				return main;

			return items.Find(item => item.lod == lodIndex);
		}

		/// <summary>
		/// Validates the main entry.
		/// </summary>
		/// <returns><c>true</c>, if main was validated, <c>false</c> otherwise.</returns>
		public bool ValidateMain()
		{
			bool valid = false;
			if (main == null)
				return false;

			valid = main.Validate();

			if (valid == false)
				return false;

			if (mainLODGroup == null)
			{
				main.warnings = "Missing 'LODGroup' component on LOD0.";
				return false;
			}

			return valid;
		}

		/// <summary>
		/// Determines if the game object is compatible for lod.
		/// </summary>
		/// <returns><c>true</c>, if game object is compatible for lod, <c>false</c> otherwise.</returns>
		/// <param name="theGameObject">The game object.</param>
		public static bool IsGameObjectCompatibleForLOD(GameObject theGameObject)
		{
#if UNITY_2019_1_OR_NEWER
			return ((theGameObject.GetComponents<MeshFilter>().Length > 0 && theGameObject.GetComponents<MeshRenderer>().Length > 0) ||
				theGameObject.GetComponents<SkinnedMeshRenderer>().Length > 0) && !IsGameObjectImmutablePrefab(theGameObject);
#else
			return (theGameObject.GetComponents<MeshFilter>().Length > 0 && theGameObject.GetComponents<MeshRenderer>().Length > 0) ||
				theGameObject.GetComponents<SkinnedMeshRenderer>().Length > 0;
#endif
		}

		/// <summary>
		/// Determines if the game object is an editable prefab
		/// </summary>
		/// <returns><c>true</c>, if game object is an editable prefab, <c>false</c> otherwise.</returns>
		/// <param name="theGameObject">The game object.</param>
		public static bool IsGameObjectEditablePrefab(GameObject theGameObject)
		{
#if UNITY_2019_1_OR_NEWER

			PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(theGameObject);
			
			return prefabAssetType == PrefabAssetType.Regular;
#else
			var prefabType = PrefabUtility.GetPrefabType(theGameObject);

			if (prefabType == PrefabType.Prefab)
				return true;
			return false;
#endif
		}

		/// <summary>
		/// Determines if the game object is an immutable prefab
		/// </summary>
		/// <returns><c>true</c>, if game object is an immutable prefab, <c>false</c> otherwise.</returns>
		/// <param name="theGameObject">The game object.</param>
		public static bool IsGameObjectImmutablePrefab(GameObject theGameObject)
		{
#if UNITY_2019_1_OR_NEWER
			return PrefabUtility.IsPartOfImmutablePrefab(theGameObject);
#else

			var prefabType = PrefabUtility.GetPrefabType(theGameObject);

			if (prefabType == PrefabType.ModelPrefab)
				return true;

			if (prefabType == PrefabType.PrefabInstance)
				return true;

			return false;
#endif
		}

		/// <summary>
		/// Determines if this instance is valid.
		/// </summary>
		/// <returns><c>true</c>, if this instance is valid, <c>false</c> otherwise.</returns>
		public bool IsValid()
		{
			return this.main != null && this.main.gameObject != null && this.mainLODGroup != null;
		}

		/// <summary>
		/// Determines if this instance is an editable prefab.
		/// </summary>
		/// <returns><c>true</c>, if this instance is an editable prefab, <c>false</c> otherwise.</returns>
		public bool IsEditablePrefab()
		{
			if (!this.IsValid())
				return false;

			return IsGameObjectEditablePrefab(this.main.gameObject);
		}

		/// <summary>
		/// Determines if this instance is an immutable prefab.
		/// </summary>
		/// <returns><c>true</c>, if this instance is an immutable prefab, <c>false</c> otherwise.</returns>
		public bool IsImmutablePrefab()
		{
			if (!this.IsValid())
				return false;

			return IsGameObjectImmutablePrefab(this.main.gameObject);
		}

		/// <summary>
		/// Determines if an LOD entry is missing
		/// </summary>
		/// <returns><c>true</c>, if an LOD entry is missing, <c>false</c> otherwise.</returns>
		public bool IsLODEntryMissing()
		{
			bool missingLOD = false;
			for (int i = 1; i < this.numLODs; i++)
			{
				InstaLODHierarchyItem hierarchyItem = this.items.Find(item => item.lod == i);

				if (hierarchyItem != null)
					continue;

				missingLOD = true;
				break;
			}
			return missingLOD;
		}

		/// <summary>
		/// Copies the mesh attributes from hierarchy main.
		/// </summary>
		/// <returns><c>true</c>, if mesh attributes from hierarchy main was copyed, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		public bool CopyMeshAttributesFromHierarchyMain(InstaLODHierarchyItem item)
		{
			if (!this.IsValid())
				return false;

			if (this.main.skinnedRenderer != null)
			{
				// remove previous renderer
				if (item.meshFilter)
				{
					UnityEngine.Object.DestroyImmediate(item.meshFilter);
				}

				if (item.meshRenderer)
				{
					UnityEngine.Object.DestroyImmediate(item.meshRenderer);
				}

				SkinnedMeshRenderer mainSkinnedRenderer = this.main.skinnedRenderer;

				SkinnedMeshRenderer renderer = item.skinnedRenderer;
				if (renderer == null)
				{
					renderer = item.gameObject.AddComponent<SkinnedMeshRenderer>();
				}

				renderer.sharedMesh = mainSkinnedRenderer.sharedMesh;
				renderer.sharedMaterials = mainSkinnedRenderer.sharedMaterials;
				renderer.shadowCastingMode = mainSkinnedRenderer.shadowCastingMode;
				renderer.receiveShadows = mainSkinnedRenderer.receiveShadows;
				renderer.probeAnchor = mainSkinnedRenderer.probeAnchor;
				renderer.updateWhenOffscreen = mainSkinnedRenderer.updateWhenOffscreen;
				renderer.localBounds = mainSkinnedRenderer.localBounds;
				renderer.quality = mainSkinnedRenderer.quality;
				renderer.rootBone = mainSkinnedRenderer.rootBone;
				renderer.bones = mainSkinnedRenderer.bones;
				renderer.skinnedMotionVectors = mainSkinnedRenderer.skinnedMotionVectors;
				renderer.updateWhenOffscreen = mainSkinnedRenderer.updateWhenOffscreen;
				renderer.localBounds = mainSkinnedRenderer.localBounds;
			}
			else
			{
				// remove previous renderer
				if (item.skinnedRenderer)
				{
					UnityEngine.Object.DestroyImmediate(item.skinnedRenderer);
				}

				MeshRenderer mainMeshRenderer = this.main.meshRenderer;
				MeshFilter mainMeshFilter = this.main.meshFilter;
				MeshFilter filter = item.meshFilter;

				if (filter == null)
				{
					filter = item.gameObject.AddComponent<MeshFilter>();
				}

				filter.sharedMesh = mainMeshFilter.sharedMesh;
				MeshRenderer renderer = item.meshRenderer;

				if (renderer == null)
				{
					renderer = item.gameObject.AddComponent<MeshRenderer>();
				}

				renderer.sharedMaterials = mainMeshRenderer.sharedMaterials;
				renderer.receiveShadows = mainMeshRenderer.receiveShadows;
				renderer.shadowCastingMode = mainMeshRenderer.shadowCastingMode;
				renderer.lightProbeUsage = mainMeshRenderer.lightProbeUsage;
				renderer.reflectionProbeUsage = mainMeshRenderer.reflectionProbeUsage;
				renderer.probeAnchor = mainMeshRenderer.probeAnchor;
			}

			int lodIndex = item.lod;

			// update to lodgroup
			LOD[] lods = this.mainLODGroup.GetLODs();
			bool didChangeLODs = false;

			if (lods[0].renderers == null || lods[0].renderers.Length == 0)
			{
				lods[0].renderers = new Renderer[] { this.main.renderer };
				didChangeLODs = true;
			}

			if (lods[lodIndex].renderers == null || lods[lodIndex].renderers.Length == 0 || lods[lodIndex].renderers[0] != item.renderer)
			{
				lods[lodIndex].renderers = new Renderer[] { item.renderer };
				didChangeLODs = true;
			}

			if (didChangeLODs)
				this.mainLODGroup.SetLODs(lods);

			return true;
		}

		/// <summary>
		/// Update the specified gameObject.
		/// </summary>
		/// <returns>The true upon success.</returns>
		/// <param name="selectedGameObject">The game object.</param>
		public bool Update(GameObject selectedGameObject)
		{
			if (selectedGameObject == null)
				return false;

			GameObject mainGameObject = selectedGameObject;
			LODGroup lodGroupComponent = null;

			// check if this gameObject is the main LODGroup or if it is a LOD mesh below
			{
				Transform transform = selectedGameObject.transform;
				while (transform != null)
				{
					lodGroupComponent = transform.GetComponent<LODGroup>();

					// main object was found
					if (lodGroupComponent != null)
					{
						mainGameObject = transform.gameObject;
						break;
					}

					// get parent transform
					transform = transform.parent;
				}
			}


			// set properties
			this.main.gameObject = mainGameObject;
			this.mainLODGroup = lodGroupComponent;
			this.selected = this.main;
			this.ValidateMain();

			if (lodGroupComponent != null)
			{
				LOD[] lods = lodGroupComponent.GetLODs();

				// set properties
				this.numLODs = lods.Length;

				for (int i = 1; i < lods.Length; i++)
				{
					// NOTE: we should probably only look at .renderer[0] to be in sync with other code
					foreach (Renderer renderer in lods[i].renderers)
					{
						if (renderer == null)
							continue;

						InstaLODHierarchyItem item = null;

						// find existing entry
						foreach (InstaLODHierarchyItem runItem in this.items)
						{
							if (renderer.gameObject == runItem.gameObject)
							{
								item = runItem;
								break;
							}
						}

						// if none found, create one
						if (item == null)
						{
							item = new InstaLODHierarchyItem();
							this.items.Add(item);
						}

						// set properties
						item.gameObject = renderer.gameObject;
						item.lod = i;
						item.Validate();

						if (selectedGameObject == item.gameObject)
						{
							this.selected = item;
						}
					}
				}
			}

			return true;
		}
	}

}