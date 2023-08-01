using System;
using System.Numerics;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Interface containing methods dedicated to triangular faces and meshes. 
    /// </summary>
    /// <remarks> A triangular mesh is a mesh which contains only triangular faces.
    /// No assumptions should be made on the valency of the mesh vertices.</remarks>
    public interface ITriMesh<TPosition>
        where TPosition : IEquatable<TPosition>,
                          IAdditionOperators<TPosition, TPosition, TPosition>,
                          IMultiplyOperators<TPosition, double, TPosition>, IDivisionOperators<TPosition, double, TPosition>
    {
        #region Properties

        /// <summary>
        /// Gets the parent <see cref="IMesh{TPosition}"/> on which the operations are carried out.
        /// </summary>
        IMesh<TPosition> ParentMesh { get; }

        #endregion

        #region Methods

        /******************** On Edges ********************/

        /// <summary>
        /// Performs an edge collapse remeshing operation.
        /// </summary>
        /// <param name="edge"> The edge to operate on. </param>
        void EdgeCollapse(IEdge<TPosition> edge);

        /// <summary>
        /// Performs an edge flip remeshing operation.
        /// </summary>
        /// <param name="edge"> The edge to operate on. </param>
        void EdgeFlip(IEdge<TPosition> edge);

        /// <summary>
        /// Performs an edge split remeshing operation.
        /// </summary>
        /// <param name="edge"> The edge to operate on. </param>
        void EdgeSplit(IEdge<TPosition> edge);

        #endregion
    }
}
