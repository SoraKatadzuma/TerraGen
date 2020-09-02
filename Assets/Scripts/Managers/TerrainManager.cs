using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Manages the terrain for the game by deciding which chunks need to be loaded and unloaded.
/// </summary>
public class TerrainManager : MonoBehaviour {
  /// <summary>
  /// The player to render chunks at.
  /// </summary>
  public Transform playerTransform;

  /// <summary>
  /// The number of chunks in each direction to render.
  /// </summary>
  public int renderDistance;

  /// <summary>
  /// The noise settings to use to generate terrain.
  /// </summary>
  public NoiseSettings noiseSettings;

  /// <summary>
  /// Makes sure this object is initialized.
  /// </summary>
  private void Start() {
    // Get the world we want to work with, then get TerrainGenerator system.
    var defaultWorld     = World.DefaultGameObjectInjectionWorld;
    var terrainGenerator = defaultWorld.GetExistingSystem<TerrainGenerator>();

    // Tell the terrain generator to generate this chunk.
    terrainGenerator.noiseSettings = noiseSettings;
    float3 chunksBegin  = playerTransform.position / noiseSettings.size;
           chunksBegin -= (renderDistance / 2.0f);
    float3 chunksEnd    = chunksBegin + renderDistance;
    for (float z = chunksBegin.z; z < chunksEnd.z; z += 1.0f)
    for (float y = chunksBegin.y; y < chunksEnd.y; y += 1.0f)
    for (float x = chunksBegin.x; x < chunksEnd.x; x += 1.0f)
      terrainGenerator.chunksToLoad.Add(new float3(x, y, z));
  }
}
