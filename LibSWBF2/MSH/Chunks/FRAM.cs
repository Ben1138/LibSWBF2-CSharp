
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


        public FRAM(MSH owner) : base(owner) {
            ChunkName = "FRAM";
        }

        public FRAM(BaseChunk from) : base(from) {
            Start = ReadInt32();
            End = ReadInt32();
            FrameRate = ReadFloat();
        }

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
