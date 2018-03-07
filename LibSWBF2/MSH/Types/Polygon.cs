using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LibSWBF2.MSH.Types {
    public class Polygon {
        public Vertex[] Vertices {
            get {
                List<Vertex> verts = new List<Vertex>();

                foreach(short index in VertexIndices) {
                    int ind = index;
                    /*if (ind >= verticesRef.Count)
                        ind = 0;*/

                    verts.Add(verticesRef[ind]);
                }

                return verts.ToArray();
            }
        }
        [Browsable(false)]
        public List<short> VertexIndices { get; private set; } = new List<short>();

        private List<Vertex> verticesRef;


        public Polygon(List<Vertex> vertices) {
            verticesRef = vertices;
        }

        public override string ToString() {
            return VertexIndices.Count + " Vertices";
        }
    }
}
