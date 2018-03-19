using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    public class HEDR : BaseChunk {
        /// <summary>
        /// Probably indicating if this MSH just contains shadow volume information
        /// </summary>
        public int Shvo { get; set; } = -99;

        /// <summary>
        /// Mesh Information
        /// </summary>
        public MSH2 Mesh { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="HEDR"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public HEDR(MSH owner) : base(owner) {
            ChunkName = "HEDR";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HEDR"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public HEDR(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "SHVO":
                        Shvo = nextChunk.ReadInt32();
                        break;
                    case "MSH2":
                        Mesh = new MSH2(nextChunk);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public override void WriteData() {
            base.WriteData();

            if (Shvo >= 0) {
                WriteHeader("SHVO");
                WriteInt32(4);
                WriteInt32(Shvo);
            }

            WriteChunk(Mesh);

            WriteChunkLength();
        }
    }
}
