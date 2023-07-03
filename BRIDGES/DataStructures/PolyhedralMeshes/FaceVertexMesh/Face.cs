using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for a face in a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class Face<TPosition> : Face<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// Ordered list of the <see cref="Vertex{TPosition}"/> around the current <see cref="Face{TPosition}"/>.
        /// </summary>
        /// <remarks> This should never return an empty list. </remarks>
        internal List<Vertex<TPosition>> _faceVertices;

        /// <summary>
        /// Ordered list of the <see cref="Edge{TPosition}"/> bounding the current <see cref="Face{TPosition}"/>.
        /// </summary>
        /// <remarks> This should never return an empty list. <br/>
        /// This is not necessary in the face-vertex mesh data structure, but it allows simplifies and speeds up methods. </remarks>
        internal List<Edge<TPosition>> _faceEdges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Face{TVector}"/> class.
        /// </summary>
        /// <param name="index"> Index of the added edge in the mesh. </param>
        /// <param name="faceVertices"> Ordered list of face's vertex. </param>
        /// <param name="faceEdges">  Ordered list of face's edges. </param>
        internal Face(int index, List<Vertex<TPosition>> faceVertices, List<Edge<TPosition>> faceEdges)
            : base(index)
        {
            // Initialise fields
            _faceVertices = faceVertices;
            _faceEdges = faceEdges;

            for (int i_E = 0; i_E < _faceEdges.Count; i_E++)
            {
                _faceEdges[i_E]._adjacentFaces.Add(this);
            }
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Face<TPosition> face && Equals(face);
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 1104277913;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Vertex<TPosition>>>.Default.GetHashCode(_faceVertices);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"Face {Index}";
        }

        #endregion

        #region Override : Face<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - On this Face ********************/

        /// <inheritdoc/>
        internal override void Unset()
        {
            // Unset Fields
            _faceVertices = null;

            // Unset Properties
            Index = -1;
        }


        /******************** Methods - On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Vertex<TPosition>> FaceVertices()
        {
            Vertex<TPosition>[] faceVertices = new Vertex<TPosition>[_faceVertices.Count];
            for (int i_FV = 0; i_FV < _faceVertices.Count; i_FV++)
            {
                faceVertices[i_FV] = _faceVertices[i_FV];
            }

            return faceVertices;
        }


        /******************** Methods - On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Edge<TPosition>> FaceEdges()
        {
            Edge<TPosition>[] faceEdges = new Edge<TPosition>[_faceEdges.Count];
            for (int i_F = 0; i_F < _faceEdges.Count; i_F++)
            {
                faceEdges[i_F] = _faceEdges[i_F];
            }

            return faceEdges;
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Face<TPosition>> AdjacentFaces()
        {
            int edgeCount = _faceEdges.Count;

            List<Face<TPosition>> result = new List<Face<TPosition>>(edgeCount);

            for (int i_FE = 0; i_FE < edgeCount; i_FE++)
            {
                Edge<TPosition> edge = _faceEdges[i_FE];

                IReadOnlyList<Face<TPosition>> edgeFaces = edge.AdjacentFaces();
                for (int i_EF = 0; i_EF < edgeFaces.Count; i_EF++)
                {
                    Face<TPosition> edgeFace = edgeFaces[i_EF];

                    if (edgeFace.Equals(this)) { continue; }
                    else if (!result.Contains(edgeFace)) { result.Add(edgeFace); }
                }
            }

            return result;
        }

        #endregion
    }
}
