using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Simular.VaporWave {
    /// <summary>
    /// 
    /// </summary>
    public struct TerrainManagementData : IComponentData {
        /// <summary>
        /// Stores the archetype for entities that the <see cref="TerrainManagementSystem"/> will be
        /// working with.
        /// </summary>
        public EntityArchetype entityArchetype;

        /// <summary>
        /// A lookup object to pass to jobs which require access to voxels of a chunk. 
        /// </summary>
        public BufferLookup<Voxel> voxelLookup;

        /// <summary>
        /// A list of chunks that need to be loaded in the coming frames.
        /// </summary>
        public NativeList<int3> chunksToLoad;
        
        /// <summary>
        /// A list of chunks that need to be unloaded in the coming frames.
        /// </summary>
        public NativeList<int3> chunksToUnload;

        /// <summary>
        /// A hash map of already loaded chunks.
        /// </summary>
        public NativeHashMap<int3, Entity> loadedChunks;

        /// <summary>
        /// The size of the terrain chunks.
        /// </summary>
        public int chunkSize;
    }
}
