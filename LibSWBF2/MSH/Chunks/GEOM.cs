using System;
using System.IO;
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
        /// <para>Geometry Segments contain all the necessary information like Vertices, Normals and UVs.</para>
        /// <para>Usually a Geometry just has one Segment</para>
        /// </summary>
        public SEGM[] Segments {
            get { return segments.ToArray(); }
        }
        private List<SEGM> segments = new List<SEGM>();


        public GEOM(MSH owner) : base(owner) {
            ChunkName = "GEOM";
        }

        public GEOM(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "BBOX":
                        BoundingBox = new BBOX(nextChunk);
                        break;
                    case "SEGM":
                        SEGM segm = new SEGM(nextChunk);
                        segments.Add(segm);
                        break;
                }
            }
        }

        /// <summary>
        /// Add a new Segment to our Geometry. Usually a Geometry just has one Segment
        /// </summary>
        /// <param name="segment">The Segment to Add</param>
        public void AddSegment(SEGM segment) {
            if (segment == null)
                throw new ArgumentNullException("Segment to Add is NULL! Specify a Segment to add", "segment");

            segments.Add(segment);
        }

        /// <summary>
        /// Remove given Segment from our Geometry. Usually a Geometry just has one Segment
        /// </summary>
        /// <param name="segment">The Segment to remove</param>
        /// <returns></returns>
        public bool RemoveSegment(SEGM segment) {
            if (segment == null)
                throw new ArgumentNullException("Segment to remove is NULL! Specify a Segment to remove", "segment");

            return segments.Remove(segment);
        }

        /// <summary>
        /// Remove a Segment at Index from our Geometry. Usually a Geometry just has one Segment
        /// </summary>
        /// <param name="index">The index of the Segment to remove</param>
        /// <returns></returns>
        public void RemoveSegment(int index) {
            if (index < 0 || index >= segments.Count)
                throw new IndexOutOfRangeException("The given index is out of bounds!");

            segments.RemoveAt(index);
        }

        /// <summary>
        /// Remove all Segments! Use with caution!
        /// </summary>
        public void ClearSegments() {
            segments.Clear();
        }

        public void ApplyReferences(MATD[] materials) {
            foreach (SEGM segment in segments) {
                segment.ApplyReferences(materials);
            }
        }

        public override void WriteData() {
            base.WriteData();

            WriteChunk(BoundingBox);

            foreach (SEGM segment in segments)
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

            if (segments.Count == 0) {
                result.AddError("Number of Segments in Geometry is zero! At least one Segment is necessary!");
            }

            try {
                result = CheckResult.Merge(result, CheckResult.Merge(segments.ToArray()));
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
