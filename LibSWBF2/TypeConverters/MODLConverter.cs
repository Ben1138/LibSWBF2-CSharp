using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using LibSWBF2.MSH.Chunks;

namespace LibSWBF2.TypeConverters {
    class MODLConverter : TypeConverter {
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
            MODL model = (MODL)context.Instance;

            List<MODL> selectable = new List<MODL>();

            //null parent is always selectable
            selectable.Add(null);

            if (model.Owner != null) {
                foreach (MODL mdl in model.Owner.Models) {
                    //can't select yourself as parent!
                    if (mdl != model)
                        selectable.Add(mdl);
                }
            }

            return new StandardValuesCollection(selectable);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value.GetType() == typeof(string)) {
                string val = (string)value;

                MODL model = (MODL)context.Instance;

                if (model.Owner != null) {
                    return model.Owner.Models.Find(mdl => mdl.Name.Equals(val));
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
