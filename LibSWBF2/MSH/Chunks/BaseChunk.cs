using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using LibSWBF2.Types;
using LibSWBF2.Exceptions;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Base class for all chunk types. Check Result is always valid if no Error Messages are added!
    /// </summary>
    public class BaseChunk {
        private static readonly string ValidChunkRegEx = "[A-Z]{1}[A-Z0-9]{3}";

        /// <summary>
        /// the MSH this Chunk belongs to
        /// </summary>
        [Browsable(false)]
        public MSH Owner { get; private set; }

        /// <summary>
        /// Name of the Chunk. This should always be 4 uppercase Letters (numbers are somewhat allowed too)
        /// </summary>
        [Browsable(false)]
        public string ChunkName { get; protected set; }

        /// <summary>
        /// The actual raw data of this chunk (byte stream). This cannot be modified manually! Use <see cref="WriteData" /> instead
        /// </summary>
        [Browsable(false)]
        public byte[] Data { get { return data.ToArray(); } }
        private List<byte> data = new List<byte>();

        protected bool EndOfData { get { return (position >= data.Count); } }
        protected int Position { get { return position; } }
        private int position = 0;


        /// <summary>
        /// Use this Constructor to create a new empty Chunk, given the MSH owner
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public BaseChunk(MSH owner) {
            ChunkName = "";
            Owner = owner;
        }

        /// <summary>
        /// Use this Constructor to create a new Chunk from a given <see cref="BaseChunk" />. This must be overriden by every subclass!
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public BaseChunk(BaseChunk from) {
            Log.Add("Process Chunk " + from.ChunkName, LogType.Info);

            ChunkName = from.ChunkName;
            data.AddRange(from.data);
            Owner = from.Owner;
        }

        /// <summary>
        /// Creates a new Chunk from given byte data
        /// </summary>
        /// <param name="inputData">The data to read from</param>
        /// <param name="offset">The offset in the byte array to start reading from</param>
        /// <returns>The new Chunk</returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        /// <exception cref="InvalidChunkException">If Chunk name is not valid or the read chunk length is negative</exception>
        public static BaseChunk FromData(byte[] inputData, int offset, MSH owner) {
            if (inputData.Length < 8 || offset + 8 > inputData.Length) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            BaseChunk chunk = new BaseChunk(owner);

            //every chunk starts with a chunk name (4 bytes)
            chunk.ChunkName = inputData.SubArray(offset, 4).GetString();
            offset += 4;

            //and an Int32 defining the length of the chunk (in bytes)
            int length = BitConverter.ToInt32(inputData, offset);
            offset += 4;

            if (!Regex.Match(chunk.ChunkName, ValidChunkRegEx).Success) {
                Log.Add(chunk.ChunkName + " is not a valid Chunk Name!", LogType.Error);
                throw new InvalidChunkException(chunk.ChunkName + " is not a valid Chunk Name!");
            }

            if (length < 0) {
                Log.Add(length + " is not a valid Chunk Length!", LogType.Error);
                throw new InvalidChunkException(length + " is not a valid Chunk Length!");
            }

            if (offset + length > inputData.Length) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            //create own data
            chunk.data.AddRange(inputData.SubArray(offset, length));

            chunk.ResetPosition();
            Log.Add("Valid Chunk " + chunk.ChunkName + " found of length " + chunk.data.Count, LogType.Info);

            return chunk;
        }

        /// <summary>
        /// Creates a new Chunk from given Stream
        /// </summary>
        /// <param name="stream">The Stream to read from</param>
        /// <returns>The new Chunk</returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        /// <exception cref="InvalidChunkException">If Chunk name is not valid or the read chunk length is negative</exception>
        public static BaseChunk FromData(ChunkStream stream, MSH owner) {
            BaseChunk chunk = new BaseChunk(owner);

            //every chunk starts with a chunk name (4 bytes)
            chunk.ChunkName = stream.ReadString(4); 

            //and an Int32 defining the length of the chunk (in bytes)
            int length = stream.ReadInt32();

            if (!Regex.Match(chunk.ChunkName, ValidChunkRegEx).Success) {
                Log.Add(chunk.ChunkName + " is not a valid Chunk Name!", LogType.Error);
                throw new InvalidChunkException(chunk.ChunkName + " is not a valid Chunk Name!");
            }

            if (length < 0) {
                Log.Add(length + " is not a valid Chunk Length!", LogType.Error);
                throw new InvalidChunkException(length + " is not a valid Chunk Length!");
            }

            //read all the data
            byte[] buffer = new byte[length];

            try {
                stream.Read(buffer, 0, length);
                chunk.data.AddRange(buffer);
            }
            catch (ArgumentOutOfRangeException ex) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!", ex);
            }

            chunk.ResetPosition();

            return chunk;
        }

        /// <summary>
        /// Flushes (clears) all data.
        /// </summary>
        public void FlushData() {
            data.Clear();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing.
        /// This Method should be overwritten by all subclasses.
        /// </summary>
        /// <returns></returns>
        public virtual CheckResult CheckIntegrity() {
            return null;
        }

        /// <summary>
        /// Since in MSH References are saved by name (string) or list index (int32), we have to assign all necessary references manually.
        /// This should be overriden by any subclass which holds references to other Chunks (e.g. to every Segment one Material is assigned)
        /// </summary>
        public virtual void ApplyReferences() { }

        /// <summary>
        /// Resets the position.
        /// </summary>
        public void ResetPosition() {
            position = 0;
        }

        /// <summary>
        /// Reads a float from given data
        /// </summary>
        /// <returns>The float we just read</returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data</exception>
        public float ReadFloat() {
            if (position + 4 > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            byte[] buffer = data.GetRange(position, 4).ToArray(); //Data.SubArray(position, 4);
            position += 4;
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reads an int32 from given data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data!</exception>
        public int ReadInt32() {
            if (position + 4 > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            byte[] buffer = data.GetRange(position, 4).ToArray(); //Data.SubArray(position, 4);
            position += 4;
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Reads an int16 from given data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data!</exception>
        public short ReadInt16() {
            if (position + 2 > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            byte[] buffer = data.GetRange(position, 2).ToArray(); //Data.SubArray(position, 2);
            position += 2;
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Reads an unsigned int16 from given data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data!</exception>
        public ushort ReadUnsignedInt16() {
            if (position + 2 > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            byte[] buffer = data.GetRange(position, 2).ToArray();  //Data.SubArray(position, 2);
            position += 2;
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Reads a vertex index from given data
        /// </summary>
        public VertexIndex ReadVertexIndex() {
            short s = ReadInt16();

            //if the highest bit is set, it indicates a new polygon
            //all the other bits still function as index!
            //if the highest bit is set, we are in negative space
            //to get the index value, lets connect our read short with a bit mask
            // (using logical AND)
            // value  1000000000000111   = -32761
            // mask   0111111111111111   = 0x7FFF in hex
            // result 0000000000000111   = 7   <-- desired index value
            short indexValue = (short)(s & 0x7FFF);

            //to just check for the highest bit, use a second mask
            // (using logical AND)
            // value  1000000000000111   = -32761
            // mask   1000000000000000   = 0x8000 in hex
            // result 1000000000000000   = -32768
            short polyBit = (short)(s & 0x8000);

            return new VertexIndex() {
                index = indexValue,
                polyBoundary = (polyBit != 0)
            };
        }

        /// <summary>
        /// Reads a Vector2 from given data
        /// </summary>
        /// <returns></returns>
        public Vector2 ReadVector2() {
            return new Vector2(
                ReadFloat(),
                ReadFloat()
            );
        }

        /// <summary>
        /// Reads a Vector3 from given data
        /// </summary>
        /// <returns></returns>
        public Vector3 ReadVector3() {
            return new Vector3(
                ReadFloat(),
                ReadFloat(),
                ReadFloat()
            );
        }

        /// <summary>
        /// Reads a Vector4 from given data
        /// </summary>
        /// <returns></returns>
        public Vector4 ReadVector4() {
            return new Vector4(
                ReadFloat(),
                ReadFloat(),
                ReadFloat(),
                ReadFloat()
            );
        }

        /// <summary>
        /// Reads a Color from chunk data
        /// </summary>
        /// <returns></returns>
        public Color ReadColor() {
            return new Color(
                ReadFloat(),
                ReadFloat(),
                ReadFloat(),
                ReadFloat()
            );
        }

        /// <summary>
        /// Reads a string from given data
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <exception cref="EndOfDataException">Unexpected end of Data!</exception>
        public string ReadString(int length) {
            if (position + length > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            Log.Add("Expected string length" + length, LogType.Info);

            byte[] buffer = data.GetRange(position, length).ToArray(); //Data.SubArray(position, length);

            //Encoding encoding = Encoding.ASCII;

            //string are stored as null-terminated strings (last charatcer is 00)
            //so ignore it for conversion
            string result = "";//encoding.GetString(buffer, 0, length - 1);
            
            for (int i = 0; i < buffer.Length-1; i++) {
                if (buffer[i] != 0)
                    result += (char)buffer[i];
                else
                    continue;
            }
            
            position += length;

            Log.Add("Read String " + result + " of length " + result.Length, LogType.Info);

            return result;
        }

        /// <summary>
        /// Reads a chunk from given data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException">Unexpected end of Data!</exception>
        public BaseChunk ReadChunk() {
            //a new Chunk as to be at least 8 Bytes in size!
            if (position + 8 > data.Count) {
                Log.Add("Unexpected end of Data!", LogType.Error);
                throw new EndOfDataException("Unexpected end of Data!");
            }

            //we just need to pass the data from our current position
            //The newly created chunk will cut it futher down later
            //byte[] chunkData = Data.SubArray(position, Data.Length - position);

            BaseChunk newChunk = FromData(data.ToArray(), position, Owner);

            //length of the chunk + header size
            position += newChunk.data.Count + 8;

            return newChunk;
        }

        /// <summary>
        /// Writes a float into the data stream
        /// </summary>
        /// <param name="value">The float to write</param>
        public void WriteFloat(float value) {
            data.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an int32 into the data stream
        /// </summary>
        /// <param name="value">The int32 to write</param>
        public void WriteInt32(int value) {
            data.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an int16 into the data stream
        /// </summary>
        /// <param name="value">The int16 to write</param>
        public void WriteInt16(short value) {
            data.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned int16 into the data stream
        /// </summary>
        /// <param name="value">The unsigned int16 to write</param>
        public void WriteUnsignedInt16(ushort value) {
            data.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a string into the data stream. Every string is a chunk on its own.
        /// Therefore, you need to specify a header name (e.g. NAME)
        /// </summary>
        /// <param name="header">Header Name</param>
        /// <param name="value">The string to write</param>
        public void WriteString(string header, string value) {
            //write chunk Header (e.g. NAME)
            WriteHeader(header);

            //number of bytes that follow (string length + zero for null termination)
            WriteInt32(value.Length + 1);

            //write string
            for (int i = 0; i < value.Length; i++)
                data.Add((byte)value[i]);

            //null terminated string
            data.Add(0);
        }

        /// <summary>
        /// Writes a Vector2 into the data stream
        /// </summary>
        /// <param name="vector">The Vector2 to write</param>
        public void WriteVector2(Vector2 vector) {
            WriteFloat(vector.X);
            WriteFloat(vector.Y);
        }

        /// <summary>
        /// Writes a Vector3 into the data stream
        /// </summary>
        /// <param name="vector">The Vector3 to write</param>
        public void WriteVector3(Vector3 vector) {
            WriteFloat(vector.X);
            WriteFloat(vector.Y);
            WriteFloat(vector.Z);
        }

        /// <summary>
        /// Writes a Vector4 into the data stream
        /// </summary>
        /// <param name="vector">The Vector4 to write</param>
        public void WriteVector4(Vector4 vector) {
            WriteFloat(vector.X);
            WriteFloat(vector.Y);
            WriteFloat(vector.Z);
            WriteFloat(vector.W);
        }

        /// <summary>
        /// Writes a Color into the data stream
        /// </summary>
        /// <param name="color">The Color to write</param>
        public void WriteColor(Color color) {
            WriteFloat(color.R);
            WriteFloat(color.G);
            WriteFloat(color.B);
            WriteFloat(color.A);
        }

        /// <summary>
        /// Writes a Vertex Index into the data stream
        /// </summary>
        /// <param name="vertexIndex">The Vertex Index to write</param>
        public void WriteVertexIndex(VertexIndex vertexIndex) {
            short value = vertexIndex.index;

            if (vertexIndex.polyBoundary) {
                //if index is a polygon boundary (begin/end), set the highest bit
                // (using logical OR)
                // value  0000000000000111   = 7
                // mask   1000000000000000   = 0x8000 in hex
                // result 1000000000000111   = desired index value (-32761 in dec)
                value = (short)(value ^ 0x8000);
            }

            data.AddRange(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a Header into the data stream
        /// </summary>
        /// <param name="header">The Header to write</param>
        /// <exception cref="InvalidChunkException">Thrown if given Header Name is invalid</exception>
        public void WriteHeader(string header) {
            if (!Regex.Match(header, ValidChunkRegEx).Success) {
                Log.Add(header + " is not a valid Header Name!", LogType.Error);
                throw new InvalidChunkException(header + " is not a valid Header Name!");
            }

            //write chunk Header (e.g. NAME)
            //since encoding is always ascii, we just can force a cast here
            for (int i = 0; i < header.Length; i++)
                data.Add((byte)header[i]);
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public virtual void WriteData() {
            //clear all data
            FlushData();

            //every chunk starts with a header name (4 bytes)
            WriteHeader(ChunkName);

            //the second 4 bytes determine the length of the chunk (number of bytes that follow)
            WriteInt32(0);

            //after that, all the chunk data follows. Since Base Chunk doesn't have any content, chunk length is zero by default
        }

        /// <summary>
        /// Writes a given Chunk into the data stream
        /// </summary>
        /// <param name="chunk">The chunk to write</param>
        public void WriteChunk(BaseChunk chunk) {
            if (chunk == null)
                return;

            chunk.WriteData();
            chunk.WriteChunkLength();

            data.AddRange(chunk.data);
            chunk.FlushData();
        }

        /// <summary>
        /// Refreshes the Chunk Length. This should be called at the end of every WriteData() override
        /// This overrides the second 4 bytes, which determine the length of the chunk (as int32).
        /// </summary>
        public void WriteChunkLength() {
            byte[] buffer = BitConverter.GetBytes(data.Count - 8);

            //write into the second 4 bytes, which determine an int32 containing the number of bytes of this chunk
            for (int i = 0; i < buffer.Length; i++)
                data[4 + i] = buffer[i];
        }
    }
}
