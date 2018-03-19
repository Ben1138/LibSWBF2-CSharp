using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Frame Information for the Zero Editor. Exact purpose is unknown
    /// </summary>
    public class FRAM : BaseChunk {
        /// <summary>
        /// Typical values are 0 or 1
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Range usually between 0 and 100
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Presumably the Frame Rate
        /// </summary>
        public float FrameRate { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FRAM"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public FRAM(MSH owner) : base(owner) {
            ChunkName = "FRAM";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FRAM"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public FRAM(BaseChunk from) : base(from) {
            Start = ReadInt32();
            End = ReadInt32();
            FrameRate = ReadFloat();
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public override void WriteData() {
            base.WriteData();

            WriteInt32(Start);
            WriteInt32(End);
            WriteFloat(FrameRate);

            WriteChunkLength();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            //always valid
            return new CheckResult();
        }
    }
}
