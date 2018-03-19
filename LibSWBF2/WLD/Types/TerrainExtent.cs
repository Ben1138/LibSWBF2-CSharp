namespace LibSWBF2.WLD.Types {
    /// <summary>
    /// <para>Terrain extent in comparison to the Map Center. E.g. for a 256x256 Terrain, these value are typically -128, -128, 128, 128</para>
    /// <para>This does also function to clip the Terrain Data</para>
    /// </summary>
    public class TerrainExtent {
        public short FromX { get; set; }
        public short ToX { get; set; }
        public short FromY { get; set; }
        public short ToY { get; set; }
    }
}
