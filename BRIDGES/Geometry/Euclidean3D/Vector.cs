using System;

using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;

using Geo_Ker = BRIDGES.Geometry.Kernel;


namespace BRIDGES.Geometry.Euclidean3D
{
    /// <summary>
    /// Structure defining a vector in three-dimensional euclidean space.
    /// </summary>
    public struct Vector :
          IEquatable<Vector>,
          Alg_Meas.IDotProduct<Vector, double>,
          Alg_Sets.IGroupAction<Vector, double>,
          Geo_Ker.IAnalytic<double>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first coordinate of this <see cref="Vector"/>.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the second coordinate of this <see cref="Vector"/>.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the third coordinate of this <see cref="Vector"/>.
        /// </summary>
        public double Z { get; set; }


        /// <summary>
        /// Gets or sets the value of the coordinate at the given index.
        /// </summary>
        /// <param name="index"> Index of the coordinate to get or set. </param>
        /// <returns> The value of the coordinate at the given index. </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The given index is lower than zero or exceeds the number of coordinates of the vector.
        /// For a three-dimensional euclidean vector, the index can range from zero to two.
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
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the vector." +
                    "For a three-dimensional euclidean vector, the index can range from zero to two.");
                }
            }

            set
            {
                if (index == 0) { X = value; }
                else if (index == 1) { Y = value; }
                else if (index == 2) { Z = value; }
                else
                {
                    throw new ArgumentOutOfRangeException("The given index is lower than zero or exceeds the number of coordinates of the vector." +
                        "For a three-dimensional euclidean vector, the index can range from zero to two.");
                }
            }
        }

        /// <summary>
        /// Gets the dimension of this <see cref="Vector"/>'s euclidean space.
        /// </summary>
        public int Dimension { get { return 3; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector"/> structure by defining its three coordinates.
        /// </summary>
        /// <param name="x"> Value of the first coordinate. </param>
        /// <param name="y"> Value of the second coordinate. </param>
        /// <param name="z"> Value of the third coordinate. </param>
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Vector"/> structure by defining its coordinates.
        /// </summary>
        /// <param name="coordinates"> Value of the coordinates. </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The length of the coordinate array is different from three, the dimension of vectors in three-dimensional euclidean space.
        /// </exception>
        public Vector(double[] coordinates)
        {
            // Verifications
            if (coordinates.Length != 3)
            {
                throw new ArgumentOutOfRangeException("The length of the coordinate array is different from three," +
                "the dimension of vectors in three-dimensional euclidean space.");
            }

            X = coordinates[0];
            Y = coordinates[1];
            Z = coordinates[2];
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Vector"/> structure from two <see cref="Point"/>.
        /// </summary>
        /// <param name="start"> <see cref="Point"/> at start. </param>
        /// <param name="end"> <see cref="Point"/> at end. </param>
        public Vector(Point start, Point end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
            Z = end.Z - start.Z;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector"/> structure from another <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="Vector"/> to deep copy. </param>
        public Vector(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new <see cref="Vector"/> with coordinates <c>(0.0, 0.0, 0.0)</c>.
        /// </summary>
        public static Vector Zero => new Vector(0d, 0d, 0d);


        /// <summary>
        /// Gets a new <see cref="Vector"/> with coordinates <c>(1.0, 0.0, 0.0)</c>.
        /// </summary>
        public static Vector WorldX => new Vector(1d, 0d, 0d);

        /// <summary>
        /// Gets a new <see cref="Vector"/> with coordinates <c>(0.0, 1.0, 0.0)</c>.
        /// </summary>
        public static Vector WorldY => new Vector(0d, 1d, 0d);

        /// <summary>
        /// Gets a new <see cref="Vector"/> with coordinates <c>(0.0, 0.0, 1.0)</c>.
        /// </summary>
        public static Vector WorldZ => new Vector(0d, 0d, 1d);

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the cross product of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector"/> for the cross product.</param>
        /// <param name="right"> Right <see cref="Vector"/> for the cross product.</param>
        /// <returns> A new <see cref="Vector"/> resulting from the cross product of two <see cref="Vector"/>.</returns>
        public static Vector CrossProduct(Vector left, Vector right) =>
            new Vector((left.Y * right.Z) - (left.Z * right.Y),
            (left.Z * right.X) - (left.X * right.Z),
            (left.X * right.Y) - (left.Y * right.X));


        /// <summary>
        /// Evaluates whether two <see cref="Vector"/> are parallel.
        /// </summary>
        /// <param name="vectorA"> <see cref="Vector"/> for the comparison. </param>
        /// <param name="vectorB"> <see cref="Vector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Vector"/> are parallel, <see langword="false"/> otherwise. </returns>
        public static bool AreParallel(Vector vectorA, Vector vectorB)
        {
            return (vectorA.Length() * vectorB.Length()) - Math.Abs(Vector.DotProduct(vectorA, vectorB)) < Settings.AbsolutePrecision;
        }


        /// <summary>
        /// Evaluates whether two <see cref="Vector"/> are orthogonal.
        /// </summary>
        /// <param name="vectorA"> <see cref="Vector"/> for the comparison. </param>
        /// <param name="vectorB"> <see cref="Vector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Vector"/> are orthogonal, <see langword="false"/> otherwise. </returns>
        public static bool AreOrthogonal(Vector vectorA, Vector vectorB)
        {
            return Math.Abs(Vector.DotProduct(vectorA, vectorB)) < Settings.AbsolutePrecision;
        }

        /// <summary>
        /// Evaluates whether a collection of <see cref="Vector"/> are orthogonal to one another.
        /// </summary>
        /// <param name="vectors"> Collection of <see cref="Vector"/> to evaluate.</param>
        /// <returns> <see langword="true"/> if the <see cref="Vector"/> are orthogonal, <see langword="false"/> otherwise.</returns>
        public static bool AreOrthogonal(params Vector[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i].SquaredLength() < Settings.AbsolutePrecision) { return false; }

                for (int j = i + 1; j < vectors.Length; j++)
                {
                    if (Math.Abs(Vector.DotProduct(vectors[i], vectors[j])) < Settings.AbsolutePrecision) { return false; }
                }
            }

            return true;
        }


        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="Vector.operator +(Vector, Vector)"/>
        public static Vector Add(Vector left, Vector right) => left + right;

        /// <inheritdoc cref="Vector.operator -(Vector, Vector)"/>
        public static Vector Subtract(Vector left, Vector right) => left - right;


        /******************** Embedding : Point ********************/

        /// <inheritdoc cref="Vector.operator +(Vector, Point)"/>
        public static Vector Add(Vector left, Point right) => left + right;

        /// <summary>
        /// Computes left the addition of a <see cref="Vector"/> with a <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> for the addition. </param>
        /// <param name="right"> <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the addition. </returns>
        public static Vector Add(Point left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }


        /// <inheritdoc cref="Vector.operator -(Vector, Point)"/>
        public static Vector Subtract(Vector left, Point right) => left - right;

        /// <summary>
        /// Computes left the subtraction of a <see cref="Vector"/> with a <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Point"/> to subtract. </param>
        /// <param name="right"> <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the subtraction. </returns>
        public static Vector Subtract(Point left, Vector right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="Vector.operator *(Vector, double)"/>
        public static Vector Multiply(Vector operand, double factor) => operand * factor;

        /// <inheritdoc cref="Vector.operator *(double, Vector)"/>
        public static Vector Multiply(double factor, Vector operand) => factor * operand;


        /// <inheritdoc cref="Vector.operator /(Vector, double)"/>
        public static Vector Divide(Vector operand, double divisor) => operand / divisor;


        /******************** Hilbert Space ********************/

        /// <inheritdoc cref="Vector.operator *(Vector, Vector)"/>
        public static double DotProduct(Vector left, Vector right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }


        /// <summary>
        /// Computes the angle between two <see cref="Vector"/>.
        /// </summary>
        /// <param name="vectorA"> <see cref="Vector"/> for the angle evaluation. </param>
        /// <param name="vectorB"> <see cref="Vector"/> for the angle evaluation. </param>
        /// <returns> The value of the angle between the two <see cref="Vector"/> (in radians). </returns>
        public static double AngleBetween(Vector vectorA, Vector vectorB)
        {
            double dotProduct = (vectorA.X * vectorB.X) + (vectorA.Y * vectorB.Y) + (vectorA.Z * vectorB.Z);
            double length = Math.Sqrt((vectorA.X * vectorA.X) + (vectorA.Y * vectorA.Y) + (vectorA.Z * vectorA.Z));
            double otherLength = Math.Sqrt((vectorB.X * vectorB.X) + (vectorB.Y * vectorB.Y) + (vectorB.Z * vectorB.Z));

            return Math.Acos(dotProduct / (length * otherLength));
        }

        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the addition. </returns>
        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the subtraction. </returns>
        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /// <summary>
        /// Computes the opposite of the given <see cref="Vector"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to be opposed. </param>
        /// <returns> The new <see cref="Vector"/>, opposite of the initial one. </returns>
        public static Vector operator -(Vector operand)
        {
            return new Vector(-operand.X, -operand.Y, -operand.Z);
        }


        /******************** Embedding : Point ********************/

        /// <summary>
        /// Computes right the addition of a <see cref="Vector"/> with a <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> for the addition. </param>
        /// <param name="right"> <see cref="Point"/> for the addition. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the addition. </returns>
        public static Vector operator +(Vector left, Point right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Computes right the subtraction of a <see cref="Vector"/> with a <see cref="Point"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> to subtract. </param>
        /// <param name="right"> <see cref="Point"/> to subtract with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the subtraction. </returns>
        public static Vector operator -(Vector left, Point right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to multiply. </param>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar multiplication. </returns>
        public static Vector operator *(Vector operand, double factor)
        {
            return new Vector(operand.X * factor, operand.Y * factor, operand.Z * factor);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Vector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number. </param>
        /// <param name="operand"> <see cref="Vector"/> to multiply. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar multiplication. </returns>
        public static Vector operator *(double factor, Vector operand)
        {
            return new Vector(factor * operand.X, factor * operand.Y, factor * operand.Z);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Vector"/> with a <see cref="double"/>-precision real number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/>-precision real number to divide with. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the scalar division. </returns>
        public static Vector operator /(Vector operand, double divisor)
        {
            return new Vector(operand.X / divisor, operand.Y / divisor, operand.Z / divisor);
        }


        /******************** Hilbert Space ********************/

        /// <summary>
        /// Computes the dot product of two <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> <see cref="Vector"/> for the dot product. </param>
        /// <param name="right"> <see cref="Vector"/> for the dot product. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the dot product of two <see cref="Vector"/>. </returns>
        public static double operator *(Vector left, Vector right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="Point"/> into a <see cref="Vector"/>.
        /// </summary>
        /// <param name="point"> <see cref="Point"/> to convert. </param>
        /// <returns> The new <see cref="Vector"/> resulting from the conversion. </returns>
        public static implicit operator Vector(Point point) => new Vector(point.X, point.Y, point.Z);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the coordinates of this <see cref="Vector"/>.
        /// </summary>
        /// <returns> The array representation of the <see cref="Vector"/>'s coordinates. </returns>
        public double[] GetCoordinates() => new double[3] { X, Y, Z };


        /// <summary>
        /// Computes the length of this <see cref="Vector"/> (using the L2-norm).
        /// </summary>
        /// <returns> The value of this <see cref="Vector"/>'s length.</returns>
        public double Length() => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Computes the squared length of this <see cref="Vector"/> (using the L2-norm).
        /// </summary>
        /// <returns> The value of this <see cref="Vector"/>'s squared length.</returns>
        public double SquaredLength() => (X * X) + (Y * Y) + (Z * Z);


        /// <summary>
        /// Unitises this <see cref="Vector"/> (using the L2-norm).
        /// </summary>
        /// <exception cref="DivideByZeroException"> The length of the <see cref="Vector"/> must be different than zero.</exception>
        public void Unitise()
        {
            double length = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

            if (length == 0d)
            {
                throw new DivideByZeroException("The length of the vector must be different than zero.");
            }

            X /= length; Y /= length; Z /= length;
        }

        /// <summary>
        /// Evaluates whether this <see cref="Vector"/>'s length is one.
        /// </summary>
        /// <returns> <see langword="true"/> if this <see cref="Vector"/> is of unit length, <see langword="false"/> otherwise. </returns>
        public bool IsUnit()
        {
            return Math.Abs((X * X) + (Y * Y) + (Z * Z) - 1d) < Settings.AbsolutePrecision;
        }


        /// <summary>
        /// Computes the angle made by this <see cref="Vector"/> with another <see cref="Vector"/>.
        /// </summary>
        /// <param name="other"> <see cref="Vector"/> to compare with. </param>
        /// <returns> The value of the angle (in radians). </returns>
        public double AngleWith(Vector other)
        {
            double dotProduct = (X * other.X) + (Y * other.Y) + (Z * other.Z);
            double length = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            double otherLength = Math.Sqrt((other.X * other.X) + (other.Y * other.Y) + (other.Z * other.Z));

            return Math.Acos(dotProduct / (length * otherLength));
        }


        /// <summary>
        /// Evaluates whether this <see cref="Vector"/> is memberwise equal to another <see cref="Vector"/>.
        /// </summary>
        /// <remarks> 
        /// Two <see cref="Vector"/> are equal if their coordinates are equal.
        /// </remarks>
        /// <param name="other"> <see cref="Vector"/> to compare with. </param>
        /// <returns> <see langword="true"/> if the two <see cref="Vector"/> are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Vector other)
        {
            return Math.Abs(this.X - other.X) < Settings.AbsolutePrecision
                    & Math.Abs(this.Y - other.Y) < Settings.AbsolutePrecision
                    & Math.Abs(this.Z - other.Z) < Settings.AbsolutePrecision;
        }

        #endregion


        #region Override : Object

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Vector vector && Equals(vector);
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

        #endregion-

        #region Explicit Implementations

        /******************** IDotProduct<Point, double> ********************/

        /// <inheritdoc/>
        double Alg_Meas.IMetric<Vector>.DistanceTo(Vector other) => (this - other).Length();

        /// <inheritdoc/>
        double Alg_Meas.INorm<Vector>.Norm() => Length();

        /// <inheritdoc/>
        double Alg_Meas.IDotProduct<Vector, double>.DotProduct(Vector other) => Vector.DotProduct(this, other);

        /******************** IGroupAction<Vector, double> ********************/

        /// <inheritdoc/>
        Vector Alg_Sets.IGroupAction<Vector, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Vector Alg_Sets.IGroupAction<Vector, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}