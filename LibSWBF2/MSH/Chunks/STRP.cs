using System;
using System.Collections.Generic;
using LibSWBF2.Types;
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
    }
}
