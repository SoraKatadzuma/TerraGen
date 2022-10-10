using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Simular.VaporWave {
    /// <summary>
    /// 
    /// </summary>
    public partial struct TerrainManagementSystem : ISystem {
        /// <summary>
        /// Adds the given chunks to the queue of chunks to load.
        /// </summary>
        /// <param name="chunks">
        /// An array of chunks to queue for load.
        /// </param>
        public static unsafe void QueueChunksForLoad(NativeList<int3> chunksToLoad,
                                                     params int3[] chunks) {
            fixed (int3* pChunks = chunks)
                chunksToLoad.AddRange(pChunks, chunks.Length);
        }

        /// <summary>
        /// Adds the given chunks to the queue of chunks to unload.
        /// </summary>
        /// <param name="chunks">
        /// An array of chunks to queue for unload.
        /// </param>
        public static unsafe void QueueChunksForUnload(NativeList<int3> chunksToUnload,
                                                       params int3[] chunks) {
            fixed (int3* pChunks = chunks)
                chunksToUnload.AddRange(pChunks, chunks.Length);
        }
        
        void ISystem.OnCreate(ref SystemState state) {
            state.EntityManager.AddComponentData(
                state.SystemHandle,
                new TerrainManagementData {
                    entityArchetype = state.EntityManager.CreateArchetype(
                        ComponentType.ReadWrite<Voxel>(),
                        ComponentType.ReadOnly<LocalToWorldTransform>()
                    ),
                    voxelLookup    = state.GetBufferLookup<Voxel>(true),
                    chunksToLoad   = new NativeList<int3>(Allocator.Persistent),
                    chunksToUnload = new NativeList<int3>(Allocator.Persistent),
                    loadedChunks   = new NativeHashMap<int3, Entity>(64, Allocator.Persistent),
                    chunkSize      = 16,
                }
            );
        }

        void ISystem.OnDestroy(ref SystemState state) {
        }

        void ISystem.OnUpdate(ref SystemState state) {
            var data = state.EntityManager.GetComponentData<TerrainManagementData>(state.SystemHandle);
            this.ScheduleChunksForUnload(ref state, ref data);
            this.ScheduleChunksForLoad(ref state, ref data);
        }
        
        private void ScheduleChunksForLoad(ref SystemState state, ref TerrainManagementData data) {
            if (data.chunksToLoad.Length == 0)
                return;

            var entities = new NativeArray<Entity>(data.chunksToLoad.Length, Allocator.Temp);
            state.EntityManager.CreateEntity(data.entityArchetype, entities);
            for (var index = 0; index < data.chunksToLoad.Length; index++) {
                var entity    = entities[index];
                var position  = data.chunksToLoad[index];
                var transform = state.EntityManager.GetComponentData<LocalToWorldTransform>(entity);

                transform.Value.Position = position * data.chunkSize;
                transform.Value.Scale    = 1f;
                
                state.EntityManager.SetComponentData(entity, transform);
            }

            data.chunksToLoad.Clear();
        }

        private void ScheduleChunksForUnload(ref SystemState state, ref TerrainManagementData data) {
            //
        }
    }
}
