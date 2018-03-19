using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    /// <summary>
    /// Use this Exception if some kind of Permission Error occours during reading / writing Files
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InsufficientPermissionsException : Exception {
        public InsufficientPermissionsException() { }
        public InsufficientPermissionsException(string message) : base(message) { }
        public InsufficientPermissionsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
