using Unity.Mathematics;

namespace sora.TerraGen {
  /// <summary>
  /// Constants about the planets that are generated.
  /// </summary>
  public struct PlanetConstants {
    /// <summary>
    /// The radius of the planets that are generated.
    /// </summary>
    public const uint RADIUS = 41_253;

    /// <summary>
    /// The radius, squared, of the planets that are generated.
    /// </summary>
    public const uint RADIUS_SQRD = RADIUS * RADIUS;

    /// <summary>
    /// The circumference of the planets that are generated.
    /// </summary>
    public const uint CIRCUMFERENCE = 907_200;

    /// <summary>
    /// Checks if the given position is in the sphere defined by the planet's radius.
    /// </summary>
    /// <param name="position">
    /// The position to evaluate.
    /// </param>
    /// <returns>
    /// True if the position is in the sphere.
    /// </returns>
    public static bool inPlanetSphere(int3 position) {
      return ((position.x * position.x) + (position.y * position.y) + (position.z * position.z)) < RADIUS_SQRD;
    }
  }
}
