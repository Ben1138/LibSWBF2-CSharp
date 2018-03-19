using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    /// <summary>
    /// Use this Exception if a Chunk abruptly ends
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class EndOfDataException : Exception {
        public EndOfDataException() { }
        public EndOfDataException(string message) : base(message) { }
        public EndOfDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
