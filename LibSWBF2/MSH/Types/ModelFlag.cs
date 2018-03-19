using System.ComponentModel;
using LibSWBF2.TypeConverters;

namespace LibSWBF2.MSH.Types {
    /// <summary>
    /// Unknown FLag
    /// </summary>
    [TypeConverter(typeof(FlagConverter))]
    public struct ModelFlag {
        /// <summary>
        /// Choose if this Flag is set or not.
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// Value of the Flag. Usually 0 or 1
        /// </summary>
        public int Value {
            get { return _value; }
            set { _value = Math.Clamp(value, 0, 999999); }
        }
        private int _value;


        public ModelFlag(bool isSet, int value) {
            IsSet = isSet;
            _value = value;
        }

        public override string ToString() {
            return IsSet + "-" + Value;
        }
    }
}
