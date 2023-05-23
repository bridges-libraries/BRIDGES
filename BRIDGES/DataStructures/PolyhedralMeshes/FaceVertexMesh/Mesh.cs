using System;
using System.Collections.Generic;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Sets = BRIDGES.Algebra.Sets;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class for a polyhedral face-vertex mesh data structure.
    /// </summary>
    /// <typeparam name="TPosition"> Type for the position of the vertex. </typeparam>
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
        internal Dictionary<int, Vertex<TPosition>> _vertices;

        /// <summary>
        /// Index for a newly created vertex.
        /// </summary>
        /// <remarks> This may not match with <see cref="VertexCount"/> if vertices are removed from the mesh. </remarks>
        protected int _newVertexIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Edge{TPosition}"/> of the current <see cref="Mesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="Edge{TPosition}"/>; Value : Corresponding <see cref="Edge{TPosition}"/>. </remarks>
        internal Dictionary<int, Edge<TPosition>> _edges;

        /// <summary>
        /// Index for a newly created edge.
        /// </summary>
        /// <remarks> This may not match with <see cref="EdgeCount"/> if edges are removed from the mesh. </remarks>
        protected int _newEdgeIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Face{TPosition}"/> of the current <see cref="Mesh{TPosition}"/>.
        /// </summary>
        /// <remarks> Key : Index of the <see cref="Face{TPosition}"/>; Value : Corresponding <see cref="Face{TPosition}"/>. </remarks>
        internal Dictionary<int, Face<TPosition>> _faces;

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
        public override int EdgeCount 
        { 
            get { return _edges.Count; }
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
            _edges = new Dictionary<int, Edge<TPosition>>();
            _faces = new Dictionary<int, Face<TPosition>>();

            // Initialise fields
            _newVertexIndex = 0;
            _newEdgeIndex = 0;
            _newFaceIndex = 0;
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="Mesh{TPosition}"/> class from its fields.
        /// </summary>
        internal Mesh(Dictionary<int, Vertex<TPosition>> vertices, Dictionary<int, Edge<TPosition>> edges, Dictionary<int, Face<TPosition>> faces,
            int newVertexIndex, int newEdgeIndex, int newFaceIndex)
        {
            // Instanciate fields
            _vertices = vertices;
            _edges = edges;
            _faces = faces;

            // Initialise fields
            _newVertexIndex = newVertexIndex;
            _newEdgeIndex = newEdgeIndex;
            _newFaceIndex = newFaceIndex;
        }

        #endregion

        #region Methods

        /******************** On Meshes ********************/

        /// <summary>
        /// Creates a halfedge mesh from the current face-vertex mesh.
        /// </summary>
        /// <returns> Halfedge mesh which represents the topology and geometry of the current face-vertex mesh. </returns>
        public HalfedgeMesh.Mesh<TPosition> ToHalfedgeMesh()
        {
            Dictionary<int, HalfedgeMesh.Vertex<TPosition>> heVertices = new Dictionary<int, HalfedgeMesh.Vertex<TPosition>>();
            Dictionary<int, HalfedgeMesh.Halfedge<TPosition>> heHalfedges = new Dictionary<int, HalfedgeMesh.Halfedge<TPosition>>();
            Dictionary<int, HalfedgeMesh.Face<TPosition>> heFaces = new Dictionary<int, HalfedgeMesh.Face<TPosition>>();


            // Add Vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);

                HalfedgeMesh.Vertex<TPosition> heVertex = new HalfedgeMesh.Vertex<TPosition>(vertexIndex, vertex.Position);

                heVertices.Add(vertexIndex, heVertex);
            }
            vertexIndexEnumerator.Dispose();


            // Add Halfedges
            var edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                Edge<TPosition> fvEdge = GetEdge(edgeIndex);

                HalfedgeMesh.Vertex<TPosition> heStartVertex = heVertices[fvEdge.StartVertex.Index];
                HalfedgeMesh.Vertex<TPosition> heEndVertex = heVertices[fvEdge.EndVertex.Index];

                // Add the halfedge pair to the mesh.
                HalfedgeMesh.Halfedge<TPosition> halfedge = new HalfedgeMesh.Halfedge<TPosition>(2 * edgeIndex, heStartVertex, heEndVertex);
                HalfedgeMesh.Halfedge<TPosition> pairHalfedge = new HalfedgeMesh.Halfedge<TPosition>((2 * edgeIndex) + 1, heEndVertex, heStartVertex);

                halfedge.PairHalfedge = pairHalfedge;
                pairHalfedge.PairHalfedge = halfedge;

                heHalfedges.Add(2 * edgeIndex, halfedge);
                heHalfedges.Add((2 * edgeIndex) + 1, pairHalfedge);
            }
            edgeIndexEnumerator.Dispose();


            // Add Faces & Manage faces connectivity
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();
            bool[] isHeAssigned = new bool[heHalfedges.Count];

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                Face<TPosition> face = GetFace(faceIndex);
                IReadOnlyList<Edge<TPosition>> faceEdges = face.FaceEdges();
                IReadOnlyList<Vertex<TPosition>> faceVertices = face.FaceVertices();

                // Get the face halfedges
                HalfedgeMesh.Halfedge<TPosition>[] heFaceHalfedges = new HalfedgeMesh.Halfedge<TPosition>[faceEdges.Count];
                for (int i = 0; i < faceEdges.Count; i++)
                {
                    HalfedgeMesh.Vertex<TPosition> heVertex = heVertices[faceVertices[i].Index];
                    HalfedgeMesh.Halfedge<TPosition> heHalfedge = heHalfedges[2 * faceEdges[i].Index];

                    if (heVertex.Equals(heHalfedge.StartVertex)) { heFaceHalfedges[i] = heHalfedge; }
                    else { heFaceHalfedges[i] = heHalfedge.PairHalfedge; }
                }

                // Create the face
                HalfedgeMesh.Face<TPosition> heFace = new HalfedgeMesh.Face<TPosition>(faceIndex, heFaceHalfedges[0]);

                heFaces.Add(faceIndex, heFace);

                // Manage face halfedges connectivity
                for (int i_He = 0; i_He < heFaceHalfedges.Length; i_He++)
                {
                    int i_PrevHe = (i_He + heFaceHalfedges.Length - 1) % (heFaceHalfedges.Length);
                    int i_NextHe = (i_He + 1) % (heFaceHalfedges.Length);

                    heFaceHalfedges[i_He].PrevHalfedge = heFaceHalfedges[i_PrevHe];
                    heFaceHalfedges[i_He].NextHalfedge = heFaceHalfedges[i_NextHe];

                    heFaceHalfedges[i_He].AdjacentFace = heFace;

                    isHeAssigned[heFaceHalfedges[i_He].Index] = true;
                }
            }
            faceIndexEnumerator.Dispose();


            // Manage boundary halfedges connectivity
            for (int i_He = 0; i_He < heHalfedges.Count; i_He++)
            {
                if (isHeAssigned[i_He]) { continue; }

                HalfedgeMesh.Halfedge<TPosition> heHalfedge = heHalfedges[i_He];
                
                // if the halfedge is non-manifold
                if (heHalfedge.PairHalfedge.IsBoundary()) 
                {
                    throw new NotImplementedException("A halfedge and its pair do not have an adjacent face. Hence the connectivity could not be managed.");
                }

                if (heHalfedge.PrevHalfedge is null)
                {
                    HalfedgeMesh.Halfedge<TPosition> hePrevHalfedge = heHalfedge.PairHalfedge.NextHalfedge.PairHalfedge;

                    while (!hePrevHalfedge.IsBoundary()) { hePrevHalfedge = hePrevHalfedge.NextHalfedge.PairHalfedge; }

                    heHalfedge.PrevHalfedge = hePrevHalfedge;
                    hePrevHalfedge.NextHalfedge = heHalfedge;
                }

                if (heHalfedge.NextHalfedge is null)
                {
                    HalfedgeMesh.Halfedge<TPosition> heNextHalfedge = heHalfedge.PairHalfedge.PrevHalfedge.PairHalfedge;

                    while (!heNextHalfedge.IsBoundary()) { heNextHalfedge = heNextHalfedge.PrevHalfedge.PairHalfedge; }

                    heHalfedge.NextHalfedge = heNextHalfedge;
                    heNextHalfedge.PrevHalfedge = heHalfedge;
                }

                isHeAssigned[i_He] = true;
            }

            return new HalfedgeMesh.Mesh<TPosition>(heVertices, heHalfedges, heFaces, _newVertexIndex, 2 * _newEdgeIndex, _newFaceIndex);
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"FvMesh with {VertexCount} vertices, {EdgeCount} edges, {FaceCount} faces.";
        }

        #endregion

        #region Override : Mesh<T,FvVertex<T>,FvEdge<T>,FvFace<T>>

        /******************** Methods - For this Mesh ********************/

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
                    if (cullIsolated && _faces[key].AdjacentFaces().Count == 0)
                    {
                        isolatedFaces.Add(key);     // Marks for removal.
                        continue;                   // Avoids storing the face in the new dictionnary of faces.
                    }

                    newFaces.Add(newFaceIndex, _faces[key]);
                    _faces[key].Index = newFaceIndex;
                    newFaceIndex += 1;
                }

                // Remove isolated faces
                foreach (int key in isolatedFaces) { RemoveFace(key); }

                // Reconfigure mesh faces.
                _faces = newFaces;
                _newFaceIndex = FaceCount;
            }

            /********** For Edges **********/

            if (EdgeCount != _newEdgeIndex)
            {
                int newEdgeIndex = 0;
                List<int> isolatedEdges = new List<int>();
                Dictionary<int, Edge<TPosition>> newEdges = new Dictionary<int, Edge<TPosition>>();

                foreach (int key in _edges.Keys)
                {
                    if (cullIsolated && _edges[key].AdjacentFaces().Count == 0)
                    {
                        isolatedEdges.Add(key);     // Marks for removal.
                        continue;                   // Avoids storing the edge in the new dictionnary of edges.
                    }

                    newEdges.Add(newEdgeIndex, _edges[key]);
                    _edges[key].Index = newEdgeIndex;
                    newEdgeIndex++;
                }

                // Remove isolated edges
                foreach (int key in isolatedEdges) { RemoveEdge(key); }

                // Reconfigure mesh edges.
                _edges = newEdges;
                _newEdgeIndex = VertexCount;
            }

            /********** For Vertices **********/

            if (VertexCount != _newVertexIndex)
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
            Mesh<TPosition> cloneFvMesh = new Mesh<TPosition>();

            // Add Vertices
            var vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);

                Vertex<TPosition> cloneVertex = new Vertex<TPosition>(vertexIndex, vertex.Position);
                cloneFvMesh._vertices.Add(vertexIndex, cloneVertex);
            }
            vertexIndexEnumerator.Dispose();


            // Add Edges
            var edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                Edge<TPosition> edge = GetEdge(edgeIndex);

                Vertex<TPosition> cloneStartVertex = cloneFvMesh.GetVertex(edge.StartVertex.Index);
                Vertex<TPosition> cloneEndVertex = cloneFvMesh.GetVertex(edge.EndVertex.Index);

                Edge<TPosition> cloneEdge = new Edge<TPosition>(edgeIndex, cloneStartVertex, cloneEndVertex);
                cloneFvMesh._edges.Add(edgeIndex, cloneEdge);
            }
            edgeIndexEnumerator.Dispose();


            // Add Faces
            var faceIndexEnumerator = _faces.Keys.GetEnumerator();

            while (faceIndexEnumerator.MoveNext())
            {
                int faceIndex = faceIndexEnumerator.Current;

                Face<TPosition> face = GetFace(faceIndex);

                IReadOnlyList<Edge<TPosition>> faceEdges = face.FaceEdges();
                List<Edge<TPosition>> cloneFaceEdges = new List<Edge<TPosition>>(faceEdges.Count);
                for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
                {
                    cloneFaceEdges.Add(cloneFvMesh.GetEdge(faceEdges[i_FE].Index));
                }

                IReadOnlyList<Vertex<TPosition>> faceVertices = face.FaceVertices();
                List<Vertex<TPosition>> cloneFaceVertices = new List<Vertex<TPosition>>(faceVertices.Count);
                for (int i_FV = 0; i_FV < faceVertices.Count; i_FV++)
                {
                    cloneFaceVertices.Add(cloneFvMesh.GetVertex(faceVertices[i_FV].Index));
                }

                Face<TPosition> cloneFace = new Face<TPosition>(faceIndex, cloneFaceVertices, cloneFaceEdges);
                cloneFvMesh._faces.Add(faceIndex, cloneFace);
            }
            faceIndexEnumerator.Dispose();


            // Manage the connectivity of the vertices (_connectedEdges).
            vertexIndexEnumerator = _vertices.Keys.GetEnumerator();

            while (vertexIndexEnumerator.MoveNext())
            {
                int vertexIndex = vertexIndexEnumerator.Current;

                Vertex<TPosition> vertex = GetVertex(vertexIndex);
                IReadOnlyList<Edge<TPosition>> connectedEdges = vertex._connectedEdges;

                Vertex<TPosition> cloneVertex = cloneFvMesh.GetVertex(vertexIndex);

                List<Edge<TPosition>> cloneConnectedEdges = new List<Edge<TPosition>>(connectedEdges.Count);
                for (int i_FE = 0; i_FE < connectedEdges.Count; i_FE++)
                {
                    cloneConnectedEdges.Add(cloneFvMesh.GetEdge(connectedEdges[i_FE].Index));
                }

                cloneVertex._connectedEdges = cloneConnectedEdges;
            }
            vertexIndexEnumerator.Dispose();


            // Manage the connectivity of edges (_adjacentFaces)
            edgeIndexEnumerator = _edges.Keys.GetEnumerator();

            while (edgeIndexEnumerator.MoveNext())
            {
                int edgeIndex = edgeIndexEnumerator.Current;

                Edge<TPosition> edge = GetEdge(edgeIndex);
                IReadOnlyList<Face<TPosition>> adjacentFaces = edge._adjacentFaces;

                Edge<TPosition> cloneEdge = cloneFvMesh.GetEdge(edgeIndex);

                List<Face<TPosition>> cloneAdjacentFaces = new List<Face<TPosition>>(adjacentFaces.Count);
                for (int i_FE = 0; i_FE < adjacentFaces.Count; i_FE++)
                {
                    cloneAdjacentFaces.Add(cloneFvMesh.GetFace(adjacentFaces[i_FE].Index));
                }

                cloneEdge._adjacentFaces= cloneAdjacentFaces;
            }
            edgeIndexEnumerator.Dispose();

            cloneFvMesh._newVertexIndex = _newVertexIndex;
            cloneFvMesh._newEdgeIndex = _newEdgeIndex;
            cloneFvMesh._newFaceIndex = _newFaceIndex;

            return cloneFvMesh;
        }

        /******************** Methods - On Vertices ********************/

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
            // If the current vertex is still connected.
            if (vertex._connectedEdges.Count != 0)
            {
                throw new InvalidOperationException("The vertex cannot be erased if it is still connected.");
            }

            // Remove the vertex from the mesh.
            _vertices.Remove(vertex.Index);

            // Unset the current vertex.
            vertex.Unset();
        }


        /******************** Methods - On Edges ********************/

        /// <inheritdoc/>
        internal override Edge<TPosition> AddEdge(Vertex<TPosition> startVertex, Vertex<TPosition> endVertex)
        {
            // Verification : Avoid looping halfedges
            if (startVertex.Equals(endVertex)) { return null; }

            // Verifications : Avoid duplicate halfedges
            Edge<TPosition> exitingEdge = EdgeBetween(startVertex, endVertex);
            if (!(exitingEdge is null)) { return null; }


            Edge<TPosition> edge = new Edge<TPosition>(_newEdgeIndex, startVertex, endVertex);

            this._edges.Add(_newEdgeIndex, edge);

            _newEdgeIndex += 1;

            return edge;
        }


        /// <inheritdoc/>
        public override Edge<TPosition> GetEdge(int index)
        {
            return _edges[index];
        }

        /// <inheritdoc/>
        public override Edge<TPosition> TryGetEdge(int index)
        {
            _edges.TryGetValue(index, out Edge<TPosition> edge);

            return edge;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Edge<TPosition>> GetEdges()
        {
            Edge<TPosition>[] result = new Edge<TPosition>[EdgeCount];

            int i_Edge = 0;
            foreach (Edge<TPosition> edge in _edges.Values)
            {
                result[i_Edge] = edge;
                i_Edge++;
            }

            return result;
        }


        /// <inheritdoc/>
        public override void RemoveEdge(Edge<TPosition> edge)
        {
            IReadOnlyList<Face<TPosition>> adjacentFaces = edge.AdjacentFaces();

            if (edge.IsBoundary() && adjacentFaces.Count == 0)
            {
                Vertex<TPosition> startVertex = edge.StartVertex;
                Vertex<TPosition> endVertex = edge.StartVertex;

                EraseEdge(edge);

                // Manage start and end vertex
                if (!startVertex.IsConnected()) { EraseVertex(startVertex); }
                if (!endVertex.IsConnected()) { EraseVertex(endVertex); }
            }
            else
            {
                // Manage connection with adjacent face
                for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
                {
                    RemoveFace(adjacentFaces[i_AF]);
                }
            }
        }


        /// <inheritdoc/>
        public override void EraseEdge(Edge<TPosition> edge)
        {

            // Manage connection with adjacent face
            IReadOnlyList<Face<TPosition>> adjacentFaces = edge.AdjacentFaces();
            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                EraseFace(adjacentFaces[i_AF]);
            }

            // Manage connection with start and end vertices
            edge.StartVertex._connectedEdges.Remove(edge);
            edge.EndVertex._connectedEdges.Remove(edge);

            // Remove the pair of edges from the mesh
            _edges.Remove(edge.Index);

            // Unset the pair of edges
            edge.Unset();
        }


        /******************** Methods - On Faces ********************/

        /// <inheritdoc/>
        public override Face<TPosition> AddFace(List<Vertex<TPosition>> vertices)
        {
            // Verifications
            if (vertices.Count < 3) { throw new ArgumentOutOfRangeException("A face must have at least three vertices."); }
            foreach (Vertex<TPosition> vertex in vertices)
            {
                if (!vertex.Equals(GetVertex(vertex.Index)))
                {
                    throw new ArgumentException("One of the input vertex does not belong to this mesh.");
                }
            }

            // Create the list of edges around the face
            List<Edge<TPosition>> edges = new List<Edge<TPosition>>();
            for (int i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1) % (vertices.Count);
                Edge<TPosition> edge = EdgeBetween(vertices[i], vertices[j]);

                if (edge is null)
                {
                    Vertex<TPosition> start = vertices[i];
                    Vertex<TPosition> end = vertices[j];

                    edge = AddEdge(start, end);
                }
                edges.Add(edge);
            }


            // Should check if the face already exists and other things (vertex belonging to the right mesh, etc...)
            Face<TPosition> face = new Face<TPosition>(_newFaceIndex, vertices, edges);

            this._faces.Add(_newFaceIndex, face);

            _newFaceIndex += 1;

            return face;
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
            // Manage connection with edges
            IReadOnlyList<Edge<TPosition>> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                Edge<TPosition> edge = faceEdges[i_FE];
                if (edge.IsBoundary())
                {
                    edge._adjacentFaces = new List<Face<TPosition>>(); // Empty the list
                    EraseEdge(edge);
                }
                else
                {
                    edge._adjacentFaces.Remove(face);
                }
            }

            // Manage isolated vertices
            IReadOnlyList<Vertex<TPosition>> faceVertices = face.FaceVertices();
            for (int i_FV = 0; i_FV < faceEdges.Count; i_FV++)
            {
                Vertex<TPosition> vertex = faceVertices[i_FV];

                if (!vertex.IsConnected()) { EraseVertex(vertex); }
            }

            // Erase the face.
            //face._faceEdges = new List<FvEdge<TPosition>>();
            EraseFace(face);
        }


        /// <inheritdoc/>
        public override void EraseFace(Face<TPosition> face)
        {
            // Manage connection with edges
            IReadOnlyList<Edge<TPosition>> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                Edge<TPosition> edge = faceEdges[i_FE];

                edge._adjacentFaces?.Remove(face);
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