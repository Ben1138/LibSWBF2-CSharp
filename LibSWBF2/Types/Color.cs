using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.Types {
    /// <summary>
    /// Representing a Color
    /// </summary>
    [TypeConverter(typeof(ColorConverter))]
    public class Color {
        
        public float R {
            get { return r; }
            set { r = Math.Clamp01(value); }
        }
        private float r;

        public float G {
            get { return g; }
            set { g = Math.Clamp01(value); }
        }
        private float g;

        public float B {
            get { return b; }
            set { b = Math.Clamp01(value); }
        }
        private float b;

        public float A {
            get { return a; }
            set { a = Math.Clamp01(value); }
        }
        private float a;


        public Color() {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }

        public Color(float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
