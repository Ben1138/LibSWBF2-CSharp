using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LibSWBF2.MSH.Chunks;
using LibSWBF2.Exceptions;

namespace LibSWBF2.MSH.Types {
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
        /// Reads the u int16.
        /// </summary>
        /// <returns></returns>
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
        /// Reads the int32.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public int ReadInt32() {
            if (Position + 4 >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[4];
            Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LibSWBF2.MSH.Types.EndOfDataException">Unexpected end of stream!</exception>
        public float ReadFloat() {
            if (Position + 4 >= Length)
                throw new EndOfDataException("Unexpected end of stream!");

            byte[] buffer = new byte[4];
            Read(buffer, 0, 4);

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Trys to read the Chunk length of the expected Chunk Name
        /// </summary>
        /// <param name="expectedChunkName">The Name of the Chunk to be expected coming next</param>
        /// <param name="throwException">Should we throw an Exception if our expectations are not met?</param>
        /// <returns></returns>
        /*public ReadChunkResult ReadChunk() {
            if (Position + 4 >= Length)
                throw new EndOfStreamException("Unexpected end of stream!");

            byte[] buffer = new byte[4];
            Read(buffer, 0, 4);

            Encoding encoding = Encoding.ASCII;
            char[] chars = encoding.GetChars(buffer, 0, 4);

            string name = new string(chars);
            int length = ReadInt32();

            bool validChunkName = Regex.Match(name, "[A-Z0-9]{4}").Success;             

            return new ReadChunkResult(length, name, validChunkName);
        }

        public BaseChunk ReadChunk() {
            if (Position + 4 >= Length)
                throw new EndOfStreamException("Unexpected end of stream!");

            BaseChunk chunk = new BaseChunk();
            chunk.FromData(this);

            return chunk;
        }*/

        /// <summary>
        /// Skip the given amount of bytes
        /// </summary>
        /// <param name="amount">The amount to skip</param>
        /*public void SkipForward(int amount) {
            if (amount < 1)
                throw new ArgumentOutOfRangeException("Cannot skip " + amount + " bytes!");
            else
                Seek(amount, SeekOrigin.Current);
        }*/
    }
}
