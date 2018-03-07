using System;
using System.ComponentModel;

namespace LibSWBF2.TypeConverters {
    class BaseConverter : TypeConverter {
        // Return true if we need to convert from a string.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        // Return true if we need to convert into a string.
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(String)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        // Return true to indicate that the object supports properties.
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
            return true;
        }

        // Return a property description collection.
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            return TypeDescriptor.GetProperties(value);
        }
    }
}
