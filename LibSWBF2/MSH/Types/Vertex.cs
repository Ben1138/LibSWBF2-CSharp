using LibSWBF2.Types;
using LibSWBF2.MSH.Chunks;

namespace LibSWBF2.MSH.Types {
    /// <summary>
    /// Representing a Vertex with all Information
    /// </summary>
    public class Vertex {
        /// <summary>
        /// Position of this Vertex in 3D space
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// Normal Vector of this Vertex
        /// </summary>
        public Vector3 normal;

        /// <summary>
        /// UV coordinate of this Vertex
        /// </summary>
        public Vector2 uvCoordinate;


        /// <summary>
        /// Create a new <see cref="Vertex"/>
        /// </summary>
        /// <param name="position">Position of the Vertex in 3D space</param>
        /// <param name="normal">Normal Vector of the Vertex</param>
        /// <param name="uv">UV coordinate of the Vertex</param>
        public Vertex(Vector3 position, Vector3 normal, Vector2 uv) {
            //since all Vectors are structs, we don't need to check for null
            this.position = position;
            this.normal = normal;
            uvCoordinate = uv;
        }

        public override string ToString() {
            return position + "  " + normal + "  " + uvCoordinate;
        }
    }
}
