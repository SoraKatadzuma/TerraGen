using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// <para>
  /// A job specifically for generating the volume of a given chunk.
  /// </para>
  /// <para>
  /// This will generate the volume of a given chunk based off 3D biome information using simplex
  /// noise generated from a given coordinate within the chunk. Initially, the algorithm will take
  /// the given cartesian coordinate and convert it to polar coordinates for climate selection. The
  /// resulting climate will then be used to select a biome. Along with that biome will come
  /// algorithms for determining the
  /// </para>
  /// <para>
  /// The temperature and humidity values are used to select a biome that corresponds to those
  /// values. The biome will then be used to select a particular algorithm that will operate on the
  /// simplex values
  /// </para>
  /// </summary>
  [BurstCompile]
  [StructLayout(LayoutKind.Sequential)]
  public struct VolumeSolverJob : IJob {
    /// <summary>
    /// Settings for the world that will be defined by the volume.
    /// </summary>
    [ReadOnly]
    public WorldSettings worldSettings;

    /// <summary>
    /// Settings for the noise generator that will be used to generate the volume.
    /// </summary>
    [ReadOnly]
    public NoiseSettings noiseSettings;

    /// <summary>
    /// Array for storing the values of the volume of the chunks to be generated.
    /// </summary>
    [WriteOnly]
    public NativeArray<float> values;

    /// <summary>
    /// The chunk location to generate from.
    /// </summary>
    [ReadOnly]
    public int3 chunk;

    /// <summary>
    /// Executes the code necessary to generate the chunk data.
    /// </summary>
    public void Execute() {

    }
  }
}
