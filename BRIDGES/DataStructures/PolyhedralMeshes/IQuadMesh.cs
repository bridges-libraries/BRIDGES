using System;
using System.Numerics;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Interface containing methods dedicated to quadrangular faces and meshes. 
    /// </summary>
    public interface IQuadMesh<TPosition>
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
    }
}
