using System.Collections.Generic;
using sora.TerraGen;
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
  /// The terrain generator that this manager uses.
  /// </summary>
  private TerrainGenerator mTerrainGenerator;

  /// <summary>
  /// The currently loaded chunks.
  /// </summary>
  private List<int3> mLoadedChunks;

  /// <summary>
  /// The current location of the player in chunk coords.
  /// </summary>
  private int3 mPlayerCurrentChunk;

  /// <summary>
  /// Makes sure this object is initialized.
  /// </summary>
  private void Start() {
    // Get the world we want to work with, then get TerrainGenerator system.
    var defaultWorld      = World.DefaultGameObjectInjectionWorld;
        mTerrainGenerator = defaultWorld.GetExistingSystem<TerrainGenerator>();
        mLoadedChunks     = new List<int3>();

    // Tell the terrain generator to generate this chunk.
    mTerrainGenerator.noiseSettings = noiseSettings;
    mPlayerCurrentChunk = new int3(playerTransform.position / noiseSettings.size);
    recalculateChunks();
  }

  /// <summary>
  /// Called each frame.
  /// </summary>
  private void Update() {
    // The current chunk the player is in.
    int3 currentChunk = new int3(playerTransform.position / noiseSettings.size);
    if (currentChunk.Equals(mPlayerCurrentChunk))
      return;

    // Recalculate the chunks that should be loaded.
    recalculateChunks();

    // Recalculate chunks to load.
    mPlayerCurrentChunk = currentChunk;
  }

  /// <summary>
  /// Recalculates the chunks to be loaded and unloaded.
  /// </summary>
  private void recalculateChunks() {
    // A list of the chunks to load.
    var  chunksToLoad = new List<int3>();
    int3 chunksBegin  = mPlayerCurrentChunk;
         chunksBegin -= (renderDistance / 2);
    int3 chunksEnd    = chunksBegin + renderDistance;
    for (int z = chunksBegin.z; z < chunksEnd.z; z++)
    for (int y = chunksBegin.y; y < chunksEnd.y; y++)
    for (int x = chunksBegin.x; x < chunksEnd.x; x++)
      chunksToLoad.Add(new int3(x, y, z));

    // Figure out which are already loaded.
    chunksToLoad.ForEach(item => {
      // Do not add this chunk.
      bool wasRemoved = mLoadedChunks.Remove(item);
      if (!wasRemoved)
        mTerrainGenerator.chunksToLoad.Add(item);
    });

    // Send the remaining previously loaded chunks as ones to unload.
    mTerrainGenerator.chunksToUnload.AddRange(mLoadedChunks);
    mLoadedChunks.Clear();
    mLoadedChunks.AddRange(chunksToLoad);
  }
}
