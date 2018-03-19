namespace LibSWBF2.WLD.Types {
    /// <summary>
    /// Representing a single Texture Layer for a Terrain
    /// </summary>
    public struct TerrainLayer {
        /// <summary>
        /// The Diffuse / Albedo Texture
        /// </summary>
        public string DiffuseTexture { get; set; }

        /// <summary>
        /// The Detail Texture when close to the Player
        /// </summary>
        public string DetailTexture { get; set; }

        /// <summary>
        /// How big this Texture Tiles (After how many Units this texture repeats)
        /// </summary>
        public float TileRange { get; set; }

        /// <summary>
        /// Usually 0
        /// </summary>
        public byte MappingType { get; set; }
    }
}
