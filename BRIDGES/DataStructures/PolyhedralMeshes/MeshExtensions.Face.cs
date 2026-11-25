using System;
using System.Collections.Generic;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public static partial class MeshExtensions
    {
        #region VTraits : IPositionTrait

        /// <summary>
        /// Computes the vector area of the face.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="face"> Face to operate on. </param>
        /// <returns> The vector whose direction is normal to the face, and whose length is the area of the face.</returns>
        public static Euc3D.Vector VectorArea<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Face face)
            where VTraits : VertexTraits.IPositionTrait
        {
            Euc3D.Vector vectorArea = new Euc3D.Vector();

            var faceVertices = face.FaceVertices();
            for (int i = 0; i < faceVertices.Count; i++)
            { 
                int j = (i + 1)%(faceVertices.Count);
                vectorArea += Euc3D.Vector.CrossProduct((Euc3D.Vector)faceVertices[i].Traits.Position, (Euc3D.Vector)faceVertices[j].Traits.Position);
            }
            vectorArea /= 2.0;

            return vectorArea;
        }

        /// <summary>
        /// Computes the vector area of the face.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="face"></param>
        /// <returns> The <see langword="double"/>-precision value of the area. </returns>
        public static double Area<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Face face)
            where VTraits : VertexTraits.IPositionTrait
        {
            return face.VectorArea().Length();
        }

        #endregion

        #region VTraits : IPositionTrait; FTraits : ITriangularTrait

        /// <summary>
        /// Computes the gradient of a scalar field 'u' over the triangular face.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="face"> Face on which the gradient is computed. </param>
        /// <param name="u"> Values of the scalar field for each face vertex. </param>
        /// <returns> The gradient vector of the scalar field. </returns>
        /// <exception cref="ArgumentException"> The number of scalar values must be equal to the number of vertices of the face. </exception>
        public static Euc3D.Vector Gradient<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Face face, IList<double> u)
            where VTraits : VertexTraits.IPositionTrait
            where FTraits : FaceTraits.ITriangularTrait
        {
            // Get the vertices in order to match the proper orientation
            var vertices = face.FaceVertices(); 

            if (vertices.Count != u.Count) { throw new ArgumentException(nameof(u), "The number of scalar values must be equal to the number of vertices of the face."); }

            Euc3D.Vector e0 = (Euc3D.Vector)(vertices[2].Traits.Position - vertices[1].Traits.Position);
            Euc3D.Vector e1 = (Euc3D.Vector)(vertices[0].Traits.Position - vertices[2].Traits.Position);
            Euc3D.Vector e2 = (Euc3D.Vector)(vertices[1].Traits.Position - vertices[0].Traits.Position);
            Euc3D.Vector n = -Euc3D.Vector.CrossProduct(e1, e0);
            n.Unitise();

            Euc3D.Vector X = u[0] * Euc3D.Vector.CrossProduct(n, e0);
            X += u[1] * Euc3D.Vector.CrossProduct(n, e1);
            X += u[2] * Euc3D.Vector.CrossProduct(n, e2);
            X /= 2.0 * face.Area();

            return X;
        }

        /// <summary>
        /// Computes the normalised gradient of a scalar field 'u' over the triangular face.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="face"> Face on which the gradient is computed. </param>
        /// <param name="u"> Values of the scalar field for each face vertex. </param>
        /// <returns> The gradient vector of the scalar field. </returns>
        /// <exception cref="ArgumentException"> The number of scalar values must be equal to the number of vertices of the face. </exception>
        public static Euc3D.Vector NormalisedGradient<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Face face, IList<double> u)
            where VTraits : VertexTraits.IPositionTrait
            where FTraits : FaceTraits.ITriangularTrait
        {
            // Get the vertices in order to match the proper orientation
            var vertices = face.FaceVertices();

            if (vertices.Count != u.Count) { throw new ArgumentException(nameof(u), "The number of scalar values must be equal to the number of vertices of the face."); }

            Euc3D.Vector e0 = (Euc3D.Vector)(vertices[2].Traits.Position - vertices[1].Traits.Position);
            Euc3D.Vector e1 = (Euc3D.Vector)(vertices[0].Traits.Position - vertices[2].Traits.Position);
            Euc3D.Vector e2 = (Euc3D.Vector)(vertices[1].Traits.Position - vertices[0].Traits.Position);
            Euc3D.Vector n = - Euc3D.Vector.CrossProduct(e1, e0);
            n.Unitise();

            Euc3D.Vector X = u[0] * Euc3D.Vector.CrossProduct(n, e0);
            X += u[1] * Euc3D.Vector.CrossProduct(n, e1);
            X += u[2] * Euc3D.Vector.CrossProduct(n, e2);
            X.Unitise();

            return X;
        }

        #endregion
    }
}
