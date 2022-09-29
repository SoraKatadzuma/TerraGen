using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace sora.TerraGen {
  /// <summary>
  /// A volume generator is responsible for generating noise and using it as volume.
  /// </summary>
  [BurstCompile]
  public struct VolumeGenerator {
    public const float RESOLUTION = 20.0f;

    // Don't allow to be larger than planet radius.
    public const float BLOAT = 5000.0f;

    /// <summary>
    /// Generates the volume data.
    /// </summary>
    /// <returns>
    /// The generated volume data.
    /// </returns>
    public static NativeArray<float> GenerateData(NoiseSettings baseNoiseSettings) {
      // Prep data.
      var size     = baseNoiseSettings.size;
      var fullSize = size * size * size;
      var noiseGen = new SimplexNoise(baseNoiseSettings);
      var result   = new NativeArray<float>(fullSize, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

      // Process data.
      var cubeBegin = (int3)baseNoiseSettings.offset;
      var cubeEnd   = (int3)(baseNoiseSettings.offset + baseNoiseSettings.size - 1);
      for (int z = 0, zloc = cubeBegin.z; z < baseNoiseSettings.size; zloc++, z++)
      for (int y = 0, yloc = cubeBegin.y; y < baseNoiseSettings.size; yloc++, y++)
      for (int x = 0, xloc = cubeBegin.x; x < baseNoiseSettings.size; xloc++, x++) {
        // Get normal vector.
        var ind  = IndexConverter.Convert(x, y, z, size);
        var loc  = new float3(xloc, yloc, zloc);
        var sum  = math.lengthsq(loc);
        var root = math.sqrt(sum);
        var norm = loc / root;

        // Scale up.
        var scaled = norm * PlanetConstants.RADIUS * RESOLUTION;
        var noise  = noiseGen.SampleFractal(scaled);
        var offset = noise * (BLOAT * BLOAT);

        // Check if within planet surface.
        if (sum > PlanetConstants.RADIUS_SQRD + offset)
          // Was not in planet surface.
          result[ind] = -1.0f;
        else
          // Default, was in planet surface.
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
