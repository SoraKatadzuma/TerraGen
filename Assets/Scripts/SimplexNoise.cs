using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

/// <summary>
/// Describes settings for a fractal noise generator.
/// </summary>
[Serializable]
public struct NoiseSettings {
  /// <summary>
  /// The offset of the noise.
  /// </summary>
  public float3 offset;

  /// <summary>
  /// The scale of the noise to be generated.
  /// </summary>
  public float scale;

  /// <summary>
  /// Frequency multiplier between successive octaves.
  /// </summary>
  public float lacunarity;

  /// <summary>
  /// How quickly the amplitude diminishes for each octave.
  /// </summary>
  public float persistence;

  /// <summary>
  /// Starting wave length.
  /// </summary>
  public float frequency;

  /// <summary>
  /// Starting displacement of the wave.
  /// </summary>
  public float amplitude;

  /// <summary>
  /// The seed that will be used to generate noise.
  /// </summary>
  public int seed;

  /// <summary>
  /// The size of noise field to generate from.
  /// </summary>
  public int size;

  /// <summary>
  /// The number of octaves of fractal noise to generate.
  /// </summary>
  public byte octaves;
}

/// <summary>
/// Generates simplex noise.
/// </summary>
[BurstCompile]
public struct SimplexNoise {
  /// <summary>
  /// The settings of a fractal noise generator.
  /// </summary>
  public NoiseSettings settings;

  /// <summary>
  /// Creates a new simplex noise generator with the given noise settings.
  /// </summary>
  /// <param name="settings">
  /// The settings for fractal noise generation.
  /// </param>
  public SimplexNoise(NoiseSettings settings) {
    this.settings = settings;
    if (settings.scale <= 0.0f)
      settings.scale = 0.0001f;
  }

  /// <summary>
  /// Generates a noise value for the given 2D point.
  /// </summary>
  /// <param name="point">
  /// The point to sample the noise field from.
  /// </param>
  /// <returns>
  /// The value at the sample point of the field.
  /// </returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float sample(float2 point) {
    return noise.snoise(point);
  }

  /// <summary>
  /// Generates a noise value for the given 3D point.
  /// </summary>
  /// <param name="point">
  /// The point to sample the noise field from.
  /// </param>
  /// <returns>
  /// The value at the sample point in the field.
  /// </returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float sample(float3 point) {
    return noise.snoise(point);
  }

  /// <summary>
  /// Generates a noise value for the given 4D point.
  /// </summary>
  /// <param name="point">
  /// The point to sample the noise field from.
  /// </param>
  /// <returns>
  /// The value at the sample point in the field.
  /// </returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float sample(float4 point) {
    return noise.snoise(point);
  }

  /// <summary>
  /// Samples a single fractal value at the given point.
  /// </summary>
  /// <param name="point">
  /// The point to sample the fractal noise at.
  /// </param>
  /// <returns>
  /// The fractal noise value at the point.
  /// </returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float sampleFractal(float2 point) {
    // Loop through octaves and layer noise.
    var frequency  = settings.frequency;
    var amplitude  = settings.amplitude;
    var result     = 0.0f;
    var scaledFreq = 0.0f;
    var loc        = point;
    for (int octave = 0; octave < settings.octaves; octave++) {
      scaledFreq = settings.scale * frequency;
      loc.x      = point.x / scaledFreq;
      loc.y      = point.y / scaledFreq;
      result     = sample(loc) * amplitude;
      amplitude *= settings.persistence;
      frequency *= settings.lacunarity;
    }

    // Return the layered noise value.
    return result;
  }

  /// <summary>
  /// Generates a fractal map of noise based on the generator settings.
  /// </summary>
  /// <returns>
  /// The a map of the noise field based on the generator settings.
  /// </returns>
  public NativeArray<float> fractal2D() {
    var size      = settings.size * settings.size;
    var noise     = new NativeArray<float>(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
    var sampleLoc = new float2();
    var index     = 0;

    // Loop through indices.
    for (int y = 0; y < settings.size; y++) {
    for (int x = 0; x < settings.size; x++) {
      index        = y * settings.size + x;
      sampleLoc    = new float2(x + settings.offset.x, y + settings.offset.y);
      noise[index] = sampleFractal(sampleLoc);
    }}

    // Send back the noise we generated.
    return noise;
  }

  /// <summary>
  /// Samples a single fractal value at the given point.
  /// </summary>
  /// <param name="point">
  /// The point to sample the fractal noise at.
  /// </param>
  /// <returns>
  /// The fractal noise value at the point.
  /// </returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float sampleFractal(float3 point) {
    // Loop through octaves and layer noise.
    var frequency  = settings.frequency;
    var amplitude  = settings.amplitude;
    var result     = 0.0f;
    var scaledFreq = 0.0f;
    var loc        = point;
    for (int octave = 0; octave < settings.octaves; octave++) {
      scaledFreq = settings.scale * frequency;
      loc.x      = point.x / scaledFreq;
      loc.y      = point.y / scaledFreq;
      loc.z      = point.z / scaledFreq;
      result     = sample(loc) * amplitude;
      amplitude *= settings.persistence;
      frequency *= settings.lacunarity;
    }

    // Return the layered noise value.
    return result;
  }

  /// <summary>
  /// Generates a fractal map of noise based on the generator settings.
  /// </summary>
  /// <returns>
  /// The a map of the noise field based on the generator settings.
  /// </returns>
  public NativeArray<float> fractal3D() {
    var size      = settings.size * settings.size;
    var noise     = new NativeArray<float>(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
    var sampleLoc = new float3();
    var index     = 0;

    // Loop through indices.
    for (int y = 0; y < settings.size; y++) {
    for (int x = 0; x < settings.size; x++) {
    for (int z = 0; z < settings.size; z++) {
      index        = (z * settings.size * settings.size) + (y * settings.size) + x;
      sampleLoc    = new float3(x + settings.offset.x, y + settings.offset.y, z + settings.offset.z);
      noise[index] = sampleFractal(sampleLoc);
    }}}

    // Send back the noise we generated.
    return noise;
  }
}
