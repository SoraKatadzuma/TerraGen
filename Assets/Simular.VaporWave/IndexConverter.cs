using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Simular.VaporWave {
    /// <summary>
    /// Converts multiple indexes into flat indexes.
    /// </summary>
    [BurstCompile]
    [GenerateTestsForBurstCompatibility]
    public struct IndexConverter {
        /// <summary>
        /// Converts an [x,y) coordinate to a 1D array index.
        /// </summary>
        /// <param name="indices">
        /// The indices packed into a <see cref="int2"/>.
        /// </param>
        /// <param name="width">
        /// The width of each dimension of the 2D array.
        /// </param>
        /// <returns>
        /// The flattened 2D index for a 1D array.
        /// </returns>
        public static int Convert(int2 indices, int width) {
            return IndexConverter.Convert(indices.x, indices.y, width);
        }

        /// <summary>
        /// Converts an [x,y) coordinate to a 1D array index.
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
        /// Converts an [x,y,z) coordinate to a 1D array index.
        /// </summary>
        /// <param name="indices">
        /// The indices packed into a <see cref="int3"/>.
        /// </param>
        /// <param name="width">
        /// The width of each dimension of the 3D array.
        /// </param>
        /// <returns>
        /// The flattened 3D index for a 1D array.
        /// </returns>
        public static int Convert(int3 indices, int width) {
            return IndexConverter.Convert(indices.x, indices.y, indices.z, width);
        }

        /// <summary>
        /// Converts an [x,y,z) coordinate to a 1D array index.
        /// </summary>
        /// <param name="indices">
        /// The indices packed into a <see cref="int3"/>.
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
        public static int Convert(int3 indices, int width, int height) {
            return IndexConverter.Convert(indices.x, indices.y, indices.z, width, height);
        }

        /// <summary>
        /// Converts an [x,y,z) coordinate to a 1D array index.
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
            return IndexConverter.Convert(x, y, z, width, width);
        }

        /// <summary>
        /// Converts an [x,y,z) coordinate to a 1D array index.
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
