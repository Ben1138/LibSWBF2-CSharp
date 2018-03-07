using System;
using System.ComponentModel;
using System.Globalization;
using LibSWBF2.Types;

namespace LibSWBF2.TypeConverters {
    class Vector2Converter : BaseConverter {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value.GetType() == typeof(string)) {
                string val = (string)value;

                float[] fields = Array.ConvertAll(val.Split(new char[] { '-' }), float.Parse);

                try {
                    return new Vector2(fields[0], fields[1]);
                }
                catch {
                    throw new InvalidCastException(
                        "Cannot convert string '" + value.ToString() + "' into a Vector2");
                }
            }
            else {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
