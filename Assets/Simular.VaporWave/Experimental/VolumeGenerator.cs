using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  ///
  /// </summary>
  [BurstCompile]
  public static class VolumeGenerator {
    /// <summary>
    ///
    /// </summary>
    /// <param name="worldSettings"></param>
    /// <param name="noiseSettings"></param>
    /// <param name="result"></param>
    public static void GenerateVolume(ref WorldSettings worldSettings,
                                      ref NoiseSettings noiseSettings,
                                      ref NativeArray<float> result) {
      var noiseGen  = new SimplexNoise(noiseSettings);
      var cubeBegin = (int3) noiseSettings.offset;
      for (int z = 0, locZ = cubeBegin.z; z < noiseSettings.size; z++, locZ++)
      for (int y = 0, locY = cubeBegin.y; y < noiseSettings.size; y++, locY++)
      for (int x = 0, locX = cubeBegin.x; x < noiseSettings.size; x++, locX++) {
        var cartesian = new float3(locX, locY, locZ);
        var spherical = SphericalCoords.FromCartesian(cartesian);
        var climate   = ClimateUtils.SelectClimate(cartesian, spherical);
        var biome     = BiomeUtils.SelectBiome(noiseGen.settings, climate, spherical.x);
        var noise     = noiseGen.SampleFractal(cartesian);
        var volume    = biome.MutateVolume(noise, cartesian);
        var index     = IndexConverter.Convert(x, y, z, noiseSettings.size);
        result[index] = ValidateVolume(ref worldSettings, math.lengthsq(cartesian), volume);
      }
    }

    private static float ValidateVolume(ref WorldSettings worldSettings, float radius, float volume) {
      return radius > worldSettings.MantleSqrRadius ||
             radius < worldSettings.LimitSqrRadius ? volume : -1.0f;
    }
  }
}
