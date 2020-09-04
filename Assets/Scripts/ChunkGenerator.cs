using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// A chunk generator is responsible for generating a chunk of voxel terrain.
/// </summary>
public class ChunkGenerator : MonoBehaviour {
  /// <summary>
  /// The settings for the noise generator.
  /// </summary>
  public NoiseSettings noiseSettings;

  /// <summary>
  /// The mesh filter for the chunk.
  /// </summary>
  private MeshFilter mMeshFilter;

  /// <summary>
  /// The mesh renderer for the chunk.
  /// </summary>
  private MeshRenderer mMeshRenderer;

  /// <summary>
  /// The mesh we will be generating.
  /// </summary>
  private Mesh mChunkMesh;

  /// <summary>
  /// The chunk we are generating.
  /// </summary>
  private GameObject mChunk;

  /// <summary>
  /// The generator that will generate the chunk.
  /// </summary>
  private ChunkGeneratorJob mGenerator;

  /// <summary>
  /// The handle to the generator job.
  /// </summary>
  private JobHandle mJobHandle;

  /// <summary>
  /// The chunk data we are trying to get.
  /// </summary>
  private NativeList<float3> mChunkData;

  /// <summary>
  /// Whether or not the job finished.
  /// </summary>
  private bool mJobFinished;

  /// <summary>
  /// MAkes sure this object is initialized when it becomes usable in the engine.
  /// </summary>
  private void Start() {
    // Initialize the chunk mesh.
    mChunk                       = new GameObject();
    mChunkMesh                   = new Mesh();
    mMeshFilter                  = mChunk.AddComponent<MeshFilter>();
    mMeshFilter.sharedMesh       = mChunkMesh;
    mMeshRenderer                = mChunk.AddComponent<MeshRenderer>();
    mMeshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
    mChunkData                   = new NativeList<float3>(Allocator.TempJob);

    // Create generator.
    mGenerator = new ChunkGeneratorJob {
      chunkDataStorage = mChunkData,
      chunkLocation    = new int3(0, 0, 0),
      noiseSettings    = noiseSettings
    };

    // Schedule job.
    mJobHandle   = mGenerator.Schedule();
    mJobFinished = false;

    // Set this chunk position so we can compare it to the chunk we're trying to generate
    mChunk.transform.position = new Vector3(32.0f, -16.0f, -16.0f);
  }


  private void Update() {
    // Wait for job to complete.
    if (!mJobHandle.IsCompleted || mJobFinished)
      return;

    // Finish job.
    mJobHandle.Complete();
    mJobFinished = true;

    // Create mesh data.
    var vertices  = new Vector3[mChunkData.Length];
    var triangles = new int    [mChunkData.Length];
    for (int index = 0; index < mChunkData.Length; index++) {
      vertices [index] = mChunkData[index];
      triangles[index] = index;
    }

    // Set vertices in chunk mesh.
    mChunkMesh.vertices  = vertices;
    mChunkMesh.triangles = triangles;
    mChunkMesh.RecalculateNormals();
    mChunkMesh.UploadMeshData(true);
    mChunkData.Dispose();
  }
}
