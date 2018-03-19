using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.Exceptions {
    /// <summary>
    /// Use this Exception if a Chunk cannot be read / is obviously invalid
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidChunkException : Exception {
        public InvalidChunkException() { }
        public InvalidChunkException(string message) : base(message) { }
        public InvalidChunkException(string message, Exception innerException) : base(message, innerException) { }
    }
}
