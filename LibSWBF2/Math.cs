namespace LibSWBF2 {
    public static class Math {
        /// <summary>
        /// Clamps a given value between min and max
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value allowed</param>
        /// <param name="max">The maximum value allowed</param>
        /// <returns>The clamped value</returns>
        public static int Clamp(int value, int min, int max) {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Clamps a given value between min and max
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value allowed</param>
        /// <param name="max">The maximum value allowed</param>
        /// <returns>The clamped value</returns>
        public static float Clamp(float value, float min, float max) {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        /// <summary>
        /// Clamps a given value between 0 and 1
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns>The clamped value</returns>
        public static float Clamp01(float value) {
            return Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Clamps a given value between 0 and 1
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns>The clamped value</returns>
        public static int Clamp01(int value) {
            return Clamp(value, 0, 1);
        }
    }
}
