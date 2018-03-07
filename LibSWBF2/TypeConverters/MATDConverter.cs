using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using LibSWBF2.MSH.Chunks;

namespace LibSWBF2.TypeConverters {
    class MATDConverter : TypeConverter {
        // Return true if we need to convert from a string.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        // Return true if we need to convert into a string.
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            SEGM segment = (SEGM)context.Instance;

            return new StandardValuesCollection(segment.Owner.Materials);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value.GetType() == typeof(string)) {
                string val = (string)value;

                SEGM segment = (SEGM)context.Instance;

                if (segment.Owner != null) {
                    return segment.Owner.Materials.Find(mat => mat.Name.Equals(val));
                }
                else {
                    return base.ConvertFrom(context, culture, value);
                }
            }
            else {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
