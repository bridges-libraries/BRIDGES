using System;

using BRIDGES.DataStructures.PolyhedralMeshes.Abstract;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    public partial class Mesh<TPosition> : Mesh<TPosition, Vertex<TPosition>, Edge<TPosition>, Face<TPosition>>
        where TPosition : IEquatable<TPosition>
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
            /// Initialises a new instance of the <see cref="TriMesh"/> class from a <see cref="FaceVertexMesh.Mesh{TVector}"/>.
            /// </summary>
            /// <param name="mesh"> Mesh to operate on. </param>
            internal TriMesh(Mesh<TPosition> mesh)
            {
                _parentMesh = mesh;
            }
            #endregion

            #region Methods

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeCollapse(IEdge{TPosition})"/>
            public void EdgeCollapse(Edge<TPosition> edge)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeFlip(IEdge{TPosition})"/>
            public void EdgeFlip(Edge<TPosition> edge)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc cref="ITriMesh{TPosition}.EdgeSplit(IEdge{TPosition})"/>
            public void EdgeSplit(Edge<TPosition> edge)
            {
                throw new NotImplementedException();
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

            void ITriMesh<TPosition>.EdgeCollapse(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e) { EdgeCollapse(e); }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }

            void ITriMesh<TPosition>.EdgeFlip(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e) { EdgeFlip(e); }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }

            void ITriMesh<TPosition>.EdgeSplit(IEdge<TPosition> edge)
            {
                if (edge is Edge<TPosition> e) { EdgeSplit(e); }
                else { throw new ArgumentException("The edge type is not consistent with the mesh type."); }
            }

            #endregion
        }

    }
}
