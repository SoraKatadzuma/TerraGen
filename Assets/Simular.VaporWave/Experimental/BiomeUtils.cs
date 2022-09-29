using Unity.Collections;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  public struct TestBiome : Biome {
    public float MutateVolume(float volume, float3 cartesian) {
      throw new System.NotImplementedException();
    }
  }

  public static class BiomeMapping {
  }

  public static class BiomeUtils {


    public static Biome SelectBiome(NoiseSettings noiseSettings, Climate climate, float altitude) {
      return new TestBiome();
    }
  }
}
