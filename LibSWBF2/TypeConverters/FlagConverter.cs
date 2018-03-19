using System;
using System.ComponentModel;
using System.Globalization;

namespace LibSWBF2.TypeConverters {
    class FlagConverter : BaseConverter {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value.GetType() == typeof(string)) {
                string val = (string)value;

                string[] fields = val.Split(new char[] { '-' });

                try {
                    return new ModelFlag(Convert.ToBoolean(fields[0]), Convert.ToInt32(fields[1]));
                }
                catch {
                    throw new InvalidCastException(
                        "Cannot convert string '" + value.ToString() + "' into a Flag Value");
                }
            }
            else {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
