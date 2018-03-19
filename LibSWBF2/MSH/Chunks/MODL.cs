using System;
using System.ComponentModel;
using LibSWBF2.Types;
using LibSWBF2.TypeConverters;
using LibSWBF2.MSH.Types;

namespace LibSWBF2.MSH.Chunks {
    /// <summary>
    /// Representing a Model
    /// </summary>
    [TypeConverter(typeof(MODLConverter))]
    public class MODL : BaseChunk {
        /// <summary>
        /// <para>The Index Numbner of the Model</para> 
        /// <para>Index Numbers start with 1 and should increment +1 for every next model</para>
        /// </summary>
        public int Index {
            get { return index; }
            private set { index = Math.Clamp(value, 1, 999999); }
        }
        private int index;

        /// <summary>
        /// Type of the Model. Most common Types are Static and Shadow. Null indicates no Geometry!
        /// </summary>
        public MTYP Type { get; set; }

        /// <summary>
        /// Some Models are tagged as Collision or Terrain Cut. Since Models Tags are deterimed by their Names respectively, they cannot be set.
        /// </summary>
        public ModelTag Tag {
            get {
                if (Name.ToLower().Contains("v-collision") || Name.ToLower().Contains("p_vehicle")) {
                    return ModelTag.VehicleCollision;
                }
                else if (Name.ToLower().Contains("collision")) {
                    return ModelTag.Collision;
                }

                if (Name.ToLower().Contains("terraincutter")) {
                    return ModelTag.TerrainCut;
                }

                if (Name.ToLower().Contains("lowrez")) {
                    return ModelTag.Lowrez;
                }

                if (Name.ToLower().Contains("p_")) {
                    return ModelTag.Miscellaneous;
                }

                return ModelTag.Common;
            }
        }

        /// <summary>
        /// The Name of the Model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <para>Optional</para>
        /// <para>The Parent of this Model (This Model is dependent on the Parent specified here)</para>
        /// <para>NULL indicates that there is no Parent specified!</para>
        /// </summary>
        public MODL Parent { get; set; } = null;
        private string parentName;

        /// <summary>
        /// <para>Optional</para>
        /// <para>Unknown purpose. Child model flag values? Common value is 1</para>
        /// <para>If IsSet is FALSE, no Flag has been set and wont be written to File</para>
        /// </summary>
        public ModelFlag Flag { get; set; }

        /// <summary>
        /// The Scale of the Model
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// The Rotation of the Model as Euler Angles (Degrees around XYZ)!
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// The Translation (Position) of the Model in 3D Space
        /// </summary>
        public Vector3 Translation { get; set; }

        /// <summary>
        /// Unknown Translation Parameter
        /// </summary>
        public float UnknownTRAN { get; set; }

        /// <summary>
        /// <para>Optional. If Model Type if set to NULL, this is not existent!</para>
        /// <para>The Models Geometry</para>
        /// </summary>
        [Browsable(false)]
        public GEOM Geometry { get; set; } = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="MODL"/> class.
        /// </summary>
        /// <param name="owner">The MSH this chunk should belong to</param>
        public MODL(MSH owner) : base(owner) {
            ChunkName = "MODL";
            Name = "New Model";
            Type = MTYP.Null;

            if (owner != null)
                Index = Owner.Models.Count + 1;
            else
                Index = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MODL"/> class.
        /// </summary>
        /// <param name="from">The <see cref="BaseChunk" /> to use for creating this Chunk. The given data will be interpreted respectively.</param>
        public MODL(BaseChunk from) : base(from) {
            while (!EndOfData) {
                BaseChunk nextChunk = ReadChunk();

                switch (nextChunk.ChunkName) {
                    case "MTYP":
                        Type = (MTYP)nextChunk.ReadInt32();
                        break;
                    case "MNDX":
                        index = nextChunk.ReadInt32();
                        break;
                    case "NAME":
                        Name = nextChunk.ReadString(nextChunk.Data.Length);
                        break;
                    case "PRNT":
                        parentName = nextChunk.ReadString(nextChunk.Data.Length);
                        break;
                    case "FLGS":
                        Flag = new ModelFlag(true, nextChunk.ReadInt32());
                        break;
                    case "TRAN":
                        Scale = nextChunk.ReadVector3();
                        Rotation = nextChunk.ReadVector3();
                        Translation = nextChunk.ReadVector3();
                        UnknownTRAN = nextChunk.ReadFloat();
                        break;
                    case "GEOM":
                        Geometry = new GEOM(nextChunk);
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

            WriteHeader("MTYP");
            WriteInt32(4);
            WriteInt32((int)Type);

            WriteHeader("MNDX");
            WriteInt32(4);
            WriteInt32(Index);

            WriteString("NAME", Name);

            if (Parent != null)
                WriteString("PRNT", Parent.Name);

            if (Flag.IsSet) {
                WriteHeader("FLGS");
                WriteInt32(4);
                WriteInt32(Flag.Value);
            }

            WriteHeader("TRAN");
            WriteInt32(40);
            WriteVector3(Scale);
            WriteVector3(Rotation);
            WriteVector3(Translation);
            WriteFloat(UnknownTRAN);

            WriteChunk(Geometry);

            WriteChunkLength();
        }

        /// <summary>
        /// Since in MSH References are saved by name (string) or list index (int32), we have to assign all necessary references manually.
        /// This should be overriden by any subclass which holds references to other Chunks (e.g. to every Segment one Material is assigned)
        /// </summary>
        /// <exception cref="NullReferenceException">No owner (MSH) set in Chunk " + ChunkName + "!</exception>
        public override void ApplyReferences() {
            if (Owner == null)
                throw new NullReferenceException("No owner (MSH) set in Chunk " + ChunkName + "!");

            if (!string.IsNullOrEmpty(parentName)) {
                foreach (MODL mdl in Owner.Models) {
                    if (mdl.Name.Equals(parentName)) {
                        Parent = mdl;
                        break;
                    }
                }
            }

            if (Geometry != null) {
                Geometry.ApplyReferences();
            }
        }

        /// <summary>
        /// Checks the integrity of the Chunk. Reports Error Messages if values are missing
        /// </summary>
        /// <returns></returns>
        public override CheckResult CheckIntegrity() {
            CheckResult result = new CheckResult();

            if (Name == null) {
                result.AddError("Name of Model (Index No "+ index +") is set to NULL! Must be a string!");
            }

            if (Type == MTYP.Null && Geometry != null) {
                result.AddError(Name + ": Model Type is set to NULL but Geometry Information is present. Geometry will be lost!");
            }

            if (Type == MTYP.Static && Geometry == null) {
                result.AddError(Name + ": Model Type is set to " + Type.ToString() + " but Geometry Information is NULL!");
            }

            return result;
        }

        public override string ToString() {
            return Name;
        }
    }
}
