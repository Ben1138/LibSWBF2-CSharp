using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    public class EndOfDataException : Exception {
        public EndOfDataException() { }
        public EndOfDataException(string message) : base(message) { }
        public EndOfDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
