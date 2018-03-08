using System.ComponentModel;
using System.Text.RegularExpressions;
using LibSWBF2.Types;
using LibSWBF2.TypeConverters;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Representing a Material
    /// </summary>
    [TypeConverter(typeof(MATDConverter))]
    public class MATD : BaseChunk {
        /// <summary>
        /// The Name of the Material
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Diffuse (Base) Color of the Material
        /// </summary>
        public Color Diffuse { get; set; }

        /// <summary>
        /// Ambient Color of the Material
        /// </summary>
        public Color Ambient { get; set; }

        /// <summary>
        /// Specular Color of the Material
        /// </summary>
        public Color Specular { get; set; }

        /// <summary>
        /// The Sharpness of the Specular highlights
        /// </summary>
        public float SpecularSharpness {
            get { return specularSharpness; }

            set {
                specularSharpness = Math.Clamp(value, 0f, 100f);
            }
        }
        private float specularSharpness;

        /// <summary>
        /// Material Attribute (Unknown values, usually 0 or 8)
        /// </summary>
        public int Attribute {
            get { return attribute; }

            set {
                attribute = Math.Clamp(value, 0, 100);
            }
        }
        private int attribute;

        /// <summary>
        /// <para>Optional</para>
        /// <para>The Name of the TGA Texture Files (Materials without Textures are possible).</para>
        /// <para>Up to 10 textures are supported.</para>
        /// <para>Usually, just one Texture is set (TX0D). There are rare cases where TX1D or TX2D are set.</para>
        /// </summary>
        public string[] Textures { get; private set; } = new string[10];
        public string Texture {
            get { return Textures[0]; }
            set { Textures[0] = value; }
        }


        public MATD(MSH owner) : base(owner) {
            ChunkName = "MATD";
            Name = "New Material";
        }

        public MATD(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "NAME":
                        Name = nextChunk.ReadString(nextChunk.Data.Length);
                        break;
                    case "DATA":
                        Diffuse = nextChunk.ReadColor();
                        Ambient = nextChunk.ReadColor();
                        Specular = nextChunk.ReadColor();

                        SpecularSharpness = nextChunk.ReadFloat();
                        break;
                    case "ATRB":
                        attribute = nextChunk.ReadInt32();
                        break;
                    
                }

                //catch texture entrys (usually TX0D)
                Match txMatch = Regex.Match(nextChunk.ChunkName, "TX[0-9]{1}D");
                if (txMatch.Success) {
                    //the number sits at the 3rd position = [2]
                    //the symbol "0" ist at position 48 in the ascii table
                    int index = txMatch.Value[2] - 48;
                    Textures[index] = nextChunk.ReadString(nextChunk.Data.Length);
                }
            }
        }

        public override void WriteData() {
            base.WriteData();

            WriteString("NAME", Name);
            WriteHeader("DATA");
            WriteInt32(52);
            WriteColor(Diffuse);
            WriteColor(Ambient);
            WriteColor(Specular);

            WriteFloat(SpecularSharpness);

            WriteHeader("ATRB");
            WriteInt32(4);
            WriteInt32(Attribute);

            for (int i = 0; i < Textures.Length; i++) {
                if (!string.IsNullOrEmpty(Textures[i]))
                    WriteString("TX" + i + "D", Textures[i]);
            }

            WriteChunkLength();
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (Name == null) {
                result.AddError("Name of the Material is set to NULL! Must be a string!");
            }

            if (specularSharpness < 0) {
                result.AddError(Name + ": Negative values for Specular Sharpness are not allowed!");
            }

            if (specularSharpness < 0) {
                result.AddError(Name + ": Negative values for Attribute are not allowed!");
            }

            return result;
        }

        public override string ToString() {
            return Name;
        }
    }
}
