using System;
using System.Collections.Generic;
using System.Text;

namespace LibSWBF2.MSH.Chunks {
    public class MATL : BaseChunk {
        public List<MATD> Materials { get; set; }


        public MATL(MSH owner) : base(owner) {
            ChunkName = "MATL";
        }

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

        public override void WriteData() {
            base.WriteData();

            WriteInt32(Materials.Count);
            foreach (MATD mat in Materials)
                WriteChunk(mat);

            WriteChunkLength();
        }
    }
}
