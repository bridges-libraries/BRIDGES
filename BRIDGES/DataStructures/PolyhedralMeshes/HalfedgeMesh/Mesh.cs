﻿using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Sets = BRIDGES.Algebra.Sets;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    /// <summary>
    /// Class for a polyhedral halfedge mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
    [Serialisation.Serialisable]
    public partial class Mesh<TPosition> : Mesh<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>,
                          Alg_Fund.IAddable<TPosition> /* To Do : Remove */,
                          Alg_Sets.IGroupAction<TPosition, double>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the <see cref="Vertex{TPosition}"/> of the current <see cref="Mesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="Vertex{TPosition}"/>; Value : Corresponding <see cref="Vertex{TPosition}"/>. </remarks>
        protected Dictionary<int, Vertex<TPosition>> _vertices;

        /// <summary>
        /// Index for a newly created vertex.
        /// </summary>
        /// <remarks> This may not match with <see cref="VertexCount"/> if vertices are removed from the mesh. </remarks>
        protected int _newVertexIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Edge{TPosition}"/> of the current <see cref="Mesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="Edge{TPosition}"/>; Value : Corresponding <see cref="Edge{TPosition}"/>. </remarks>
        protected Dictionary<int, Halfedge<TPosition>> _halfedges;

        /// <summary>
        /// Index for a newly created edge.
        /// </summary>
        /// <remarks> This may not match with <see cref="EdgeCount"/> if edges are removed from the mesh. </remarks>
        protected int _newHalfedgeIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Face{TPosition}"/> of the current <see cref="Mesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="Face{TPosition}"/>; Value : Corresponding <see cref="Face{TPosition}"/>. </remarks>
        protected Dictionary<int, Face<TPosition>> _faces;

        /// <summary>
        /// Index for a newly created face.
        /// </summary>
        /// <remarks> This may not match with <see cref="FaceCount"/> if faces are removed from the mesh. </remarks>
        protected int _newFaceIndex;


        /// <summary>
        /// Background field for the <see cref="Tri"/> property
        /// </summary>
        private TriMesh _triangular = null;

        /// <summary>
        /// Background field for the <see cref="Quad"/> property
        /// </summary>
        public QuadMesh __quadrangular = null;

        /// <summary>
        /// Background field for the <see cref="Hexa"/> property
        /// </summary>
        public HexaMesh _hexagonal = null;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int VertexCount
        {
            get { return _vertices.Count; }
        }

        /// <inheritdoc/>
        public int HalfedgeCount
        {
            get { return _halfedges.Count; }
        }

        /// <inheritdoc/>
        public override int EdgeCount
        {
            get { return _halfedges.Count / 2; }
        }

        /// <inheritdoc/>
        public override int FaceCount
        {
            get { return _faces.Count; }
        }


        /// <inheritdoc cref="IMesh{TPosition}.Tri"/>
        public TriMesh Tri
        {
            get 
            {
                if (_triangular is null) { _triangular = new TriMesh(this); return _triangular; }
                else { return _triangular; }
            }
        }

        /// <inheritdoc cref="IMesh{TPosition}.Quad"/>
        public QuadMesh Quad
        {
            get
            {
                if (__quadrangular is null) { __quadrangular = new QuadMesh(this); return __quadrangular; }
                else { return __quadrangular; }
            }
        }

        /// <inheritdoc cref="IMesh{TPosition}.Hexa"/>
        public HexaMesh Hexa
        {
            get
            {
                if (_hexagonal is null) { _hexagonal = new HexaMesh(this); return _hexagonal; }
                else { return _hexagonal; }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Mesh{TPosition}"/> class.
        /// </summary>
        public Mesh()
            : base()
        {
            // Instanciate fields
            _vertices = new Dictionary<int, Vertex<TPosition>>();
            _halfedges = new Dictionary<int, Halfedge<TPosition>>();
            _faces = new Dictionary<int, Face<TPosition>>();

            // Initialise fields
            _newVertexIndex = 0;
            _newHalfedgeIndex = 0;
            _newFaceIndex = 0;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Mesh{TPosition}"/> class from its fields.
        /// </summary>
        internal Mesh(Dictionary<int, Vertex<TPosition>> vertices, Dictionary<int, Halfedge<TPosition>> halfedges, Dictionary<int, Face<TPosition>> faces,
            int newVertexIndex, int newHalfedgeIndex, int newFaceIndex)
        {
            // Instanciate fields
            _vertices = vertices;
            _halfedges = halfedges;
            _faces = faces;

            // Initialise fields
            _newVertexIndex = newVertexIndex;
            _newHalfedgeIndex = newHalfedgeIndex;
            _newFaceIndex = newFaceIndex;
        }

        #endregion

        #region Methods

        /******************** On Halfedges ********************/

        /// <summary>
        /// Adds a new pair of halfedges to the mesh from their end vertices.
        /// </summary>
        /// <remarks>
        /// Halfedges are always created in pairs, hence one as an index 2i and the pair 2i+1.<br/>
        /// This method does not manage the connectivity within the mesh.
        /// </remarks>
        /// <param name="startVertex"> Start vertex of the halfedge. </param>
        /// <param name="endVertex"> End vertex of the halfedge. </param>
        /// <returns> The first halfedge in the pair if it was added, <see langword="null"/> otherwise .</returns>
        internal Halfedge<TPosition> AddPair(Vertex<TPosition> startVertex, Vertex<TPosition> endVertex)
        {
            // Verification : Avoid looping halfedges
            if (startVertex.Equals(endVertex)) { return null; }

            // Verifications : Avoid duplicate halfedges
            Halfedge<TPosition> exitingHalfedge = HalfedgeBetween(startVertex, endVertex);
            if (!(exitingHalfedge is null)) { return null; }

            Halfedge<TPosition> firstHalfedge = new Halfedge<TPosition>(_newHalfedgeIndex, startVertex, endVertex);
            Halfedge<TPosition> pairHalfedge = new Halfedge<TPosition>(_newHalfedgeIndex + 1, endVertex, startVertex);

            firstHalfedge.PairHalfedge = pairHalfedge;
            pairHalfedge.PairHalfedge = firstHalfedge;

            _halfedges.Add(_newHalfedgeIndex, firstHalfedge);
            _halfedges.Add(_newHalfedgeIndex + 1, pairHalfedge);

            _newHalfedgeIndex += 2;

            return firstHalfedge;
        }


        /// <summary>
        /// Returns the halfedge at the given index in the mesh.
        /// </summary>
        /// <returns> The hlafedge at the given index in the mesh. </returns>
        public Halfedge<TPosition> GetHalfedge(int index)
        {
            return _halfedges[index];
        }

        /// <summary>
        /// Returns the halfedge at the given index in the mesh if it exists, <see langword="null"/> otherwise. 
        /// </summary>
        /// <param name="index"> The index of the halfedge to look for. </param>
        /// <returns> The halfedge if it exists, <see langword="null"/> otherwise. </returns>
        public Halfedge<TPosition> TryGetHalfedge(int index)
        {
            _halfedges.TryGetValue(index, out Halfedge<TPosition> halfedge);

            return halfedge;
        }

        /// <summary>
        /// Returns the list of halfedges of the current mesh.
        /// </summary>
        /// <remarks> 
        /// If some halfedges were removed from the mesh, the index of the halfedge in the returned list might not match the halfedge index in the mesh. <br/>
        /// The index of the halfedges in the mesh is accessible through the Index property.
        /// </remarks>
        /// <returns> List of edges of the mesh. </returns>
        public IReadOnlyList<Halfedge<TPosition>> GetHalfedges()
        {
            Halfedge<TPosition>[] result = new Halfedge<TPosition>[HalfedgeCount];

            int i_He = 0;
            foreach (Halfedge<TPosition> halfedge in _halfedges.Values)
            {
                result[i_He] = halfedge;
                i_He++;
            }

            return result;
        }


        /// <summary>
        /// Looks for the halfedge starting at <paramref name="startVertex"/> and ending at <paramref name="endVertex"/>.
        /// </summary>
        /// <param name="startVertex"> Start vertex of the halfedge. </param>
        /// <param name="endVertex"> End vertex of the halfedge. </param>
        /// <returns> The halfedge if it exists, <see langword="null"/> otherwise.</returns>
        public Halfedge<TPosition> HalfedgeBetween(Vertex<TPosition> startVertex, Vertex<TPosition> endVertex)
        {
            IReadOnlyList<Halfedge<TPosition>> outgoings = startVertex.OutgoingHalfedges();
            for (int i_OHe = 0; i_OHe < outgoings.Count; i_OHe++)
            {
                Halfedge<TPosition> outgoing = outgoings[i_OHe];

                if (outgoing.EndVertex.Equals(endVertex)) { return outgoing; }
            }

            return null;
        }

        /// <summary>
        /// Gets the list of halfedges forming a loop, starting from <paramref name="firstHalfedge"/>.
        /// </summary>
        /// <param name="firstHalfedge"> Halfedge to start the computation of the loop. </param>
        /// <returns> The ordered list of halfedges in the loop. An empty list can be returned. </returns>
        private IReadOnlyList<Halfedge<TPosition>> HalfedgeLoop(Halfedge<TPosition> firstHalfedge)
        {
            List<Halfedge<TPosition>> halfedges = new List<Halfedge<TPosition>>();

            if (firstHalfedge is null) { return halfedges; }

            halfedges.Add(firstHalfedge);
            Halfedge<TPosition> halfedge = firstHalfedge.NextHalfedge;

            while (!halfedge.Equals(firstHalfedge))
            {
                halfedges.Add(halfedge);
                halfedge = halfedge.NextHalfedge;
            }
            return halfedges;
        }


        /// <summary>
        /// Removes the halfedge and its pair halfedge by keeping the mesh manifold. 
        /// </summary>
        /// <param name="halfedge"> The halfedge to remove.</param>
        public void RemoveHalfedge(Halfedge<TPosition> halfedge)
        {
            Halfedge<TPosition> pairHalfedge = halfedge.PairHalfedge;

            // If the halfedge and its pair halfedge are naked.
            if (halfedge.IsBoundary() && pairHalfedge.IsBoundary())
            {
                Vertex<TPosition> startVertex = halfedge.StartVertex;
                Vertex<TPosition> endVertex = halfedge.StartVertex;

                EraseHalfedge(halfedge);

                if (!startVertex.IsConnected()) { EraseVertex(startVertex);}

                if (!endVertex.IsConnected()) { EraseVertex(endVertex); }
            }
            // If the pair halfedge is naked (but not the halfedge).
            else if (!halfedge.IsBoundary() && pairHalfedge.IsBoundary()) { RemoveFace(halfedge.AdjacentFace); }

            // If the halfedge is naked (but not the pair halfedge).
            else if (!pairHalfedge.IsBoundary() && halfedge.IsBoundary()) { RemoveFace(pairHalfedge.AdjacentFace); }

            // If neither the halfedge and the pair halfedge are naked.
            else
            {
                RemoveFace(halfedge.AdjacentFace);
                RemoveFace(pairHalfedge.AdjacentFace);
            }
        }


        /// <summary>
        /// Erases any reference to the halfedge and its pair halfedge, then delete them from the mesh.
        /// </summary>
        /// <param name="halfedge"> The halfedge to erase.</param>
        public void EraseHalfedge(Halfedge<TPosition> halfedge)
        {
            Halfedge<TPosition> pairHalfedge = halfedge.PairHalfedge;

            /***** Manage connection of the neighbour edges *****/

            halfedge.PrevHalfedge.NextHalfedge = pairHalfedge.NextHalfedge;
            halfedge.NextHalfedge.PrevHalfedge = pairHalfedge.PrevHalfedge;
            pairHalfedge.PrevHalfedge.NextHalfedge = halfedge.NextHalfedge;
            pairHalfedge.NextHalfedge.PrevHalfedge = halfedge.PrevHalfedge;

            /***** Manage connection with start and end vertices *****/

            // If the halfedge is stored in its start vertex.
            if (halfedge.StartVertex.OutgoingHalfedge.Equals(halfedge))
            {
                // If the start vertex is only connected to the erased halfedge pair.
                if (halfedge.PrevHalfedge.Equals(pairHalfedge)) { halfedge.StartVertex.OutgoingHalfedge = null; }

                else { halfedge.StartVertex.OutgoingHalfedge = pairHalfedge.NextHalfedge; }
            }

            // If the pair halfedge is stored in its start vertex.
            if (pairHalfedge.StartVertex.OutgoingHalfedge.Equals(pairHalfedge))
            {
                // If the start vertex is only connected to the erased halfedge pair.
                if (pairHalfedge.PrevHalfedge.Equals(halfedge)) { pairHalfedge.StartVertex.OutgoingHalfedge = null; }
                else { pairHalfedge.StartVertex.OutgoingHalfedge = halfedge.NextHalfedge; }
            }

            /***** Manage connection with adjacent face *****/

            // If neither the halfedge and its pair halfedge are on a boundary.
            if ((!(halfedge.AdjacentFace is null)) && (!(pairHalfedge.AdjacentFace is null)))
            {
                EraseFace(halfedge.AdjacentFace, halfedge.NextHalfedge);

                // We cannot call erase the adjacent face of the pair halfedge.
                // Most of the work has already been done in the above "EraseFace".
                _faces.Remove(pairHalfedge.AdjacentFace.Index);
                pairHalfedge.AdjacentFace.Unset();
            }
            // If the pair halfedge is on a boundary but not the halfedge.
            else if (!(halfedge.AdjacentFace is null))
            {
                EraseFace(halfedge.AdjacentFace, halfedge.NextHalfedge);
            }
            // If the halfedge is on a boundary but not the pair halfedge.
            else if (!(pairHalfedge.AdjacentFace is null))
            {
                EraseFace(pairHalfedge.AdjacentFace, pairHalfedge.NextHalfedge);
            }

            // Remove the pair of edges from the mesh
            _halfedges.Remove(halfedge.Index);
            _halfedges.Remove(pairHalfedge.Index);

            // Unset the pair of edges

            halfedge.Unset();
            pairHalfedge.Unset();
        }


        /******************** On Faces ********************/

        /// <summary>
        /// Erases a face after the connection between halfedges has been adapted for a halfedge pair erasure. <br/>
        /// Deletes the reference of adjacent face in a loop of former face halfedges and in the disconnected halfedge pair.
        /// </summary>
        /// <param name="face"> Face to erase. </param>
        /// <param name="firstHalfedge"> Halfedge to start the computation of the loop. </param>
        private void EraseFace(Face<TPosition> face, Halfedge<TPosition> firstHalfedge)
        {
            // Manage connection with edges
            foreach (Halfedge<TPosition> edge in HalfedgeLoop(firstHalfedge))
            {
                edge.AdjacentFace = null;
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }


        /******************** On Meshes ********************/

        /// <summary>
        /// Creates a face-vertex mesh from the current halfedge mesh.
        /// </summary>
        /// <returns> Face-vertex mesh which represents the topology and geometry of the current halfedge mesh. </returns>
        public FaceVertexMesh.Mesh<TPosition> ToFaceVertexMesh()
        {
            Dictionary<int, FaceVertexMesh.Vertex<TPosition>> fvVertices = new Dictionary<int, FaceVertexMesh.Vertex<TPosition>>();
            Dictionary<int, FaceVertexMesh.Edge<TPosition>> fvEdges = new Dictionary<int, FaceVertexMesh.Edge<TPosition>>();
            Dictionary<int, FaceVertexMesh.Face<TPosition>> fvFaces = new Dictionary<int, FaceVertexMesh.Face<TPosition>>();

            // Add vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);

                FaceVertexMesh.Vertex<TPosition> fvVertex = new FaceVertexMesh.Vertex<TPosition>(vertexIndex, vertex.Position);

                fvVertices.Add(vertexIndex, fvVertex);
            }
            vertexIndexEnumerator.Dispose();

            // Add edges & Manage vertices connectivity (_connectedEdges)
            var halfedgeIndexEnumerator = _halfedges.Keys.GetEnumerator();

            while (halfedgeIndexEnumerator.MoveNext())
            {
                int halfedgeIndex = halfedgeIndexEnumerator.Current;

                Halfedge<TPosition> halfedge = GetHalfedge(halfedgeIndex);

                FaceVertexMesh.Vertex<TPosition> fvStartVertex = fvVertices[halfedge.StartVertex.Index];
                FaceVertexMesh.Vertex<TPosition> fvEndVertex = fvVertices[halfedge.EndVertex.Index];

                FaceVertexMesh.Edge<TPosition> fvEdge = new FaceVertexMesh.Edge<TPosition>(halfedgeIndex / 2, fvStartVertex, fvEndVertex);
                fvEdges.Add(halfedgeIndex / 2, fvEdge);

                fvStartVertex._connectedEdges.Add(fvEdge);
                fvEndVertex._connectedEdges.Add(fvEdge);

                // Skip the pair halfedge
                halfedgeIndexEnumerator.MoveNext();
            }
            halfedgeIndexEnumerator.Dispose();


            // Add faces & Manage edges connectivity (_adjacentfaces)
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                Face<TPosition> face = GetFace(faceIndex);

                IReadOnlyList<Halfedge<TPosition>> faceHalfedges = face.FaceHalfedges();

                List<FaceVertexMesh.Edge<TPosition>> fvFaceEdges = new List<FaceVertexMesh.Edge<TPosition>>(faceHalfedges.Count);
                for (int i_FH = 0; i_FH < faceHalfedges.Count; i_FH++)
                {
                    fvFaceEdges.Add(fvEdges[faceHalfedges[i_FH].Index / 2]);
                }

                List<FaceVertexMesh.Vertex<TPosition>> fvFaceVertices = new List<FaceVertexMesh.Vertex<TPosition>>(faceHalfedges.Count);
                for (int i_FH = 0; i_FH < faceHalfedges.Count; i_FH++)
                {
                    fvFaceVertices.Add(fvVertices[faceHalfedges[i_FH].StartVertex.Index]);
                }

                FaceVertexMesh.Face<TPosition> fvFace = new FaceVertexMesh.Face<TPosition>(faceIndex, fvFaceVertices, fvFaceEdges);
                fvFaces.Add(faceIndex, fvFace);

                for (int i_FE = 0; i_FE < fvFaceEdges.Count; i_FE++)
                {
                    fvFaceEdges[i_FE]._adjacentFaces.Add(fvFace);
                }
            }
            faceIndexEnumerator.Dispose();


            return new FaceVertexMesh.Mesh<TPosition>(fvVertices, fvEdges, fvFaces, _newVertexIndex, _newHalfedgeIndex / 2, _newFaceIndex);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"HeMesh (V:{VertexCount}, H:{HalfedgeCount}, F:{FaceCount})";
        }

        #endregion

        #region Override : Mesh<T,HeVertex<T>,HeEdge<T>,HeFace<T>>

        /******************** For this Mesh ********************/

        /// <inheritdoc/>
        public override void CleanMesh(bool cullIsolated = true)
        {
            /* Isolated Face : Face with no adjacent faces.
             * Isolated Edge : Edge (and pair edge) with no adjacent faces.
             * Isolated Vertex : Vertex with connected edges.
             * 
             * Could be replaced by:
             * Isolated Edge : Edge (and pair edge) with no adjacent faces, and whose start and end vertices have a valency of 1.
             * Isolated Vertex : Vertex with connected edges.
             */


            /********** For Faces **********/

            if ((FaceCount != _newFaceIndex) || cullIsolated)
            {
                int newFaceIndex = 0;
                List<int> isolatedFaces = new List<int>();
                Dictionary<int, Face<TPosition>> newFaces = new Dictionary<int, Face<TPosition>>();


                foreach (int key in _faces.Keys)
                {
                    int nb_Adjacentface = _faces[key].AdjacentFaces().Count;
                    if (cullIsolated && nb_Adjacentface == 0)
                    {
                        isolatedFaces.Add(key);     // Marks for removal.
                        continue;                   // Avoids storing the face in the new dictionnary of faces.
                    }
                    else
                    {
                        newFaces.Add(newFaceIndex, _faces[key]);
                        _faces[key].Index = newFaceIndex;
                        newFaceIndex += 1;
                    }
                }

                // Remove isolated faces
                foreach (int key in isolatedFaces) { RemoveFace(key); }

                // Reconfigure mesh faces.
                _faces = newFaces;
                _newFaceIndex = FaceCount;
            }

            /********** For Halfedges **********/

            if ((HalfedgeCount != _newHalfedgeIndex))
            {
                int newHeIndex = 0;
                List<Halfedge<TPosition>> isolatedHe = new List<Halfedge<TPosition>>();
                Dictionary<int, Halfedge<TPosition>> newHalfedges = new Dictionary<int, Halfedge<TPosition>>();

                var enumerator = _halfedges.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Halfedge<TPosition> halfedge = enumerator.Current;

                    if (cullIsolated && halfedge.IsBoundary() && halfedge.PairHalfedge.IsBoundary())
                    {
                        isolatedHe.Add(halfedge);        // Marks for removal.
                        enumerator.MoveNext();          // Passes to the pair edge.
                    }
                    else
                    {
                        // Stores the edge
                        newHalfedges.Add(newHeIndex, halfedge);
                        halfedge.Index = newHeIndex;
                        newHeIndex += 1;

                        // Move to the next edge
                        enumerator.MoveNext();
                        halfedge = enumerator.Current;

                        // Stores the pair edge
                        newHalfedges.Add(newHeIndex, halfedge);
                        halfedge.Index = newHeIndex;
                        newHeIndex += 1;
                    }
                }

                // Remove isolated edges
                foreach (Halfedge<TPosition> halfedge in isolatedHe) { RemoveHalfedge(halfedge); }

                // Reconfigure mesh edges.
                _halfedges = newHalfedges;
                _newHalfedgeIndex = HalfedgeCount;
            }

            /********** For Vertices **********/

            if ((VertexCount != _newVertexIndex))
            {
                int newVertexIndex = 0;
                List<int> isolatedVertices = new List<int>();
                Dictionary<int, Vertex<TPosition>> newVertices = new Dictionary<int, Vertex<TPosition>>();

                foreach (int key in _vertices.Keys)
                {
                    if (cullIsolated && !_vertices[key].IsConnected())
                    {
                        isolatedVertices.Add(key);      // Marks for removal.
                        continue;                       // Avoids storing the vertex in the new dictionnary of vertices.
                    }

                    newVertices.Add(newVertexIndex, _vertices[key]);
                    _vertices[key].Index = newVertexIndex;
                    newVertexIndex++;
                }

                // Remove isolated vertices
                foreach (int key in isolatedVertices) { RemoveVertex(key); }

                _vertices = newVertices;
                _newVertexIndex = VertexCount;
            }
        }

        /// <inheritdoc/>
        public override object Clone()
        {
            Mesh<TPosition> cloneHeMesh = new Mesh<TPosition>();

            // Add Vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);

                Vertex<TPosition> cloneVertex = new Vertex<TPosition>(vertexIndex, vertex.Position);

                cloneHeMesh._vertices.Add(vertexIndex, cloneVertex);
            }
            vertexIndexEnumerator.Dispose();

            // Add Halfedges
            var halfedgeIndexEnumerator = _halfedges.Keys.GetEnumerator();

            while (halfedgeIndexEnumerator.MoveNext())
            {
                int heIndex = halfedgeIndexEnumerator.Current;

                Halfedge<TPosition> halfedge = GetHalfedge(heIndex);

                Vertex<TPosition> cloneStartVertex = cloneHeMesh.GetVertex(halfedge.StartVertex.Index);
                Vertex<TPosition> cloneEndVertex = cloneHeMesh.GetVertex(halfedge.EndVertex.Index);

                // Add the halfedge pair to the mesh.
                Halfedge<TPosition> cloneHe = new Halfedge<TPosition>(heIndex, cloneStartVertex, cloneEndVertex);
                Halfedge<TPosition> clonePairHe = new Halfedge<TPosition>(heIndex + 1, cloneEndVertex, cloneStartVertex);

                cloneHe.PairHalfedge = clonePairHe;
                clonePairHe.PairHalfedge = cloneHe;

                cloneHeMesh._halfedges.Add(heIndex, cloneHe);
                cloneHeMesh._halfedges.Add(heIndex + 1, clonePairHe);

                // Move because the pair halfedge is already added.
                halfedgeIndexEnumerator.MoveNext();
            }
            halfedgeIndexEnumerator.Dispose();

            // Add Faces & Manage faces connectivity
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                Face<TPosition> face = GetFace(faceIndex);
                Halfedge<TPosition> faceHe = face.FirstHalfedge;

                Halfedge<TPosition> cloneFaceHe = cloneHeMesh.GetHalfedge(faceHe.Index);

                Face<TPosition> cloneFace = new Face<TPosition>(faceIndex, cloneFaceHe);

                cloneHeMesh._faces.Add(faceIndex, cloneFace);
            }
            faceIndexEnumerator.Dispose();

            // Manage vertices connectivity (OutgoingHalfedge)
            vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);

                Vertex<TPosition> cloneVertex = cloneHeMesh.GetVertex(vertexIndex);

               cloneVertex.OutgoingHalfedge = cloneHeMesh.GetHalfedge(vertex.OutgoingHalfedge.Index);
            }
            vertexIndexEnumerator.Dispose();

            // Manage halfedges connectivity (PreviousHalfedge, NextHalfedge, AdjacentFace)
            halfedgeIndexEnumerator = _halfedges.Keys.GetEnumerator();

            while (halfedgeIndexEnumerator.MoveNext())
            {
                int heIndex = halfedgeIndexEnumerator.Current;

                Halfedge<TPosition> halfedge = GetHalfedge(heIndex);

                Halfedge<TPosition> cloneHalfedge = cloneHeMesh.GetHalfedge(heIndex);

                cloneHalfedge.PrevHalfedge = cloneHeMesh.GetHalfedge(halfedge.PrevHalfedge.Index);
                cloneHalfedge.NextHalfedge = cloneHeMesh.GetHalfedge(halfedge.NextHalfedge.Index);

                cloneHalfedge.AdjacentFace = cloneHeMesh.GetFace(halfedge.AdjacentFace.Index);
            }
            halfedgeIndexEnumerator.Dispose();

            
            cloneHeMesh._newVertexIndex = _newVertexIndex;
            cloneHeMesh._newHalfedgeIndex = _newHalfedgeIndex;
            cloneHeMesh._newFaceIndex = _newFaceIndex;


            return cloneHeMesh;
        }


        /******************** On Vertices ********************/

        /// <inheritdoc/>
        public override Vertex<TPosition> AddVertex(TPosition position)
        {
            // Creates new instance of vertex.
            Vertex<TPosition> vertex = new Vertex<TPosition>(_newVertexIndex, position);

            _vertices.Add(_newVertexIndex, vertex);

            _newVertexIndex += 1;

            return vertex;
        }


        /// <inheritdoc/>
        public override Vertex<TPosition> GetVertex(int index)
        {
            return _vertices[index];
        }

        /// <inheritdoc/>
        public override Vertex<TPosition> TryGetVertex(int index)
        {
            _vertices.TryGetValue(index, out Vertex<TPosition> vertex);

            return vertex;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Vertex<TPosition>> GetVertices()
        {
            Vertex<TPosition>[] result = new Vertex<TPosition>[VertexCount];

            int i_Vertex = 0;
            foreach (Vertex<TPosition> vertex in _vertices.Values)
            {
                result[i_Vertex] = vertex;
                i_Vertex++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveVertex(Vertex<TPosition> vertex)
        {
            IReadOnlyList<Face<TPosition>> adjacentFaces = vertex.AdjacentFaces();

            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                RemoveFace(adjacentFaces[i_AF]);
            }
        }


        /// <inheritdoc/>
        protected override void EraseVertex(Vertex<TPosition> vertex)
        {
            // If the vertex is still connected.
            if (!(vertex.OutgoingHalfedge is null))
            {
                throw new InvalidOperationException("The vertex cannot be erased if it is still connected.");
            }

            // Remove the vertex from the mesh.
            _vertices.Remove(vertex.Index);

            // Unset the current vertex.
            vertex.Unset();
        }


        /******************** On Edges ********************/

        /// <inheritdoc/>
        internal override Edge<TPosition> AddEdge(Vertex<TPosition> startVertex, Vertex<TPosition> endVertex)
        {
            Halfedge<TPosition> halfedge = AddPair(startVertex, endVertex);

            return halfedge.GetEdge();
        }


        /// <inheritdoc/>
        public override Edge<TPosition> GetEdge(int index)
        {
            Halfedge<TPosition> halfedge = _halfedges[2 * index];

            return halfedge.GetEdge();
        }

        /// <inheritdoc/>
        public override Edge<TPosition> TryGetEdge(int index)
        {
            _halfedges.TryGetValue(2 * index, out Halfedge<TPosition> halfedge);

            return halfedge?.GetEdge(); ;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Edge<TPosition>> GetEdges()
        {
            Edge<TPosition>[] result = new Edge<TPosition>[EdgeCount];

            int i_Edge = 0;
            var enumerator = _halfedges.Values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Halfedge<TPosition> halfedge = enumerator.Current;

                result[i_Edge] = halfedge.GetEdge();

                enumerator.MoveNext();
                i_Edge++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override Edge<TPosition> EdgeBetween(Vertex<TPosition> vertexA, Vertex<TPosition> vertexB)
        {
            IReadOnlyList<Halfedge<TPosition>> outgoings = vertexA.OutgoingHalfedges();
            for (int i_OHe = 0; i_OHe < outgoings.Count; i_OHe++)
            {
                Halfedge<TPosition> outgoing = outgoings[i_OHe];

                if (outgoing.EndVertex.Equals(vertexB)) { return outgoing.GetEdge(); }
            }

            return null;
        }


        /// <inheritdoc/>
        public override void RemoveEdge(Edge<TPosition> edge)
        {
            RemoveHalfedge(_halfedges[2 * edge.Index]);
        }


        /// <inheritdoc/>
        public override void EraseEdge(Edge<TPosition> edge)
        {
            EraseHalfedge(_halfedges[2 * edge.Index]);
        }


        /******************** On Faces ********************/

        /// <inheritdoc/>
        public override Face<TPosition> AddFace(List<Vertex<TPosition>> vertices)
        {
            #region Initializations

            // Create the instance of the face being created
            Face<TPosition> newFace = new Face<TPosition>(_newFaceIndex);

            int faceHeCount = vertices.Count;

            #endregion

            #region Verifications

            // On Vertices
            foreach (Vertex<TPosition> vertex in vertices)
            {
                // Check that all vertex indices exist in this mesh
                if (!(this.GetVertex(vertex.Index).Equals(vertex)))
                {
                    throw new NullReferenceException("One of the specified vertex was not found in the mesh.");
                }
                // Check that all vertices are on a boundary
                if (!vertex.IsBoundary())
                {
                    throw new ArgumentException("One of the specified vertex was not on the boundary of the mesh.");
                }
            }

            #endregion

            #region Existence of Halfedges

            /* For each consecutive vertices, look for an existing halfedge :
             * If it exists, verify that it doesn't already have a face.
             * If it doesn't exist, mark for creation of a new HeEdge pair.
             */

            Halfedge<TPosition>[] faceHalfedges = new Halfedge<TPosition>[faceHeCount];
            bool[] isHeNew = new bool[faceHeCount];

            for (int i_He = 0; i_He < faceHeCount; i_He++)
            {
                Vertex<TPosition> startVertex = vertices[i_He];
                Vertex<TPosition> endVertex = vertices[(i_He + 1) % faceHeCount];

                // Look for the HeEdge
                Halfedge<TPosition> faceHalfedge = HalfedgeBetween(startVertex, endVertex);

                if (faceHalfedge == null) { isHeNew[i_He] = true; }           // If the halfedge doesn't exist.
                else if (!faceHalfedge.IsBoundary())                          // If the halfedge exists, but already has an adjacent face (non-manifold).
                {
                    throw new InvalidOperationException("One of the requested halfedge already belongs to a face.");
                }
                else { faceHalfedges[i_He] = faceHalfedge; }                  // If the halfedge exists, and is on the boundary.


                // To prevent non-manifold vertices, uncomment the line below :
                // if (isHeNew[i_Halfedge] && isHeNew[(i_Halfedge + faceHeCount - 1) % faceHeCount] && startVertex.IsConnected()) { return null; }
            }

            #endregion

            #region Creation of Missing Edges

            /* Create the missing halfedge pair and manage connection of the halfedge to the new face
             * (This could be done in the previous loop but it avoids having to tidy up
             * any recently added halfedges should a non-manifold edge be found.)
             */

            for (int i_He = 0; i_He < faceHeCount; i_He++)
            {
                if (isHeNew[i_He])                                        // Create a new halfedge pair
                {
                    Vertex<TPosition> startVertex = vertices[i_He];
                    Vertex<TPosition> endVertex = vertices[(i_He + 1) % faceHeCount];

                    faceHalfedges[i_He] = AddPair(startVertex, endVertex);
                }

                faceHalfedges[i_He].AdjacentFace = newFace;        // Connect the current halfedge with the face
            }

            #endregion

            #region Connecting Edges

            /***** Manages the connection of halfedges around the vertices of the new face. *****/

            for (int i_Vertex = 0; i_Vertex < vertices.Count; i_Vertex++)
            {
                Vertex<TPosition> vertex = vertices[i_Vertex];

                int i_IncFaceHe = (i_Vertex + faceHeCount - 1) % faceHeCount;   // Index of the incoming face halfedge at the vertex.
                int i_OutFaceHe = i_Vertex;                                       // Index of the outgoing face halfedge at the vertex.

                /******************** Evaluate Situation ********************/

                int situation = 0;
                if (isHeNew[i_IncFaceHe]) { situation += 1; }       // If the incoming face halfedge is new .
                if (isHeNew[i_OutFaceHe]) { situation += 2; }       // If the outgoing face halfedge is new.

                /* Check for non-manifold vertex case (i.e. both current halfedges are new but the vertex between them is already part of another face.
                 * This vertex will have at least two outgoing boundary halfedge (Not strictly allowed, but it could happen if faces are added in an "bad" order).
                 * 
                 * TODO: If a mesh has non-manifold vertices perhaps it should be considered invalid. 
                 * Any operations performed on such a mesh cannot be relied upon to perform correctly as the adjacency information may not be correct.
                 */
                if (situation == 3 && vertex.IsConnected()) { situation++; }

                /******************** Manage Connection ********************/

                if (situation > 0) // At least one of the above considered face halfedge pair is new
                {
                    // Bondary edges at the vertex 
                    Halfedge<TPosition> outBoundary = null;      // Incoming boundary halfedge at the vertex
                    Halfedge<TPosition> incBoundary = null;      // Outgoing boundary halfedge at the vertex

                    switch (situation)
                    {
                        // iterate through halfedges clockwise around vertex v2 until boundary

                        case 1: // If the incoming face halfedge is new, but the outgoing face halfedge is old.
                            incBoundary = faceHalfedges[i_OutFaceHe].PrevHalfedge;
                            outBoundary = faceHalfedges[i_IncFaceHe].PairHalfedge;
                            break;
                        case 2: // If the incoming face halfedge is old, but the outgoing face halfedge is new.
                            incBoundary = faceHalfedges[i_OutFaceHe].PairHalfedge;
                            outBoundary = faceHalfedges[i_IncFaceHe].NextHalfedge;
                            break;
                        case 3: // If both the incoming and the outgoing face halfedge are new.
                            incBoundary = faceHalfedges[i_OutFaceHe].PairHalfedge;
                            outBoundary = faceHalfedges[i_IncFaceHe].PairHalfedge;
                            break;
                        case 4: // If both the incoming and the outgoing face halfedge are new (and the vertex is non-manifold).
                            // Two boundary have to be managed here:
                            // For the first boundary
                            incBoundary = vertex.OutgoingHalfedge.PrevHalfedge;
                            outBoundary = faceHalfedges[i_IncFaceHe].PairHalfedge;
                            incBoundary.NextHalfedge = outBoundary;
                            outBoundary.PrevHalfedge = incBoundary;
                            // For the second boundary
                            incBoundary = faceHalfedges[i_OutFaceHe].PairHalfedge;
                            outBoundary = vertex.OutgoingHalfedge;
                            break;
                    }

                    // Connect vertex's boundary halfedge
                    incBoundary.NextHalfedge = outBoundary;
                    outBoundary.PrevHalfedge = incBoundary;

                    // Connect face halfedges
                    faceHalfedges[i_IncFaceHe].NextHalfedge = faceHalfedges[i_OutFaceHe];
                    faceHalfedges[i_OutFaceHe].PrevHalfedge = faceHalfedges[i_IncFaceHe];

                    // Ensures that if a vertex lies on a boundary, then its outgoingHalfedge is a boundary halfedge.
                    vertex.OutgoingHalfedge = outBoundary;
                }

                else // None of the above considered face halfedge pair are new (includes trickery for non-manifold vertex)
                {
                    IReadOnlyList<Halfedge<TPosition>> outgoingHalfedges = vertex.OutgoingHalfedges();

                    // Gets the number of boundary outgoing halfedge at this vertex.
                    int outgoingBoundaryHeCount = outgoingHalfedges.Count;
                    
                    if (outgoingBoundaryHeCount == 0) // In the case where the vertex is manifold.
                    {
                        // Do nothing !
                    }
                    else if (outgoingBoundaryHeCount == 1) // In the case where the vertex is non-manifold but only with only due to one face ("simple case")
                    {
                        // Verifications
                        if (!faceHalfedges[i_IncFaceHe].NextHalfedge.Equals(faceHalfedges[i_OutFaceHe]))
                        {
                            throw new InvalidOperationException("Two of the requested halfedges for the creation of a face are not consecutive.");
                        }
                        // Assign the only boundary outgoing edge as the outgoing edge of vertex.
                        foreach (Halfedge<TPosition> outgoingHe in outgoingHalfedges)
                        {
                            if (outgoingHe.IsBoundary()) { vertex.OutgoingHalfedge = outgoingHe; break; }
                        }
                    }
                    else // In the case where the vertex is non-manifold ("difficult case")
                    {
                        if (faceHalfedges[i_IncFaceHe].NextHalfedge.Equals(faceHalfedges[i_OutFaceHe])) // If "luckily" the arrangement of the faces around the vertex is fine.
                        {
                            foreach (Halfedge<TPosition> outgoingHe in outgoingHalfedges)
                            {
                                if (outgoingHe.IsBoundary()) { vertex.OutgoingHalfedge = outgoingHe; break; }
                            }
                        }
                        else // If the arrangement of the faces around the vertex is not compatible with the face to add.
                        {
                            Halfedge<TPosition> next_IncFaceHe = faceHalfedges[i_IncFaceHe].NextHalfedge;
                            Halfedge<TPosition> prev_OutFaceHe = faceHalfedges[i_OutFaceHe].PrevHalfedge;

                            // Assign new outgoing edge to the vertex
                            foreach (Halfedge<TPosition> outgoingHe in outgoingHalfedges)
                            {
                                if (outgoingHe.Equals(next_IncFaceHe)) { continue; } // Beware : The outgoing edge can not be any boundary edge.
                                if (outgoingHe.IsBoundary()) { vertex.OutgoingHalfedge = outgoingHe; break; }
                            }

                            // Connect face edges to each other
                            faceHalfedges[i_IncFaceHe].NextHalfedge = faceHalfedges[i_OutFaceHe];
                            faceHalfedges[i_OutFaceHe].PrevHalfedge = faceHalfedges[i_IncFaceHe];

                            // Connect the surroundings edges
                            prev_OutFaceHe.NextHalfedge = vertex.OutgoingHalfedge;
                            next_IncFaceHe.PrevHalfedge = vertex.OutgoingHalfedge.PrevHalfedge;

                            vertex.OutgoingHalfedge.PrevHalfedge.NextHalfedge = next_IncFaceHe;
                            vertex.OutgoingHalfedge.PrevHalfedge = prev_OutFaceHe;
                        }
                    }
                }
            }

            #endregion

            // Finally set the first halfedge and returns the face.
            newFace.FirstHalfedge = faceHalfedges[0];

            this._faces.Add(_newFaceIndex, newFace);

            _newFaceIndex += 1;

            return newFace;
        }


        /// <inheritdoc/>
        public override Face<TPosition> GetFace(int index)
        {
            return _faces[index];
        }

        /// <inheritdoc/>
        public override Face<TPosition> TryGetFace(int index)
        {
            _faces.TryGetValue(index, out Face<TPosition> face);

            return face;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Face<TPosition>> GetFaces()
        {
            Face<TPosition>[] result = new Face<TPosition>[FaceCount];

            int i_Face = 0;
            foreach (Face<TPosition> face in _faces.Values)
            {
                result[i_Face] = face;
                i_Face++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveFace(Face<TPosition> face)
        {
            // Save the face vertices for future treatment
            IReadOnlyList<Vertex<TPosition>> faceVertices = face.FaceVertices();

            // Manage connection with edges
            foreach (Halfedge<TPosition> halfedge in face.FaceHalfedges())
            {
                if (halfedge.PairHalfedge.IsBoundary())
                {
                    halfedge.AdjacentFace = null;
                    halfedge.PairHalfedge.AdjacentFace = null;
                    EraseHalfedge(halfedge);
                }
                else
                {
                    halfedge.AdjacentFace = null;
                    if (!halfedge.StartVertex.OutgoingHalfedge.IsBoundary())
                    {
                        halfedge.StartVertex.OutgoingHalfedge = halfedge;
                    }
                }
            }

            // Manage isolated vertices
            foreach (Vertex<TPosition> vertex in faceVertices)
            {
                if (!vertex.IsConnected()) { EraseVertex(vertex); }
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }


        /// <inheritdoc/>
        public override void EraseFace(Face<TPosition> face)
        {
            // Manage connection with edges
            foreach (Halfedge<TPosition> halfedge in face.FaceHalfedges())
            {
                halfedge.AdjacentFace = null;
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }


        /******************** Methods - On Specific Meshes ********************/

        /// <inheritdoc cref="Mesh{TPosition, TVertex, TEdge, TFace}.GetTriMesh"/>
        protected override ITriMesh<TPosition> GetTriMesh() => Tri;

        /// <inheritdoc cref="Mesh{TPosition, TVertex, TEdge, TFace}.GetQuadMesh"/>
        protected override IQuadMesh<TPosition> GetQuadMesh() => Quad;

        /// <inheritdoc cref="Mesh{TPosition, TVertex, TEdge, TFace}.GetHexaMesh"/>
        protected override IHexaMesh<TPosition> GetHexaMesh() => Hexa;

        #endregion
    }
}