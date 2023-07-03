using System;


namespace BRIDGES.Serialisation
{
    public static partial class Serialise
    {
        #region Euclidean 3D

        /// <summary>
        /// Serialises a point into a given file format.
        /// </summary>
        /// <param name="point"> Point to serialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> A string representation of the point. </returns>
        [Serialiser(typeof(Geometry.Euclidean3D.Point))]
        public static string Point(Geometry.Euclidean3D.Point point, Formats.ObjectFormat format)
        {
            if (format == Formats.ObjectFormat.Json)
            {
                return String.Format("{0}\"Coordinates\":[{1},{2},{3}]{4}", "{", point.X, point.Y, point.Z, "}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
