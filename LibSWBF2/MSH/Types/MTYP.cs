namespace LibSWBF2.MSH.Types {
    /// <summary>
    /// Type of the Model. Most common Types are Static and Shadow. Null indicates no Geometry!
    /// </summary>
    public enum MTYP {
        Null = 0,
        Skin = 1,
        Envelope = 3,
        Static = 4,
        Shadow = 6
    }
}
