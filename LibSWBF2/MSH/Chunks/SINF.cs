using System;
using System.Collections.Generic;
using System.ComponentModel;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Selection Information for the Zero Editor.
    /// </summary>
    public class SINF : BaseChunk {
        /// <summary>
        /// Name of the selection
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Frame Information for the Zero Editor
        /// </summary>
        [Browsable(false)]
        public FRAM FrameInformation { get; set; }

        /// <summary>
        /// Bounding Box surrounding the selected object 
        /// </summary>
        [Browsable(false)]
        public BBOX BoundingBox { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SINF"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public SINF(MSH owner) : base(owner) {
            ChunkName = "SINF";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SINF"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public SINF(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "NAME":
                        Name = nextChunk.ReadString(nextChunk.Data.Length);
                        break;
                    case "FRAM":
                        FrameInformation = new FRAM(nextChunk);
                        break;
                    case "BBOX":
                        BoundingBox = new BBOX(nextChunk);
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

            WriteString("NAME", Name);
            WriteChunk(FrameInformation);
            WriteChunk(BoundingBox);

            WriteChunkLength();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (Name == null) {
                result.AddError("Name of the Selection is set to NULL! Must be a string!");
            }

            if (FrameInformation == null) {
                result.AddError("Frame Information of the Selection is set to NULL!");
            }

            if (BoundingBox == null) {
                result.AddError("Bounding Box of the Selection is set to NULL!");
            }


            try {
                result = CheckResult.Merge(result, FrameInformation.CheckIntegrity(), BoundingBox.CheckIntegrity());
            }
            catch (ArgumentNullException ex) {
                result.AddError(ex.Message);
                return result;
            }
            catch (Exception ex) {
                result.AddError("An Unknown Error occured! " + ex.Message);
                return result;
            }

            return result;
        }
    }
}
