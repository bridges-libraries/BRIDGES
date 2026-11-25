using System;
using System.Collections.Generic;

using Euc3D = BRIDGES.Geometry.Euclidean3D;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public static partial class MeshExtensions
    {
        #region VTraits : PositionTrait

        /// <summary>
        /// Computes the vector area of the face.
        /// </summary>
        /// <typeparam name="VTraits"> Type defining the traits of the mesh vertices. </typeparam>
        /// <typeparam name="ETraits"> Type defining the traits of the mesh edges. </typeparam>
        /// <typeparam name="FTraits"> Type defining the traits of the mesh faces. </typeparam>
        /// <param name="face"> Face to operate on. </param>
        /// <returns> The vector whose direction is normal to the face, and whose length is the area of the face.</returns>
        public static Euc3D.Vector VectorArea<VTraits, ETraits, FTraits>(this Mesh<VTraits, ETraits, FTraits>.Face face)
            where VTraits : VertexTraits.PositionTrait
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
            where VTraits : VertexTraits.PositionTrait
        {
            return face.VectorArea().Length();
        }

        #endregion
    }
}
