using Unity.Burst;
using Unity.Mathematics;

namespace Simular.VaporWave {
    
    public static class CoordinateExtensions {
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
        public static float3 ToSpherical(this float3 coords) {
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
        public static float3 ToCartesian(this float3 coords) {
            var a = coords.x * math.sin(coords.y);
            return new float3 {
                x = a * math.cos(coords.z),
                y = a * math.sin(coords.z),
                z = coords.x * math.cos(coords.y)
            };
        }
    }
}
