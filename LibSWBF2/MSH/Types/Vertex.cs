using LibSWBF2.Types;
using LibSWBF2.MSH.Chunks;

namespace LibSWBF2.MSH.Types {
    /// <summary>
    /// Representing a Vertex with all Information
    /// </summary>
    public class Vertex : BaseChunk {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uvCoordinate;


        public Vertex(Vector3 position, Vector3 normal, Vector2 uv) {
            this.position = position;
            this.normal = normal;
            uvCoordinate = uv;
        }

        public override CheckResult CheckIntegrity() {
            //always valid
            return new CheckResult();
        }

        public override string ToString() {
            return position + "  " + normal + "  " + uvCoordinate;
        }
    }
}
