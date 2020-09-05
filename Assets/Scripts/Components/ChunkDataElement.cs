using Unity.Entities;

namespace sora.TerraGen {
  /// <summary>
  /// Represents the different types of voxels that can be generated.
  /// </summary>
  public enum Voxel : ushort {
    Undefined,
  }

  /// <summary>
  /// Represents the primitive data of a chunk. This data is strung together in a buffer defined by
  /// Unity.
  /// </summary>
  [InternalBufferCapacity(512)]
  public struct ChunkDataElement : IBufferElementData {
    /// <summary>
    /// The voxel that is stored in point in the chunk.
    /// </summary>
    public Voxel voxel;

    /// <summary>
    /// The volume data for a point in the chunk.
    /// </summary>
    public float volume;
  }
}
