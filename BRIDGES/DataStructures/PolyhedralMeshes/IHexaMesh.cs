using System;
using System.Numerics;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Interface containing methods dedicated to hexagonal faces and meshes. 
    /// </summary>
    /// <remarks> An hexagonal mesh is a mesh which contains only hexagonal faces.
    /// No assumptions should be made on the valency of the mesh vertices.</remarks>
    public interface IHexaMesh<TPosition>
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
