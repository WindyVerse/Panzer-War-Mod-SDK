/**
 * InstaLODSettingsComponent.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODSettingsComponent.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using InstaLOD;

[System.Serializable]
public class InstaLODSettingsComponent : MonoBehaviour
{
	public enum OperationType
	{
		Optimize,
		Remesh,
		Imposterize
	}

	[System.Serializable]
	public struct SettingsContainer
	{
		public SettingsContainer(bool defaults)
		{
			optimize = new InstaLODOptimizeSettings(/*percentTriangles:*/1.0f);
			remeshing = new InstaLODRemeshingSettings(/*width:*/2048u, /*height:*/2048u);
			imposterize = new InstaLODImposterizeSettings(/*width:*/2048u, /*height:*/2048u);
			operationType = OperationType.Optimize;
		}

		public InstaLODOptimizeSettings optimize;
		public InstaLODRemeshingSettings remeshing;
		public InstaLODImposterizeSettings imposterize;
		public OperationType operationType; 
	}

	[HideInInspector]
	public SettingsContainer settings = new SettingsContainer(true);
}