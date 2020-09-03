using Unity.Collections;
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
  /// The noise generator we will be using.
  /// </summary>
  private SimplexNoise mNoiseGenerator;

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
  /// MAkes sure this object is initialized when it becomes usable in the engine.
  /// </summary>
  private void Start() {
    // Initialize the noise generator.
    noiseSettings.size += 1;
    mNoiseGenerator     = new SimplexNoise(noiseSettings);

    // Initialize the chunk mesh.
    mChunk                       = new GameObject();
    mChunkMesh                   = new Mesh();
    mMeshFilter                  = mChunk.AddComponent<MeshFilter>();
    mMeshFilter.sharedMesh       = mChunkMesh;
    mMeshRenderer                = mChunk.AddComponent<MeshRenderer>();
    mMeshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

    // Generate mesh.
    generate();

    // Set this chunk position so we can compare it to the chunk we're trying to generate
    mChunk.transform.position = new Vector3(32.0f, -16.0f, -16.0f);
  }

  const float RADIUS = 64.0f;
  private bool inSphere(float x, float y, float z) {
    return (x*x) + (y*y) + (z*z) < RADIUS;
  }

  /// <summary>
  /// Generates a single chunk with the noise generator.
  /// </summary>
  public void generate() {
    // Generate mesh data.
    var volumeData = mNoiseGenerator.fractal3D();
    var meshData   = MarchingCubes.generate(volumeData, noiseSettings.size - 1, lod: 1);
    var vertices   = new Vector3[meshData.Length];
    var triangles  = new int    [meshData.Length];
    for (int index = 0; index < meshData.Length; index++) {
      vertices[index]  = meshData[index];
      triangles[index] = index;
    }

    // Set vertices in chunk mesh.
    mChunkMesh.vertices  = vertices;
    mChunkMesh.triangles = triangles;
    mChunkMesh.RecalculateNormals();
    mChunkMesh.UploadMeshData(true);

    // Get rid of mesh data.
    volumeData.Dispose();
    meshData.Dispose();
  }
}
