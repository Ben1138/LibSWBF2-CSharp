using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSWBF2.WLD.Types {
    public struct TerrainLayer {
        public string DiffuseTexture { get; set; }
        public string DetailTexture { get; set; }
        public float TileRange { get; set; }
        public byte MappingType { get; set; }
    }
}
