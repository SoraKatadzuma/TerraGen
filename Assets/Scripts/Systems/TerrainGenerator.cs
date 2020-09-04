using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// A terrain generator is responsible for generating chunks of a given terrain as fast as it
/// possibly can.
/// </summary>
public sealed class TerrainGenerator : SystemBase {
  /// <summary>
  /// The top level noise settings for generating chunks.
  /// </summary>
  public NoiseSettings noiseSettings;

  /// <summary>
  /// Tracks the chunks that are loaded.
  /// </summary>
  public NativeList<int3> mLoadedChunks;

  /// <summary>
  /// A list of chunks that need to be loaded by the terrain generator.
  /// </summary>
  public NativeList<int3> chunksToLoad;

  /// <summary>
  /// A list of chunks that need to be unloaded by the terrain generator.
  /// </summary>
  public NativeList<int3> chunksToUnload;

  /// <summary>
  /// The archetype for the entities created by this generator.
  /// </summary>
  private EntityArchetype mChunkEntityArchetype;

  /// <summary>
  /// The material that chunks use.
  /// </summary>
  private Material mChunkMaterial;

  /// <summary>
  /// Makes sure this object is initialized in Unity.
  /// </summary>
  protected override void OnCreate() {
    // The bags to store.
    chunksToLoad   = new NativeList<int3>(Allocator.Persistent);
    chunksToUnload = new NativeList<int3>(Allocator.Persistent);

    // Create chunk entity archetype.
    mChunkEntityArchetype = EntityManager.CreateArchetype(
      typeof(Translation),
      typeof(RenderMesh),
      typeof(RenderBounds),
      typeof(LocalToWorld)
    );

    // Create material.
    mChunkMaterial = new Material(Shader.Find("Standard"));
  }

  /// <summary>
  /// Called each frame this object needs to be updated.
  /// </summary>
  protected override void OnUpdate() {
    // Check for chunks to load.
    if (chunksToLoad.Length > 0)
      scheduleChunksForLoad();

    // Check for chunks to unload.
    if (chunksToUnload.Length > 0)
      scheduleChunksForUnload();
  }

  /// <summary>
  /// Performs the necessary operations to generated and load chunks.
  /// </summary>
  private void scheduleChunksForLoad() {
    // Attempt to load chunks.
    var entityCount = chunksToLoad.Length;
    var newChunks   = EntityManager.CreateEntity(mChunkEntityArchetype, entityCount, Allocator.TempJob);
    for (int index = 0; index < entityCount; index++) {
      // Set chunk world position.
      EntityManager.SetComponentData(newChunks[index], new Translation {
        Value = chunksToLoad[index] * noiseSettings.size
      });

      // Set render bounds.
      EntityManager.SetComponentData(newChunks[index], new RenderBounds {
        Value = new AABB {
          Center  = new float3(0.0f, 0.0f, 0.0f),
          Extents = new float3(1.0f, 1.0f, 1.0f) * noiseSettings.size
        }
      });

      // Create chunk render mesh.
      EntityManager.SetSharedComponentData(newChunks[index], new RenderMesh {
        mesh           = new Mesh(),
        material       = mChunkMaterial,
        layer          = LayerMask.GetMask("Default"),
        // castShadows    = ShadowCastingMode.On,
        // receiveShadows = true
      });
    }

    // Create parallel job to complete.
    var jobHandles = new NativeArray<JobHandle>(entityCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
    var meshLists  = new NativeList<float3>[entityCount];
    var chunkJobs  = new List<ChunkGeneratorJob>();
    for (int index = 0; index < entityCount; index++) {
      // Prep generator job.
      meshLists[index]     = new NativeList<float3>(Allocator.TempJob);
      noiseSettings.offset = chunksToLoad[index] * noiseSettings.size;
      var generator        = new ChunkGeneratorJob {
        chunkDataStorage = meshLists[index],
        chunkLocation    = chunksToLoad[index],
        noiseSettings    = noiseSettings
      };

      chunkJobs.Add(generator);
      jobHandles[index] = chunkJobs[index].Schedule();
    }

    // Execute generation job.
    JobHandle.CompleteAll(jobHandles);

    // Update render meshes.
    var vertices  = new List<Vector3>();
    var triangles = new List<int>();
    for (int index = 0; index < entityCount; index++) {
      // Get generation results and mesh.
      var meshData = meshLists[index];
      var mesh     = EntityManager.GetSharedComponentData<RenderMesh>(newChunks[index]).mesh;

      // Get verts.
      for (int vertIndex = 0; vertIndex < meshData.Length; vertIndex++) {
        vertices.Add(meshData[vertIndex]);
        triangles.Add(vertIndex);
      }

      // Set mesh data.
      mesh.vertices  = vertices.ToArray();
      mesh.triangles = triangles.ToArray();
      mesh.RecalculateNormals();
      mesh.UploadMeshData(true);

      // Clear verts.
      vertices.Clear();
      triangles.Clear();
    }

    // Dispose of resources.
    chunksToLoad.Clear();
    newChunks.Dispose();
    jobHandles.Dispose();
    for (int index = 0; index < entityCount; index++)
      meshLists[index].Dispose();
  }

  /// <summary>
  /// Performs the necessary operations to unload chunks.
  /// </summary>
  private void scheduleChunksForUnload() {
    // TODO: unload chunks.
  }
}
