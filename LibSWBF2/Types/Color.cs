using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.Types {
    /// <summary>
    /// Representing a Color
    /// </summary>
    [TypeConverter(typeof(ColorConverter))]
    public class Color {

        /// <summary>
        /// Red Value (Range 0 to 1)
        /// </summary>
        public float R {
            get { return r; }
            set { r = Math.Clamp01(value); }
        }
        private float r;

        /// <summary>
        /// Green Value (Range 0 to 1)
        /// </summary>
        public float G {
            get { return g; }
            set { g = Math.Clamp01(value); }
        }
        private float g;

        /// <summary>
        /// Blue Value (Range 0 to 1)
        /// </summary>
        public float B {
            get { return b; }
            set { b = Math.Clamp01(value); }
        }
        private float b;

        /// <summary>
        /// Alpha Value (Range 0 to 1, 1 being fully opaque)
        /// </summary>
        public float A {
            get { return a; }
            set { a = Math.Clamp01(value); }
        }
        private float a;


        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class (with RGBA values 0, 0, 0, 0)
        /// </summary>
        public Color() {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">Red Value (Range 0 to 1)</param>
        /// <param name="g">Green Value (Range 0 to 1)</param>
        /// <param name="b">Blue Value (Range 0 to 1)</param>
        /// <param name="a">Alpha Value (Range 0 to 1, 1 being fully opaque)</param>
        public Color(float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString() {
            return R + "-" + G + "-" + B + "-" + A;
        }
    }
}
