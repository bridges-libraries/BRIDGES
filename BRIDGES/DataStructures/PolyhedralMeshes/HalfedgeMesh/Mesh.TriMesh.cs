using System;
using System.Collections.Generic;
using System.Net;

namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    public partial class Mesh<TPosition>
    {
        /// <summary>
        /// Class containing methods dedicated to triangular faces and meshes. 
        /// </summary>
        public class TriMesh : ITriMesh<TPosition>
        {
            #region Fields

            private readonly Mesh<TPosition> _parentMesh;

            #endregion

            #region Constructors 

            /// <summary>
            /// Initialises a new instance of the <see cref="TriMesh"/> class from a <see cref="HalfedgeMesh.Mesh{TVector}"/>.
            /// </summary>
            /// <param name="mesh"> Mesh to operate on. </param>
            internal TriMesh(Mesh<TPosition> mesh)
            {
                _parentMesh = mesh;
            }
            #endregion

            #region Methods

            /******************** On Edges ********************/

            /// <summary>
            /// Performs a edge collapse remeshing operation.
            /// </summary>
            /// <param name="halfedge"> Halfedge to operate on. </param>
            public void EdgeCollapse(Halfedge<TPosition> halfedge)
            {
                Vertex<TPosition> start = halfedge.StartVertex;
                Vertex<TPosition> end = halfedge.EndVertex;

                List<Vertex<TPosition>> neighborhood = new List<Vertex<TPosition>>();

                Halfedge<TPosition> he = halfedge.PrevHalfedge;
                while (!he.Equals(halfedge.PairHalfedge))
                {
                    neighborhood.Add(he.StartVertex);
                    he = he.PairHalfedge.PrevHalfedge;
                }
                if (!halfedge.PairHalfedge.IsBoundary()) 
                { 
                    neighborhood.RemoveAt(neighborhood.Count - 1);
                }

                he = halfedge.PairHalfedge.PrevHalfedge;
                while (!he.Equals(halfedge))
                {
                    neighborhood.Add(he.StartVertex);
                    he = he.PairHalfedge.PrevHalfedge;
                }
                if (!halfedge.IsBoundary())
                {
                    neighborhood.RemoveAt(neighborhood.Count - 1);
                }


                _parentMesh.RemoveVertex(start);
                _parentMesh.RemoveVertex(end);

                /* To Do : Improved with a smarter evaluation of the new vertex position*/
                TPosition newPosition = start.Position.Add(end.Position);
                newPosition = newPosition.Multiply(0.5);
                Vertex<TPosition> newVertex = _parentMesh.AddVertex(newPosition);

                for (int i = 0; i < neighborhood.Count; i++)
                {
                    int j = (i + 1) % (neighborhood.Count);
                    _parentMesh.AddFace(newVertex, neighborhood[i], neighborhood[j]);
                }
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeCollapse(IEdge{TPosition})"/>
            public void EdgeCollapse(Edge<TPosition> edge)
            {
                Halfedge<TPosition> halfedge = edge.CorrespondingHalfedge;
                EdgeCollapse(halfedge);
            }


            /// <summary>
            /// Performs a edge flip remeshing operation.
            /// </summary>
            /// <param name="halfedge"> Halfedge to operate on. </param>
            public void EdgeFlip(Halfedge<TPosition> halfedge)
            {
                if (halfedge.IsBoundary() | halfedge.PairHalfedge.IsBoundary()) { return; }

                Vertex<TPosition> start = halfedge.StartVertex;
                Vertex<TPosition> end = halfedge.StartVertex;

                Vertex<TPosition> left = halfedge.NextHalfedge.EndVertex;
                Vertex<TPosition> right = halfedge.PairHalfedge.NextHalfedge.EndVertex;

                _parentMesh.EraseHalfedge(halfedge);

                _parentMesh.AddFace(right, left, start);
                _parentMesh.AddFace(left, right, end);
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeFlip(IEdge{TPosition})"/>
            public void EdgeFlip(Edge<TPosition> edge)
            {
                Halfedge<TPosition> halfedge = edge.CorrespondingHalfedge;
                EdgeFlip(halfedge);
            }


            /// <summary>
            /// Performs a edge split remeshing operation.
            /// </summary>
            /// <param name="halfedge"> Halfedge to operate on. </param>
            public void EdgeSplit(Halfedge<TPosition> halfedge)
            {
                Vertex<TPosition> start = halfedge.StartVertex;
                Vertex<TPosition> end = halfedge.StartVertex;

                Vertex<TPosition> left = null, right = null;
                if (!halfedge.IsBoundary()) { left = halfedge.NextHalfedge.EndVertex; }
                if (!halfedge.PairHalfedge.IsBoundary()) { right = halfedge.PairHalfedge.NextHalfedge.EndVertex; }

                _parentMesh.EraseHalfedge(halfedge);

                /* To Do : Improved with a smarter evaluation of the new vertex position*/
                TPosition newPosition = start.Position.Add(end.Position);
                newPosition = newPosition.Multiply(0.5);
                Vertex<TPosition> newVertex = _parentMesh.AddVertex(newPosition);

                if (!(left is null))
                {
                    _parentMesh.AddFace(left, start, newVertex); 
                    _parentMesh.AddFace(end, left, newVertex);
                }
                if (!(right is null))
                {
                    _parentMesh.AddFace(start, right, newVertex);
                    _parentMesh.AddFace(right, end, newVertex);
                }
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeSplit(IEdge{TPosition})"/>
            public void EdgeSplit(Edge<TPosition> edge)
            {
                Halfedge<TPosition> halfedge = edge.CorrespondingHalfedge;
                EdgeSplit(halfedge);
            }

            #endregion


            #region Explicit : IMesh<TPosition>

            /******************** Properties ********************/

            /// <inheritdoc cref="ITriMesh{TPosition}.ParentMesh"/>
            IMesh<TPosition> ITriMesh<TPosition>.ParentMesh
            {
                get { return _parentMesh; }
            }


            /******************** Methods - On Vertices ********************/

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeCollapse(IEdge{TPosition})"/>
            void ITriMesh<TPosition>.EdgeCollapse(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e) 
                {
                    Halfedge<TPosition> halfedge = e.CorrespondingHalfedge;
                    EdgeCollapse(halfedge); 
                }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeFlip(IEdge{TPosition})"/>
            void ITriMesh<TPosition>.EdgeFlip(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e)
                {
                    Halfedge<TPosition> halfedge = e.CorrespondingHalfedge;
                    EdgeFlip(halfedge);
                }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }
            
            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeSplit(IEdge{TPosition})"/>
            void ITriMesh<TPosition>.EdgeSplit(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e)
                {
                    Halfedge<TPosition> halfedge = e.CorrespondingHalfedge;
                    EdgeSplit(halfedge);
                }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }

            #endregion
        }
    }
}
