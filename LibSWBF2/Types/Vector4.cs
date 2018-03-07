using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.Types {
    /// <summary>
    /// A 4-Dimensional Vector
    /// </summary>
    [TypeConverter(typeof(Vector4Converter))]
    public struct Vector4 {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector4(float x, float y, float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString() {
            return string.Format("{0}-{1}-{2}-{3}", X, Y, Z, W);
        }
    }
}
