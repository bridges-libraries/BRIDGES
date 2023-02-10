using System;
using System.Collections.Generic;
using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// A polygon structure defined as a closed succession of position (not necessarily planar).
    /// </summary>
    /// <typeparam name="TVector"> A spatial vector type.</typeparam>
    public class Polygon<TVector>        
    {
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon{TVector}"/> structure.
        /// </summary>
        /// <param name="vertices"> Vertices of the new polyhedron.</param>        
        public Polygon(List<TVector> vertices)
        {
            this._vertices = new List<TVector>(vertices);
        }

        #endregion

        #region Static Methods
        /******************** On Triangles ********************/
        /// <summary>
        /// Computes the angle (in radians) between the lines (<paramref name="p"/>,<paramref name="q"/>) and (<paramref name="p"/>,<paramref name="r"/>)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double Angle(TVector p, TVector q, TVector r)
        {
            var a = (p.Subtraction(q)).Length();
            var b = (p.Subtraction(r)).Length();
            var c = (q.Subtraction(r)).Length();
            return (Math.Acos((a * a + b * b - c * c) / (2 * a * b)));
        }
        /// <summary>
        /// Computes the cosine of the angle (in radians) between the lines (<paramref name="p"/>,<paramref name="q"/>) and (<paramref name="p"/>,<paramref name="r"/>)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double Cos(TVector p, TVector q, TVector r)
        {
            var a = (p.Subtraction(q)).Length();
            var b = (p.Subtraction(r)).Length();
            var c = (q.Subtraction(r)).Length();
            return ((a * a + b * b - c * c) / (2 * a * b));
        }
        /// <summary>
        /// Computes the Voronoi Area of three points, at point <paramref name="p"/>
        /// </summary>
        /// <param name="p">The point where area is evaluated.</param>
        /// <param name="q">The second point of the triangle.</param>
        /// <param name="r">The third point of the triangle.</param>
        /// <returns></returns>
        private static double VoronoiArea(TVector p, TVector q, TVector r)
        {
            bool isObtuse = (Angle(p, q, r) > Math.PI / 2.0) | ((Angle(q, r, p) > Math.PI / 2.0) | (Angle(r, p, q) > Math.PI / 2.0));
            if (!isObtuse)
            {
                var a = (p.Subtraction(r)).SquaredLength();
                var alphaA = Polygon<TVector>.Angle(q, p, r);
                var b = (p.Subtraction(q)).SquaredLength();
                var alphaB = Polygon<TVector>.Angle(r, p, q);
                return (1 / 8.0 * (a / Math.Tan(alphaA) + b / Math.Tan(alphaB)));
            }
            else if (Angle(p, q, r) > Math.PI / 2.0)
            {
                return (BarycentricArea(p, q, r) / 2.0);
            }
            else
            {
                return (BarycentricArea(p, q, r) / 4.0);
            }
        }
        /// <summary>
        /// Computes the barycentric area of three points, at point <paramref name="p"/>
        /// </summary>
        /// <param name="p">The point where area is evaluated.</param>
        /// <param name="q">The second point of the triangle.</param>
        /// <param name="r">The third point of the triangle.</param>
        /// <returns></returns>
        private static double BarycentricArea(TVector p, TVector q, TVector r)
        {
            double a_2 = (p.Subtraction(q)).SquaredLength();
            double b_2 = (p.Subtraction(r)).SquaredLength();
            double c_2 = (q.Subtraction(r)).SquaredLength();
            double area = (1.0 / 4.0) * Math.Sqrt(4 * (a_2 * b_2 + a_2 * c_2 + b_2 * c_2) - ((a_2 + b_2 + c_2) * (a_2 + b_2 + c_2)));
            return (area / 3.0);
        }
        /// <summary>
        /// Computes the area of a triangle at the point <paramref name="p"/> according to <paramref name="discreteAreaSettings"/> for the solution
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <param name="discreteAreaSettings">Optional sttings for computing area, solution settings are used by default.</param>
        /// <returns></returns>
        public static double Area(TVector p, TVector q, TVector r, Settings.DiscreteArea discreteAreaSettings = Settings._areaSettings)
        {
            switch (discreteAreaSettings)
            {
                case (ENPC.Settings.DiscreteArea.Barycentric):
                    {
                        return (BarycentricArea(p, q, r));
                    }
                case (ENPC.Settings.DiscreteArea.VoronoiCell):
                    {
                        return (VoronoiArea(p, q, r));
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }
        /******************** On Conversions ********************/

        /// <summary>
        /// Computes the area of the polyhedron delimited by the specified vertices.
        /// </summary>
        /// <param name="vertices"> The vertices of the polygon to evaluate.</param>
        /// <returns> The area of the polygon.</returns>
        /// <remarks>  The fomula is algebraically exact for convex planar polygons, but may yield some inconsistencies for non-convex polygons.</remarks>
        public static double Area(List<TVector> vertices)
        {
            double area = 0.0;

            int nb_FaceVertex = vertices.Count;
            for (int i_Vertex = 1; i_Vertex < nb_FaceVertex - 1; i_Vertex++)
            {
                // Use of Heron's formula for area of triangles (Seems valid in any dimension)
                double a_2 = (vertices[i_Vertex].Subtraction(vertices[0])).SquaredLength();
                double b_2 = (vertices[i_Vertex + 1].Subtraction(vertices[i_Vertex])).SquaredLength();
                double c_2 = (vertices[0].Subtraction(vertices[i_Vertex + 1])).SquaredLength();
                area += (1.0 / 4.0) * Math.Sqrt(4 * (a_2 * b_2 + a_2 * c_2 + b_2 * c_2) - ((a_2 + b_2 + c_2) * (a_2 + b_2 + c_2)));
            }

            return area;
        }
        /// <summary>
        /// Computes the area of a polygon at a given <paramref name="index"/>.
        /// </summary>
        /// <param name="vertices">The list of vertices to evaluate.</param>
        /// <param name="index"> The index of the vertex of which we want the tributary area in the polygon.</param>
        /// <param name="discreteAreaSettings">Settings for computing area.</param>
        /// <returns>The tributary area of vertex <paramref name="index"/></returns>
        public static double AreaAt(List<TVector> vertices, int index, Settings.DiscreteArea discreteAreaSettings = Settings._areaSettings)
        {
            double area = 0.0;

            int n = vertices.Count;
            for (int i = 1; i < n - 1; i++)
            {
                area += Area(vertices[index], vertices[(index + i) % n], vertices[(index + i + 1) % n], discreteAreaSettings);
            }
            return (area);
        }
        /******************** On Conversions ********************/
        /// <summary>
        /// Computes an angle (in radians) at a given vertex index.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon to evaluate.</param>
        /// <param name="index">The index to evaluate at.</param>
        /// <returns>The angle (in radians).</returns>
        public static double Angle(List<TVector> vertices, int index)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException("index", "the index to look for should be inferior to the list length");
            }
            //We use the law of cosines
            else if (index == 0)
            {
                return (Angle(vertices[0], vertices[1], vertices[vertices.Count - 1]));
            }
            else if (index == (vertices.Count - 1))
            {
                return (Angle(vertices[vertices.Count - 1], vertices[vertices.Count - 2], vertices[0]));
            }
            else
            {
                return (Angle(vertices[index], vertices[index - 1], vertices[index + 1]));
            }
        }
        /// <summary>
        /// Computes the cotangent of the angle at a given vertex position.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon to evaluate.</param>
        /// <param name="index"></param>
        /// <returns> The cotan of the angle</returns>
        /// <remarks> Cotan weight is useful for Laplacian Smoothing and works at least for Euclidean and Minkowksi metrics.</remarks>
        public static double Cotan(List<TVector> vertices, int index)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException("index", "the index to look for should be inferior to the list length");
            }
            //We use the cosine approximation, as well as the identity cotan(acos(x))=x/sqrt(1-x^2)
            else if (index == 0)
            {
                var a = (vertices[1].Subtraction(vertices[0])).SquaredLength();
                var b = (vertices[0].Subtraction(vertices[vertices.Count - 1])).SquaredLength();
                var c = (vertices[1].Subtraction(vertices[vertices.Count - 1])).SquaredLength();
                return ((a + b - c) / Math.Sqrt(4 * a * b - (a + b - c) * (a + b - c)));

            }
            else if (index == (vertices.Count - 1))
            {
                var a = (vertices[vertices.Count - 1].Subtraction(vertices[vertices.Count - 2])).SquaredLength();
                var b = (vertices[0].Subtraction(vertices[vertices.Count - 1])).SquaredLength();
                var c = (vertices[0].Subtraction(vertices[vertices.Count - 2])).SquaredLength();
                return ((a + b - c) / Math.Sqrt(4 * a * b - (a + b - c) * (a + b - c)));
            }
            else
            {
                var a = (vertices[index + 1].Subtraction(vertices[index])).SquaredLength();
                var b = (vertices[index].Subtraction(vertices[index - 1])).SquaredLength();
                var c = (vertices[index + 1].Subtraction(vertices[index - 1])).SquaredLength();
                return ((a + b - c) / Math.Sqrt(4 * a * b - (a + b - c) * (a + b - c)));
            }
        }
        /// <summary>
        /// Computes whether a polygon is obtuse, i.e. if it has one inner angle superior to 90°.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns>A boolean </returns>
        public static bool IsObtuse(List<TVector> vertices)
        {
            bool result = false;
            for (int i = 0; i < vertices.Count; i++)
            {
                result |= (Angle(vertices, i) > Math.PI / 2.0);
            }
            return (result);
        }
        /// <summary>
        /// Cotan of the angle at <paramref name="Va"/> in the triangle <paramref name="Va"/>,<paramref name="Vb"/>,<paramref name="Vc"/>
        /// </summary>
        /// <param name="Va"></param>
        /// <param name="Vb"></param>
        /// <param name="Vc"></param>
        /// <returns></returns>
        public static double Cotan(TVector Va, TVector Vb, TVector Vc)
        {
            var a = (Vb.Subtraction(Va)).SquaredLength();
            var b = (Va.Subtraction(Vc)).SquaredLength();
            var c = (Vb.Subtraction(Vc)).SquaredLength();
            return ((a + b - c) / Math.Sqrt(4 * a * b - (a + b - c) * (a + b - c)));
        }
        /******************** On Conversions ********************/

        /// <summary>
        /// Converts a <see cref="Polyline{TVector}"/> to a <see cref="Polygon{TVector}"/>.
        /// </summary>
        /// <param name="polyline"> The polyline to convert.</param>
        /// <exception cref="ArgumentException"> The specified polyline should be closed to be converted to a polyhedron.</exception>
        public static explicit operator Polygon<TVector>(Polyline<TVector> polyline)
        {
            if (!polyline.IsClosed)
            {
                throw new ArgumentException("The specified polyline should be closed to be converted to a polyhedron.");
            }
            return new Polygon<TVector>(polyline.GetVertices());
        }

        #endregion

        #region Methods

        /******************** On this Polygon ********************/

        /// <summary>
        /// Computes the area of the polygon.
        /// </summary>
        /// <returns> The area of the polygon.</returns>
        public double Area()
        {
            return Polygon<TVector>.Area(_vertices);
        }
        /// <summary>
        /// Computes the angle (in radians) at a given vertex index
        /// </summary>
        /// <param name="index">The index to evaluate at</param>
        /// <returns>The angle (in radians)</returns>
        public double Angle(int index)
        {
            return Polygon<TVector>.Angle(_vertices, index);
        }
        /// <summary>
        /// Returns the cotangent at a given vertex
        /// </summary>
        /// <param name="index">The index to evaluate at.</param>
        /// <returns>The cotangent of the angle.</returns>
        public double Cotan(int index)
        {
            return Polygon<TVector>.Cotan(_vertices, index);
        }
        /// <summary>
        /// Computes whether a polygon is obtuse, i.e. if it has an inner angle superior to 90°.
        /// </summary>
        /// <returns></returns>
        public bool IsObtuse()
        {
            return IsObtuse(_vertices);
        }
        /// <summary>
        /// Evaluates the tributary area of the point in the polygon.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="discreteAreaSettings">Settings for computing area.</param>
        /// <returns>The tributary area of a point in the polygon.</returns>
        public double AreaAt(int index, Settings.DiscreteArea discreteAreaSettings = Settings._areaSettings)
        {
            return (AreaAt(_vertices, index, discreteAreaSettings));
        }
        /******************** On Vertices ********************/

        /// <summary>
        /// Returns the list of vertices of the polyline.
        /// </summary>
        /// <returns> The list of vertices of the polyline.</returns>
        public List<TVector> GetVertices()
        {
            return new List<TVector>(_vertices);
        }

        /// <summary>
        /// Returns the vertex of the polyline at the specified index.
        /// </summary>
        /// <param name="index"> The index of the vertex to retrieve.</param>
        /// <returns> The vertex at the specified index.</returns>
        public TVector GetVertex(int index)
        {
            return _vertices[index];
        }

        /// <summary>
        /// Adds a vertex at the end of the polyline
        /// </summary>
        /// <param name="vertex"> Vertex to add.</param>
        public void AddVertex(TVector vertex)
        {
            _vertices.Add((TVector)vertex.DeepCopy());
        }
        /// <summary>
        /// Adds a list of vertices at the end of the polyline
        /// </summary>
        /// <param name="vertices"> List of vertices to add.</param>
        public void AddVertices(List<TVector> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)

                this._vertices.Add((TVector)vertices[i].DeepCopy());
        }
        /// <summary>
        /// Adds a vertex to the polyline at a given index.
        /// </summary>
        /// <param name="vertex"> Vertex to add.</param>
        /// <param name="index"> Index of the vertex to add.</param>
        public void InsertVertex(TVector vertex, int index)
        {
            _vertices.Insert(index, vertex);
        }

        #endregion

    }
}
