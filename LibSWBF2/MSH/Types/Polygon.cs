using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LibSWBF2.MSH.Types {
    public class Polygon {
        //to avoid knowing our parent, we get a reference in the constructor to the vertex list instead
        //we need this to build up our own vertex list from vertex indices
        private List<Vertex> verticesRef;

        /// <summary>
        /// The Vertices this Polygon is made of
        /// </summary>
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

        /// <summary>
        /// Gets the vertex indices.
        /// </summary>
        [Browsable(false)]
        public List<short> VertexIndices { get; private set; } = new List<short>();


        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        /// <param name="vertices">The overall Vertex List</param>
        /// <exception cref="ArgumentNullException">Thrown if no Reference to the overall vertex list is given</exception>
        public Polygon(List<Vertex> vertices) {
            verticesRef = vertices ?? throw new ArgumentNullException("You need to pass the reference to the overall vertex list!");
        }

        public override string ToString() {
            return VertexIndices.Count + " Vertices";
        }
    }
}
