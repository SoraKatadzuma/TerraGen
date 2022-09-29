namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// Climate altitude as an enumeration, used in the selection of temperatures and humidity by the
  /// altitude. Further used to determine the biome that a particular point in space is defined as.
  /// </summary>
  /// <remarks>
  /// The Ocean Layers and Mountain Layers apply to anything subterranean (under the crust, or soil
  /// of the planet). They don't necessarily have different meanings, but they do change the
  /// algorithms used to generate the terrain in those locations.
  /// </remarks>
  public enum TerrainLayer : byte {
  #region Ocean Layers
    /// <summary>
    /// The deepest dark and dank portions of terrain. Typically filled completely by water in
    /// oceans and bedrock in the crust, the abyss layer is a dangerous and volatile layer. Who
    /// knows what life and vegetation exist in the abyss.
    /// </summary>
    Abyss,

    /// <summary>
    /// The layer below sea level, but above the abyss. It's where the deep sea and deep terrain
    /// life and vegetation that is normally unseen, exist. It's a harsh terrain layer with many
    /// caverns and minimal life and vegetation.
    /// </summary>
    Fathoms,

    /// <summary>
    /// Below sea level, extending from the beaches to the ocean shelf. Experiences tidal currents
    /// and is home to most marine and additionally subterranean life and vegetation.
    /// </summary>
    Subaqueous,

    /// <summary>
    /// The layer at which sea and water accumulate into the oceans and exposed caves. It's where
    /// most life and vegetation inhabits, being able to breathe the atmosphere and receiving
    /// sunlight.
    /// </summary>
    SeaLevel,
  #endregion
  #region Mountain Layers
    /// <summary>
    /// The layer at which most common woodlands, forests, and other temperate climates exists. It
    /// also contains, as it's name suggests, foothills and other small mountainous terrain. Common
    /// to specifically, deciduous forests.
    /// </summary>
    FootHills,

    /// <summary>
    /// The layer at which many of the different types of forests mix with others. Defines,
    /// typically, the moderate hills and mountains that define unique terrain.
    /// </summary>
    Montane,

    /// <summary>
    /// The layer that defines most of the coniferous forests and some of the tallest mountains.
    /// These are typically very mountainous, cold, and snowy. Inhabited by those who can with
    /// stand the colder temperatures and lack of vegetation.
    /// </summary>
    Alpine,

    /// <summary>
    /// The layer that defines the tallest of the tall mountains and is typically consistent of
    /// lichen and moss. It's a very harsh layer of the terrain, little to no life exists here.
    /// </summary>
    Nival,
  #endregion
  }
}
