namespace Simular.VaporWave {
    /// <summary>
    /// <para>A set of extension methods for numbers to make certain calculations easier.</para>
    /// </summary>
    public static class NumberExtensions {
        /// <summary>
        /// Remaps the value from the original range to the new range.
        /// </summary>
        /// <param name="value">The value to remap.</param>
        /// <param name="min1">The minimum value of the original range.</param>
        /// <param name="max1">The maximum value of the original range.</param>
        /// <param name="min2">The minimum value of the new range.</param>
        /// <param name="max2">The maximum value of the new range.</param>
        /// <returns>The remapped value.</returns>
        public static float Remap(this float value, float min1, float max1, float min2, float max2) {
            return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
        }

        /// <summary>
        /// Remaps the value from the original range to the new range.
        /// </summary>
        /// <param name="value">The value to remap.</param>
        /// <param name="min1">The minimum value of the original range.</param>
        /// <param name="max1">The maximum value of the original range.</param>
        /// <param name="min2">The minimum value of the new range.</param>
        /// <param name="max2">The maximum value of the new range.</param>
        /// <returns>The remapped value.</returns>
        public static int Remap(this int value, int min1, int max1, int min2, int max2) {
            return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
        }
    }
}
