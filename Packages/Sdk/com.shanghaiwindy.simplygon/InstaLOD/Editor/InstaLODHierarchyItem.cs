/**
 * InstaLODHierarchyItem.cs (InstaLODUnity)
 *
 * Copyright © 2018 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODHierarchyItem.cs
 * @copyright 2017-2018 InstaLOD GmbH. All rights reserved.
 * @section License
 */
 
using UnityEngine;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODHierarchyItem class represents an item in a game object's LOD hierarchy.
	/// </summary>
	[System.Serializable]
	public class InstaLODHierarchyItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODHierarchyItem"/> class.
		/// </summary>
		public InstaLODHierarchyItem()
		{
			lod = 0;
			warnings = null;
		}
		public int lod;
		public string warnings;
		public GameObject gameObject;
		public SkinnedMeshRenderer skinnedRenderer
		{
			get
			{
				if (gameObject == null)
					return null;
				return gameObject.GetComponent<SkinnedMeshRenderer>();
			}
		}
		public MeshRenderer meshRenderer
		{
			get
			{
				if (gameObject == null)
					return null;
				return gameObject.GetComponent<MeshRenderer>();
			}
		}
		public MeshFilter meshFilter
		{
			get
			{
				if (gameObject == null)
					return null;
				return gameObject.GetComponent<MeshFilter>();
			}
		}
		public InstaLODSettingsComponent lodSettingsComponent
		{
			get
			{
				if (gameObject == null)
					return null;

				InstaLODSettingsComponent component = gameObject.GetComponent<InstaLODSettingsComponent>();

				if (component == null)
					return gameObject.AddComponent<InstaLODSettingsComponent>();

				return component;
			}
		}
		public Renderer renderer
		{
			get
			{
				if (meshRenderer != null)
					return meshRenderer;
					
				return skinnedRenderer;
			}
		}

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <returns>The validate.</returns>
		public bool Validate()
		{
			bool valid = (skinnedRenderer != null || (meshRenderer != null && meshFilter != null));

			warnings = null;

			if (valid == false)
				warnings = "Game objects that are part of a 'LOD Group' must have either a 'SkinnedMeshRenderer' component or  both a 'MeshRenderer' and a 'MeshFilter' component";

			return valid;
		}
	}
}