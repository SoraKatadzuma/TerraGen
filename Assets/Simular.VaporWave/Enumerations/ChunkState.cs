namespace Simular.VaporWave {
    /// <summary>
    /// Represents the different states that the chunk can be in.
    /// </summary>
    public enum ChunkState {
        Created, Loaded, Dirty, MeshReady, MarkedForDelete,
    }
}
