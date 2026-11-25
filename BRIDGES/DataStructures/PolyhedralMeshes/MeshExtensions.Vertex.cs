using System;
using System.Collections.Generic;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public static partial class MeshExtensions
    {
        #region VTraits : PositionTrait

        /// <summary>
        /// Computes the influence area of the vertex, according to the expected area type.
        /// </summary>
        /// <param name="vertex"> Vertex whose area to compute. </param>
        /// <param name="areaType"> Nature of the vertex area to compute. </param>
        /// <returns> The <see langword="double"/>-precision value of the area. </returns>
        /// <exception cref="NotImplementedException"> The type of vertex area provided is not implemented. </exception>
        public static double InfluenceArea<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Vertex vertex, Euc3D.VertexAreaType areaType = Euc3D.VertexAreaType.VoronoiCell)
            where VTraits : VertexTraits.IPositionTrait
        {
            return areaType switch
            {
                Euc3D.VertexAreaType.Barycentric => InfluenceArea_Barycentric(vertex),
                Euc3D.VertexAreaType.VoronoiCell => InfluenceArea_VoronoiCell(vertex),
                _ => throw new NotImplementedException("The type of vertex area provided is not implemented."),
            };
        }


        private static double InfluenceArea_Barycentric<VTraits, ETraits, FTraits>(Mesh<VTraits, ETraits, FTraits>.Vertex vertex)
            where VTraits : VertexTraits.IPositionTrait
        {
            double result = 0.0;
            foreach (Mesh<VTraits, ETraits, FTraits>.Face adjacentFace in vertex.AdjacentFaces())
            {
                result += adjacentFace.Area() / (adjacentFace.FaceVertices().Count);
            }
            return result;
        }

        private static double InfluenceArea_VoronoiCell<VTraits, ETraits, FTraits>(Mesh<VTraits, ETraits, FTraits>.Vertex vertex)
            where VTraits : VertexTraits.IPositionTrait
        {
            double result = 0.0;
            foreach (Mesh<VTraits, ETraits, FTraits>.Face face in vertex.AdjacentFaces())
            {
                List<Euc3D.Point> faceVertexPosition = new List<Euc3D.Point>();
                foreach (Mesh<VTraits, ETraits, FTraits>.Vertex faceVertex in face.FaceVertices())
                {
                    faceVertexPosition.Add(faceVertex.Traits.Position);
                }

                int inFaceIndex = faceVertexPosition.IndexOf(vertex.Traits.Position);

                result += Euc3D.Polygon.AreaAt(faceVertexPosition, inFaceIndex, Euc3D.VertexAreaType.VoronoiCell);
            }

            return (result);
        }

        #endregion
    }
}
