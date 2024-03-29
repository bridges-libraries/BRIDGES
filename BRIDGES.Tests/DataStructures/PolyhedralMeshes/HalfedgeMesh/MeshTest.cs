﻿using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.Tests.DataStructures.PolyhedralMeshes.HalfedgeMesh
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
        /// Tests the <see cref="Mesh{TPosition}.HalfedgeCount"/> property.
        /// </summary>
        [TestMethod("Property HalfedgeCount")]
        public void HalfedgeCount()
        {
            // Arrange
            int expected = 24;
            //Act
            int actual = _parallelepiped.HalfedgeCount;
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
            int halfedgeCount = parallelepiped.HalfedgeCount;
            int edgeCount = parallelepiped.EdgeCount;
            int faceCount = parallelepiped.FaceCount;

            // Assert
            Assert.AreEqual(0, vertexCount);
            Assert.AreEqual(0, halfedgeCount);
            Assert.AreEqual(0, edgeCount);
            Assert.AreEqual(0, faceCount);
        }

        #endregion

        #region Methods

        /***** On Halfedges *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddPair(Vertex{TPosition}, Vertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method AddPair(HeVertex,HeVertex)")]
        public void AddPair_HeVertex_HeVertex()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = new Mesh<Euc3D.Point>();

            Euc3D.Point pointA = new Euc3D.Point(1.0, 2.0, 4.0);
            Euc3D.Point pointB = new Euc3D.Point(3.0, 5.0, 6.0);

            // Act
            Vertex<Euc3D.Point> vertexA = heMesh.AddVertex(pointA);
            Vertex<Euc3D.Point> vertexB = heMesh.AddVertex(pointB);

            Halfedge<Euc3D.Point> halfedge = heMesh.AddPair(vertexA, vertexB);

            // Assert
            Assert.AreEqual(2, heMesh.VertexCount);
            Assert.AreEqual(2, heMesh.HalfedgeCount);
            Assert.AreEqual(1, heMesh.EdgeCount);
            Assert.AreEqual(0, heMesh.FaceCount);

            Assert.AreEqual(0, halfedge.Index);
            Assert.IsTrue(vertexA.Equals(halfedge.StartVertex));
            Assert.IsTrue(vertexB.Equals(halfedge.EndVertex));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetHalfedge(int)"/>.
        /// </summary>
        [TestMethod("Method GetHalfedge(Int)")]
        public void GetHalfedge_Int()
        {
            // Arrange
            int existingIndex = 8;
            Euc3D.Point startPoint = new Euc3D.Point(3, 1, 2);
            Euc3D.Point endPoint = new Euc3D.Point(2, 2.5, 5);

            int absentIndex = 32;
            bool throwsException = false;

            // Act
            Halfedge<Euc3D.Point> existingHe = _parallelepiped.GetHalfedge(existingIndex);
            Vertex<Euc3D.Point> startVertex = existingHe.StartVertex;
            Vertex<Euc3D.Point> endVertex = existingHe.EndVertex;

            Halfedge<Euc3D.Point> absentHe = default;
            try { absentHe = _parallelepiped.GetHalfedge(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingHe.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(throwsException);
            Assert.IsTrue(absentHe is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.TryGetHalfedge(int)"/>.
        /// </summary>
        [TestMethod("Method TryGetHalfedge(Int)")]
        public void TryGetHalfedge_Int()
        {
            // Arrange
            int existingIndex = 21;
            Euc3D.Point startPoint = new Euc3D.Point(3, 4, 1);
            Euc3D.Point endPoint = new Euc3D.Point(2, 5.5, 4);

            int absentIndex = 42;

            // Act
            Halfedge<Euc3D.Point> existingEdge = _parallelepiped.TryGetHalfedge(existingIndex);
            Vertex<Euc3D.Point> startVertex = existingEdge.StartVertex;
            Vertex<Euc3D.Point> endVertex = existingEdge.EndVertex;

            Halfedge<Euc3D.Point> absentEdge = _parallelepiped.TryGetHalfedge(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingEdge.Index);
            Assert.IsTrue(startPoint.Equals(startVertex.Position));
            Assert.IsTrue(endPoint.Equals(endVertex.Position));

            Assert.IsTrue(absentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetHalfedges()"/>.
        /// </summary>
        [TestMethod("Method GetHalfedges()")]
        public void GetHalfedges()
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
            IReadOnlyList<Halfedge<Euc3D.Point>> halfedges = _parallelepiped.GetHalfedges();

            // Arrange
            Assert.AreEqual(24, halfedges.Count);

            for (int i = 0; i < halfedges.Count; i++)
            {
                Assert.AreEqual(i, halfedges[i].Index);
            }

            Assert.IsTrue(p0.Equals(halfedges[6].EndVertex.Position));
            Assert.IsTrue(p1.Equals(halfedges[13].StartVertex.Position));
            Assert.IsTrue(p2.Equals(halfedges[16].EndVertex.Position));
            Assert.IsTrue(p3.Equals(halfedges[21].StartVertex.Position));
            Assert.IsTrue(p4.Equals(halfedges[8].EndVertex.Position));
            Assert.IsTrue(p5.Equals(halfedges[11].StartVertex.Position));
            Assert.IsTrue(p6.Equals(halfedges[19].EndVertex.Position));
            Assert.IsTrue(p7.Equals(halfedges[20].StartVertex.Position));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.HalfedgeBetween(Vertex{TPosition}, Vertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method HalfedgeBetween(HeVertex,HeVertex)")]
        public void HalfedgeBetween_HeVertex_HeVertex()
        {
            // Arrange
            Vertex<Euc3D.Point> vertex2 = _parallelepiped.GetVertex(2);
            Vertex<Euc3D.Point> vertex3 = _parallelepiped.GetVertex(3);
            Vertex<Euc3D.Point> vertex6 = _parallelepiped.GetVertex(6);

            int existingIndex = 16;

            // Act
            Halfedge<Euc3D.Point> existingHe = _parallelepiped.HalfedgeBetween(vertex6, vertex2);
            
            Halfedge<Euc3D.Point> absentHe = _parallelepiped.HalfedgeBetween(vertex6, vertex3);

            // Assert
            Assert.AreEqual(existingIndex, existingHe.Index);
            Assert.IsTrue(vertex6.Equals(existingHe.StartVertex));
            Assert.IsTrue(vertex2.Equals(existingHe.EndVertex));

            Assert.IsTrue(absentHe is null);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.RemoveHalfedge(Halfedge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveHalfedge(HeHalfedge)")]
        public void RemoveHalfedge_HeHalfedge()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Halfedge<Euc3D.Point> halfedge = heMesh.GetHalfedge(17);

            // Act
            heMesh.RemoveHalfedge(halfedge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(22, heMesh.HalfedgeCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.EraseHalfedge(Halfedge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseHalfedge(HeHalfedge)")]
        public void EraseHalfedge_HeHalfedge()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Halfedge<Euc3D.Point> halfedge = heMesh.GetHalfedge(19);

            // Act
            heMesh.EraseHalfedge(halfedge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(22, heMesh.HalfedgeCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }

        #endregion


        #region Overrides

        /******************** Abstract.Mesh<T,Vertex<T>,Edge<T>,Face<T>>********************/

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
            points[2]= new Euc3D.Point(8, 4, 1);
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
        [TestMethod("Method RemoveVertex(HeVertex)")]
        public void RemoveVertex_HeVertex()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Vertex<Euc3D.Point> vertex = heMesh.GetVertex(4);

            // Act
            heMesh.RemoveVertex(vertex);

            // Assert
            Assert.AreEqual(7, heMesh.VertexCount);
            Assert.AreEqual(18, heMesh.HalfedgeCount);
            Assert.AreEqual(9, heMesh.EdgeCount);
            Assert.AreEqual(3, heMesh.FaceCount);
        }


        /***** On Edges *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddEdge(Vertex{TPosition}, Vertex{TPosition})"/>.
        /// </summary>
        [TestMethod("Method AddEdge(HeVertex,HeVertex)")]
        public void AddEdge_HeVertex_HeVertex()
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
            Assert.AreEqual(2, heMesh.HalfedgeCount);
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
        [TestMethod("Method RemoveEdge(HeEdge)")]
        public void RemoveEdge_HeEdge()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Edge<Euc3D.Point> edge = heMesh.GetEdge(5);

            // Act
            heMesh.RemoveEdge(edge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(22, heMesh.HalfedgeCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.EraseEdge(Edge{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseEdge(HeEdge)")]
        public void EraseEdge_HeEdge()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Edge<Euc3D.Point> edge = heMesh.GetEdge(4);

            // Act
            heMesh.EraseEdge(edge);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(22, heMesh.HalfedgeCount);
            Assert.AreEqual(11, heMesh.EdgeCount);
            Assert.AreEqual(4, heMesh.FaceCount);
        }


        /***** On Faces *****/

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddFace(List{Vertex{TPosition}})"/>.
        /// </summary>
        [TestMethod("Method AddFace(List<HeVertex>)")]
        public void AddFace_HeVertexList()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = new Mesh<Euc3D.Point>();

            Euc3D.Point point0 = new Euc3D.Point(1.0, 0.0, 2.0);
            Euc3D.Point point1 = new Euc3D.Point(4.0, 1.0, 3.0);
            Euc3D.Point point2 = new Euc3D.Point(5.0, 4.0, 4.0);
            Euc3D.Point point3 = new Euc3D.Point(2.0, 3.0, 3.0);

            // Act
            Vertex<Euc3D.Point> vertex0 = heMesh.AddVertex(point0);
            Vertex<Euc3D.Point> vertex1 = heMesh.AddVertex(point1);
            Vertex<Euc3D.Point> vertex2 = heMesh.AddVertex(point2);
            Vertex<Euc3D.Point> vertex3 = heMesh.AddVertex(point3);

            List<Vertex<Euc3D.Point>> vertices = new List<Vertex<Euc3D.Point>>() { vertex0, vertex1, vertex2, vertex3 };

            Face<Euc3D.Point> face = heMesh.AddFace(vertices);

            Halfedge<Euc3D.Point> faceHalfedge = heMesh.GetHalfedge(0);

            // Assert
            Assert.AreEqual(4, heMesh.VertexCount);
            Assert.AreEqual(8, heMesh.HalfedgeCount);
            Assert.AreEqual(4, heMesh.EdgeCount);
            Assert.AreEqual(1, heMesh.FaceCount);

            Assert.AreEqual(0, face.Index);
            Assert.IsTrue(faceHalfedge.Equals(face.FirstHalfedge));
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.AddFace(List{Vertex{TPosition}})"/>.
        /// </summary>
        [TestMethod("Method AddFace(List<HeVertex> - Non Manifold A)")]
        public void AddFace_HeVertexList_NonManifold_A()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = new Mesh<Euc3D.Point>();

            Euc3D.Point point0 = new Euc3D.Point(2.0, 1.0, 3.0);
            Euc3D.Point point1 = new Euc3D.Point(5.0, 1.0, 4.0);
            Euc3D.Point point2 = new Euc3D.Point(4.5, 3.0, 1.5);
            Euc3D.Point point3 = new Euc3D.Point(2.0, 5.0, 4.5);
            Euc3D.Point point4 = new Euc3D.Point(0.0, 3.0, 2.5);
            Euc3D.Point point5 = new Euc3D.Point(-1.0, 1.0, 3.5);
            Euc3D.Point point6 = new Euc3D.Point(0.5, -1.0, 5.0);
            Euc3D.Point point7 = new Euc3D.Point(2.0, -2.0, 2.0);
            Euc3D.Point point8 = new Euc3D.Point(3.5, -1.0, 5.5);

            // Act
            Vertex<Euc3D.Point> vertex0 = heMesh.AddVertex(point0);
            Vertex<Euc3D.Point> vertex1 = heMesh.AddVertex(point1);
            Vertex<Euc3D.Point> vertex2 = heMesh.AddVertex(point2);
            Vertex<Euc3D.Point> vertex3 = heMesh.AddVertex(point3);
            Vertex<Euc3D.Point> vertex4 = heMesh.AddVertex(point4);
            Vertex<Euc3D.Point> vertex5 = heMesh.AddVertex(point5);
            Vertex<Euc3D.Point> vertex6 = heMesh.AddVertex(point6);
            Vertex<Euc3D.Point> vertex7 = heMesh.AddVertex(point7);
            Vertex<Euc3D.Point> vertex8 = heMesh.AddVertex(point8);

            List<Vertex<Euc3D.Point>> faceVertices_A = new List<Vertex<Euc3D.Point>>() { vertex0, vertex1, vertex2 };
            List<Vertex<Euc3D.Point>> faceVertices_B = new List<Vertex<Euc3D.Point>>() { vertex0, vertex3, vertex4 };
            List<Vertex<Euc3D.Point>> faceVertices_C = new List<Vertex<Euc3D.Point>>() { vertex0, vertex7, vertex8 };
            List<Vertex<Euc3D.Point>> faceVertices_D = new List<Vertex<Euc3D.Point>>() { vertex0, vertex5, vertex6 };

            List<Vertex<Euc3D.Point>> faceVertices_E = new List<Vertex<Euc3D.Point>>() { vertex0, vertex6, vertex7 };
            List<Vertex<Euc3D.Point>> faceVertices_F = new List<Vertex<Euc3D.Point>>() { vertex0, vertex8, vertex1 };
            List<Vertex<Euc3D.Point>> faceVertices_G = new List<Vertex<Euc3D.Point>>() { vertex0, vertex4, vertex5 };
            List<Vertex<Euc3D.Point>> faceVertices_H = new List<Vertex<Euc3D.Point>>() { vertex0, vertex2, vertex3 };

            Face<Euc3D.Point> face_A = heMesh.AddFace(faceVertices_A);
            Face<Euc3D.Point> face_B = heMesh.AddFace(faceVertices_B);
            Face<Euc3D.Point> face_C = heMesh.AddFace(faceVertices_C);
            Face<Euc3D.Point> face_D = heMesh.AddFace(faceVertices_D);

            Face<Euc3D.Point> face_E = heMesh.AddFace(faceVertices_E);
            Face<Euc3D.Point> face_F = heMesh.AddFace(faceVertices_F);
            Face<Euc3D.Point> face_G = heMesh.AddFace(faceVertices_G);
            Face<Euc3D.Point> face_H = heMesh.AddFace(faceVertices_H);

            // Assert
            Assert.AreEqual(9, heMesh.VertexCount);
            Assert.AreEqual(32, heMesh.HalfedgeCount);
            Assert.AreEqual(16, heMesh.EdgeCount);
            Assert.AreEqual(8, heMesh.FaceCount);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetFace(int)"/>.
        /// </summary>
        [TestMethod("Method GetFace(Int)")]
        public void GetFace_Int()
        {
            // Arrange
            int existingIndex = 4; 
            Halfedge<Euc3D.Point> faceHalfedge = _parallelepiped.GetHalfedge(7);

            int absentIndex = 6;
            bool throwsException = false;

            // Act
            Face<Euc3D.Point> existingFace = _parallelepiped.GetFace(existingIndex);

            Face<Euc3D.Point> absentFace = default;
            try { absentFace = _parallelepiped.GetFace(absentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            Assert.IsTrue(faceHalfedge.Equals(existingFace.FirstHalfedge));

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
            Halfedge<Euc3D.Point> faceHalfedge = _parallelepiped.GetHalfedge(0);

            int absentIndex = 12;

            // Act
            Face<Euc3D.Point> existingFace = _parallelepiped.TryGetFace(existingIndex);

            Face<Euc3D.Point> absentFace = _parallelepiped.TryGetFace(absentIndex);

            // Assert
            Assert.AreEqual(existingIndex, existingFace.Index);
            Assert.IsTrue(faceHalfedge.Equals(existingFace.FirstHalfedge));

            Assert.IsTrue(absentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.GetFaces()"/>.
        /// </summary>
        [TestMethod("Method GetFaces()")]
        public void GetFaces()
        {
            // Arrange
            Halfedge<Euc3D.Point> faceHalfedge0 = _parallelepiped.GetHalfedge(0);
            Halfedge<Euc3D.Point> faceHalfedge1 = _parallelepiped.GetHalfedge(1);
            Halfedge<Euc3D.Point> faceHalfedge2 = _parallelepiped.GetHalfedge(3);
            Halfedge<Euc3D.Point> faceHalfedge3 = _parallelepiped.GetHalfedge(5);
            Halfedge<Euc3D.Point> faceHalfedge4 = _parallelepiped.GetHalfedge(7);
            Halfedge<Euc3D.Point> faceHalfedge5 = _parallelepiped.GetHalfedge(19);

            // Act
            IReadOnlyList<Face<Euc3D.Point>> faces = _parallelepiped.GetFaces();

            // Arrange
            Assert.AreEqual(6, faces.Count);

            for (int i = 0; i < faces.Count; i++)
            {
                Assert.AreEqual(i, faces[i].Index);
            }

            Assert.IsTrue(faceHalfedge0.Equals(faces[0].FirstHalfedge));
            Assert.IsTrue(faceHalfedge1.Equals(faces[1].FirstHalfedge));
            Assert.IsTrue(faceHalfedge2.Equals(faces[2].FirstHalfedge));
            Assert.IsTrue(faceHalfedge3.Equals(faces[3].FirstHalfedge));
            Assert.IsTrue(faceHalfedge4.Equals(faces[4].FirstHalfedge));
            Assert.IsTrue(faceHalfedge5.Equals(faces[5].FirstHalfedge));
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.RemoveFace(Face{TPosition})"/>.
        /// </summary>
        [TestMethod("Method RemoveFace(HeFace)")]
        public void RemoveFace_HeFace()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Face<Euc3D.Point> face = heMesh.GetFace(0);

            // Act
            heMesh.RemoveFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(24, heMesh.HalfedgeCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }


        /// <summary>
        /// Tests the method <see cref="Mesh{TPosition}.EraseFace(Face{TPosition})"/>.
        /// </summary>
        [TestMethod("Method EraseFace(HeFace)")]
        public void EraseFace_HeFace()
        {
            // Arrange
            Mesh<Euc3D.Point> heMesh = _parallelepiped.Clone() as Mesh<Euc3D.Point>;

            Face<Euc3D.Point> face = heMesh.GetFace(4);

            // Act
            heMesh.EraseFace(face);

            // Assert
            Assert.AreEqual(8, heMesh.VertexCount);
            Assert.AreEqual(24, heMesh.HalfedgeCount);
            Assert.AreEqual(12, heMesh.EdgeCount);
            Assert.AreEqual(5, heMesh.FaceCount);
        }

        #endregion


        /********** Missing **********/

        // CleanMesh(Bool)
        // Clone()
        // ToFaceVertexMesh

        /********** Too quantitative **********/

        // RemoveVertex_HeVertex()
        // RemoveEdge_HeEdge()
        // EraseEdge_HeEdge()
        // RemoveFace_HeFace()
        // EraseFace_HeFace()
    }
}
