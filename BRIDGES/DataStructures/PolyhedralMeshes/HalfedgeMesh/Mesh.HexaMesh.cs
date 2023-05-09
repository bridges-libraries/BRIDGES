using System;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh
{
    public partial class Mesh<TPosition>
    {
        /// <summary>
        /// Class containing methods dedicated to hexagonal faces and meshes. 
        /// </summary>
        public class HexaMesh : IHexaMesh<TPosition>
        {
            #region Fields

            private readonly Mesh<TPosition> _parentMesh;

            #endregion

            #region Constructors 

            /// <summary>
            /// Initialises a new instance of the <see cref="HexaMesh"/> class from a <see cref="HalfedgeMesh.Mesh{TVector}"/>.
            /// </summary>
            /// <param name="mesh"> Mesh to operate on. </param>
            internal HexaMesh(Mesh<TPosition> mesh)
            {
                _parentMesh = mesh;
            }

            #endregion


            #region Explicit : IMesh<TPosition>

            /// <inheritdoc cref="IHexaMesh{TPosition}.ParentMesh"/>
            IMesh<TPosition> IHexaMesh<TPosition>.ParentMesh
            {
                get { return _parentMesh; }
            }

            #endregion
        }

    }
}
