using System;
using System.Collections.Generic;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Represents a closed succession of vertices (not necessarily planar) defining a face.
    /// </summary>
    public class Polygon
    {
        #region Fields

        /// <summary>
        /// List of vertices of the polyline.
        /// </summary>
        private List<Point> _vertices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        /// <param name="vertices"> Vertices for the polygon.</param>
        
        public Polygon(IList<Point> vertices)
        {
            _vertices = new List<Point>(vertices);
        }

        #endregion

        #region Public Static Methods

        //     -----     On Triangles     -----     //

        /// <summary>
        /// Computes the cosine of the angle between the lines (<paramref name="p"/>,<paramref name="q"/>) and (<paramref name="p"/>,<paramref name="r"/>)
        /// </summary>
        /// <param name="p"> Start point of the two lines. </param>
        /// <param name="q"> End point of the first line. </param>
        /// <param name="r"> End point of the second line. </param>
        /// <returns> The value of the cosine of the angle, in radians. </returns>
        public static double Cos(Point p, Point q, Point r)
        {
            var a = p.SquaredDistanceTo(q);
            var b = p.SquaredDistanceTo(r);
            var c = q.SquaredDistanceTo(r);

            return ((a + b - c) / (2 * Math.Sqrt(a * b)));
        }

        /// <summary>
        /// Computes the cotangent of the angle between the lines (<paramref name="p"/>,<paramref name="q"/>) and (<paramref name="p"/>,<paramref name="r"/>)
        /// </summary>
        /// <param name="p"> Start point of the two lines. </param>
        /// <param name="q"> End point of the first line. </param>
        /// <param name="r"> End point of the second line. </param>
        /// <returns> The value of the cotangent of the angle, in radians. </returns>
        public static double Cotan(Point p, Point q, Point r)
        {
            var a = q.SquaredDistanceTo(p);
            var b = p.SquaredDistanceTo(r);
            var c = q.SquaredDistanceTo(r);

            //We use the cosine approximation, as well as the identity cotan(acos(x))=x/sqrt(1-x^2)
            return ((a + b - c) / Math.Sqrt(4 * a * b - (a + b - c) * (a + b - c)));
        }


        /// <summary>
        /// Computes the angle between the lines (<paramref name="p"/>,<paramref name="q"/>) and (<paramref name="p"/>,<paramref name="r"/>)
        /// </summary>
        /// <param name="p"> Start point of the two lines. </param>
        /// <param name="q"> End point of the first line. </param>
        /// <param name="r"> End point of the second line. </param>
        /// <returns> The value of the angle, in radians. </returns>
        public static double Angle(Point p, Point q, Point r) => Math.Acos(Cos(p, q, r));


        /// <summary>
        /// Computes the area at the point <paramref name="p"/> in a triangle.
        /// </summary>
        /// <param name="p"> First point of the triangle, where the area is evaluated. </param>
        /// <param name="q"> Second point of the triangle. </param>
        /// <param name="r"> Third point of the triangle. </param>
        /// <param name="areaType"> Nature of the area to compute.</param>
        /// <returns> The value of tributary area of the point. </returns>
        /// <exception cref="NotImplementedException"> The nature of the area to compute is not implemented. </exception>
        public static double AreaAt(Point p, Point q, Point r, VertexAreaType areaType = VertexAreaType.VoronoiCell)
        {
            return areaType switch
            {
                VertexAreaType.Barycentric => BarycentricArea(p, q, r),
                VertexAreaType.VoronoiCell => VoronoiArea(p, q, r),
                _ => throw new NotImplementedException("The nature of the area to compute is not implemented."),
            };
        }


        /// <summary>
        /// Computes the area of a triangle delimited by the specified vertices.
        /// </summary>
        /// <param name="p"> First point of the triangle. </param>
        /// <param name="q"> Second point of the triangle. </param>
        /// <param name="r"> Third point of the triangle. </param>
        /// <returns> The area of the triangle. </returns>
        public static double Area(Point p, Point q, Point r)
        {
            // Use of Heron's formula for area of triangles
            double a_2 = q.SquaredDistanceTo(p);
            double b_2 = r.SquaredDistanceTo(q);
            double c_2 = p.SquaredDistanceTo(r);

            return 0.25 * Math.Sqrt(4 * (a_2 * b_2 + a_2 * c_2 + b_2 * c_2) - ((a_2 + b_2 + c_2) * (a_2 + b_2 + c_2)));
        }


        //     -----     On Polygons : At Vertex     -----     //

        /// <summary>
        /// Computes the cosine of the angle at a given vertex index in a polygon.
        /// </summary>
        /// <param name="vertices"> Vertices of the polygon to evaluate. </param>
        /// <param name="index"> Index of the vertex at which the cosine is evaluated. </param>
        /// <returns> The value of the cosine of the angle, in radians. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The index of the vertex must be positive and lower than the number of vertices. </exception>
        public static double Cos(IList<Point> vertices, int index)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The index of the vertex must be positive and lower than the number of vertices.");
            }

            int i = (index + vertices.Count - 1) % vertices.Count;
            // int j = index;
            int k = (index + 1) % vertices.Count;

            return Cos(vertices[index], vertices[k], vertices[i]);
        }

        /// <summary>
        /// Computes the cotangent of the angle at a given vertex index in a polygon.
        /// </summary>
        /// <remarks> Cotan weight is useful for Laplacian Smoothing and works at least for Euclidean and Minkowksi metrics. </remarks>
        /// <param name="vertices"> Vertices of the polygon to evaluate. </param>
        /// <param name="index"> Index of the vertex at which the cotangent is evaluated. </param>
        /// <returns> The value of the cotangent of the angle, in radians. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The index of the vertex must be positive and lower than the number of vertices. </exception>
        public static double Cotan(IList<Point> vertices, int index)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The index of the vertex must be positive and lower than the number of vertices.");
            }

            int i = (index + vertices.Count - 1) % vertices.Count;
            // int j = index;
            int k = (index + 1) % vertices.Count;

            return Cotan(vertices[index], vertices[k], vertices[i]);
        }


        /// <summary>
        /// Computes the angle at a given vertex index in a polygon.
        /// </summary>
        /// <param name="vertices"> Vertices of the polygon to evaluate. </param>
        /// <param name="index"> Index of the vertex at which the angle is evaluated. </param>
        /// <returns> The value of the angle, in radians. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The index of the vertex must be positive and lower than the number of vertices. </exception>
        public static double Angle(IList<Point> vertices, int index)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The index of the vertex must be positive and lower than the number of vertices.");
            }

            //We use the law of cosines
            if (index == 0)
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
        /// Computes the area at the given vertex index in a polygon.
        /// </summary>
        /// <param name="vertices"> Vertices of the polygon to evaluate. </param>
        /// <param name="index"> Index of the vertex at which the area is evaluated. </param>
        /// <param name="areaType"> Nature of the area to compute.</param>
        /// <returns> The value of tributary area of the vertex. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> The index of the vertex must be positive and lower than the number of vertices. </exception>
        public static double AreaAt(IList<Point> vertices, int index, VertexAreaType areaType = VertexAreaType.VoronoiCell)
        {
            if (index >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "The index of the vertex must be positive and lower than the number of vertices.");
            }

            double area = 0.0;

            int n = vertices.Count;
            for (int i = 1; i < n - 1; i++)
            {
                area += AreaAt(vertices[index], vertices[(index + i) % n], vertices[(index + i + 1) % n], areaType);
            }

            return area;
        }


        //     -----     On Polygons     -----     //

        /// <summary>
        /// Evaluates whether a polygon is obtuse, i.e. if it has one inner angle superior to 90°.
        /// </summary>
        /// <param name="vertices"> Vertices of the polygon to evaluate. </param>
        /// <returns> <see langword="true"/> if at least one inner angle is superior to 90°, <see langword="false"/> otherwise. </returns>
        public static bool IsObtuse(IList<Point> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (Angle(vertices, i) > Math.PI / 2.0) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Computes the area of a polygon delimited by the specified vertices.
        /// </summary>
        /// <remarks> 
        /// The fomula is algebraically exact for convex planar polygons, but may yield some inconsistencies for non-convex polygons.
        /// </remarks>
        /// <param name="vertices"> The vertices of the polygon to evaluate.</param>
        /// <returns> The area of the polygon. </returns>
        public static double Area(IList<Point> vertices)
        {
            double area = 0.0;

            double a_2 = 0.0;
            double b_2 = 0.0;
            double c_2 = 0.0;

            int nb_FaceVertex = vertices.Count;
            for (int i_Vertex = 1; i_Vertex < nb_FaceVertex - 1; i_Vertex++)
            {
                // Use of Heron's formula for area of triangles
                a_2 = vertices[i_Vertex].SquaredDistanceTo(vertices[0]);
                b_2 = vertices[i_Vertex + 1].SquaredDistanceTo(vertices[i_Vertex]);
                c_2 = vertices[0].SquaredDistanceTo(vertices[i_Vertex + 1]);

                area += 0.25 * Math.Sqrt(4 * (a_2 * b_2 + a_2 * c_2 + b_2 * c_2) - ((a_2 + b_2 + c_2) * (a_2 + b_2 + c_2)));
            }

            return area;
        }

        #endregion

        #region Other Static Methods

        //     -----     On Triangles     -----     //

        /// <summary>
        /// Computes the Voronoi area at the point <paramref name="p"/> in a triangle.
        /// </summary>
        /// <remarks> The Voronoi area is explained <a href="http://rodolphe-vaillant.fr/?e=20">here</a>. </remarks>
        /// <param name="p"> First point of the triangle, where the area is evaluated.</param>
        /// <param name="q"> Second point of the triangle. </param>
        /// <param name="r"> Third point of the triangle. </param>
        /// <returns> The value of tributary area of the point. </returns>
        private static double VoronoiArea(Point p, Point q, Point r)
        {
            double a = p.SquaredDistanceTo(r);
            double b = p.SquaredDistanceTo(q);
            double c = q.SquaredDistanceTo(r);

            double[] distances = new double[3] { a, b, c };
            Array.Sort(distances);

            bool isObtuse = (distances[0] + distances[1] < distances[2]);
            if (!isObtuse)
            {
                var xq = (c + b - a) / (2 * Math.Sqrt(c * b)); // The cosine of the angle at q
                var xr = (a + c - b) / (2 * Math.Sqrt(a * c)); // The cosine of the angle at r

                //We use the fact that cotan(acos(alpha))=alpha/sqrt(1-alpha^2)
                return (0.125 * (a * xq / Math.Sqrt(1 - xq * xq) + b * xq / Math.Sqrt(1 - xr * xr)));
            }
            else if (c > (a + b))
            {
                return (BarycentricArea(p, q, r) / 2.0);
            }
            else
            {
                return (BarycentricArea(p, q, r) / 4.0);
            }
        }

        /// <summary>
        /// Computes the barycentric area at the point <paramref name="p"/> in a triangle.
        /// </summary>
        /// <param name="p"> First point of the triangle, where the area is evaluated.</param>
        /// <param name="q"> Second point of the triangle. </param>
        /// <param name="r"> Third point of the triangle. </param>
        /// <returns> The value of tributary area of the point. </returns>
        private static double BarycentricArea(Point p, Point q, Point r)
        {
            double a_2 = p.SquaredDistanceTo(q);
            double b_2 = p.SquaredDistanceTo(r);
            double c_2 = q.SquaredDistanceTo(r);

            double area = (1.0 / 4.0) * Math.Sqrt(4 * (a_2 * b_2 + a_2 * c_2 + b_2 * c_2) - ((a_2 + b_2 + c_2) * (a_2 + b_2 + c_2)));

            return (area / 3.0);
        }

        #endregion

        #region Public Methods

        //     -----     On Vertices     -----    //

        /// <summary>
        /// Returns the vertex of the polyline at the specified index.
        /// </summary>
        /// <param name="index"> The index of the vertex to retrieve. </param>
        /// <returns> The vertex at the specified index. </returns>
        public Point GetVertex(int index) => _vertices[index];

        /// <summary>
        /// Returns the list of vertices of the polygon.
        /// </summary>
        /// <returns> The list of vertices of the polygon.</returns>
        public IReadOnlyList<Point> GetVertices() => _vertices;


        /// <summary>
        /// Adds a vertex at the end of the polyline.
        /// </summary>
        /// <param name="vertex"> Vertex to add.</param>
        public void AddVertex(Point vertex) => _vertices.Add(vertex);

        /// <summary>
        /// Adds a list of vertices at the end of the polyline.
        /// </summary>
        /// <param name="vertices"> List of vertices to add. </param>
        public void AddVertices(List<Point> vertices) => _vertices.AddRange(vertices);


        /// <summary>
        /// Inserts a vertex to the polyline at a given index.
        /// </summary>
        /// <param name="vertex"> Vertex to add. </param>
        /// <param name="index"> Index at which the vertex should be inserted. </param>
        public void InsertVertex(Point vertex, int index)
        {
            _vertices.Insert(index, vertex);
        }


        //     -----     On Polygons     -----     //

        /// <summary>
        /// Computes the cosine of the angle at a given vertex index in the polygon.
        /// </summary>
        /// <param name="index"> Index of the vertex at which the cosine is evaluated. </param>
        /// <returns> The value of the cosine of the angle, in radians. </returns>
        public double Cos(int index) => Cos(_vertices, index);

        /// <summary>
        /// Computes the cotangent of the angle at a given vertex index in the polygon.
        /// </summary>
        /// <param name="index"> Index of the vertex at which the cotangent is evaluated. </param>
        /// <returns> The value of the cotangent of the angle, in radians. </returns>
        public double Cotan(int index) => Cotan(_vertices, index);


        /// <summary>
        /// Computes the angle at a given vertex index in the polygon.
        /// </summary>
        /// <param name="index"> Index of the vertex at which the angle is evaluated. </param>
        /// <returns> The value of the angle, in radians. </returns>
        public double Angle(int index) => Angle(_vertices, index);

        /// <summary>
        /// Computes the area at the given vertex index in the polygon.
        /// </summary>
        /// <param name="index"> Index of the vertex at which the area is evaluated. </param>
        /// <param name="areaType"> Nature of the area to compute.</param>
        /// <returns> The value of tributary area of the vertex. </returns>
        public double AreaAt(int index, VertexAreaType areaType = VertexAreaType.VoronoiCell) => AreaAt(_vertices, index, areaType);


        /// <summary>
        /// Evaluates whether the polygon is obtuse, i.e. if it has one inner angle superior to 90°.
        /// </summary>
        /// <returns> <see langword="true"/> if at least one inner angle is superior to 90°, <see langword="false"/> otherwise. </returns>
        public bool IsObtuse() => IsObtuse(_vertices);

        /// <summary>
        /// Computes the area of the polygon.
        /// </summary>
        /// <returns> The value of the area. </returns>
        public double Area() => Area(_vertices);

        #endregion
    }
}
