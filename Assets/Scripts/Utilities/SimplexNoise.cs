using System;
using System.Runtime.CompilerServices;
using sora.TerraGen;
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
  public float Sample(float2 point) {
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
  public float Sample(float3 point) {
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
  public float Sample(float4 point) {
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
  public float SampleFractal(float2 point) {
    // Loop through octaves and layer noise.
    var frequency  = settings.frequency;
    var amplitude  = settings.amplitude;
    var result     = 0.0f;
    var loc        = point;
    for (var octave = 0; octave < settings.octaves; octave++) {
      var scaledFreq = settings.scale * frequency;
      loc.x          = point.x / scaledFreq;
      loc.y          = point.y / scaledFreq;
      result         = Sample(loc) * amplitude;
      amplitude     *= settings.persistence;
      frequency     *= settings.lacunarity;
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
  public NativeArray<float> Fractal2D() {
    var size  = settings.size * settings.size;
    var noise = new NativeArray<float>(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

    // Loop through indices.
    for (var y = 0; y < settings.size; y++) {
      for (var x = 0; x < settings.size; x++) {
        var index    = IndexConverter.Convert(x, y, settings.size);
        noise[index] = SampleFractal(new float2 {
          x = x + settings.offset.x,
          y = y + settings.offset.y
        });
      }
    }

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
  public float SampleFractal(float3 point) {
    // Loop through octaves and layer noise.
    var frequency  = settings.frequency;
    var amplitude  = settings.amplitude;
    var result     = 0.0f;
    var loc        = point;
    for (var octave = 0; octave < settings.octaves; octave++) {
      var scaledFreq = settings.scale * frequency;

      loc.x      = point.x / scaledFreq;
      loc.y      = point.y / scaledFreq;
      loc.z      = point.z / scaledFreq;
      result     = Sample(loc) * amplitude;
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
  public NativeArray<float> Fractal3D() {
    var size      = settings.size * settings.size;
    var noise     = new NativeArray<float>(size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

    // Loop through indices.
    for (var z = 0; z < settings.size; z++) {
      for (var y = 0; y < settings.size; y++) {
        for (var x = 0; x < settings.size; x++) {
          var index    = IndexConverter.Convert(x, y, z, settings.size);
          noise[index] = SampleFractal(new float3 {
            x = x + settings.offset.x,
            y = y + settings.offset.y,
            z = z + settings.offset.z
          });
        }
      }
    }

    // Send back the noise we generated.
    return noise;
  }
}
