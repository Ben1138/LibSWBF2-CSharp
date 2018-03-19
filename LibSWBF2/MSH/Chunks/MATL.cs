using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.MSH.Chunks {
    public class MATL : BaseChunk {
        /// <summary>
        /// Represents a List of all Materials
        /// </summary>
        public List<MATD> Materials { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MATL"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public MATL(MSH owner) : base(owner) {
            ChunkName = "MATL";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MATL"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public MATL(BaseChunk from) : base(from) {
            Materials = new List<MATD>();

            //The first four bytes of data contain sa 32-bit integer specifying the number of MATD sections contained in this section.
            int len = ReadInt32();

            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "MATD":
                        Materials.Add(new MATD(nextChunk));
                        break;
                }
            }

            if (len != Materials.Count) {
                Log.Add("The number of expected materials (" + len + ") does not match the number of actually read materials (" + Materials.Count + ")!", LogType.Warning);
            }
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public override void WriteData() {
            base.WriteData();

            WriteInt32(Materials.Count);
            foreach (MATD mat in Materials)
                WriteChunk(mat);

            WriteChunkLength();
        }
    }
}
