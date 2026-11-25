using System;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceTraits
{
    /// <summary>
    /// Face normal vector trait.
    /// </summary>
    public interface NormalTrait
    {
        #region Properties

        /// <summary>
        /// Gets or sets the normal vector of the face.
        /// </summary>
        public Geometry.Euclidean3D.Vector Normal { get; set; }

        #endregion
    }
}
