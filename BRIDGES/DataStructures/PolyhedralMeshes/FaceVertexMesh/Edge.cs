using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for an edge in a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class Edge<TPosition> : Edge<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// List of <see cref="Face{TPosition}"/> connected to the current <see cref="Edge{TPosition}"/>.
        /// </summary>
        /// <remarks> This is not necessary in the face-vertex mesh data structure, but it allows simplifies and speeds up methods. </remarks>
        internal List<Face<TPosition>> _adjacentFaces;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Edge{TPosition}"/> class.
        /// </summary>
        /// <param name="index"> Index of the added edge in the mesh. </param>
        /// <param name="startVertex"> Start vertex of the edge.</param>
        /// <param name="endVertex"> End vertex of the edge.</param>
        internal Edge(int index, Vertex<TPosition> startVertex, Vertex<TPosition> endVertex)
            : base(index, startVertex, endVertex)
        {
            // Instanciate fields
            _adjacentFaces = new List<Face<TPosition>>(2);

            // Initialise vertex fields
            startVertex._connectedEdges.Add(this);
            endVertex._connectedEdges.Add(this);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Edge<TPosition> edge ? this.Equals(edge) : false;
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 2018062386;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex<TPosition>>.Default.GetHashCode(StartVertex);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex<TPosition>>.Default.GetHashCode(EndVertex);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"FvEdge {Index} from vertex {StartVertex.Index} to {EndVertex.Index}.";
        }

        #endregion

        #region Override : Vertex<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - For this Edges ********************/

        /// <inheritdoc/>
        public override bool IsBoundary()
        {
            return _adjacentFaces.Count < 2;
        }


        /// <inheritdoc/>
        internal override void Unset()
        {
            // Unset Fields
            _adjacentFaces = null;

            // Unset Properties
            Index = -1;
            StartVertex = null;
            EndVertex = null;
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Face<TPosition>> AdjacentFaces()
        {
            Face<TPosition>[] adjacentFaces = new Face<TPosition>[_adjacentFaces.Count];
            for (int i_AF = 0; i_AF < _adjacentFaces.Count; i_AF++)
            {
                adjacentFaces[i_AF] = _adjacentFaces[i_AF];
            }

            return adjacentFaces;
        }

        #endregion
    }
}
