using System;
using System.Text;

namespace LibSWBF2 {
    public static class Extensions {
        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static string GetString(this byte[] bytes) {
            Encoding encoding = Encoding.ASCII;
            char[] chars = encoding.GetChars(bytes, 0, bytes.Length);

            return new string(chars);
        }
    }
}
