using System;
using System.IO;
using System.Text;
using LibSWBF2.Exceptions;

namespace LibSWBF2.Types {
    public class ChunkStream : FileStream {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkStream"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <exception cref="IOException"></exception>
        public ChunkStream(string path, FileMode fileMode, FileAccess fileAccess) 
            : base(path, fileMode, fileAccess) {
            
            if (!CanRead) {
                throw new IOException(path + " is not readable!");
            }
            
            //Set Position to begin
            Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Skips the given amount of bytes
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public void SkipBytes(int bytesToSkip) {
            if (Position + bytesToSkip >= Length) {
                throw new EndOfDataException("Unexpected end of stream!");
            }

            Position += bytesToSkip;
        }

        /// <summary>
        /// Reads a Int16
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public new byte ReadByte() {
            if (Position + 1 >= Length) {
                throw new EndOfDataException("Unexpected end of stream!");
            }

            byte[] buffer = new byte[1];
            Read(buffer, 0, 1);

            return buffer[0];
        }

        /// <summary>
        /// Reads a Int16
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public short ReadInt16() {
            if (Position + 2 >= Length) {
                throw new EndOfDataException("Unexpected end of stream!");
            }

            byte[] buffer = new byte[2];
            Read(buffer, 0, 2);

            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Reads an unsigned Int16
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public ushort ReadUInt16() {
            if (Position + 2 >= Length) {
                throw new EndOfDataException("Unexpected end of stream!");
            }

            byte[] buffer = new byte[2];
            Read(buffer, 0, 2);
            
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Reads a Int32
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public int ReadInt32() {
            if (Position + 4 >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[4];
            Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Reads a Int64
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public long ReadInt64() {
            if (Position + 8 >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[8];
            Read(buffer, 0, 8);

            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Reads a float
        /// </summary>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public float ReadFloat() {
            if (Position + 4 >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[4];
            Read(buffer, 0, 4);

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads a string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        /// <exception cref="LibSWBF2.Exceptions.InvalidChunkException">could not read string!</exception>
        public string ReadString(int length) {
            if (Position + length >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[length];
            Read(buffer, 0, length);

            Encoding encoding = Encoding.ASCII;
            char[] chars;

            try {
                chars = encoding.GetChars(buffer, 0, length);
            }
            catch (DecoderFallbackException ex) {
                throw new InvalidChunkException("could not read string!", ex);
            }

            return new string(chars);
        }
    }
}
