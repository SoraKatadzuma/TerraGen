using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace sora.TerraGen {
  /// <summary>
  /// A volume generator is responsible for generating noise and using it as volume.
  /// </summary>
  [BurstCompile]
  public struct VolumeGenerator {
    /// <summary>
    /// Generates the volume data.
    /// </summary>
    /// <returns>
    /// The generated volume data.
    /// </returns>
    public static NativeArray<float> generateData(NoiseSettings topLevelSettings) {
      // Prep data.
      var size     = topLevelSettings.size;
      var fullSize = size * size * size;
      var noiseGen = new SimplexNoise(topLevelSettings);
      var result   = new NativeArray<float>(fullSize, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

      // Process data.
      int3 cubeBegin = (int3)topLevelSettings.offset;
      int3 cubeEnd   = (int3)(topLevelSettings.offset + topLevelSettings.size - 1);
      for (int z = 0, zloc = cubeBegin.z; z < topLevelSettings.size; zloc++, z++)
      for (int y = 0, yloc = cubeBegin.y; y < topLevelSettings.size; yloc++, y++)
      for (int x = 0, xloc = cubeBegin.x; x < topLevelSettings.size; xloc++, x++) {
        var loc = new int3(xloc, yloc, zloc);
        result[(z * size * size) + (y * size) + x] = PlanetConstants.inPlanetSphere(loc) ? 1.0f : -1.0f;
      }

      // Return the result.
      return result;
    }
  }
}
