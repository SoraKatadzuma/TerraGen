using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  ///
  /// </summary>
  public partial class TerrainGenerator : SystemBase {
    private readonly NativeQueue<int3> _chunksToLoad = new(Allocator.Persistent);
    private readonly NativeQueue<int3> _chunksToUnload = new(Allocator.Persistent);
    private readonly NativeList<JobHandle> _runningJobs = new(Allocator.Persistent);


    private readonly Material _material = new(Shader.Find("Standard"));
    private readonly Dictionary<int3, Entity> _loadedChunks = new();

    /// <summary>
    /// Adds the given chunks to the queue of chunks to load.
    /// </summary>
    /// <param name="chunks">
    /// An array of chunks to queue for load.
    /// </param>
    public void QueueChunksForLoad(params int3[] chunks) {
      foreach (var chunk in chunks)
        _chunksToLoad.Enqueue(chunk);
    }

    /// <summary>
    /// Adds the given chunks to the queue of chunks to unload.
    /// </summary>
    /// <param name="chunks">
    /// An array of chunks to queue for unload.
    /// </param>
    public void UnloadChunks(params int3[] chunks) {
      foreach (var chunk in chunks)
        _chunksToUnload.Enqueue(chunk);
    }

    protected override void OnCreate() {
    }

    protected override void OnDestroy() {
      foreach (var jobHandle in _runningJobs) {
        jobHandle.Complete();
      }
    }

    protected override void OnUpdate() {
      ScheduleChunksForLoad();
      UnscheduleChunksForUnload();
      if (_runningJobs.Length == 0)
        return;

      FinishExistingJobs();
      ScheduleNewJobs();
    }

    private void ScheduleChunksForLoad() {
      // Sanitize chunks.

    }

    private void UnscheduleChunksForUnload() {

    }

    private void FinishExistingJobs() {

    }

    private void ScheduleNewJobs() {

    }
  }
}
