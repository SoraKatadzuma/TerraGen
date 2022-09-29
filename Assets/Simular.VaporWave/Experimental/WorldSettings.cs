using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// Defines some settings for the world that is being generated.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct WorldSettings {
    public readonly float3 worldOffset;
    public readonly uint worldSeed;
    public readonly uint mantleRadius;
    public readonly uint crustRadius;
    public readonly uint limitRadius;

    public uint MantleSqrRadius => mantleRadius * mantleRadius;
    public uint CrustSqrRadius => crustRadius * crustRadius;
    public uint LimitSqrRadius => limitRadius * limitRadius;
  }
}
