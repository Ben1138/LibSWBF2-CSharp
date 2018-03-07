using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.Types {
    /// <summary>
    /// A 3-Dimensional Vector
    /// </summary>
    [TypeConverter(typeof(Vector3Converter))]
    public struct Vector3 {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() {
            return string.Format("{0}-{1}-{2}", X, Y, Z);
        }
    }
}
