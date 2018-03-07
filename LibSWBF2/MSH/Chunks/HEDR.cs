using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    public class HEDR : BaseChunk {
        public int Shvo { get; set; } = -99;
        public MSH2 Mesh { get; set; }


        public HEDR(MSH owner) : base(owner) {
            ChunkName = "HEDR";
        }

        public HEDR(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "SHVO":
                        Shvo = nextChunk.ReadInt32();
                        break;
                    case "MSH2":
                        Mesh = new MSH2(nextChunk);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void WriteData() {
            base.WriteData();

            if (Shvo >= 0) {
                WriteHeader("SHVO");
                WriteInt32(4);
                WriteInt32(Shvo);
            }

            WriteChunk(Mesh);

            WriteChunkLength();
        }
    }
}
