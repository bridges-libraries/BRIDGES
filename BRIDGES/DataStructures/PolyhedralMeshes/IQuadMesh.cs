using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Sets = BRIDGES.Algebra.Sets;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Interface containing methods dedicated to quadrangular faces and meshes. 
    /// </summary>
    public interface IQuadMesh<TPosition>
        where TPosition : IEquatable<TPosition>,
                          Alg_Fund.IAddable<TPosition> /* To Do : Remove */,
                          Alg_Sets.IGroupAction<TPosition, double>
    {
        #region Properties

        /// <summary>
        /// Gets the parent <see cref="IMesh{TPosition}"/> on which the operations are carried out.
        /// </summary>
        IMesh<TPosition> ParentMesh { get; }

        #endregion
    }
}
