using Unity.Burst;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// Utilities for things related to climate values.
  /// </summary>
  [BurstCompile]
  public static class ClimateUtils {
    private const float _HALF_PI = math.PI / 2f;

    /// <summary>
    /// Selects a climate for the given cartesian coordinates.
    /// </summary>
    /// <param name="cartesian">
    /// The cartesian coordinates to select a climate value for.
    /// </param>
    /// <param name="spherical">
    /// The spherical coordinates to select a climate value for.
    /// </param>
    /// <returns>
    /// The selected climate.
    /// </returns>
    public static Climate SelectClimate(float3 cartesian, float3 spherical) {
      // Select variation in -1 to 1 range based on x,z plane.
      var variation   = noise.snoise(new float2(cartesian.x, cartesian.z)) * 2 - 1;
      var temperature = SelectTemperature(variation, ref spherical);
      var humidity    = SelectHumidity(variation, temperature);
      return new Climate((Temperature)temperature, (Humidity)humidity, TerrainLayer.SeaLevel);
    }

    private static int SelectHumidity(float variation, int temperature) {
      // Humidity is inversely proportionate to temperature,
      // However the enum is designed with this in mind.
      var humidity = temperature + (int) math.round(variation);

      // There are 8 enumerations but the values are between 0 and 7.
      return math.clamp(humidity, 0, 7);
    }

    private static int SelectTemperature(float variation, ref float3 spherical) {
      // Temperature is inversely proportionate to latitude,
      // However the enum is designed with this in mind.
      spherical.z += variation * math.radians(5);

      // There are 8 enumerations but the values are between 0 and 7.
      return (int)math.floor(7 * (spherical.z / _HALF_PI));
    }
  }
}
