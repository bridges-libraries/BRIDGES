using System;
using System.Collections.Generic;

using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a dense vector.
    /// </summary>
    public sealed class DenseVector : Vector,
        IEquatable<DenseVector>, IEnumerable<double>,
        Alg_Meas.IDotProduct<DenseVector, double>,
        Alg_Sets.IGroupAction<DenseVector, double>
    {
        #region Fields

        /// <summary>
        /// Components of this <see cref="DenseVector"/>.
        /// </summary>
        private readonly double[] _components;

        #endregion

        #region Properties

        /// <inheritdoc cref="Vector.Size"/>
        public override int Size => _components.Length;

        /// <inheritdoc cref="Vector.this[int]"/>
        public override double this[int index]
        {  
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class of given size, containing only zeros.
        /// </summary>
        /// <param name="size"> Number of component of the <see cref="DenseVector"/>. </param>
        public DenseVector(int size)
            : base(size)
        {
            _components = new double[size];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the <see cref="DenseVector"/>. </param>
        public DenseVector(IList<double> components)
            : base(components.Count)
        {
            int size = components.Count;

            _components = new double[size];
            for (int i = 0; i < size; i++)
            {
                _components[i] = components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from another <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="DenseVector"/> to deep copy. </param>
        public DenseVector(DenseVector vector)
            :base (vector.Size)
        {
            int size = vector.Size;

            _components = new double[size];
            for (int i = 0; i < size; i++)
            {
                _components[i] = vector._components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="SparseVector"/> to copy. </param>
        public DenseVector(SparseVector vector)
            : base(vector.Size)
        {
            int size = vector.Size;

            _components = new double[size];
            foreach(var (rowIndex, value) in vector.NonZeros())
            {
                _components[rowIndex] = value;
            }
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the new <see cref="DenseVector"/>. </param>
        internal DenseVector(ref double[] components)
            : base (components.Length)
        {
            _components = components;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="DenseVector"/> for the addition.
        /// </summary>
        /// <param name="size"> Number of component of the <see cref="DenseVector"/>. </param>
        /// <returns> The <see cref="DenseVector"/> of the given size, with zeros on every coordinates. </returns>
        public static DenseVector Zero(int size) => new DenseVector(size);

        /// <summary>
        /// Returns the unit <see cref="DenseVector"/> of a given <paramref name="size"/>, with one at the given row <paramref name="index"/> and zeros elsewhere.
        /// </summary>
        /// <param name="size"> Size of the <see cref="DenseVector"/>. </param>
        /// <param name="index"> Index of the standard vector, i.e of the component equal to one. </param>
        /// <returns> The new <see cref="DenseVector"/> representing the standard vector. </returns>
        public static DenseVector StandardVector(int size, int index) => new DenseVector(size) { [index] = 1d };

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(DenseVector, DenseVector)"/>
        public static DenseVector Add(DenseVector left, DenseVector right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector, DenseVector)"/>
        public static DenseVector Subtract(DenseVector left, DenseVector right) => left - right;


        /// <inheritdoc cref="operator -(DenseVector)"/>
        public static DenseVector Opposite(DenseVector operand) => -operand;


        /******************** Embedding : SparseVector ********************/

        /// <inheritdoc cref="operator +(DenseVector, SparseVector)"/>
        public static DenseVector Add(DenseVector left, SparseVector right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector, SparseVector)"/>
        public static DenseVector Subtract(DenseVector left, SparseVector right) => left - right;


        /******************** Embedding : Vector ********************/

        /// <inheritdoc cref="operator +(DenseVector, Vector)"/>
        public static DenseVector Add(DenseVector left, Vector right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector, Vector)"/>
        public static DenseVector Subtract(DenseVector left, Vector right) => left - right;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(DenseVector, double)"/>
        public static DenseVector Multiply(DenseVector operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, DenseVector)"/>
        public static DenseVector Multiply(double factor, DenseVector operand) => factor * operand;


        /// <inheritdoc cref="operator /(DenseVector, double)"/>
        public static DenseVector Divide(DenseVector operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="DenseVector"/> and the right <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            int size = left.Size;

            double result = 0d;
            for (int i = 0; i < size; i++)
            {
                result += left[i] * right[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="DenseVector"/> and the right <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            double result = 0d;
            foreach ((int rowIndex, double value) in right.NonZeros())
            {
                result += left[rowIndex] * value;
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="DenseVector"/> and the right <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="Vector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(DenseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return DenseVector.TransposeMultiply(left, denseRight); }
            else if (right is SparseVector sparseRight) { return DenseVector.TransposeMultiply(left, sparseRight); }
            else
            {
                if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

                int size = left.Size;
                double[] otherComponents = right.ToArray();

                double result = 0d;
                for (int i = 0; i < size; i++)
                {
                    result += left[i] * otherComponents[i];
                }

                return result;
            }
        }


        /******************** On DenseVector Sets ********************/

        /// <summary>
        /// Ortho-normalise the set of <see cref="DenseVector"/> using a Gram-Schimdt process. 
        /// </summary>
        /// <remarks> If the vectors are not linearly independent the number of vectors will change. </remarks>
        /// <param name="vectors"> Set of <see cref="DenseVector"/> to operate on. </param>
        /// <returns> The ortho-normal set of <see cref="DenseVector"/>. </returns>
        public static DenseVector[] GramSchmidt(IEnumerable<DenseVector> vectors)
        {
            List<DenseVector> results = new List<DenseVector>();

            foreach (DenseVector denseVector in vectors)
            {
                DenseVector vector = new DenseVector(denseVector);

                for (int i_R = 0; i_R < results.Count; i_R++)
                {
                    DenseVector result = new DenseVector(results[i_R]);

                    double numerator = DenseVector.TransposeMultiply(vector, result);
                    double denominator = DenseVector.TransposeMultiply(result, result);

                    result *= (numerator / denominator);

                    vector -= result;
                }

                double length = DenseVector.TransposeMultiply(vector, vector);
                if (length > Settings.AbsolutePrecision)
                {
                    vector /= length;
                    results.Add(vector);
                }
            }

            return results.ToArray();
        }

        #endregion

        #region Operators

        /******************** Algebraic Additive Group ********************/

        /// <summary>
        /// Computes the addition of two <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator +(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left[i] + right[i];
            }

            return new DenseVector(ref components);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator -(DenseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left[i] - right[i];
            }

            return new DenseVector(ref components);
        }


        /// <summary>
        /// Computes the opposite of the <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector"/> from which the opposite is computed. </param>
        /// <returns> The <see cref="DenseVector"/>, opposite of the initial one. </returns>
        public static DenseVector operator -(DenseVector operand)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = -operand[i];
            }

            return new DenseVector(ref components);
        }


        /******************** Embedding : SparseVector ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseVector"/> with a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator +(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            DenseVector result = new DenseVector(left);

            foreach ((int rowIndex, double value) in right.NonZeros())
            {
                result[rowIndex] += value;
            }

            return result;
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="DenseVector"/> with a <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator -(DenseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            DenseVector result = new DenseVector(left);

            foreach ((int rowIndex, double value) in right.NonZeros())
            {
                result[rowIndex] -= value;
            }

            return result;
        }


        /******************** Embedding : Vector ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseVector"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of left with right as a <see cref="Vector"/> is not implemented. </exception>
        public static DenseVector operator +(DenseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return left + denseRight; }
            else if (right is SparseVector sparseRight) { return left + sparseRight; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} with a {right.GetType()} as {typeof(Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="DenseVector"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of left with right as a <see cref="Vector"/> is not implemented. </exception>
        public static DenseVector operator -(DenseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return left - denseRight; }
            else if (right is SparseVector sparseRight) { return left - sparseRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as {typeof(Vector)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="DenseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the right scalar multiplication. </returns>
        public static DenseVector operator *(DenseVector operand, double factor)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = factor * operand[i];
            }

            return new DenseVector(ref components);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="DenseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="DenseVector"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the left scalar multiplication. </returns>
        public static DenseVector operator *(double factor, DenseVector operand)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = operand[i] * factor;
            }

            return new DenseVector(ref components);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="DenseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the scalar division. </returns>
        public static DenseVector operator /(DenseVector operand, double divisor)
        {
            int size = operand.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = operand[i] / divisor;
            }

            return new DenseVector(ref components);
        }


        /******************** Equality Operators ********************/

        /// <summary>
        /// Evaluates whether two <see cref="DenseVector"/> are equal or not.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="DenseVector"/> are equal, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The equality comparison of a <see cref="DenseVector"/> with a <see cref="DenseVector"/> is not implemented. </exception>
        public static bool operator ==(DenseVector left, DenseVector right)
        {
            if (left is null) { return right is null; }
            else if (right is null) { return false; }
            else if (left.Size != right.Size) { return false; }

            int size = left.Size;
            for (int i = 0; i < size; i++)
            {
                if (left[i] != right[i]) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Evaluates whether two <see cref="DenseVector"/> are different.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="DenseVector"/> are different, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The inequality comparison of a <see cref="DenseVector"/> with a <see cref="DenseVector"/> is not implemented. </exception>
        public static bool operator !=(DenseVector left, DenseVector right)
        {
            if (left is null) { return right != null; }
            else if (right is null) { return true; }
            else if (left.Size != right.Size) { return true; }

            int size = left.Size;
            for (int i = 0; i < size; i++)
            {
                if (left[i] != right[i]) { return true; }
            }

            return false;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(DenseVector other)
        {
            if (Size != other.Size) { return false; }

            int size = Size;
            for (int i = 0; i < size; i++)
            {
                if (this[i] != other[i]) { return false; }
            }

            return true;
        }

        #endregion


        #region Overrides

        /******************** Vector ********************/

        /// <inheritdoc cref="Vector.ToArray()"/>
        public override double[] ToArray()=> (double[]) _components.Clone();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
        public override IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Size; i++) { yield return this[i]; }
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public override bool Equals(Vector other) => other is DenseVector dense ? Equals(dense) : base.Equals(other);


        /******************** object ********************/

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
            return $"Dense Vector (S:{Size})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IDotProduct<DenseVector, double>, ********************/

        /// <inheritdoc cref="Alg_Meas.IMetric{TSelf}.DistanceTo(TSelf)"/>
        double Alg_Meas.IMetric<DenseVector>.DistanceTo(DenseVector other) => (this - other).Norm();

        /// <inheritdoc cref="Alg_Meas.IDotProduct{TSelf,TValue}.DotProduct(TSelf)"/>
        double Alg_Meas.IDotProduct<DenseVector, double>.DotProduct(DenseVector other) => TransposeMultiply(this, other);


        /******************** IGroupAction<Vector, double> ********************/

        /// <inheritdoc/>
        DenseVector Alg_Sets.IGroupAction<DenseVector, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        DenseVector Alg_Sets.IGroupAction<DenseVector, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
