using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    public class MSH2 : BaseChunk {
        /// <summary>
        /// <para>Optional, Dispensable</para>
        /// <para>Selection Information for the Zero Editor.</para>
        /// <para>Returns NULL if no Information is specified!</para>
        /// </summary>
        public SINF SelectionInformation { get; set; }

        /// <summary>
        /// <para>Dispensable</para>
        /// <para>Presumably just used to store the last Camera Position used by the modeller.</para>
        /// </summary>
        public CAMR Camera { get; set; }

        /// <summary>
        /// All Materials this MSH contains
        /// </summary>
        public MATL MaterialList { get; set; }

        /// <summary>
        /// All Models this MSH contains
        /// </summary>
        public List<MODL> Models { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MSH2"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public MSH2(MSH owner) : base(owner) {
            ChunkName = "MSH2";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSH2"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public MSH2(BaseChunk from) : base(from) {
            Models = new List<MODL>();

            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();
                
                switch (nextChunk.ChunkName) {
                    case "SINF":
                        SelectionInformation = new SINF(nextChunk);
                        break;
                    case "CAMR":
                        Camera = new CAMR(nextChunk);
                        break;
                    case "MATL":
                        MaterialList = new MATL(nextChunk);
                        break;
                    case "MODL":
                        Models.Add(new MODL(nextChunk));
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

            WriteChunk(SelectionInformation);
            WriteChunk(Camera);
            WriteChunk(MaterialList);

            foreach (MODL model in Models)
                WriteChunk(model);

            WriteChunkLength();
        }
    }
}
