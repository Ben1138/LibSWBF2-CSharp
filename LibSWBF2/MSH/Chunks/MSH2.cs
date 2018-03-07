using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    public class MSH2 : BaseChunk {
        public SINF SelectionInformation { get; set; }
        public CAMR Camera { get; set; }
        public MATL MaterialList { get; set; }
        public List<MODL> Models { get; set; }


        public MSH2(MSH owner) : base(owner) {
            ChunkName = "MSH2";
        }

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
