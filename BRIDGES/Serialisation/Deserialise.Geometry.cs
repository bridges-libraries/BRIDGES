using System;

using Json = System.Text.Json;


namespace BRIDGES.Serialisation
{
    public static partial class Deserialise
    {
        /// <summary>
        /// Deserialises a string representation to a <see cref="Geometry.Euclidean3D.Point"/>.
        /// </summary>
        /// <param name="text"> String representation to deserialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> The deserialised <see cref="Geometry.Euclidean3D.Point"/> from the string representation. </returns>
        /// <exception cref="NotImplementedException"></exception>
        [Deserialiser(typeof(Geometry.Euclidean3D.Point))]
        public static Geometry.Euclidean3D.Point Point(string text, Formats.ObjectFormat format)
        {
            if (format == Formats.ObjectFormat.Json)
            {
                using (Json.JsonDocument document = Json.JsonDocument.Parse(text))
                {
                    Json.JsonElement root = document.RootElement;

                    Json.JsonElement jCoordinates = root.GetProperty("Coordinates");

                    double[] coordinates = new double[3];

                    int i = 0;
                    foreach (Json.JsonElement jCoordinate in jCoordinates.EnumerateArray())
                    {
                        jCoordinate.TryGetDouble(out coordinates[i]);
                        i++;
                    }

                    return new Geometry.Euclidean3D.Point(coordinates);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
