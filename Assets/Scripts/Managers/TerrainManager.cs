using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Manages the terrain for the game by deciding which chunks need to be loaded and unloaded.
/// </summary>
public class TerrainManager : MonoBehaviour {
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
    terrainGenerator.chunksToLoad.Add(new float3( 0.0f, 0.0f,  0.0f));
    terrainGenerator.chunksToLoad.Add(new float3(-1.0f, 0.0f,  0.0f));
    terrainGenerator.chunksToLoad.Add(new float3(-1.0f, 0.0f, -1.0f));
    terrainGenerator.chunksToLoad.Add(new float3( 0.0f, 0.0f, -1.0f));
  }
}
