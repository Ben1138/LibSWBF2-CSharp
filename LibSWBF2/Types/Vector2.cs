using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.Types {
    /// <summary>
    /// A 2-Dimensional Vector
    /// </summary>
    [TypeConverter(typeof(Vector2Converter))]
    public struct Vector2 {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return string.Format("{0}-{1}", X, Y);
        }
    }
}
