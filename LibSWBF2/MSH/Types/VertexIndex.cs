namespace LibSWBF2.MSH.Types {
    /// <summary>
    /// Representing a Vertex index in MSH. A Vertex Index can be marked as a polygon boundary
    /// </summary>
    public struct VertexIndex {
        public short index;
        public bool polyBoundary;
    }
}
