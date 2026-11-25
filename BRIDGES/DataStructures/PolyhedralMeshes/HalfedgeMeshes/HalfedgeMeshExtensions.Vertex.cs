using System;
using System.Collections.Generic;

using Euc3D = BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    public static partial class HalfedgeMeshExtensions
    {
        /// <summary>
        /// Computes the divergence of a normalised vector field 'X'.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="HTraits"> Type defining the traits of the mesh halfedges. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="vertex"> Vertex on which the divergence is computed. </param>
        /// <param name="X"> Value of the vector field on each adjacent face of the vertex. </param>
        /// <returns> The divergence of the vector field. </returns>
        /// <exception cref="ArgumentException"> The number of vector values must be equal to the number of adjacent face of the vertex. </exception>
        public static double Divergence<VTraits, HTraits, ETraits, FTraits>(this HalfedgeMesh<VTraits, HTraits, ETraits, FTraits>.Vertex vertex, IList<Euc3D.Vector> X)
            where VTraits : VertexTraits.IPositionTrait
            where FTraits : FaceTraits.ITriangularTrait
        {
            IReadOnlyList<HalfedgeMesh<VTraits, HTraits, ETraits, FTraits>.Halfedge> outgoingHalfedges = vertex.OutgoingHalfedges();

            if (outgoingHalfedges.Count != X.Count) { throw new ArgumentException(nameof(X), "The number of vector values must be equal to the number of adjacent face of the vertex."); }

            var divX = 0.0;
            for (int i = 0; i < outgoingHalfedges.Count; i++)
            {
                HalfedgeMesh<VTraits, HTraits, ETraits, FTraits>.Halfedge outgoingHalfedge = outgoingHalfedges[i];

                if (outgoingHalfedge.AdjacentFace is not null)
                {
                    Euc3D.Point v1 = outgoingHalfedge.Start.Traits.Position;
                    Euc3D.Point v2 = outgoingHalfedge.Next.Start.Traits.Position;
                    Euc3D.Point v3 = outgoingHalfedge.Next.Next.Start.Traits.Position;

                    Euc3D.Vector e1 = new Euc3D.Vector(v1, v3);
                    Euc3D.Vector e2 = new Euc3D.Vector(v1, v2);

                    var face = outgoingHalfedge.AdjacentFace;
                    Euc3D.Vector faceX = X[face.Index];

                    double cotanTheta1 = Euc3D.Polygon.Cotan(v2, v3, v1);
                    double cotanTheta2 = Euc3D.Polygon.Cotan(v3, v1, v2);

                    divX += 0.5 * cotanTheta1 * Euc3D.Vector.DotProduct(e2, faceX);
                    divX += 0.5 * cotanTheta2 * Euc3D.Vector.DotProduct(e1, faceX);
                }
            }

            return divX;
        }
    }
}
