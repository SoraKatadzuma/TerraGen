using System;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace Simular.VaporWave {
    /// <summary>
    /// Represents a chunk of voxels and their state.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Chunk : IComponentData {
        private byte _state;

        /// <summary>
        /// Provides access to the state of the chunk.
        /// </summary>
        public ChunkState State {
            get => (ChunkState)this._state;
            set => this._state = (byte)value;
        }
    }
}
