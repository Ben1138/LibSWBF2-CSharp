using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    public class InsufficientPermissionsException : Exception {
        public InsufficientPermissionsException() { }
        public InsufficientPermissionsException(string message) : base(message) { }
        public InsufficientPermissionsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
