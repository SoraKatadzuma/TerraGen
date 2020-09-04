using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

/// <summary>
/// Responsible for generating a chunk.
/// </summary>
[BurstCompile]
public struct ChunkGeneratorJob : IJob {
  /// <summary>
  /// The storage for generated chunk data.
  /// </summary>
  public NativeList<float3> chunkDataStorage;

  /// <summary>
  /// The location to generate the chunk at.
  /// </summary>
  [ReadOnly]
  public int3 chunkLocation;

  /// <summary>
  /// The nooise settings for the chunk.
  /// </summary>
  public NoiseSettings noiseSettings;

  /// <summary>
  /// Called by the scheduler to do a chunk generation job.
  /// </summary>
  public void Execute() {
    // Update noise settings.
    noiseSettings.size += 1;

    // Create noise generator, get volume data.
    var noiseGenerator = new SimplexNoise(noiseSettings);
    var volumeData     = noiseGenerator.fractal3D();

    // Get chunk data.
    var chunkData = MarchingCubes.generate(volumeData, noiseSettings.size - 1, lod: 1);
    chunkDataStorage.AddRange(chunkData);

    // Dispose.
    volumeData.Dispose();
    chunkData.Dispose();
  }
}
