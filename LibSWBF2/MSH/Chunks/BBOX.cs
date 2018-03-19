using LibSWBF2.Types;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Bounding Box surrounding the selected object 
    /// </summary>
    public class BBOX : BaseChunk {
        /// <summary>
        /// Probably the Position of the Bounding Box
        /// </summary>
        public Vector3 Translation { get; set; }

        /// <summary>
        /// Probably the Rotation of the Bounding Box as Quaternion
        /// </summary>
        public Vector4 Rotation { get; set; }

        /// <summary>
        /// Probably the Dimension of the Bounding Box
        /// </summary>
        public Vector4 Dimension { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BBOX"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public BBOX(MSH owner) : base(owner) {
            ChunkName = "BBOX";
        }

        public BBOX(BaseChunk from) : base(from) {
            Rotation = ReadVector4();
            Translation = ReadVector3();
            Dimension = ReadVector4();
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public override void WriteData() {
            base.WriteData();

            WriteVector4(Rotation);
            WriteVector3(Translation);
            WriteVector4(Dimension);

            WriteChunkLength();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            return new CheckResult();
        }
    }
}