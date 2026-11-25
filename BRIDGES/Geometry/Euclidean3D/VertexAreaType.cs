using System;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Nature of the vertex area.
    /// </summary>
    public enum VertexAreaType
    {
        /// <summary>
        /// Area is evaluated using barycentric approximation.
        /// </summary>
        /// <remarks> Fast and robust but slightly inaccurate. </remarks>
        Barycentric,
        /// <summary>
        /// Area is evaluated using Voronoi cell.
        /// </summary>
        /// <remarks> Might be inaccurate for very obtuse triangles, but more accurate that <see cref="Barycentric"/> in general. </remarks>
        VoronoiCell
    }
}
