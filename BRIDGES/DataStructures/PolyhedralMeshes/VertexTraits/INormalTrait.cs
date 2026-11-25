using System;


namespace BRIDGES.DataStructures.PolyhedralMeshes.VertexTraits
{
    /// <summary>
    /// Vertex normal vector trait.
    /// </summary>
    public interface INormalTrait
    {
        #region Properties

        /// <summary>
        /// Gets or sets the normal vector of the vertex.
        /// </summary>
        public Geometry.Euclidean3D.Vector Normal { get; set; }

        #endregion
    }
}
