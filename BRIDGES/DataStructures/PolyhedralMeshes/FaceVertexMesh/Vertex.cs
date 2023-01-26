using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for a vertex in a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class Vertex<TPosition> : Vertex<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Fields

        /// <summary>
        /// List of <see cref="Edge{TPosition}"/> connected to the current <see cref="Vertex{TPosition}"/>.
        /// </summary>
        /// <remarks> This is not necessary in the face-vertex mesh data structure, but it allows simplifies and speeds up methods. </remarks>
        internal List<Edge<TPosition>> _connectedEdges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vertex{TPosition}"/> class from its index and position.
        /// </summary>
        /// <param name="index"> Index of the new vertex in the mesh. </param>
        /// <param name="position"> Position of the vertex. </param>
        internal Vertex(int index, TPosition position) 
            : base(index, position)
        {
            // Initialise Fields
            _connectedEdges = new List<Edge<TPosition>>();
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Vertex<TPosition> vertex ? this.Equals(vertex) : false;
        }

        /// <inheritdoc cref="object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = 2096401715;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TPosition>.Default.GetHashCode(Position);
            return hashCode;
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"FvVertex {Index} at {Position}.";
        }

        #endregion

        #region Override : Vertex<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - For this Vertex ********************/

        /// <inheritdoc/>
        public override bool IsBoundary()
        {
            int edgeValency = _connectedEdges.Count;

            if (edgeValency == 0) { return true; }

            for (int i_CE = 0; i_CE < edgeValency; i_CE++)
            {
                if (_connectedEdges[i_CE].IsBoundary()) { return true; }
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool IsConnected()
        {
            return _connectedEdges.Count != 0;
        }

        /// <inheritdoc/>
        public override int Valence()
        {
            return _connectedEdges.Count;
        }


        /// <inheritdoc/>
        internal override void Unset()
        {
            // Unset Fields
            _connectedEdges = null;
            // Unset Properties 
            Index = -1;
            Position = default;
        }


        /******************** Methods - On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Vertex<TPosition>> NeighbourVertices()
        {
            int edgeValency = _connectedEdges.Count;

            Vertex<TPosition>[] result = new Vertex<TPosition>[edgeValency];

            for (int i_CE = 0; i_CE < edgeValency; i_CE++)
            {
                Edge<TPosition> edge = _connectedEdges[i_CE];
                if (edge.StartVertex.Equals(this)) { result[i_CE] = edge.EndVertex; }
                else { result[i_CE] = edge.StartVertex; }
            }

            return result;
        }


        /******************** Methods - On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Edge<TPosition>> ConnectedEdges()
        {
            Edge<TPosition>[] connectedEdges = new Edge<TPosition>[_connectedEdges.Count];
            for (int i_CE = 0; i_CE < _connectedEdges.Count; i_CE++)
            {
                connectedEdges[i_CE] = _connectedEdges[i_CE];
            }

            return connectedEdges;
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Face<TPosition>> AdjacentFaces()
        {
            int edgeValency = _connectedEdges.Count;

            List<Face<TPosition>> result = new List<Face<TPosition>>(edgeValency);

            for (int i_CE = 0; i_CE < edgeValency; i_CE++)
            {
                Edge<TPosition> edge = _connectedEdges[i_CE];

                IReadOnlyList<Face<TPosition>> edgeFaces = edge.AdjacentFaces();
                for (int i_EF = 0; i_EF < edgeFaces.Count; i_EF++)
                {
                    Face<TPosition> edgeFace = edgeFaces[i_EF];

                    if (!result.Contains(edgeFace)) { result.Add(edgeFace); }
                }
            }

            return result;
        }

        #endregion
    }
}
