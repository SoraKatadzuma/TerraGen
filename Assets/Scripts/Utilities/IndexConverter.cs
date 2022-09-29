using Unity.Burst;

namespace sora.TerraGen {
  /// <summary>
  /// Converts multiple indexes into flat indexes.
  /// </summary>
  [BurstCompile]
  public struct IndexConverter {
    /// <summary>
    /// Converts an [x,y) coordinate to just an x coordinate.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the 2D array.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the 2D array.
    /// </param>
    /// <param name="width">
    /// The width of each dimension of the 2D array.
    /// </param>
    /// <returns>
    /// The flattened 2D index for a 1D array.
    /// </returns>
    public static int Convert(int x, int y, int width) {
      return y * width + x;
    }

    /// <summary>
    /// Converts an [x,y,z) coordinate to just an x coordinate.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the 3D array.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the 3D array.
    /// </param>
    /// <param name="z">
    /// The z coordinate of the 3D array.
    /// </param>
    /// <param name="width">
    /// The width of each dimension of the 3D array.
    /// </param>
    /// <returns>
    /// The flattened 3D index for a 1D array.
    /// </returns>
    public static int Convert(int x, int y, int z, int width) {
      return Convert(x, y, z, width, width);
    }

    /// <summary>
    /// Converts an [x,y,z) coordinate to just an x coordinate.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the 3D array.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the 3D array.
    /// </param>
    /// <param name="z">
    /// The z coordinate of the 3D array.
    /// </param>
    /// <param name="width">
    /// The length of the first dimension of the 3D array.
    /// </param>
    /// <param name="height">
    /// The length of the second dimension of the 3D array.
    /// </param>
    /// <returns>
    /// The flattened 3D index for a 1D array.
    /// </returns>
    public static int Convert(int x, int y, int z, int width, int height) {
      return z * height * width + y * width + x;
    }
  }
}
