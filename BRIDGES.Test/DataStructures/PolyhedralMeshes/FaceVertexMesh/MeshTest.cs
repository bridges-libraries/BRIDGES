using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Test.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    /// <summary>
    /// Class testing the members of the <see cref="Mesh{TPosition}"/> data structure.
    /// </summary>
    [TestClass]
    public class MeshTest
    {
        #region Test Fields

        private static Mesh<Euc3D.Point> _parallelepiped = new Mesh<Euc3D.Point>();


        /******************** MSTest Processes ********************/

        /// <summary>
        /// Initialises the fields of the test class.
        /// </summary>
        /// <param name="context"> Context of the test. </param>
        [ClassInitialize]
        public static void InitialiseClass(TestContext context)
        {
            _parallelepiped = InitialiseParalelepiped();
        }

        /// <summary>
        /// Cleans the fields of the test class.
        /// </summary>
        [ClassCleanup]
        public static void CleanUpClass()
        {
            _parallelepiped = null;
        }


        /******************** Helpers ********************/

        /// <summary>
        /// Generates a parallelepiped in three-dimensional euclidean space.
        /// </summary>
        /// <returns></returns>
        private static Mesh<Euc3D.Point> InitialiseParalelepiped()
        {
            Mesh<Euc3D.Point> parallelepiped = new Mesh<Euc3D.Point>();

            // Create vertice's position
            Euc3D.Point p0 = new Euc3D.Point(3, 1, 2);
            Euc3D.Point p1 = new Euc3D.Point(8, 1, 2);
            Euc3D.Point p2 = new Euc3D.Point(8, 4, 1);
            Euc3D.Point p3 = new Euc3D.Point(3, 4, 1);
            Euc3D.Point p4 = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point p5 = new Euc3D.Point(7, 2.5, 5);
            Euc3D.Point p6 = new Euc3D.Point(7, 5.5, 4);
            Euc3D.Point p7 = new Euc3D.Point(2, 5.5, 4);

            // Add vertices
            Vertex<Euc3D.Point> v0 = parallelepiped.AddVertex(p0);
            Vertex<Euc3D.Point> v1 = parallelepiped.AddVertex(p1);
            Vertex<Euc3D.Point> v2 = parallelepiped.AddVertex(p2);
            Vertex<Euc3D.Point> v3 = parallelepiped.AddVertex(p3);
            Vertex<Euc3D.Point> v4 = parallelepiped.AddVertex(p4);
            Vertex<Euc3D.Point> v5 = parallelepiped.AddVertex(p5);
            Vertex<Euc3D.Point> v6 = parallelepiped.AddVertex(p6);
            Vertex<Euc3D.Point> v7 = parallelepiped.AddVertex(p7);

            // Add faces
            parallelepiped.AddFace(v0, v1, v2, v3);

            parallelepiped.AddFace(v1, v0, v4, v5);
            parallelepiped.AddFace(v2, v1, v5, v6);
            parallelepiped.AddFace(v3, v2, v6, v7);
            parallelepiped.AddFace(v0, v3, v7, v4);

            parallelepiped.AddFace(v7, v6, v5, v4);

            return parallelepiped;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Mesh{TPosition}.VertexCount"/> property.
        /// </summary>
        [TestMethod("Property VertexCount")]
        public void VertexCount()
        {
            // Arrange
            int expected = 8;
            //Act
            int actual = _parallelepiped.VertexCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="Mesh{TPosition}.EdgeCount"/> property.
        /// </summary>
        [TestMethod("Property EdgeCount")]
        public void EdgeCount()
        {
            // Arrange
            int expected = 12;
            //Act
            int actual = _parallelepiped.EdgeCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="Mesh{TPosition}.FaceCount"/> property.
        /// </summary>
        [TestMethod("Property FaceCount")]
        public void FaceCount()
        {
            // Arrange
            int expected = 6;
            //Act
            int actual = _parallelepiped.FaceCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="Mesh{TPosition}"/>.
        /// </summary>
        [TestMethod("Constructor()")]
        public void Constructor()
        {
            // Arrange
            Mesh<Euc3D.Point> parallelepiped = new Mesh<Euc3D.Point>();

            //Act
            int vertexCount = parallelepiped.VertexCount;
            int edgeCount = parallelepiped.EdgeCount;
            int faceCount = parallelepiped.FaceCount;

            // Assert
            Assert.AreEqual(0, vertexCount);
            Assert.AreEqual(0, edgeCount);
            Assert.AreEqual(0, faceCount);
        }

        #endregion


        #region Overrides

        /******************** Abstract.Mesh<T,Vertex<T>,Edge<T>,Face<T>> ********************/

        /***** On Vertices *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddVertex(TPosition)"/>.
        /// </summary>
        [TestMethod("Method AddVertex(TPosition)")]
        public void AddVertex_TPosition()
        {
            // Arrange
            Mesh<Euc3D.Point> mesh = new Mesh<Euc3D.Point>();

            Euc3D.Point point = new Euc3D.Point(1.0, 2.0, 4.0);

            // Act
            Vertex<Euc3D.Point> vertex = mesh.AddVertex(point);

            // Assert
            Assert.AreEqual(1, mesh.VertexCount);
            Assert.IsTrue(point.Equals(vertex.Position));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetVertex(int)"/>.
        /// </summary>
        [TestMethod("Method GetVertex(Int)")]
        public void GetVertex_Int()
        {
            // Arrange
            int existingIndex = 4;
            Euc3D.Point point = new Euc3D.Point(2, 2.5, 5);

            int absentIndex = 10;
            bool throwsException = false;

            // Act
            Vertex<Euc3D.Point> existingVertex = _parallelepiped.GetVertex(existingIndex);
            Euc3D.Point position = existingVertex.Position;

            Vertex<Euc3D.Point> absentVertex = default;
            try { absentVertex = _parallelepiped.GetVertex(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingVertex.Index);
            Assert.IsTrue(point.Equals(position));

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.TryGetVertex(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetVertex(Int)")]
        public void TryGetVertex_Int()
        {
            // Arrange
            int existingIndex = 6;
            Euc3D.Point point = new Euc3D.Point(7, 5.5, 4);

            int absentIndex = 12;

            // Act
            Vertex<Euc3D.Point> vertex = _parallelepiped.TryGetVertex(existingIndex);
            Euc3D.Point position = vertex.Position;

            Vertex<Euc3D.Point> absentVertex = _parallelepiped.TryGetVertex(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, vertex.Index);
            Assert.IsTrue(point.Equals(position));

            Assert.IsTrue(absentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetVertices()"/>.
        /// </summary>
        [TestMethod("Method GetVertices()")]
        public void GetVertices()
        {
            // Arrange
            Euc3D.Point[] points = new Euc3D.Point[8];

            points[0] = new Euc3D.Point(3, 1, 2);
            points[1] = new Euc3D.Point(8, 1, 2);
            points[2] = new Euc3D.Point(8, 4, 1);
            points[3] = new Euc3D.Point(3, 4, 1);
            points[4] = new Euc3D.Point(2, 2.5, 5);
            points[5] = new Euc3D.Point(7, 2.5, 5);
            points[6] = new Euc3D.Point(7, 5.5, 4);
            points[7] = new Euc3D.Point(2, 5.5, 4);

            // Act
            IReadOnlyList<Vertex<Euc3D.Point>> vertices = _parallelepiped.GetVertices();

            // Arrange
            Assert.AreEqual(points.Length, vertices.Count);

            for (int i = 0; i < points.Length; i++)
            {
                Assert.IsTrue(points[i].Equals(vertices[i].Position));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.RemoveVertex(Vertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveVertex(FvVertex)")]
        public void RemoveVertex_FvVertex()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Vertex<Euc3D.Point> vertex = heMesh.GetVertex(4);

            // Act
            heMesh.RemoveVertex(vertex);

            // Assert
            Assert.AreEqual(7, heMesh.VertexCount);
            Assert.AreEqual(9, heMesh.EdgeCount);
            Assert.AreEqual(3, heMesh.FaceCount);
        }


        /***** On Edges *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddEdge(Vertex{TPosition}, Vertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method AddEdge(FvVertex,FvVertex)")]
        public void AddEdge_HeVertex_FvVertex()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = new Mesh<Euc3D.Point>();

            Euc3D.Point pointA = new Euc3D.Point(1.0, 2.0, 4.0);
            Euc3D.Point pointB = new Euc3D.Point(3.0, 5.0, 6.0);

            // Act
            Vertex<Euc3D.Point> vertexA = heMesh.AddVertex(pointA);
            Vertex<Euc3D.Point> vertexB = heMesh.AddVertex(pointB);

            Edge<Euc3D.Point> edge = heMesh.AddEdge(vertexA, vertexB);

            // Assert
            Assert.AreEqual(2, heMesh.VertexCount);
            Assert.AreEqual(1, heMesh.EdgeCount);
            Assert.AreEqual(0, heMesh.FaceCount);

            Assert.AreEqual(0, edge.Index);
            Assert.IsTrue(vertexA.Equals(edge.StartVertex));
            Assert.IsTrue(vertexB.Equals(edge.EndVertex));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetEdge(int)"/>.
        /// </summary>
        [TestMethod("Method GetEdge(Int)")]
        public void GetEdge_Int()
        {
            // Arrange
            int existingIndex = 5;
            Euc3D.Point startPoint = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point endPoint = new Euc3D.Point(7, 2.5, 5);

            int absentIndex = 15;
            bool throwsException = false;

            // Act
            Edge<Euc3D.Point> existingEdge = _parallelepiped.GetEdge(existingIndex);
            Vertex<Euc3D.Point> startVertex = existingEdge.StartVertex;
            Vertex<Euc3D.Point> endVertex = existingEdge.EndVertex;

            Edge<Euc3D.Point> absentEdge = default;
            try { absentEdge = _parallelepiped.GetEdge(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingEdge.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.TryGetEdge(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetEdge(Int)")]
        public void TryGetEdge_Int()
        {
            // Arrange
            int existingIndex = 11;
            Euc3D.Point startPoint = new Euc3D.Point(2, 5.5, 4);
            Euc3D.Point endPoint = new Euc3D.Point(2, 2.5, 5);

            int absentIndex = 12;

            // Act
            Edge<Euc3D.Point> existingEdge = _parallelepiped.TryGetEdge(existingIndex);
            Vertex<Euc3D.Point> startVertex = existingEdge.StartVertex;
            Vertex<Euc3D.Point> endVertex = existingEdge.EndVertex;

            Edge<Euc3D.Point> absentEdge = _parallelepiped.TryGetEdge(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingEdge.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(absentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetEdges()"/>.
        /// </summary>
        [TestMethod("Method GetEdges()")]
        public void GetEdges()
        {
            // Arrange
            Euc3D.Point p0 = new Euc3D.Point(3, 1, 2);
            Euc3D.Point p1 = new Euc3D.Point(8, 1, 2);
            Euc3D.Point p2 = new Euc3D.Point(8, 4, 1);
            Euc3D.Point p3 = new Euc3D.Point(3, 4, 1);
            Euc3D.Point p4 = new Euc3D.Point(2, 2.5, 5);
            Euc3D.Point p5 = new Euc3D.Point(7, 2.5, 5);
            Euc3D.Point p6 = new Euc3D.Point(7, 5.5, 4);
            Euc3D.Point p7 = new Euc3D.Point(2, 5.5, 4);

            // Act
            IReadOnlyList<Edge<Euc3D.Point>> edges = _parallelepiped.GetEdges();

            // Arrange
            Assert.AreEqual(12, edges.Count);

            for (int i = 0; i < edges.Count; i++)
            {
                Assert.AreEqual(i, edges[i].Index);
            }

            Assert.IsTrue(p0.Equals(edges[3].EndVertex.Position));
            Assert.IsTrue(p1.Equals(edges[1].StartVertex.Position));
            Assert.IsTrue(p2.Equals(edges[8].EndVertex.Position));
            Assert.IsTrue(p3.Equals(edges[3].StartVertex.Position));
            Assert.IsTrue(p4.Equals(edges[11].EndVertex.Position));
            Assert.IsTrue(p5.Equals(edges[7].StartVertex.Position));
            Assert.IsTrue(p6.Equals(edges[7].EndVertex.Position));
            Assert.IsTrue(p7.Equals(edges[10].StartVertex.Position));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.RemoveEdge(Edge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveEdge(FvEdge)")]
        public void RemoveEdge_FvEdge()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Edge<Euc3D.Point> edge = heMesh.GetEdge(5);

            // Act
            heMesh.RemoveEdge(edge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.EraseEdge(Edge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseEdge(FvEdge)")]
        public void EraseEdge_FvEdge()
        {
            // Arrange
            Mesh<Euc3D.Point> mesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Edge<Euc3D.Point> edge = mesh.GetEdge(4);

            // Act
            mesh.EraseEdge(edge);

            // Assert
            Assert.AreEqual(8, mesh.VertexCount);
            Assert.AreEqual(11, mesh.EdgeCount);
            Assert.AreEqual(4, mesh.FaceCount);
        }


        /***** On Faces *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddFace(List{Vertex{TPosition}})"/>.
        /// </summary>
        [TestMethod("Method AddFace(List<FvVertex>)")]
        public void AddEdge_FvVertexList()
        {
            // Arrange
            Mesh<Euc3D.Point> mesh = new Mesh<Euc3D.Point>();

            Euc3D.Point[] points = new Euc3D.Point[4];
            points[0] = new Euc3D.Point(1.0, 0.0, 2.0);
            points[1] = new Euc3D.Point(4.0, 1.0, 3.0);
            points[2] = new Euc3D.Point(5.0, 4.0, 4.0);
            points[3] = new Euc3D.Point(2.0, 3.0, 3.0);

            // Act
            List<Vertex<Euc3D.Point>> vertices = new List<Vertex<Euc3D.Point>>();
            for (int i = 0; i < 4; i++) { vertices.Add(mesh.AddVertex(points[i])); }

            Face<Euc3D.Point> face = mesh.AddFace(vertices);

            IReadOnlyList<Vertex<Euc3D.Point>> faceVertices = face.FaceVertices();
            IReadOnlyList<Edge<Euc3D.Point>> faceEdges = face.FaceEdges();

            // Assert
            Assert.AreEqual(4, mesh.VertexCount);
            Assert.AreEqual(4, mesh.EdgeCount);
            Assert.AreEqual(1, mesh.FaceCount);

            Assert.AreEqual(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.IsTrue(faceVertices[i_V].Equals(vertices[i_V])); }
            for (int i_V = 0; i_V < 4; i_V++) { Assert.AreEqual(i_V, faceEdges[i_V].Index); }

        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetFace(int)"/>.
        /// </summary>
        [TestMethod("Method GetFace(Int)")]
        public void GetFace_Int()
        {
            // Arrange
            int existingIndex = 4;
            Edge<Euc3D.Point>[] edges = new Edge<Euc3D.Point>[4];
            edges[0] = _parallelepiped.GetEdge(3);
            edges[1] = _parallelepiped.GetEdge(10);
            edges[2] = _parallelepiped.GetEdge(11);
            edges[3] = _parallelepiped.GetEdge(4);

            int absentIndex = 6;
            bool throwsException = false;

            // Act
            Face<Euc3D.Point> existingFace = _parallelepiped.GetFace(existingIndex);
            IReadOnlyList<Edge<Euc3D.Point>> faceEdges = existingFace.FaceEdges();

            Face<Euc3D.Point> absentFace = default;
            try { absentFace = _parallelepiped.GetFace(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.IsTrue(edges[i_E].Equals(faceEdges[i_E])); }

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.TryGetFace(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetFace(Int)")]
        public void TryGetFace_Int()
        {
            // Arrange
            int existingIndex = 0;
            Edge<Euc3D.Point>[] edges = new Edge<Euc3D.Point>[4];
            edges[0] = _parallelepiped.GetEdge(0);
            edges[1] = _parallelepiped.GetEdge(1);
            edges[2] = _parallelepiped.GetEdge(2);
            edges[3] = _parallelepiped.GetEdge(3);

            int absentIndex = 12;

            // Act
            Face<Euc3D.Point> existingFace = _parallelepiped.TryGetFace(existingIndex);
            IReadOnlyList<Edge<Euc3D.Point>> faceEdges = existingFace.FaceEdges();

            Face<Euc3D.Point> absentFace = _parallelepiped.TryGetFace(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.IsTrue(edges[i_E].Equals(faceEdges[i_E])); }

            Assert.IsTrue(absentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetFaces()"/>.
        /// </summary>
        [TestMethod("Method GetFaces()")]
        public void GetFaces()
        {
            // Arrange
            Edge<Euc3D.Point>[] edges = new Edge<Euc3D.Point>[6];
            edges[0] = _parallelepiped.GetEdge(0);
            edges[1] = _parallelepiped.GetEdge(0);
            edges[2] = _parallelepiped.GetEdge(1);
            edges[3] = _parallelepiped.GetEdge(2);
            edges[4] = _parallelepiped.GetEdge(3);
            edges[5] = _parallelepiped.GetEdge(9);

            // Act
            IReadOnlyList<Face<Euc3D.Point>> faces = _parallelepiped.GetFaces();

            Edge<Euc3D.Point>[] facesFirstEdge = new Edge<Euc3D.Point>[6];
            for (int i_F = 0; i_F < 6; i_F++)
            {
                IReadOnlyList<Edge<Euc3D.Point>> faceEdges = faces[i_F].FaceEdges();
                facesFirstEdge[i_F] = faceEdges[0];
            }

            // Arrange
            Assert.AreEqual(6, faces.Count);

            for (int i = 0; i < 6; i++) { Assert.AreEqual(i, faces[i].Index); }
            for (int i_F = 0; i_F < 6; i_F++) { Assert.IsTrue(edges[i_F].Equals(facesFirstEdge[i_F])); }
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.RemoveFace(Face{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveFace(FvFace)")]
        public void RemoveFace_FvFace()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Face<Euc3D.Point> face = heMesh.GetFace(0);

            // Act
            heMesh.RemoveFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.EraseFace(Face{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseFace(FvFace)")]
        public void EraseFace_FvFace()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Face<Euc3D.Point> face = heMesh.GetFace(4);

            // Act
            heMesh.EraseFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }

        #endregion


        /********** Missing **********/

        // CleanMesh(Bool)
        // Clone()
        // ToHalfedgeMesh

        /********** Too quantitative **********/

        // RemoveVertex_HeVertex()
        // RemoveEdge_HeEdge()
        // EraseEdge_HeEdge()
        // RemoveFace_HeFace()
        // EraseFace_HeFace()
    }
}
