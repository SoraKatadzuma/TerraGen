using Unity.Burst;
using Unity.Mathematics;

namespace sora.TerraGen {
  /// <summary>
  /// Responsible for converting coordinates between different coordinate
  /// systems.
  /// </summary>
  [BurstCompile]
  public struct CoordinateConverter {
    /// <summary>
    /// Converts cartesian coordinates to spherical coordinates.
    /// </summary>
    /// <param name="coords">
    /// The coordinates to convert.
    /// </param>
    /// <returns>
    /// The cartesian coordinates as spherical coordinates.
    /// </returns>
    public static float3 CartesianToSpherical(float3 coords) {
      var result = new float3();
      if (coords.x == 0f)
        coords.x = math.EPSILON;

      result.x = math.length(coords);
      result.y = math.asin(coords.y / result.x);
      result.z = math.atan(coords.z / coords.x);
      if (coords.x < 0)
        result.z += math.PI;

      return result;
    }

    /// <summary>
    /// Converts spherical coordinates to cartesian coordinates.
    /// </summary>
    /// <param name="coords">
    /// The coordinates to convert.
    /// </param>
    /// <returns>
    /// The spherical coordinates as cartesian coordinates.
    /// </returns>
    public static float3 SphericalToCartesian(float3 coords) {
      var result = new float3();
      var a      = coords.x * math.cos(coords.z);

      result.x = a * math.cos(coords.y);
      result.y = coords.x * math.sin(coords.z);
      result.z = a * math.sin(coords.y);
      return result;
    }
  }
}
