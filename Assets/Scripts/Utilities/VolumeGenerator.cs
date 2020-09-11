using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace sora.TerraGen {
  /// <summary>
  /// A volume generator is responsible for generating noise and using it as volume.
  /// </summary>
  [BurstCompile]
  public struct VolumeGenerator {
    private static float3 normalize(float3 vec) {
      var sum  = (vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z);
      var root = math.sqrt(sum);
      return vec / root;
    }

    public const float RESOLUTION = 20.0f;

    // Don't allow to be larger than planet radius.
    public const float BLOAT = 5000.0f;

    /// <summary>
    /// Generates the volume data.
    /// </summary>
    /// <returns>
    /// The generated volume data.
    /// </returns>
    public static NativeArray<float> generateData(NoiseSettings baseNoiseSettings) {
      // Prep data.
      var size     = baseNoiseSettings.size;
      var fullSize = size * size * size;
      var noiseGen = new SimplexNoise(baseNoiseSettings);
      var result   = new NativeArray<float>(fullSize, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

      // Process data.
      int3 cubeBegin = (int3)baseNoiseSettings.offset;
      int3 cubeEnd   = (int3)(baseNoiseSettings.offset + baseNoiseSettings.size - 1);
      for (int z = 0, zloc = cubeBegin.z; z < baseNoiseSettings.size; zloc++, z++)
      for (int y = 0, yloc = cubeBegin.y; y < baseNoiseSettings.size; yloc++, y++)
      for (int x = 0, xloc = cubeBegin.x; x < baseNoiseSettings.size; xloc++, x++) {
        // Get normal vector.
        var ind  = (z * size * size) + (y * size) + x;
        var loc  = new float3(xloc, yloc, zloc);
        var norm = normalize(loc);// math.max(xloc, math.max(yloc, zloc));

        // Scale up.
        var scaled = norm * PlanetConstants.RADIUS * RESOLUTION;
        var noise  = noiseGen.sampleFractal(scaled);
        var offset = noise * (BLOAT * BLOAT);

        // Check if within planet surface.
        var value = (loc.x * loc.x) + (loc.y * loc.y) + (loc.z * loc.z);
        if (value > PlanetConstants.RADIUS_SQRD + offset)
          // Was not in planet surface.
          result[ind] = -1.0f;
        else
          // Defualt, was in planet surface.
          result[ind] = 1.0f;
      }

      // Create a biome layer object to filter the data.
      // var biomeLayer = new BiomeLayer(baseNoiseSettings);
      //     biomeLayer.Evaluate(result);

      // Return the result.
      return result;
    }
  }
}
