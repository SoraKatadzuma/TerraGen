using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  ///
  /// </summary>
  public interface Biome {
    float MutateVolume(float volume, float3 cartesian);
  }
}
