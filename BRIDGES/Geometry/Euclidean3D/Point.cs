using System;

using Alg_Fund = BRIDGES.Algebra.Fundamentals;
using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a point in three-dimensional euclidean space.
    /// </summary>
    [Serialisation.Serialisable]
    public struct Point :
          IEquatable<Point>,
          Alg_Fund.IAddable<Point> /* To Do : Remove */, 
          Alg_Meas.IDotProduct<Point, double>,
          Alg_Sets.IGroupAction<Point, double>,
          Geo_Ker.IAnalytic<double>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first coordinate of this <see cref="Point"/>.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the second coordinate of this <see cref="Point"/>.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the third coordinate of this <see cref="Point"/>.
        /// </summary>
        public double Z { get; set; }


        /// <summary>
        /// Gets or sets the value of the coordinate at the given index.
        /// </summary>
        /// <param name="index"> Index of the coordinate to get or set. </param>
        /// <returns> The value of the coordinate at the given index. </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The given index is lower than zero or exceeds the number of coordinates of the point.
        /// For a three-dimensional euclidean point, the index can range from zero to two.
        /// </exception>
        public double this[int index]
        {
            get
            {
                if (index == 0) { return X; }
                else if (index == 1) { return Y; }
                else if (index == 2) { return Z; }
                else
                {
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the point." +
                        "For a three-dimensional euclidean point, the index can range from zero to two.");
                }
            }

            set
            {
                if (index == 0) { X = value; }
                else if (index == 1) { Y = value; }
                else if (index == 2) { Z = value; }
                else
                {
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the point." +
                        "For a three-dimensional euclidean point, the index can range from zero to two.");
                }
            }
        }

        /// <summary>
        /// Gets the dimension of this <see cref="Point"/>'s euclidean space.
        /// </summary>
        public int Dimension => 3;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure by defining its three coordinates.
        /// </summary>
        /// <param name="x"> Value of the first coordinate. </param>
        /// <param name="y"> Value of the second coordinate. </param>
        /// <param name="z"> Value of the third coordinate. </param>
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Point"/> structure by defining its coordinates.
        /// </summary>
        /// <param name="coordinates"> Value of the coordinates. </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the coordinate array is different from three, the dimension of points in three-dimensional euclidean space. 
        /// </exception>
        public Point(double[] coordinates)
        {
            // Verifications
            if (coordinates.Length != 3) { throw new ArgumentOutOfRangeException("The length of the coordinate array is different from three," +
                "the dimension of points in three-dimensional euclidean space."); }

            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure from another <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to deep copy. </param>
        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Point"/> with coordinates <c>(0.0, 0.0, 0.0)</c>.
        /// </summary>
        public static Point Zero => new Point(0d, 0d, 0d);

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the cross product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Point"/> for the cross product.</param>
        /// <param name="right"> Right <see cref="Point"/> for the cross product.</param>
        /// <returns> A new <see cref="Point"/> resulting from the cross product of two <see cref="Point"/>.</returns>
        public static Point CrossProduct(Point left, Point right) =>
            new Point((left.Y * right.Z) - (left.Z * right.Y),
            (left.Z * right.X) - (left.X * right.Z),
            (left.X * right.Y) - (left.Y * right.X));


        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(Point, Point)"/>
        public static Point Add(Point left, Point right) => left + right;

        /// <inheritdoc cref="operator -(Point, Point)"/>
        public static Point Subtract(Point left, Point right) => left - right;


        /******************** Embedding : Vector ********************/

        /// <inheritdoc cref="operator +(Point, Vector)"/>
        public static Point Add(Point left, Vector right) => left + right;

        /// <summary>
        /// Computes left the addition of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> <see cref="Point"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition. </returns>
        public static Point Add(Vector left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }


        /// <inheritdoc cref="operator -(Point, Vector)"/>
        public static Point Subtract(Point left, Vector right) => left - right;

        /// <summary>
        /// Computes the left subtraction of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> <see cref="Point"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point Subtract(Vector left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="Point.operator *(Point, double)"/>
        public static Point Multiply(Point operand, double factor) => operand * factor;
        /// <inheritdoc cref="Point.operator *(double, Point)"/>
        public static Point Multiply(double factor, Point operand) => factor * operand;

        /// <inheritdoc cref="Point.operator /(Point, double)"/>
        public static Point Divide(Point operand, double divisor) => operand / divisor;


        /******************** Hilbert Space ********************/

        /// <inheritdoc cref="operator *(Point, Point)"/>
        public static double DotProduct(Point left, Point right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the addition. </param>
        /// <param name="right"> <see cref="Point"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition. </returns>
        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> to subtract. </param>
        /// <param name="right"> <see cref="Point"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point operator -(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /// <summary>
        /// Computes the opposite of the given <see cref="Point"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to be opposed. </param>
        /// <returns> The new <see cref="Point"/>, opposite of the initial one. </returns>
        public static Point operator -(Point operand)
        {
            return new Point(-operand.X, -operand.Y, -operand.Z);
        }


        /******************** Vector Embedding ********************/

        /// <summary>
        /// Computes right the addition of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the addition. </param>
        /// <param name="right"> <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="Point"/> resulting from the addition. </returns>
        public static Point operator +(Point left, Vector right)
        {
            return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="Point"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> to subtract. </param>
        /// <param name="right"> <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the subtraction. </returns>
        public static Point operator -(Point left, Vector right)
        {
            return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Point"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to multiply. </param>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar multiplication. </returns>
        public static Point operator *(Point operand, double factor)
        {
            return new Point(operand.X * factor, operand.Y * factor, operand.Z * factor);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Point"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <param name="operand"> <see cref="Point"/> to multiply. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar multiplication. </returns>
        public static Point operator *(double factor, Point operand)
        {
            return new Point(factor * operand.X, factor * operand.Y, factor * operand.Z);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Point"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Point"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Point"/> resulting from the scalar division. </returns>
        public static Point operator /(Point operand, double divisor)
        {
            return new Point(operand.X / divisor, operand.Y / divisor, operand.Z / divisor);
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Computes the dot product of two <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the dot product. </param>
        /// <param name="right"> <see cref="Point"/> for the dot product. </param>
        /// <returns> The new <see cref="Point"/> resulting from the dot product of two <see cref="Point"/>. </returns>
        public static double operator *(Point left, Point right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Vector"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="vector"> <see cref="Vector"/> to convert. </param>
        /// <returns> The new <see cref="Point"/> resulting from the conversion. </returns>
        public static implicit operator Point(Vector vector) => new Point(vector.X, vector.Y, vector.Z);

        /// <summary>
        /// Converts a <see cref="Projective3D.Point"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Projective3D.Point"/> to convert. </param>
        /// <returns> The new <see cref="Point"/> resulting from the conversion. </returns>
        /// <exception cref="InvalidCastException"> The projective point cannot be converted. It represents an euclidean point at infinity. </exception>
        public static explicit operator Point(Projective3D.Point point)
        {
            if (point[3] == 0d)
            {
                throw new InvalidCastException("The projective point cannot be converted because. It represents an euclidean point at infinity.");
            }

            return new Point(point[0] / point[3], point[1] / point[3], point[2] / point[3]);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the coordinates of this <see cref="Point"/>.
        /// </summary>
        /// <returns> The array representation of the <see cref="Point"/>'s coordinates. </returns>
        public double[] GetCoordinates() => new double[3] { X, Y, Z };


        /// <summary>
    /// Computes the distance of this <see cref="Point"/> to another <see cref="Point"/> (using the L2-norm).
    /// </summary>
    /// <param name="other"> <see cref="Point"/> to evaluate the distance to. </param>
    /// <returns> The value of the distance between the two <see cref="Point"/>. </returns>
        public double DistanceTo(Point other) => (this - other).Norm();

        /// <summary>
        /// Computes the L2-norm this <see cref="Point"/>.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double Norm() => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Computes the L2-norm this <see cref="Point"/>.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        public double SquaredNorm() =>(X * X) + (Y * Y) + (Z * Z);


        /// <summary>
        /// Evaluates whether this <see cref="Point"/> is equal to another <see cref="Point"/>, memberwise.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Point"/> are equal if their coordinates are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Point"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Point"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Point other)
        {
            return Math.Abs(this.X - other.X) < Settings.AbsolutePrecision
                    & Math.Abs(this.Y - other.Y) < Settings.AbsolutePrecision
                    & Math.Abs(this.Z - other.Z) < Settings.AbsolutePrecision;
        }

        #endregion


        #region Override : Object

        /******************** object ********************/

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(point);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IAddable<Point> ********************/

        /// <inheritdoc/>
        Point Alg_Fund.IAddable<Point>.Add(Point right) => this + right;


        /******************** IDotProduct<Point, double> ********************/

        /// <inheritdoc/>
        double Alg_Meas.IDotProduct<Point, double>.DotProduct(Point other) => Point.DotProduct(this, other);


        /******************** IGroupAction<Point, double> ********************/

        /// <inheritdoc/>
        Point Alg_Sets.IGroupAction<Point, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Point Alg_Sets.IGroupAction<Point, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}