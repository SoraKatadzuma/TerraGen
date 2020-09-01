using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Prng = System.Random;

/// <summary>
/// Describes settings for a fractal noise generator.
/// </summary>
[System.Serializable]
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
  public float value(float2 point) {
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
  public float value(float3 point) {
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
  public float value(float4 point) {
    return noise.snoise(point);
  }

  /// <summary>
  /// Generates a fractal map of noise based on the generator settings.
  /// </summary>
  /// <returns>
  /// The a map of the noise field based on the generator settings.
  /// </returns>
  public float[,] fractal2D() {
    var prng      = new Prng(settings.seed);
    var noise     = new float[settings.size, settings.size];
    var sample    = new float2();
    var output    = 0.0f;
    var frequency = settings.frequency;
    var amplitude = settings.amplitude;
    var maxNoise  = float.MinValue;
    var minNoise  = float.MaxValue;

    // Loop through indices.
    for (int y = 0; y < settings.size; y++) {
    for (int x = 0; x < settings.size; x++) {
      // Reset variables.
      amplitude = settings.amplitude;
      frequency = settings.frequency;
      output    = 0;

      // Loop through octaves.
      for (int octave = 0; octave < settings.octaves; octave++) {
        sample.x   = (x + settings.offset.x) / settings.scale * frequency;
        sample.y   = (y + settings.offset.y) / settings.scale * frequency;
        output     = value(sample) * 2 - 1;
        output    *= amplitude;
        amplitude *= settings.persistence;
        frequency *= settings.lacunarity;
      }

      // Make sure to update noise scales.
      noise[x, y] = output;
      if (output > maxNoise) maxNoise = output;
      if (output < minNoise) minNoise = output;
    }}

    // Normalize noise.
    for (int y = 0; y < settings.size; y++)
    for (int x = 0; x < settings.size; x++)
      noise[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noise[x, y]);

    // Send back the noise we generated.
    return noise;
  }

  /// <summary>
  /// Generates a fractal map of noise based on the generator settings.
  /// </summary>
  /// <returns>
  /// The a map of the noise field based on the generator settings.
  /// </returns>
  public float[,,] fractal3D() {
    // var prng      = new Prng(settings.seed);
    var noise     = new float[settings.size, settings.size, settings.size];
    var sample    = new float3();
    var output    = 0.0f;
    var frequency = settings.frequency;
    var amplitude = settings.amplitude;
    var maxNoise  = float.MinValue;
    var minNoise  = float.MaxValue;

    // Loop through indices.
    for (int z = 0; z < settings.size; z++) {
    for (int y = 0; y < settings.size; y++) {
    for (int x = 0; x < settings.size; x++) {
      // Reset variables.
      amplitude = settings.amplitude;
      frequency = settings.frequency;
      output    = 0;

      // Loop through octaves.
      for (int octave = 0; octave < settings.octaves; octave++) {
        sample.x   = (x + settings.offset.x) / settings.scale * frequency;
        sample.y   = (y + settings.offset.y) / settings.scale * frequency;
        sample.z   = (z + settings.offset.z) / settings.scale * frequency;
        output     = value(sample) * 2 - 1;
        output    *= amplitude;
        amplitude *= settings.persistence;
        frequency *= settings.lacunarity;
      }

      // Make sure to update noise scales.
      noise[x, y, z] = output;
      if (output > maxNoise) maxNoise = output;
      if (output < minNoise) minNoise = output;
    }}}

    // Normalize noise.
    for (int z = 0; z < settings.size; z++)
    for (int y = 0; y < settings.size; y++)
    for (int x = 0; x < settings.size; x++)
      noise[x, y, z] = Mathf.InverseLerp(minNoise, maxNoise, noise[x, y, z]);

    // Send back the noise we generated.
    return noise;
  }
}
