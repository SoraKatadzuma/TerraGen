using System.Runtime.InteropServices;

namespace Simular.VaporWave.Experimental {
  /// <summary>
  /// <para>
  /// Specifies a climate as a packed unsigned short which can be decomposed into it's parts.
  /// </para>
  /// <para>
  /// Defines a climate at a specific terrain layer, temperature, and humidity. Used in selecting
  /// a biome which contains an algorithm for generating the type of terrain expected at that biome.
  /// </para>
  /// </summary>
  [StructLayout(LayoutKind.Explicit, Pack = 2)]
  public struct Climate {
    /// <summary>
    /// The value of the climate as a packed unsigned short.
    /// </summary>
    private ushort value;

    /// <summary>
    /// Decomposes the climate value into it's temperature enumeration.
    /// </summary>
    public Temperature Temperature {
      get => (Temperature)(value & 0x0F);
      set => this.value = (ushort)((this.value & 0x0FF0) | (ushort)value);
    }

    /// <summary>
    /// Decomposes the climate value into it's humidity enumeration.
    /// </summary>
    public Humidity Humidity {
      get => (Humidity)((value >> 4) & 0x0F);
      set => this.value = (ushort)((this.value & 0x0F0F) | (ushort)value << 4);
    }

    /// <summary>
    /// Decomposes the climate value into it's terrain layer enumeration.
    /// </summary>
    public TerrainLayer TerrainLayer {
      get => (TerrainLayer)((value >> 8) & 0x0F);
      set => this.value = (ushort)((this.value & 0x00FF) | (ushort)value << 8);
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
    /// <param name="terrainLayer">
    /// The terrain layer of the climate.
    /// </param>
    public Climate(Temperature temperature, Humidity humidity, TerrainLayer terrainLayer) {
      value  = (ushort)temperature;
      value |= (ushort)((ushort)humidity << 4);
      value |= (ushort)((ushort)terrainLayer << 8);
    }
  }
}
