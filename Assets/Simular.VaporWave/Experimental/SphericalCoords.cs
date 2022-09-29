using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Mathematics;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// Specifies spherical coordinates.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct SphericalCoords {
    private float3 coords;

    /// <summary>
    /// The distance from the center of the sphere.
    /// </summary>
    /// <remarks>
    /// Typically altitude is measured from the surface of the sphere but in this instance we don't
    /// care where it's measured from.
    /// </remarks>
    public float Altitude {
      get => coords.x;
      set => coords.x = value;
    }

    /// <summary>
    /// The horizontal angle of the coordinates.
    /// </summary>
    public float Longitude {
      get => coords.y;
      set => coords.y = value;
    }

    /// <summary>
    /// The vertical angle of the coordinates.
    /// </summary>
    public float Latitude {
      get => coords.z;
      set => coords.z = value;
    }

    /// <summary>
    /// Creates a new spherical coordinate from the provided float3.
    /// </summary>
    /// <param name="coords">
    /// The coordinates to be wrapped in this struct.
    /// </param>
    /// <param name="cartesian">
    /// Whether the coordinates are cartesian or not. If cartesian, they will be converted to
    /// spherical before assigned.
    /// </param>
    public SphericalCoords(in float3 coords, in bool cartesian = false) {
      this.coords = cartesian ? FromCartesian(coords) : coords;
    }

    /// <summary>
    /// Creates spherical coordinates from cartesian coordinates and returns the result as a float3.
    /// </summary>
    /// <param name="coords">
    /// The cartesian coordinates to convert to spherical coordinates.
    /// </param>
    /// <returns>
    /// The spherical coordinates stored in a float3.
    /// </returns>
    [BurstCompile]
    public static float3 FromCartesian(in float3 coords) {
      var result = new float3();
      result.x = math.length(coords);
      result.y = math.acos(coords.z / result.x); // Azimuth or longitude
      result.z = math.atan2(coords.y, coords.x); // Inclination or latitude
      return result;
    }

    /// <summary>
    /// Creates cartesian coordinates from spherical coordinates and returns the result as a float3.
    /// </summary>
    /// <param name="coords">
    /// The spherical coordinates to convert to cartesian coordinates.
    /// </param>
    /// <returns>
    /// The cartesian coordinates stored in a float3.
    /// </returns>
    [BurstCompile]
    public static float3 ToCartesian(in float3 coords) {
      var a = coords.x * math.sin(coords.y);
      return new float3 {
        x = a * math.cos(coords.z),
        y = a * math.sin(coords.z),
        z = coords.x * math.cos(coords.y)
      };
    }
  }
}
