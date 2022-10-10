using System;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace Simular.VaporWave {
    /// <summary>
    /// Represents a theoretical volume pixel, a coordinate in 3D euclidean space that has a pairing
    /// volume to it which can be used to solve a contour within that space and generate a mesh.
    /// </summary>
    [Serializable]
    [InternalBufferCapacity(512)]
    [StructLayout(LayoutKind.Sequential)]
    public struct Voxel : IBufferElementData {
        /// <summary>
        /// The identity descriptor of the voxel, helps determine whether it exists and what is at
        /// it's location. ID == 0 means that the voxel is air and nothing exists there.
        /// </summary>
        public int id;

        /// <summary>
        /// Implicitly casts a <c>Voxel</c> to an integer.
        /// </summary>
        /// <param name="v">
        /// The <c>Voxel</c> to cast to an integer.
        /// </param>
        /// <returns>
        /// The identity descriptor of the <c>Voxel</c>.
        /// </returns>
        public static implicit operator int(Voxel v) {
            return v.id;
        }

        /// <summary>
        /// Implicitly casts an integer to a <c>Voxel</c>.
        /// </summary>
        /// <param name="i">
        /// The integer to cast to a <c>Voxel</c>.
        /// </param>
        /// <returns>
        /// The constructed voxel from the integer.
        /// </returns>
        public static implicit operator Voxel(int i) {
            return new Voxel { id = i, };
        }
    }
}
