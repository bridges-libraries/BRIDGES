using System;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh
{
    public partial class Mesh<TPosition>
    {
        /// <summary>
        /// Class containing methods dedicated to quadrangular faces and meshes. 
        /// </summary>
        public class QuadMesh : IQuadMesh<TPosition>
        {
            #region Fields

            private readonly Mesh<TPosition> _parentMesh;

            #endregion

            #region Constructors 

            /// <summary>
            /// Initialises a new instance of the <see cref="QuadMesh"/> class from a <see cref="FaceVertexMesh.Mesh{TVector}"/>.
            /// </summary>
            /// <param name="mesh"> Mesh to operate on. </param>
            internal QuadMesh(Mesh<TPosition> mesh)
            {
                _parentMesh = mesh;
            }

            #endregion


            #region Explicit : IMesh<TPosition>

            /// <inheritdoc cref="IQuadMesh{TPosition}.ParentMesh"/>
            IMesh<TPosition> IQuadMesh<TPosition>.ParentMesh
            {
                get { return _parentMesh; }
            }

            #endregion
        }

    }
}
