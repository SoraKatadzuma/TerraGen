using Unity.Mathematics;

namespace sora.TerraGen {
  /// <summary>
  /// Constants about the planets that are generated.
  /// </summary>
  public struct PlanetConstants {
    /// <summary>
    /// The sea level of the planets that are generated.
    /// </summary>
    public const int SEALEVEL = 41_253;

    /// <summary>
    /// The radius of the planets that are generated.
    /// </summary>
    public const int RADIUS = 41_253;

    /// <summary>
    /// The radius, squared, of the planets that are generated.
    /// </summary>
    public const int RADIUS_SQRD = RADIUS * RADIUS;

    /// <summary>
    /// The circumference of the planets that are generated.
    /// </summary>
    public const int CIRCUMFERENCE = 907_200;
  }
}
