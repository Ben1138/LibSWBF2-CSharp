using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    public class InvalidChunkException : Exception {
        public InvalidChunkException() { }
        public InvalidChunkException(string message) : base(message) { }
        public InvalidChunkException(string message, Exception innerException) : base(message, innerException) { }
    }
}
