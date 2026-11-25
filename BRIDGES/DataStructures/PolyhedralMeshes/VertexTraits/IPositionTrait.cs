using System;


namespace BRIDGES.DataStructures.PolyhedralMeshes.VertexTraits
{
    /// <summary>
    /// Vertex position trait.
    /// </summary>
    public interface IPositionTrait
    {
        #region Properties

        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        public Geometry.Euclidean3D.Point Position { get; set; }

        #endregion
    }
}
