using System;
using System.Collections.Generic;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;
using BRIDGES.DataStructures.PolyhedralMeshes;

using BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for a vertex in a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    public class Vertex<TPosition> : Vertex<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>
    {
        #region Properties

        /// <summary>
        /// An outgoing halfedge of the current vertex.
        /// </summary>
        /// <remarks> If the current vertex is on the boundary, the outgoing halfedge must be the boundary one.</remarks>
        public Halfedge<TPosition> OutgoingHalfedge { get; internal set; }

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
            OutgoingHalfedge = null;
        }

        #endregion

        #region Methods

        /******************** On Halfedges ********************/

        /// <summary>
        /// Identifies the halfedges whose start is the current vertex. 
        /// </summary>
        /// <returns> The list of outgoing halfedges. An empty list can be returned. </returns>
        public IReadOnlyList<Halfedge<TPosition>> OutgoingHalfedges()
        {
            List<Halfedge<TPosition>> result = new List<Halfedge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }


            Halfedge<TPosition> firstOutgoing = OutgoingHalfedge;
            result.Add(firstOutgoing);

            Halfedge<TPosition> outgoing = firstOutgoing.PrevHalfedge.PairHalfedge;

            while (!firstOutgoing.Equals(outgoing))
            {
                result.Add(outgoing);
                outgoing = outgoing.PrevHalfedge.PairHalfedge;
            }

            return result;
        }


        /// <summary>
        /// Identifies the halfedges whose end is the current vertex. 
        /// </summary>
        /// <returns> The list of incomming halfedges. An empty list can be returned. </returns>
        public IReadOnlyList<Halfedge<TPosition>> IncomingHalfedges()
        {
            List<Halfedge<TPosition>> result = new List<Halfedge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }

            Halfedge<TPosition> firstIncoming = OutgoingHalfedge.PairHalfedge;
            result.Add(firstIncoming);

            Halfedge<TPosition> incoming = firstIncoming.PairHalfedge.PrevHalfedge;

            while (!firstIncoming.Equals(incoming))
            {
                result.Add(incoming);
                incoming = incoming.PairHalfedge.PrevHalfedge;
            }
            return result;
        }

        /// <summary>
        /// Computes the normal vector of the vertex based on the adjacent face normals.
        /// </summary>
        /// <typeparam name="T"> A vertex parameter type.</typeparam>
        /// <param name="vertex"> The vertex to evaluate.</param>
        /// <param name="faceWeight"> Indicates whether the normal vector should be weigted by the area of the adjacent faces.</param>
        /// <returns> The normal vector of the vertex.</returns>
        public Vector Normal<T>(bool faceWeight = false)
            where T : IVertex<Point>
        {
            Vector normal = new Vector();
            double weight = 1;

            foreach (IFace<Point> face in this.AdjacentFaces())
            {
                //if (faceWeight) { weight = face.FaceArea(); }

                normal += face.Normal() * weight;
            }
            normal.Unitize();

            return normal;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Vertex<TPosition> vertex && Equals(vertex);
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
            return $"HeVertex {Index} at {Position}.";
        }

        #endregion

        #region Override : Vertex<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** For this Vertex ********************/

        /// <inheritdoc/>
        public override bool IsBoundary()
        {
            if (OutgoingHalfedge is null) { return true; }

            else if (OutgoingHalfedge.IsBoundary()) { return true; }

            else { return false; }
        }

        /// <inheritdoc/>
        public override bool IsConnected()
        {
            return !(OutgoingHalfedge is null);
        }

        /// <inheritdoc/>
        public override int Valence()
        {
            return OutgoingHalfedges().Count;
        }


        /// <inheritdoc/>
        internal override void Unset()
        {
            // Instanciate properties
            OutgoingHalfedge = null;

            // Initialise properties
            Index = -1;
            Position = default;
        }


        /******************** On Vertices ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Vertex<TPosition>> NeighbourVertices()
        {
            IReadOnlyList<Halfedge<TPosition>> outgoings = OutgoingHalfedges();

            int edgeValency = outgoings.Count;
            Vertex<TPosition>[] result = new Vertex<TPosition>[edgeValency];

            for (int i_OHe = 0; i_OHe < edgeValency; i_OHe++)
            {
                result[i_OHe] = outgoings[i_OHe].EndVertex;
            }

            return result;
        }


        /******************** On Edges ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Edge<TPosition>> ConnectedEdges()
        {
            List<Edge<TPosition>> result = new List<Edge<TPosition>>();

            // If the vertex is not connected
            if (OutgoingHalfedge is null) { return result; }


            Halfedge<TPosition> firstOutgoing = OutgoingHalfedge;
            Edge<TPosition> firstEdge = firstOutgoing.GetEdge();
            result.Add(firstEdge);

            Halfedge<TPosition> outgoing = firstOutgoing.PrevHalfedge.PairHalfedge;

            while (!firstOutgoing.Equals(outgoing))
            {
                Edge<TPosition> edge = outgoing.GetEdge();

                result.Add(edge);
                outgoing = outgoing.PrevHalfedge.PairHalfedge;
            }

            return result;
        }


        /******************** On Faces ********************/

        /// <inheritdoc/>
        public override IReadOnlyList<Face<TPosition>> AdjacentFaces()
        {
            IReadOnlyList<Halfedge<TPosition>> outgoings = OutgoingHalfedges();

            int edgeValency = outgoings.Count;
            List<Face<TPosition>> result = new List<Face<TPosition>>(edgeValency);

            for (int i_OHe = 0; i_OHe < edgeValency; i_OHe++)
            {
                Halfedge<TPosition> outgoing = outgoings[i_OHe];

                if (!(outgoing.AdjacentFace is null)) { result.Add(outgoing.AdjacentFace); }
            }

            return result;
        }

        internal double InfluenceArea()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
