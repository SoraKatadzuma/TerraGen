namespace Simular.VaporWave {
  /// <summary>
  /// Specifies climate humidity as an enumeration for selection by an algorithm which takes
  /// temperature as an argument.
  /// </summary>
  public enum Humidity : byte {
    /// <summary>
    /// Little to no water in the relative atmosphere of the location. Causes the formation of
    /// deserts in that location. Often accompanied by nearly no bodies of water.
    /// </summary>
    SuperArid,

    /// <summary>
    /// Minimal water in the relative atmosphere of the location. Causes the formation of scrubs,
    /// desert like landscapes with some life and vegetation. Often accompanied by minimal bodies
    /// of water.
    /// </summary>
    PerArid,

    /// <summary>
    /// Less than moderate water in the relative atmosphere. Causes the formation of dry sparse
    /// woodlands, moderate forest-like landscapes with life and vegetation in moderate abundance.
    /// Often accompanied by spare bodies of water.
    /// </summary>
    Arid,

    /// <summary>
    /// Moderate water in the relative atmosphere. Causes the formation of dry dense woodlands,
    /// moderate forest-like landscapes with life and vegetation in moderate abundance. Often
    /// accompanied by occasional bodies of water.
    /// </summary>
    SemiArid,

    /// <summary>
    /// Moderate water in the relative atmosphere. Causes the formation of dry sparse forests,
    /// landscapes with above moderate life and vegetation. Often accompanied by occasional bodies
    /// of water.
    /// </summary>
    SubHumid,

    /// <summary>
    /// Above moderate water in the relative atmosphere. Causes the formation of moist sparse
    /// forests, landscapes with above moderate life and vegetation. Often accompanied by moderate
    /// bodies of water.
    /// </summary>
    Humid,

    /// <summary>
    /// Lots of water in the relative atmosphere. Causes the formation of moist dense forests,
    /// landscapes with lots of life and vegetation. Often accompanied by moderate bodies of water.
    /// </summary>
    PerHumid,

    /// <summary>
    /// Nearly maximum water in the relative atmosphere. Causes the formation of rain forests,
    /// landscapes with extremely dense vegetation and an abundance of life. Often accompanied by
    /// many bodies of water.
    /// </summary>
    SuperHumid,
  }
}
