namespace Simular.VaporWave {
  /// <summary>
  /// Specifies climate temperatures as an enumeration for selection by an algorithm which takes
  /// latitude as an argument.
  /// </summary>
  public enum Temperature : byte {
    /// <summary>
    /// Small range extending from the equator of a planet. Temperatures are equatorial, fairly warm,
    /// and consistent year round. Habitable but harsh. Precipitation is periodic, depending on
    /// distance from nearest body of water.
    /// </summary>
    Tropical,

    /// <summary>
    /// Slightly north of the tropical temperature range. Temperatures fluctuate minimally, mildly
    /// habitable. Precipitation is periodic, depending on distance from nearest body of water.
    /// </summary>
    SubTropical,

    /// <summary>
    /// Lower half of the temperate range above tropical and below the boreal ranges. Temperature
    /// fluctuates less intensively with the seasons. Nicely habitable, typically experiences less
    /// precipitation than cool temperate climates.
    /// </summary>
    WarmTemperate,

    /// <summary>
    /// Upper half of the temperate range above tropical and below the boreal temperature ranges.
    /// Temperatures fluctuate intensively with the seasons. Comfortable habitation, second most
    /// precipitous temperature range.
    /// </summary>
    CoolTemperate,

    /// <summary>
    /// Lower half of the boreal range above temperate and below the polar temperature ranges.
    /// Temperature fluctuates intensively with the seasons. Comfortable habitation, typically
    /// experiences the most precipitation of any other temperature climate.
    /// </summary>
    WarmBoreal,

    /// <summary>
    /// Upper half of the boreal range above temperate and below the polar temperature ranges.
    /// Temperature fluctuates less intensively with the seasons. Nicely habitable, common moderate
    /// precipitation.
    /// </summary>
    CoolBoreal,

    /// <summary>
    /// Slightly south of the polar range. Temperatures fluctuate minimally with the seasons, mildly
    /// habitable. Precipitation is periodic, depending on distance from nearest body of water.
    /// </summary>
    SubPolar,

    /// <summary>
    /// Small range extending from the poles of a planet. Temperatures are arctic, fairly cold, and
    /// consistent year round. Habitable but harsh. Precipitation is periodic, depending on distance
    /// from nearest body of water.
    /// </summary>
    Polar
  }
}
