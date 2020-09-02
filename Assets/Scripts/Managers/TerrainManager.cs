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
    int3 chunksBegin  = new int3(playerTransform.position / noiseSettings.size);
         chunksBegin -= (renderDistance / 2);
    int3 chunksEnd    = chunksBegin + renderDistance;
    for (int z = chunksBegin.z; z < chunksEnd.z; z++)
    for (int y = chunksBegin.y; y < chunksEnd.y; y++)
    for (int x = chunksBegin.x; x < chunksEnd.x; x++)
      terrainGenerator.chunksToLoad.Add(new int3(x, y, z));
  }
}
