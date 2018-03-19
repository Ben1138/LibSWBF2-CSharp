using LibSWBF2.Types;

namespace LibSWBF2.WLD.Types {
    /// <summary>
    /// Represents an Object placed in the world with its Name, Name of the Mesh File (MSH), Position and Rotation respectively.
    /// </summary>
    public struct WorldObject {
        public string name;
        public string meshName;
        public Vector3 position;
        public Vector4 rotation;
    }
}
