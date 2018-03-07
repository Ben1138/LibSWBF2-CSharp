using System;
using System.Collections.Generic;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Represents a Polygon
    /// </summary>
    public class STRP : BaseChunk {
        /// <summary>
        /// List of Vertices that define this Polygon
        /// </summary>
        public Vertex[] Vertices { get { return vertices.ToArray(); } }
        private List<Vertex> vertices = new List<Vertex>();
        private List<ushort> vertexIndices = new List<ushort>();


        public STRP(MSH owner) : base(owner) {
            ChunkName = "STRP";
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (vertices.Count < 3) {
                result.AddError("A Polygon needs at least 3 Vertices but " + vertices.Count + " are provided!");
            }

            return result;
        }

        /// <summary>
        /// <para>Creates a new STRP instance based on the values read from the given stream</para>
        /// <para>Chunk Header excluded!</para>
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The newly created instance</returns>
        public static STRP FromStream(ChunkStream stream, MSH owner) {
            STRP poly = new STRP(owner);

            //the first 4 bytes store an 32-Bit Int containing the length of the following 16-Bit Int (short) array
            int length = stream.ReadInt32();
            Log.Add("Vertex Count " + length, LogType.Info);

            //read all the indices
            for (int i = 0; i < length; i++) {
                ushort vertID = stream.ReadUInt16();
                Log.Add("Add Vertex " + vertID, LogType.Info);
                poly.vertexIndices.Add(vertID);
            }

            return poly;
        }
    }
}
