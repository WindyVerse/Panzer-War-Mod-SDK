/**
 * InstaLODMeshWrapper.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODMeshWrapper.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using Unity.Collections;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODMeshWrapper class manages conversion between Unity meshes and InstaLOD meshes.
	/// </summary>
	internal class InstaLODMeshWrapper : IDisposable
	{
		public UnityEngine.Object owningObject;
		public Mesh sharedMesh;
		public Material[] materials;
		public Dictionary<int, int> globalToLocalMaterialIndex;
		public InstaLODMesh nativeMesh;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODMeshWrapper"/> class.
		/// </summary>
		/// <param name="theOwningObject">The owning object.</param>
		/// <param name="theSharedMesh">The shared mesh.</param>
		/// <param name="submeshMaterials">Submesh materials.</param>
		/// <param name="materialToGlobalMap">Material to global map.</param>
		/// <param name="transform">Transform.</param>
		/// <param name="localToGlobalBoneMap">Local to global bone map.</param>
		/// <param name="transformedVertices">Transformed vertices.</param>
		public InstaLODMeshWrapper(GameObject theOwningObject, Mesh theSharedMesh, Material[] submeshMaterials = null, Dictionary<Material, int> materialToGlobalMap = null, Transform transform = null, Dictionary<int, int> localToGlobalBoneMap = null, Vector3[] transformedVertices = null, UInt32 submeshIndex = 0u)
		{
			owningObject = theOwningObject;
			sharedMesh = theSharedMesh;
			materials = submeshMaterials;
			_transform = transform;
			globalToLocalMaterialIndex = new Dictionary<int, int>();

			if (sharedMesh == null)
				return;

			UInt32 vertexCount = (UInt32)sharedMesh.vertices.Length;
			UInt32 indexCount = (UInt32)sharedMesh.triangles.Length;

			UInt32[] indices = new UInt32[indexCount];
			Int32[] materialIds = new Int32[indexCount / 3];
			UInt32[] unityLocalMaterialID = new UInt32[indexCount / 3];
			UInt32[] submeshID = new UInt32[indexCount / 3];

			GetTriangleIndicesFromUnityMesh(sharedMesh, ref indices, ref unityLocalMaterialID);

			// Build global to local material map, this is necessary if multiple meshes
			// with shared materials are processed.
			if (submeshMaterials != null && materialToGlobalMap != null)
			{
				for (int i = 0; i < unityLocalMaterialID.Length; i++)
				{
					if (materialIds.Length <= i)
					{
						Debug.LogWarning("Material IDs don't match. Please reimport the model.");
						break;
					}

					UInt32 faceSubmeshID = unityLocalMaterialID[i];

					if (submeshMaterials.Length <= faceSubmeshID)
					{
						Debug.LogWarning("Submesh Material IDs don't match. Please reimport the model.");
						continue;
					}

					Material material = submeshMaterials[faceSubmeshID];

					if (!materialToGlobalMap.ContainsKey(material))
						continue;

					int globalMaterialID = materialToGlobalMap[material];
					materialIds[i] = globalMaterialID;

					if (!globalToLocalMaterialIndex.ContainsKey(globalMaterialID))
					{
						globalToLocalMaterialIndex.Add(globalMaterialID, (int)faceSubmeshID);
					}
				}
			}

			// set submesh indices, this must be set to zero
			// as InstaLOD will move the mesh id for each append mesh.
			// This does mean that the submesh id from the second appended mesh, 
			// which was 0, will be 1 after the append
			for (int i = 0; i < submeshID.Length; i++)
			{
				submeshID[i] = 0;
			}

			NativeArray<BoneWeight1> allBoneWeights = sharedMesh.GetAllBoneWeights();
			NativeArray<byte> allBonesPerVertex = sharedMesh.GetBonesPerVertex();
			Byte [] bonesPerVertex = new Byte[allBonesPerVertex.Length];
			for (int i = 0; i < allBonesPerVertex.Length; i++)
			{
				bonesPerVertex[i] = allBonesPerVertex[i];
			}
				
			InstaLODBoneWeights [] boneWeights = new InstaLODBoneWeights[allBoneWeights.Length];

			for (Int32 weightIndex = 0; weightIndex< allBoneWeights.Length; weightIndex++)
			{
				boneWeights[weightIndex].BoneIndex = localToGlobalBoneMap != null ? localToGlobalBoneMap[allBoneWeights[weightIndex].boneIndex] : allBoneWeights[weightIndex].boneIndex;
				boneWeights[weightIndex].BoneWeight = allBoneWeights[weightIndex].weight;
			}

			Vector3[] vertices = sharedMesh.vertices;
			Vector3[] normals = sharedMesh.normals;
			Vector4[] tangents = sharedMesh.tangents;

			if (normals.Length == 0)
			{
				sharedMesh.RecalculateNormals();
				normals = sharedMesh.normals;
			}
			if (tangents.Length == 0)
			{
				sharedMesh.RecalculateTangents();
				tangents = sharedMesh.tangents;
			}

			if (transformedVertices != null)
			{
				vertices = transformedVertices;
			}

			if (_transform != null)
			{
				Matrix4x4 localToWorld = _transform.localToWorldMatrix;
				for (UInt32 i = 0; i < vertexCount; i++)
				{
					vertices[i] = localToWorld.MultiplyPoint(vertices[i]);
					normals[i] = localToWorld.MultiplyVector(normals[i]);

					Vector4 tangent4 = tangents[i];
					Vector3 tangent = new Vector3(tangent4.x, tangent4.y, tangent4.z);
					tangent = localToWorld.MultiplyVector(tangent);
					tangents[i] = new Vector4(tangent.x, tangent.y, tangent.z, tangent4.w);
				}
			}

			_verticesHandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
			_trianglesHandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
			_texCoords0Handle = GCHandle.Alloc(sharedMesh.uv, GCHandleType.Pinned);
			_texCoords1Handle = GCHandle.Alloc(sharedMesh.uv2, GCHandleType.Pinned);
			_texCoords2Handle = GCHandle.Alloc(sharedMesh.uv3, GCHandleType.Pinned);
			_texCoords3Handle = GCHandle.Alloc(sharedMesh.uv4, GCHandleType.Pinned);
			_tangentsHandle = GCHandle.Alloc(tangents, GCHandleType.Pinned);
			_normalsHandle = GCHandle.Alloc(normals, GCHandleType.Pinned);
			_colorsHandle = GCHandle.Alloc(sharedMesh.colors, GCHandleType.Pinned);
			_materialIDsHandle = GCHandle.Alloc(materialIds, GCHandleType.Pinned);
			_subMeshIDsHandle = GCHandle.Alloc(submeshID, GCHandleType.Pinned);
			_boneWeightsHandle = GCHandle.Alloc(boneWeights, GCHandleType.Pinned);
			_bonesPerVertexHandle = GCHandle.Alloc(bonesPerVertex, GCHandleType.Pinned);

			nativeMesh = new InstaLODMesh()
			{
				VertexCount = vertexCount, 
				IndexCount = indexCount,

				Vertices = _verticesHandle.AddrOfPinnedObject(),
				Indices = _trianglesHandle.AddrOfPinnedObject(),
				TexCoords0 = sharedMesh.uv.Length == 0 ? IntPtr.Zero : _texCoords0Handle.AddrOfPinnedObject(),
				TexCoords1 = sharedMesh.uv2.Length == 0 ? IntPtr.Zero : _texCoords1Handle.AddrOfPinnedObject(),
				TexCoords2 = sharedMesh.uv3.Length == 0 ? IntPtr.Zero : _texCoords2Handle.AddrOfPinnedObject(),
				TexCoords3 = sharedMesh.uv4.Length == 0 ? IntPtr.Zero : _texCoords3Handle.AddrOfPinnedObject(),
				Tangents = _tangentsHandle.AddrOfPinnedObject(),
				Normals = _normalsHandle.AddrOfPinnedObject(),
				Colors = sharedMesh.colors.Length == 0 ? IntPtr.Zero : _colorsHandle.AddrOfPinnedObject(),
				MaterialIDs = materialIds.Length == 0 ? IntPtr.Zero : _materialIDsHandle.AddrOfPinnedObject(),
				SubmeshIDs = submeshID.Length == 0 ? IntPtr.Zero : _subMeshIDsHandle.AddrOfPinnedObject(),
				BoneWeights = boneWeights.Length == 0 ? IntPtr.Zero : _boneWeightsHandle.AddrOfPinnedObject(),
				BonesPerVertex = bonesPerVertex.Length == 0 ? IntPtr.Zero : _bonesPerVertexHandle.AddrOfPinnedObject()
			};
		}

		/// <summary>
		/// Converts the native InstaLOD mesh back to a unity mesh.
		/// </summary>
		/// <returns>The unity mesh.</returns>
		/// <param name="nativeMesh">Native mesh.</param>
		/// <param name="inverseWorldTransform">If set to <c>true</c> inverse world transform.</param>
		/// <param name="isNewMaterial">Determines if the process generated a new material.</param>
		public Mesh ConvertNativeMesh(InstaLODMesh nativeMesh, bool inverseWorldTransform = true, bool isNewMaterial = true)
		{
			Mesh optimizedMesh = new Mesh();
						
			// NOTE: automatically select proper index format for mesh
			if (nativeMesh.IndexCount < 65535)
			{
				optimizedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
			}
			else
			{
				optimizedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			}

			UInt32 newVertexCount = nativeMesh.VertexCount;
			UInt32 newIndexCount = nativeMesh.IndexCount;
			byte [] bonesPerVertex = GetNativeArray<byte>(nativeMesh.BonesPerVertex, newVertexCount);
			// bonesPerVertex is an array filled with the same number.
			byte numberOfBoneWeightsPerVertex = (byte)bonesPerVertex.Length > 0? bonesPerVertex[0]: (byte) 1;

			// copy content 
			InstaLODBoneWeights[] newBoneWeights = GetNativeArray<InstaLODBoneWeights>(nativeMesh.BoneWeights, newVertexCount * numberOfBoneWeightsPerVertex);

			// inverse world transform
			Vector3[] vertices = GetNativeArray<Vector3>(nativeMesh.Vertices, newVertexCount);
			Vector3[] normals = GetNativeArray<Vector3>(nativeMesh.Normals, newVertexCount);
			Vector4[] tangents = GetNativeArray<Vector4>(nativeMesh.Tangents, newVertexCount);
			if (_transform != null && inverseWorldTransform)
			{
				Matrix4x4 worldToLocal = _transform.worldToLocalMatrix;

				for (UInt32 i = 0; i < newVertexCount; i++)
				{
					vertices[i] = worldToLocal.MultiplyPoint(vertices[i]);
					normals[i] = worldToLocal.MultiplyVector(normals[i]);

					Vector4 tangent4 = tangents[i];
					Vector3 tangent = new Vector3(tangent4.x, tangent4.y, tangent4.z);
					tangent = worldToLocal.MultiplyVector(tangent);
				
					tangents[i] = new Vector4(tangent.x, tangent.y, tangent.z, tangent4.w);
				}
			}
			optimizedMesh.vertices = vertices;
			optimizedMesh.normals = normals;
			optimizedMesh.tangents = tangents;

			// NOTE: local material id's will be remapped if no new material is generated
			UInt32[] faceSubmeshIndices = GetNativeArray<UInt32>(nativeMesh.MaterialIDs, newIndexCount / 3);

			if (sharedMesh == null)
			{
				faceSubmeshIndices = new UInt32[faceSubmeshIndices.Length];
			}

			if (faceSubmeshIndices.Length == 0)
				return optimizedMesh;

			ApplyTriangleIndicesToUnityMesh(sharedMesh != null ? sharedMesh.subMeshCount : 1, 
				optimizedMesh, 
				GetNativeArray<UInt32>(nativeMesh.Indices, newIndexCount), 
				faceSubmeshIndices, 
				isNewMaterial ? null : globalToLocalMaterialIndex);
			
			optimizedMesh.uv = GetNativeArray<Vector2>(nativeMesh.TexCoords0, newVertexCount);
			optimizedMesh.uv2 = GetNativeArray<Vector2>(nativeMesh.TexCoords1, newVertexCount);
			optimizedMesh.uv3 = GetNativeArray<Vector2>(nativeMesh.TexCoords2, newVertexCount);
			optimizedMesh.uv4 = GetNativeArray<Vector2>(nativeMesh.TexCoords3, newVertexCount);
			optimizedMesh.colors = GetNativeArray<Color>(nativeMesh.Colors, newVertexCount);
			optimizedMesh.bindposes = sharedMesh != null ? sharedMesh.bindposes : null;
			optimizedMesh.ClearBlendShapes();

			if (newBoneWeights.Length > 0)
			{
				BoneWeight1 [] newUnityBoneWeights = new BoneWeight1 [newBoneWeights.Length];
				for(int i = 0; i < newBoneWeights.Length; i++)
				{
					newUnityBoneWeights[i].weight = newBoneWeights[i].BoneWeight;
					newUnityBoneWeights[i].boneIndex = newUnityBoneWeights[i].weight == 0.0f ? 0 : newBoneWeights[i].BoneIndex;
				}
				optimizedMesh.SetBoneWeights(new NativeArray<byte>(bonesPerVertex, Allocator.Temp), new NativeArray<BoneWeight1>(newUnityBoneWeights, Allocator.Temp));
			}

			return optimizedMesh;
		}

		public void Dispose()
		{
			if (_verticesHandle.IsAllocated)
			{
				_verticesHandle.Free();
			}

			if (_trianglesHandle.IsAllocated)
			{
				_trianglesHandle.Free();
			}

			if (_texCoords0Handle.IsAllocated)
			{
				_texCoords0Handle.Free();
			}

			if (_texCoords1Handle.IsAllocated)
			{
				_texCoords1Handle.Free();
			}

			if (_texCoords2Handle.IsAllocated)
			{
				_texCoords2Handle.Free();
			}

			if (_texCoords3Handle.IsAllocated)
			{
				_texCoords3Handle.Free();
			}

			if (_tangentsHandle.IsAllocated)
			{
				_tangentsHandle.Free();
			}

			if (_normalsHandle.IsAllocated)
			{
				_normalsHandle.Free();
			}

			if (_colorsHandle.IsAllocated)
			{
				_colorsHandle.Free();
			}

			if (_materialIDsHandle.IsAllocated)
			{
				_materialIDsHandle.Free();
			}

			if (_subMeshIDsHandle.IsAllocated)
			{
				_subMeshIDsHandle.Free();
			}

			if (_boneWeightsHandle.IsAllocated)
			{
				_boneWeightsHandle.Free();
			}

			if (_bonesPerVertexHandle.IsAllocated)
			{
				_bonesPerVertexHandle.Free();
			}

			materials = null;
			sharedMesh = null;
		}

		/// <summary>
		/// Gets the triangle indices from unity mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="vertexIndices">Vertex indices.</param>
		/// <param name="faceSubmeshIDs">Face submesh identifier.</param>
		protected static void GetTriangleIndicesFromUnityMesh(Mesh mesh, ref UInt32[] vertexIndices, ref UInt32[] faceSubmeshIDs)
		{
			UInt32 currentIndex = 0;

			if (mesh.subMeshCount == 0)
				return;
			
			for (int submeshIndex = 0; submeshIndex < mesh.subMeshCount; submeshIndex++)
			{
				int[] submeshIndices = mesh.GetTriangles(submeshIndex);

				for (UInt32 i = 0; i < submeshIndices.Length; i++, currentIndex++)
				{
					vertexIndices[currentIndex] = (UInt32)submeshIndices[i];
					faceSubmeshIDs[currentIndex / 3] = (UInt32)submeshIndex;
				}
			}
		}

		/// <summary>
		/// Applies the triangle indices to unity mesh.
		/// </summary>
		/// <param name="originalSubMeshCount">Original sub mesh count.</param>
		/// <param name="mesh">Mesh.</param>
		/// <param name="vertexIndices">Vertex indices.</param>
		/// <param name="faceSubmeshIDs">Face submesh identifier.</param>
		protected static void ApplyTriangleIndicesToUnityMesh(int originalSubMeshCount, Mesh mesh, UInt32[] vertexIndices, UInt32[] faceSubmeshIDs, Dictionary<int, int> globalToLocalMap = null)
		{
			List<List<int>> data = new List<List<int>>();

			// initialize list
			for (int i = 0; i < originalSubMeshCount; i++)
			{
				data.Add(new List<int>());
			}

			// sort all triangles to their mateiralId list entry
			if (globalToLocalMap == null)
			{
				for (int t = 0; t < vertexIndices.Length; t++)
				{
					data[(int)faceSubmeshIDs[t / 3]].Add((int)vertexIndices[t]);
				}
			}
			else
			{
				for (int t = 0; t < vertexIndices.Length; t++)
				{
					if (!globalToLocalMap.ContainsKey((int)faceSubmeshIDs[t / 3]))
					{
						throw new Exception("material index invalid");
					}

					data[globalToLocalMap[(int)faceSubmeshIDs[t / 3]]].Add((int)vertexIndices[t]);
				}
			}			

			// apply all triangle groups (=submesh) to the mesh
			mesh.subMeshCount = originalSubMeshCount;
			for (int i = 0; i < data.Count; i++)
			{
				mesh.SetTriangles(data[i].ToArray(), i);
			}
		}

		/// <summary>
		/// Gets the native array.
		/// </summary>
		/// <returns>The native array.</returns>
		/// <param name="array">Array.</param>
		/// <param name="length">Length.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetNativeArray<T>(IntPtr array, UInt32 length)
		{
			T[] result = new T[0];
			int size = Marshal.SizeOf(typeof(T));

			if (array == IntPtr.Zero)
				return result;

			result = new T[length];

			if (IntPtr.Size == 4)
			{
				// 32-bit system
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (T)Marshal.PtrToStructure(array, typeof(T));
					array = new IntPtr(array.ToInt32() + size);
				}
			}
			else
			{
				// 64-bit system
				for (int i = 0; i < result.Length; i++)
				{
					result[i] = (T)Marshal.PtrToStructure(array, typeof(T));
					array = new IntPtr(array.ToInt64() + size);
				}
			}

			return result;
		}

		private Transform _transform;

		private GCHandle _verticesHandle;
		private GCHandle _trianglesHandle;
		private GCHandle _texCoords0Handle;
		private GCHandle _texCoords1Handle;
		private GCHandle _texCoords2Handle;
		private GCHandle _texCoords3Handle;
		private GCHandle _tangentsHandle;
		private GCHandle _normalsHandle;
		private GCHandle _colorsHandle;
		private GCHandle _materialIDsHandle;
		private GCHandle _subMeshIDsHandle;
		private GCHandle _boneWeightsHandle;
		private GCHandle _bonesPerVertexHandle;
	}
}