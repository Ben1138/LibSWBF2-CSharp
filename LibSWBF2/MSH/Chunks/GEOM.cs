using System;
using System.Collections.Generic;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Geometry Information. Usually a Geometry just has one Segment
    /// </summary>
    public class GEOM : BaseChunk {
        /// <summary>
        /// The Bounding Box of the Geometry
        /// </summary>
        public BBOX BoundingBox { get; set; }

        /// <summary>
        /// <para>A Geometry is made of multiple Segments. Exactly one Material is assigned to each Segment</para>
        /// <para>A Geometry contains at least one Segment</para>
        /// </summary>
        public List<SEGM> Segments { get; private set; } = new List<SEGM>();


        /// <summary>
        /// Initializes a new instance of the <see cref="GEOM"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public GEOM(MSH owner) : base(owner) {
            ChunkName = "GEOM";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GEOM"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public GEOM(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "BBOX":
                        BoundingBox = new BBOX(nextChunk);
                        break;
                    case "SEGM":
                        SEGM segm = new SEGM(nextChunk);
                        Segments.Add(segm);
                        break;
                }
            }
        }

        /// <summary>
        /// Since in MSH References are saved by name (string) or list index (int32), we have to assign all necessary references manually.
        /// This should be overriden by any subclass which holds references to other Chunks (e.g. to every Segment one Material is assigned)
        /// </summary>
        public override void ApplyReferences() {
            foreach (SEGM segment in Segments) {
                segment.ApplyReferences();
            }
        }

        /// <summary>
        /// Writes the complete data stream new from scratch.
        /// Every Chunk inheriting from this must override this function
        /// </summary>
        public override void WriteData() {
            base.WriteData();

            WriteChunk(BoundingBox);

            foreach (SEGM segment in Segments)
                WriteChunk(segment);

            WriteChunkLength();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (BoundingBox == null) {
                result.AddError("Bounding Box of Geometry is set to NULL!");
            }

            if (Segments.Count == 0) {
                result.AddError("Number of Segments in Geometry is zero! At least one Segment is necessary!");
            }

            try {
                result = CheckResult.Merge(result, CheckResult.Merge(Segments.ToArray()));
            }
            catch (ArgumentNullException ex) {
                result.AddError(ex.Message);
                return result;
            }
            /*catch {
                throw;
                ///result.AddError("An Unknown Error occured! " + ex.Message);
                //return result;
            }*/

            return result;
        }
    }
}
