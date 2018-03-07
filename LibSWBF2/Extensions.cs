using System;
using System.Drawing;
using System.Text;

namespace LibSWBF2 {
    public static class Extensions {
        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static Color ColorFromNormalized(float r, float g, float b, float a) {
            r = Math.Clamp01(r);
            g = Math.Clamp01(g);
            b = Math.Clamp01(b);
            a = Math.Clamp01(a);

            return Color.FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static void ColorToNormalized(Color color, out float r, out float g, out float b, out float a) {
            r = color.R / (float)255;
            g = color.G / (float)255;
            b = color.B / (float)255;
            a = color.A / (float)255;
        }

        public static string GetString(this byte[] bytes) {
            Encoding encoding = Encoding.ASCII;
            char[] chars = encoding.GetChars(bytes, 0, bytes.Length);

            return new string(chars);
        }
    }
}
