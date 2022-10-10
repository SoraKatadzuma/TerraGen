using System;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Simular.VaporWave {
    /// <summary>
    /// <para>
    /// Specifies a climate as a packed unsigned short which can be decomposed into it's parts.
    /// </para>
    /// <para>
    /// Defines a climate at a specific terrain layer, temperature, and humidity. Used in selecting
    /// a biome which contains an algorithm for generating the type of terrain expected at that biome.
    /// </para>
    /// </summary>
    [Serializable]
    [GenerateTestsForBurstCompatibility]
    [StructLayout(LayoutKind.Sequential)]
    public struct Climate {
        /// <summary>
        /// <para>
        /// Contains settings used in the sampling of noise during selection of a climate and
        /// generation of voxels for a world chunk.
        /// </para>
        /// </summary>
        [Serializable]
        [GenerateTestsForBurstCompatibility]
        [StructLayout(LayoutKind.Sequential)]
        public struct Entropy {
            /// <summary>
            /// Provides more randomness to the noise generation by offsetting the sample location
            /// per octave.
            /// </summary>
            /// <remarks>
            /// Must be provided and managed by caller, and must have a length that equals the
            /// number of octaves that will be sampled.
            /// </remarks>
            public NativeArray<float3> offsets;

            /// <summary>
            /// The noise function to use to generate the entropy for the climate.
            /// </summary>
            public NoiseFunction function;
            
            /// <summary>
            /// Controls the frequency strength per octave of noise.
            /// </summary>
            /// <remarks>
            /// Causes the frequency to increase each octave.
            /// </remarks>
            public float lacunarity;
            
            /// <summary>
            /// Controls the amplitude strength per octave of noise.
            /// </summary>
            /// <remarks>
            /// Causes the amplitude to decrease each octave.
            /// </remarks>
            public float persistence;
            
            /// <summary>
            /// Controls the overall strength of the noise, amplifying both lacunarity and
            /// persistence per octave.
            /// </summary>
            public float noiseScale;
            
            /// <summary>
            /// The number of times to sample the noise, which creates the fractal effect of the
            /// noise.
            /// </summary>
            /// <remarks>
            /// Setting this high will eat CPU resources. It's recommended not to go above 8, you
            /// may not even see a difference after 4.
            /// </remarks>
            public int octaves;
        }

        /// <summary>
        /// <para>
        /// Contains settings used in the selection of a climate and generation of voxels for a
        /// world chunk.
        /// </para>
        /// </summary>
        [Serializable]
        [GenerateTestsForBurstCompatibility]
        [StructLayout(LayoutKind.Sequential)]
        public struct Settings {
            private Random _deviator;

            /// <summary>
            /// <para>
            /// Describes the variability of temperatures for a world's climates.
            /// </para>
            /// <para>
            /// This variability is used in determining a random deviation from the normal
            /// temperatures for a world's climates. The random deviation is applied at climate
            /// selection time by a solver.
            /// </para>
            /// </summary>
            public float2 temperatureVariability;

            /// <summary>
            /// <para>
            /// Describes the variability of humidity for a world's climates.
            /// </para>
            /// <para>
            /// This variability is used in determining a random deviation from the normal humidity
            /// for a world's climates. The random deviation is applied at climate selection time by
            /// a solver.
            /// </para>
            /// </summary>
            public float2 humidityVariability;

            /// <summary>
            /// <para>
            /// Describes the variability of stratum for a world's climates.
            /// </para>
            /// <para>
            /// This variability is used in determining a random deviation from the normal stratum
            /// for a world's climates. The random deviation is applied at climate selection time by
            /// a solver.
            /// </para>
            /// </summary>
            public float2 stratumVariability;

            /// <summary>
            /// <para>
            /// Describes the variability of latitude for a world's climates.
            /// </para>
            /// <para>
            /// This variability is used in determining a random deviation from the normal latitude
            /// for a world's climates. The random deviation is applied at climate selection time by
            /// a solver.
            /// </para>
            /// </summary>
            /// <remarks>
            /// <para>
            /// This is a unique case. Some generation is 3D, based on real spherical planets with
            /// real climate and atmospheric dynamics. This variability is designed for that. It is
            /// primarily used to shift the sample point of the latitude in order to offset the
            /// temperature and humidity in a more realistic manner.
            /// </para>
            /// </remarks>
            public float2 latitudeVariability;
            
            /// <summary>
            /// <para>
            /// Whether or not a world's climates are planetary.
            /// </para>
            /// <para>
            /// Whether or not the climate being selected is planetary, meaning, it abides by
            /// planetary atmospheric dynamics. When set true, it will evaluate temperature and
            /// humidity based on a latitudinal angle derived from the coordinates provided.
            /// Otherwise it will use linear interpolations and deviations to select all of the
            /// parts of the climate.
            /// </para>
            /// </summary>
            public bool planetary;

        #region Properties
            /// <summary>
            /// Samples a random deviation value between the variability minimum and maximum for
            /// a climate's temperature.
            /// </summary>
            public float RandomTemperatureDeviation {
                get => this._deviator.NextFloat(this.temperatureVariability.x,
                                                this.temperatureVariability.y);
            }

            /// <summary>
            /// Samples a random deviation value between the variability minimum and maximum for
            /// a climate's humidity.
            /// </summary>
            public float RandomHumidityDeviation {
                get => this._deviator.NextFloat(this.humidityVariability.x,
                                                this.humidityVariability.y);
            }

            /// <summary>
            /// Samples a random deviation value between the variability minimum and maximum for
            /// a climate's stratum.
            /// </summary>
            public float RandomStratumDeviation {
                get => this._deviator.NextFloat(this.stratumVariability.x,
                                                this.stratumVariability.y);
            }

            /// <summary>
            /// Samples a random deviation value between the variability minimum and maximum for
            /// a latitude.
            /// </summary>
            public float RandomLatitudeDeviation {
                get => this._deviator.NextFloat(this.latitudeVariability.x,
                                                this.latitudeVariability.y);
            }
        #endregion

            /// <summary>
            /// Creates a new <c>Climate.Settings</c> object for use in climate selection and voxel
            /// generation.
            /// </summary>
            /// <param name="seed">
            /// The seed for the pseudo random number generator used to sample random deviations for
            /// the different parts of a world's climate.
            /// </param>
            public Settings(in uint seed = 65535) {
                this._deviator              = new Random(seed);
                this.temperatureVariability = default;
                this.humidityVariability    = default;
                this.stratumVariability     = default;
                this.latitudeVariability    = default;
                this.planetary              = default;
            }
        }


        /// <summary>
        /// Defines parameters for climate selection.
        /// </summary>
        [GenerateTestsForBurstCompatibility]
        [StructLayout(LayoutKind.Sequential)]
        public struct SelectionParams {
            /// <summary>
            /// The settings for climates of a given world.
            /// </summary>
            public Settings climateSettings;
            
            /// <summary>
            /// The settings for climate entropy.
            /// </summary>
            public Entropy entropySettings;
            
            /// <summary>
            /// The coordinates to generate the climate for.
            /// </summary>
            public float3 climateCoords;
            
            /// <summary>
            /// The limits of the terrain, used in mapping the stratum.
            /// </summary>
            public int2 terrainLimits;
        }
        
        private short value;

    #if UNITY_EDITOR
        [Tooltip("Sets the temperature for this climate.")]
        public Temperature temperature;

        [Tooltip("Sets the humidity for this climate.")]
        public Humidity humidity;

        [Tooltip("Sets the stratum for this climate.")]
        public Stratum stratum;
    #endif

        /// <summary>
        /// Decomposes the climate value into it's temperature enumeration.
        /// </summary>
        public Temperature Temperature {
            get => (Temperature)(this.value & 0x0F);
            set => this.value = (short)((this.value & 0x0FF0) | (byte)value);
        }

        /// <summary>
        /// Decomposes the climate value into it's humidity enumeration.
        /// </summary>
        public Humidity Humidity {
            get => (Humidity)((this.value >> 4) & 0x0F);
            set => this.value = (short)((this.value & 0x0F0F) | (short)value << 4);
        }

        /// <summary>
        /// Decomposes the climate value into it's terrain layer enumeration.
        /// </summary>
        public Stratum Stratum {
            get => (Stratum)((this.value >> 8) & 0x0F);
            set => this.value = (short)((this.value & 0x00FF) | (short)value << 8);
        }

        /// <summary>
        /// Creates the climate value from the provided temperature and humidity.
        /// </summary>
        /// <param name="temperature">
        /// The temperature of the climate.
        /// </param>
        /// <param name="humidity">
        /// The humidity of the climate.
        /// </param>
        /// <param name="stratum">
        /// The terrain layer of the climate.
        /// </param>
        public Climate(Temperature temperature = Temperature.Tropical,
                       Humidity humidity = Humidity.Arid,
                       Stratum stratum = Stratum.Abyss) {
            this.value =  (short)temperature;
            this.value |= (short)((short)humidity << 4);
            this.value |= (short)((short)stratum << 8);

        #if UNITY_EDITOR
            this.temperature = temperature;
            this.humidity    = humidity;
            this.stratum     = stratum;
        #endif
        }

        /// <summary>
        /// Selects a climate given the selection parameters.
        /// </summary>
        /// <param name="params">
        /// The parameters needed to select a climate.
        /// </param>
        /// <returns>
        /// The selected climate.
        /// </returns>
        [BurstCompile]
        public static Climate SelectClimate(in SelectionParams @params) {
            if (@params.climateSettings.planetary)
                return Climate.SelectPlanetaryClimate(@params);
            
            /* Apply random amplification to variations to create the deviation, which will be used
             * as the basis for selection purposes. Note that the sampled coordinate swaps the Y and
             * Z axis. This allows the noise to be treated like layers, which is useful for heightmap
             * generation.*/
            var cartesian            = @params.climateCoords;
            var variation            = Climate.SampleFractal(cartesian.xzy, @params.entropySettings);
            var temperatureDeviation = variation * @params.climateSettings.RandomTemperatureDeviation;
            var humidityDeviation    = variation * @params.climateSettings.RandomHumidityDeviation;
            var stratumDeviation     = variation * @params.climateSettings.RandomStratumDeviation;
            
            // Select climate stratum and calculate climate bias.
            var stratum = Climate.SelectStratum(@params.terrainLimits, cartesian.y, stratumDeviation);
            var bias    = stratum.ClimateBias();
            
            // Select temperature and humidity, return climate results.
            var settings    = @params.climateSettings;
            var temperature = Climate.SelectTemperature(settings.temperatureVariability, temperatureDeviation, bias);
            var humidity    = Climate.SelectHumidity(settings.humidityVariability, humidityDeviation, bias);
            return new Climate(temperature, humidity, stratum);
        }

        /// <summary>
        /// Selects a climate for a planetary body, given the selection parameters.
        /// </summary>
        /// <param name="params">
        /// The parameters needed to select a climate.
        /// </param>
        /// <returns>
        /// The selected climate.
        /// </returns>
        [BurstCompile]
        public static Climate SelectPlanetaryClimate(in SelectionParams @params) {
            // Select stratum and latitude variations. Stratum is 3D based, latitude is x,z plane based.
            var cartesian         = @params.climateCoords;
            var spherical         = cartesian.ToSpherical();
            var latitudeVariation = Climate.SampleFractal(new float3(cartesian.xz, 0f), @params.entropySettings);
            var stratumVariation  = Climate.SampleFractal(cartesian, @params.entropySettings);
            
            // Apply random amplification to variations to create final deviation.
            var temperatureDeviation = latitudeVariation * @params.climateSettings.RandomLatitudeDeviation;
            var humidityDeviation    = latitudeVariation * @params.climateSettings.RandomLatitudeDeviation;
            var stratumDeviation     = stratumVariation * @params.climateSettings.RandomStratumDeviation;
            
            // Select climate stratum and calculate climate bias.
            var stratum = Climate.SelectStratum(@params.terrainLimits, spherical.x, stratumDeviation);
            var bias    = stratum.ClimateBias();
            
            // Select temperature and humidity, return climate results.
            var temperature = Climate.SelectPlanetaryTemperature(spherical.z, temperatureDeviation, bias);
            var humidity    = Climate.SelectPlanetaryHumidity(temperature, humidityDeviation, bias);
            return new Climate(temperature, humidity, stratum);
        }

        /// <summary>
        /// Selects a temperature for a given basis.
        /// </summary>
        /// <param name="variability">
        /// The variability of the temperature.
        /// </param>
        /// <param name="basis">
        /// A temperature generated from a deviation from 0.
        /// </param>
        /// <param name="bias">
        /// The bias of the temperature derived from the stratum.
        /// </param>
        /// <returns>
        /// The weighted basis remapped into the <see cref="VaporWave.Temperature"/> range.
        /// </returns>
        [BurstCompile]
        public static Temperature SelectTemperature(in float2 variability,
                                                    in float basis,
                                                    in int bias) {
            return (Temperature)math.clamp(
                basis.Remap(variability.x, variability.y, 0, 8) - bias,
                (int)Temperature.Tropical,
                (int)Temperature.Polar
            );
        }
        
        /// <summary>
        /// Selects a temperature based on a given latitude.
        /// </summary>
        /// <param name="latitude">
        /// The latitude to select the temperature of.
        /// </param>
        /// <param name="deviation">
        /// How much the latitude should deviate at the location it's derived.
        /// </param>
        /// <param name="bias">
        /// The bias of the temperature derived from the stratum.
        /// </param>
        /// <returns>
        /// The temperature of the latitude.
        /// </returns>
        [BurstCompile]
        public static Temperature SelectPlanetaryTemperature(in float latitude,
                                                             in float deviation,
                                                             in int bias) {
            // Add variation to the latitude, clamp to range, remap to temperature range, apply bias,
            // clamp range again. The bias is subtracted because it is inversely proportionate to the
            // temperature.
            const float halfPI = math.PI / 2f;

            var temperature = math.clamp(latitude + deviation, 0, halfPI);
            temperature = temperature.Remap(0, halfPI, 0, 8) - bias;
            
            return (Temperature)math.clamp(
                temperature, 
                (int)Temperature.Tropical,
                (int)Temperature.Polar
            );
        }
        
        /// <summary>
        /// Selects a humidity for a given basis.
        /// </summary>
        /// <param name="variability">
        /// The variability of the humidity.
        /// </param>
        /// <param name="basis">
        /// A humidity generated from a deviation from 0.
        /// </param>
        /// <param name="bias">
        /// The bias of the humidity derived from the stratum.
        /// </param>
        /// <returns>
        /// The weighted basis remapped into the <see cref="VaporWave.Humidity"/> range.
        /// </returns>
        [BurstCompile]
        public static Humidity SelectHumidity(in float2 variability,
                                              in float basis,
                                              in int bias) {
            return (Humidity)math.clamp(
                basis.Remap(variability.x, variability.y, 0, 8) + bias,
                (int)Humidity.SuperArid,
                (int)Humidity.SuperHumid
            );
        }

        /// <summary>
        /// Selects a humidity based on a given temperature.
        /// </summary>
        /// <param name="temperature">
        /// The temperature to act as the basis for the humidity.
        /// </param>
        /// <param name="deviation">
        /// How much the humidity should deviate at the location it's derived.
        /// </param>
        /// <param name="bias">
        /// A bias for the humidity based on the stratum of the location.
        /// </param>
        /// <returns>
        /// An integer value that represents the humidity of the temperature.
        /// </returns>
        [BurstCompile]
        public static Humidity SelectPlanetaryHumidity(in Temperature temperature,
                                                       in float deviation,
                                                       in int bias) {
            // Humidity is inversely proportionate to the temperature, and bias is inversely
            // proportionate to temperature. This means that we need to add the bias to the humidity
            // for it to balance the climate for the location.
            var humidity = (int)temperature + deviation + bias;
            return (Humidity)math.clamp(humidity, (int)Humidity.SuperArid, (int)Humidity.SuperHumid);
        }
        
        /// <summary>
        /// Selects a stratum based on a given elevation.
        /// </summary>
        /// <param name="limits">
        /// The bounds of the elevation.
        /// </param>
        /// <param name="elevation">
        /// The elevation from the center of the world.
        /// </param>
        /// <param name="deviation">
        /// How much should the elevation deviate at the location it's derived.
        /// </param>
        /// <returns>
        /// A byte value that represents the stratum of the elevation.
        /// </returns>
        [BurstCompile]
        public static Stratum SelectStratum(in int2 limits,
                                            in float elevation,
                                            in float deviation) {
            // Add variation to the elevation, clamp to range, remap into Stratum range.
            var min = limits.x * limits.x;
            var max = limits.y * limits.y;
            var res = math.clamp(elevation + deviation, limits.x, limits.y);
            return (Stratum)(byte)res.Remap(min, max, (int)Stratum.Abyss, (int)Stratum.Nival);
        }
        
        
        [BurstCompile]
        private static float SampleFractal(in float3 coords, in Entropy entropy) {
            var result    = 0f;
            var frequency = 1f;
            var amplitude = 1f;
            for (var octave = 0; octave < entropy.octaves; octave++) {
                var zoomFreq = entropy.noiseScale * frequency;
                var zoomAmpl = entropy.noiseScale * amplitude;
                var location = coords / zoomFreq + entropy.offsets[octave];
                result = entropy.function switch {
                    NoiseFunction.Perlin  => noise.cnoise(location) * zoomAmpl,
                    NoiseFunction.Simplex => noise.snoise(location) * zoomAmpl,
                    _                     => throw new IndexOutOfRangeException(),
                };

                frequency *= entropy.lacunarity;
                amplitude *= entropy.persistence;
            }

            return result;
        }
    }
}
