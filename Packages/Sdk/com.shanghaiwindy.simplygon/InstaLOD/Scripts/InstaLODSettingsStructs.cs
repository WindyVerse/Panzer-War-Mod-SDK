/**
 * InstaLODSettingsStructs.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODSettingsStructs.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InstaLOD
{
	public enum MeshFeatureImportance
	{
		Off = 0,
		Lowest = 1,
		Low = 2,
		Normal = 3,
		High = 4,
		Highest = 5
	};

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct NativeBool
	{
		NativeBool(bool value)
		{
			_value = value ? (byte)1 : (byte)0;
		}

		private byte _value;
		public bool Value
		{
			get { return _value != 0; }
			set { _value = value ? (byte)1 : (byte)0; }
		}

		public static implicit operator NativeBool(bool b)
		{
			return new NativeBool(b);
		}

		static public implicit operator bool(NativeBool binary)
		{
			return (binary.Value);
		}
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODSkeletonOptimizeSettings
	{
		public InstaLODSkeletonOptimizeSettings(bool defaults)
		{
			LeafBoneWeldDistance = (0.0f);
			MaximumBoneDepth = (0);
			MaximumBoneInfluencesPerVertex = (4);
			MinimumBoneInfluenceThreshold = (0.0f);
			IgnoreJointIndices = IntPtr.Zero;
			IgnoreJointIndicesCount = (0);
		}

		public float LeafBoneWeldDistance;
		public UInt32 MaximumBoneDepth;
		public UInt32 MaximumBoneInfluencesPerVertex;
		public float MinimumBoneInfluenceThreshold;
		public IntPtr IgnoreJointIndices;
		public UInt32 IgnoreJointIndicesCount;
	};

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOptimizeSettings
	{
		public MeshFeatureImportance AutomaticQuality;
		public float PercentTriangles;
		public UInt32 AbsoluteTriangles;
		public float MaxDeviation;
		public float WeldingThreshold;
		public UInt32 ScreenSizeInPixels;
		public NativeBool OptimizerVertexWeights;
		public NativeBool RecalculateNormals;
		public NativeBool LockBoundaries;
		public NativeBool LockSplits;
		public NativeBool ProtectBoundaries;
		public NativeBool ProtectSplits;
		public NativeBool NormalizeMeshScale;
		public NativeBool OptimalPlacement;
		public NativeBool WeldingProtectDistinctUVShells;
		public NativeBool Deterministic;
		public float HardAngleThreshold;

		public MeshFeatureImportance SilhouetteImportance;
		public MeshFeatureImportance TextureImportance;
		public MeshFeatureImportance ShadingImportance;
		public MeshFeatureImportance SkinningImportance;
		public MeshFeatureImportance BoundaryImportance;

		public InstaLODSkeletonOptimizeSettings SkeletonSettings;

		public InstaLODOptimizeSettings(float percentTriangles)
		{
			AutomaticQuality = MeshFeatureImportance.Off;
			PercentTriangles = 1.0f;
			AbsoluteTriangles = 0;
			MaxDeviation = 0f;
			WeldingThreshold = 0f;
			ScreenSizeInPixels = 0;
			OptimizerVertexWeights = false;
			RecalculateNormals = false;
			LockBoundaries = false;
			LockSplits = false;
			ProtectBoundaries = true;
			ProtectSplits = true;
			OptimalPlacement = true;
			NormalizeMeshScale = false;
			WeldingProtectDistinctUVShells = true;
			Deterministic = false;
			HardAngleThreshold = 80f;

			SilhouetteImportance = MeshFeatureImportance.Normal;
			TextureImportance = MeshFeatureImportance.Normal;
			ShadingImportance = MeshFeatureImportance.Normal;
			SkinningImportance = MeshFeatureImportance.Normal;
			BoundaryImportance = MeshFeatureImportance.Normal;

			SkeletonSettings = new InstaLODSkeletonOptimizeSettings(true);
		}
	}

	public enum Resolution
	{
		_128 = 128,
		_256 = 256,
		_512 = 512,
		_1024 = 1024,
		_2048 = 2048,
		_4096 = 4096,
		_8k = 8192,
	}

	public enum SuperSampling
	{
		Off = 0,
		x2 = 1,
		x4 = 2
	}
	public enum ExportFormats
	{
		TGA8Dithered,
		PNG8Dithered,
		HDR,
		EXR16,
		JPEG
	}

	public enum UVPackShellRotation
	{
		None = 0, /**< Do not rotate shells. */
		Allow90 = 1, /**< Allow 90 degree shell rotations when packing. */
		Arbitrary = 2 /**< Allow arbitrary rotations when packing. */
	}

	public enum UVUnwrapStrategy
	{
		Organic = 0,            /**< Stretch based unwrapping suitable for organic type meshes. Computationally intensive for high poly meshes. */
		HardSurfaceAngle = 1,   /**< Angle-based projections, suitable for hard-surface type meshes. */
		HardSurfaceAxial = 2,   /**< Axial projections, suitable for hard-surface type meshes. */
		Auto = 3
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODMeshMergeSettings
	{
		public UInt32 GutterSizeInPixels;
		public SuperSampling SuperSampling;

		public UVPackShellRotation ShellRotation;

		public MeshFeatureImportance UVImportance;
		public MeshFeatureImportance GeometricImportance;
		public MeshFeatureImportance TextureImportance;
		public MeshFeatureImportance VisualImportance;

		public float ZeroAreaUVThreshold;
		public NativeBool GenerateZeroAreaUV;
		public NativeBool WorldspaceNormalizeShells;
		public NativeBool StackDuplicateShells;
		public NativeBool Deterministic;

		public InstaLODBakeOutputSettings BakeOutput;

		public InstaLODMeshMergeSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);
			GutterSizeInPixels = 5;
			SuperSampling = SuperSampling.x2;
			ShellRotation = UVPackShellRotation.Arbitrary;
			UVImportance = MeshFeatureImportance.Normal;
			GeometricImportance = MeshFeatureImportance.Normal;
			TextureImportance = MeshFeatureImportance.Normal;
			VisualImportance = MeshFeatureImportance.Normal;
			ZeroAreaUVThreshold = 0.0f;
			StackDuplicateShells = true;
			GenerateZeroAreaUV = false;
			WorldspaceNormalizeShells = true;
			Deterministic = false;
		}
	}

	public enum ImposterType
	{
		AABB = 0,
		Billboard = 1,
		HybridBillboardCloud = 2,
		Flipbook = 3,
		Vista = 4
	}



	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODImposterizeSettings
	{
		public ImposterType Type;
		public NativeBool TwoSidedQuads;

		public UInt32 QuadXYCount;
		public UInt32 QuadXZCount;
		public UInt32 QuadYZCount;

		public UInt32 CloudFaceCount;
		public float CloudPolyFaceFactor;
		public NativeBool EnableCloudQuadXY;
		public NativeBool EnableCloudQuadXZ;
		public NativeBool EnableCloudQuadYZ;
		public Vector3 CloudNormal;
		public float CloudNormalConformity;
		public UInt32 QuadSubdivisionsU;
		public UInt32 QuadSubdivisionsV;

		public float AABBDisplacement;

		public UInt32 FlipbookFramesPerAxis;

		public UInt32 GutterSizeInPixels;

		public NativeBool AlphaCutOut;
		public UInt32 AlphaCutOutResolution;
		public NativeBool AlphaCutOutSubdivide;
		public NativeBool Deterministic;

		public InstaLODBakeOutputSettings BakeOutput;

		public float VistaDistance;
		public NativeBool ParallelImposterPlane;

		public InstaLODImposterizeSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);
			// NOTE: we disable super sampling by default for imposters
			BakeOutput.SuperSampling = SuperSampling.Off;

			TwoSidedQuads = true;
			Type = ImposterType.Billboard;

			QuadXYCount = 1;
			QuadXZCount = 0;
			QuadYZCount = 1;

			QuadSubdivisionsU = 1;
			QuadSubdivisionsV = 1;

			CloudFaceCount = 700;
			CloudPolyFaceFactor = 0.5f;
			EnableCloudQuadXY = true;
			EnableCloudQuadXZ = false;
			EnableCloudQuadYZ = true;

			AABBDisplacement = 0.0f;
			CloudNormal = new Vector3(0.35f, 0.4f, 0.8f);
			CloudNormalConformity = 0.5f;

			FlipbookFramesPerAxis = 8;
			GutterSizeInPixels = 5;

			AlphaCutOut = false;
			AlphaCutOutResolution = 16;
			AlphaCutOutSubdivide = false;

			Deterministic = false;

			VistaDistance = 1.0f;
			ParallelImposterPlane = true;
		}
	}

	/**
	 * The RemeshFaceCountTarget enumeration specifies different types of fuzzy
	 * face count targets used by the IRemeshingOperation.
	 */
	public enum RemeshFaceCountTarget
	{
		Lowest = 0,
		Low = 1,
		Normal = 2,
		High = 3,
		Highest
	}

	public enum RemeshResolution
	{
		Lowest = 100,
		Low = 150,
		Normal = 256,
		High = 512,
		Highest = 1024
	}

	/**
	 * The RemeshSurfaceMode enumeration specifies different types of
	 * surface construction used by the IRemeshingOperation.
	 */
	public enum RemeshSurfaceMode
	{
		Reconstruct = 0,
		Optimize = 1,
		ConvexHull = 2, 
		UV = 3
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODBakeOutputSettings
	{
		public InstaLODBakeOutputSettings(UInt32 width, UInt32 height)
		{
			Width = width;
			Height = height;

			SourceMeshUVChannelIndex = 0;
			SolidifyTexturePages = true;
			SuperSampling = SuperSampling.x2;
			NormalizePositionByAABB = true;

			TexturePageNormalTangentSpace = true;
			TexturePageNormalObjectSpace = false;
			TexturePageMeshID = false;
			TexturePageVertexColor = false;
			TexturePageAmbientOcclusion = false;
			TexturePageBentNormals = false;
			TexturePageThickness = false;
			TexturePageDisplacement = false;
			TexturePagePosition = false;
			TexturePageCurvature = false;
			TexturePageTransfer = false;
			TexturePageReflectance = false;
			TexturePageOpacity = false;
			TexturePageCustom = true;
			ExportFormat = ExportFormats.PNG8Dithered;
			CompressionQuality = 100u;
		}

		public UInt32 Width;
		public UInt32 Height;

		public UInt32 SourceMeshUVChannelIndex;
		public NativeBool SolidifyTexturePages;
		public SuperSampling SuperSampling;
		public NativeBool NormalizePositionByAABB;

		public NativeBool TexturePageNormalTangentSpace;
		public NativeBool TexturePageNormalObjectSpace;
		public NativeBool TexturePageMeshID;
		public NativeBool TexturePageVertexColor;
		public NativeBool TexturePageAmbientOcclusion;
		public NativeBool TexturePageBentNormals;
		public NativeBool TexturePageThickness;
		public NativeBool TexturePageDisplacement;
		public NativeBool TexturePagePosition;
		public NativeBool TexturePageCurvature;
		public NativeBool TexturePageTransfer;
		public NativeBool TexturePageReflectance;
		public NativeBool TexturePageOpacity;
		public NativeBool TexturePageCustom;
		public ExportFormats ExportFormat;
		public UInt32 CompressionQuality;
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODRemeshingSettings
	{
		public InstaLODRemeshingSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);

			SurfaceMode = RemeshSurfaceMode.Reconstruct;
			SurfaceConstructionIgnoreBackface = false;
			Resolution = RemeshResolution.Normal;
			FaceCountTarget = RemeshFaceCountTarget.Normal;
			MaximumTriangles = 0;
			ScreenSizeInPixels = 0;
			ScreenSizePixelMergeDistance = 2;
			ScreenSizeInPixelsAutomaticTextureSize = false;
			HardAngleThreshold = 70.0f;
			WeldDistance = 0.0f;
			GutterSizeInPixels = 5;
			SurfaceModeOptimizeLockBoundaries = false;
			BakeAverageNormalsAsRayDirections = false;
			UnwrapStrategy = UVUnwrapStrategy.Auto;
			Deterministic = false;
		}

		public RemeshSurfaceMode SurfaceMode;
		public NativeBool SurfaceConstructionIgnoreBackface;
		public RemeshResolution Resolution;
		public RemeshFaceCountTarget FaceCountTarget;
		public UInt32 MaximumTriangles;
		public UInt32 ScreenSizeInPixels;
		public UInt32 ScreenSizePixelMergeDistance;
		public NativeBool ScreenSizeInPixelsAutomaticTextureSize;
		public float HardAngleThreshold;
		public float WeldDistance;
		public UInt32 GutterSizeInPixels;
		public NativeBool SurfaceModeOptimizeLockBoundaries;
		public NativeBool BakeAverageNormalsAsRayDirections;
		public UVUnwrapStrategy UnwrapStrategy;
		public NativeBool Deterministic;
		public InstaLODBakeOutputSettings BakeOutput;
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOcclusionCullCamera
	{
		public Vector3 Position;
		public Vector3 Right;
		public Vector3 Up;
		public Vector3 Forward;
		public float NearPlane;
		public float FarPlane;
		public float FieldOfViewInDegrees;
		public NativeBool IsOrthogonal;
		public float OrthogonalScale;
		public float Aspect;
	}

	/**
	 * The OcclusionCullingStrategy enum represents different types of strategies
	 * used by the occlusion operation.
	 */
	public enum OcclusionCullingStrategy
	{
		PerFace = 0,
		PerMesh = 1,
		PerVertexAdjacency = 2
	}

	/**
	 * The OcclusionCullDataUsage enum represents different ways on how
	 * the computed occlusion data can be used by the occlusion operation.
	 */
	public enum OcclusionCullDataUsage
	{
		RemoveGeometry = 0,
		WriteToVertexColors = 1,
		WriteOptimizerWeightsToVertexColors = 3
	}

	/**
	 * The OcclusionCullMode enum represents different ways on how
	 * the occlusion culling will operate.
	 */
	public enum OcclusionCullMode
	{
		AutomaticInterior = 0,
		CameraBased = 1
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOcclusionCullSettings
	{
		public InstaLODOcclusionCullSettings(UInt32 resolution)
		{
			Mode = (OcclusionCullMode.AutomaticInterior);
			CullingStrategy = (OcclusionCullingStrategy.PerFace);
			DataUsage = (OcclusionCullDataUsage.RemoveGeometry);
			AutomaticPrecision = (MeshFeatureImportance.Normal);
			Resolution = resolution;
			AdjacencyDepth = (5);
			SubMeshVisbilityFaceThreshold = (1);
			IgnoreBackface = (false);
			AlphaMaskThreshold = (0.5f);
			OptimizerWeight = (-0.8f);
			Deterministic = (false);
		}

		public OcclusionCullMode Mode;
		public OcclusionCullingStrategy CullingStrategy;
		public OcclusionCullDataUsage DataUsage;
		public MeshFeatureImportance AutomaticPrecision;
		public UInt32 Resolution;
		public UInt32 AdjacencyDepth;
		public UInt32 SubMeshVisbilityFaceThreshold;
		public NativeBool IgnoreBackface;
		public NativeBool Deterministic;
		public float AlphaMaskThreshold;
		public float OptimizerWeight;
	}

	/**
	 * The NormalHealingMode enum specifies how
	 * strong normals are repaired with the MTK.
	 */
	public enum NormalHealingMode
	{
		Off = 0,
		Minimal = 1,
		Default = 2
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODMeshToolKitSettings
	{
		public InstaLODMeshToolKitSettings(float weldingThreshold)
		{
			WeldingThreshold = weldingThreshold;
			WeldingNormalAngleThreshold = 80.0f;
			WeldingBoundaries = false;
			HealTJunctionThreshold = 0.0f;
			RemoveDegenerateFacesThreshold = 0.0f;
			FixNonManifold = false;
			ConformNormals = false;
			ConformWindingOrder = false;
			FlipNormals = false;
			FlipWindingOrder = false;
			FillHoles = false;
			NormalHealingModeSettings = NormalHealingMode.Off;
			RecalculateNormals = false;
			HardAngleThreshold = 80.0f;
			WeightedNormals = true;
			MinimumSubMeshBoundingSphereRadius = 0.0f;
			MinimumFaceArea = 0.0f;
			RemoveSubmeshIndices = false;
			CalculateSubmeshIndicesByAdjacency = false;
			CalculateSubmeshIndicesByMaterial = false;
			Deterministic = false;
		}

		public float WeldingThreshold;
		public float WeldingNormalAngleThreshold;
		public NativeBool WeldingBoundaries;
		public float HealTJunctionThreshold;
		public float RemoveDegenerateFacesThreshold;
		public NativeBool FixNonManifold;
		public NativeBool ConformNormals;
		public NativeBool ConformWindingOrder;
		public NativeBool FlipNormals;
		public NativeBool FlipWindingOrder;
		public NativeBool FillHoles;
		public NormalHealingMode NormalHealingModeSettings;
		public NativeBool RecalculateNormals;
		public float HardAngleThreshold;
		public NativeBool WeightedNormals;
		public float MinimumSubMeshBoundingSphereRadius;
		public float MinimumFaceArea;
		public NativeBool RemoveSubmeshIndices;
		public NativeBool CalculateSubmeshIndicesByAdjacency;
		public NativeBool CalculateSubmeshIndicesByMaterial;
		public NativeBool Deterministic;
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODUnwrapSettings
	{
		public InstaLODUnwrapSettings(UInt32 width, UInt32 height)
		{
			UnwrapStrategy = UVUnwrapStrategy.Auto;
			TextureWidth = width;
			TextureHeight = height;
			GutterSizeInPixels = 5;
			StretchImportance = MeshFeatureImportance.Normal;
			ShellStitching = true;
			InsertNormalSplits = true;
			UVScale = 1.0f;
			PostProcessLayout = false;
			Deterministic = false;
		}

		public UVUnwrapStrategy UnwrapStrategy;
		public UInt32 TextureWidth;
		public UInt32 TextureHeight;
		public UInt32 GutterSizeInPixels;
		public MeshFeatureImportance StretchImportance;

		public NativeBool ShellStitching;
		public NativeBool InsertNormalSplits;
		public float UVScale;
		public NativeBool PostProcessLayout;
		public NativeBool Deterministic;
	};

	/**
	 * The IsotropicRemeshingEdgeMode enumeration specifies different types of
	 * criteria to be used to determine the target edge size.
	 */
	public enum IsotropicEdgeMode
	{
		Automatic = 0,
		Absolute = 1,
		BoundingSphereRelative = 2,
		Count = 3
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODIsotropicRemeshSettings
	{
		public InstaLODIsotropicRemeshSettings(float targetEdgeSize)
		{
			EdgeMode = IsotropicEdgeMode.Automatic;
			TargetEdgeSize = targetEdgeSize;
			Precision = MeshFeatureImportance.Normal;
			WeldingThreshold = 0.0f;
			GeometricFeatureAngleInDegrees = 0.0f;
			CollapseThreshold = 0.75f;
			PreserveColorSplits = false;
			PreserveTexCoordSplits = true;
			PreserveNormalSplits = false;
			PreserveVolume = false;
			RecalculateNormals = false;
			HardAngleThreshold = 80.0f;
			WeightedNormals = true;
			Deterministic = false;
		}

		public IsotropicEdgeMode EdgeMode;
		public float TargetEdgeSize;
		public MeshFeatureImportance Precision;
		public float WeldingThreshold;
		public float GeometricFeatureAngleInDegrees;
		public float CollapseThreshold;
		public NativeBool PreserveColorSplits;
		public NativeBool PreserveTexCoordSplits;
		public NativeBool PreserveNormalSplits;
		public NativeBool PreserveVolume;
		public NativeBool RecalculateNormals;
		public float HardAngleThreshold;
		public NativeBool WeightedNormals;
		public NativeBool Deterministic;
	}
}