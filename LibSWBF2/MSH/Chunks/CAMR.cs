using System;
using System.Collections.Generic;
using System.Text;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    ///  Presumably just used to store the last Camera Position used by the modeller. 
    /// </summary>
    public class CAMR : BaseChunk {
        /// <summary>
        /// The Name of the Camera
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Presumably, this data contains the position and direction of the camera. Order unknown.
        /// </summary>
        public float[] CameraData { get; private set; }


        public CAMR(MSH owner) : base(owner) {
            ChunkName = "CAMR";
        }

        public CAMR(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "NAME":
                        Name = nextChunk.ReadString(nextChunk.Data.Length);
                        break;
                    case "DATA":
                        //consists of floats only
                        //a float has a size of 4 bytes
                        int len = nextChunk.Data.Length / 4;
                        CameraData = new float[len];

                        for (int i = 0; i < CameraData.Length; i++) {
                            CameraData[i] = nextChunk.ReadFloat();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (Name == null) {
                result.AddError("Name of the Camera is set to NULL! Must be a string!");
            }

            return result;
        }

        public override void WriteData() {
            base.WriteData();

            WriteString("NAME", Name);

            WriteHeader("DATA");
            WriteInt32(CameraData.Length * 4);

            foreach (float d in CameraData)
                WriteFloat(d);

            WriteChunkLength();
        }
    }
}
