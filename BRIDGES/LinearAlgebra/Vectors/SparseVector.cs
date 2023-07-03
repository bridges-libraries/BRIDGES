using System;
using System.Collections.Generic;

using Alg_Sets = BRIDGES.Algebra.Sets;
using Alg_Meas = BRIDGES.Algebra.Measure;


namespace BRIDGES.LinearAlgebra.Vectors
{
    /// <summary>
    /// Class defining a sparse vector.
    /// </summary>
    public sealed class SparseVector : Vector,
        IEquatable<SparseVector>, IEnumerable<double>,
        Alg_Meas.IDotProduct<SparseVector, double>,
        Alg_Sets.IGroupAction<SparseVector, double>
    {
        #region Fields

        /// <summary>
        /// Size of this <see cref="SparseVector"/>.
        /// </summary>
        private readonly int _size;

        /// <summary>
        /// Non-zero components of this <see cref="SparseVector"/>.
        /// </summary>
        /// <remarks> <list type="bullet"> 
        /// <item><term>Key</term><description> Row index of a non-zero component. </description></item>
        /// <item><term>Value</term><description> Non-zero component at the given row index. </description></item>
        /// </list> </remarks>
        private readonly Dictionary<int, double> _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of non-zero values in the current sparse vector.
        /// </summary>
        public int NonZerosCount => _components.Count;

        /// <inheritdoc/>
        public override int Size => _size;

        /// <inheritdoc/>
        public override double this[int index]
        {
            get {  return _components.TryGetValue(index, out double val) ? val : 0d; }
            set 
            {
                if(_components.ContainsKey(index)) { _components[index] = value; }
                else { _components.Add(index, value); }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class of given size, containing only zeros.
        /// </summary>
        /// <param name="size"> Number of components of the <see cref="SparseVector"/>. </param>
        public SparseVector(int size)
            : base(size)
        {
            _size = size;
            _components = new Dictionary<int, double>(size);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class of given size, with given values.
        /// </summary>
        /// <param name="size"> Number of components of the <see cref="SparseVector"/>. </param>
        /// <param name="rowIndices"> Row indices of the non-zero values of the <see cref="SparseVector"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="SparseVector"/>. </param>
        /// <exception cref="ArgumentException"> The numbers of row indices and values should be the same. </exception>
        public SparseVector(int size, IEnumerable<int> rowIndices, IEnumerable<double> values)
            : base(size)
        {
            _size = size;
            _components = new Dictionary<int, double>(size);

            IEnumerator<int> rowEnumerator = rowIndices.GetEnumerator();
            IEnumerator<double> valueEnumerator = values.GetEnumerator();
            try
            {
                while(rowEnumerator.MoveNext())
                {
                    if(!valueEnumerator.MoveNext())
                    {
                        throw new ArgumentException("The numbers of row indices and values should be the same.");
                    }

                    _components.Add(rowEnumerator.Current, valueEnumerator.Current);
                }

                if (valueEnumerator.MoveNext())
                {
                    throw new ArgumentException("The numbers of row indices and values should be the same.");
                }
            }
            finally
            {
                rowEnumerator.Dispose();
                valueEnumerator.Dispose();
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class from another <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="other"> <see cref="SparseVector"/> to deep copy. </param>
        public SparseVector(SparseVector other)
            : base(other.Size)
        {
            _size = other.Size;
            _components = new Dictionary<int, double>(other._components);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector"/> class of given size, with given components.
        /// </summary>
        /// <param name="size"> Number of components of the <see cref="SparseVector"/>. </param>
        /// <param name="components"> Non-zero components of the <see cref="SparseVector"/>, associated with there row index. </param>
        internal SparseVector(int size, ref Dictionary<int, double> components)
            : base(size)
        {
            _size = size;
            _components = components;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="SparseVector"/> for the addition.
        /// </summary>
        /// <param name="size"> Number of component of the <see cref="SparseVector"/>. </param>
        /// <returns> The <see cref="SparseVector"/> of the given size and with zeros on every coordinates. </returns>
        public static SparseVector Zero(int size) => new SparseVector(size);

        /// <summary>
        /// Returns the unit <see cref="SparseVector"/> of a given <paramref name="size"/>, with one at the given row <paramref name="index"/> and zeros elsewhere.
        /// </summary>
        /// <param name="size"> Size of the <see cref="SparseVector"/>. </param>
        /// <param name="index"> Index of the standard vector, i.e of the component equal to one. </param>
        /// <returns> The new <see cref="SparseVector"/> representing the standard vector. </returns>
        public static SparseVector StandardVector(int size, int index) => new SparseVector(size) { [index] = 1d };

        #endregion

        #region Static Methods

        /******************** Algebraic Additive Group ********************/

        /// <inheritdoc cref="operator +(SparseVector, SparseVector)"/>
        public static SparseVector Add(SparseVector left, SparseVector right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector, SparseVector)"/>
        public static SparseVector Subtract(SparseVector left, SparseVector right) => left - right;


        /// <inheritdoc cref="operator -(SparseVector)"/>
        public static SparseVector Opposite(SparseVector operand) => -operand;


        /******************** Embed In : DenseVector ********************/

        /// <inheritdoc cref="operator +(SparseVector, DenseVector)"/>
        public static DenseVector Add(SparseVector left, DenseVector right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector, DenseVector)"/>
        public static DenseVector Subtract(SparseVector left, DenseVector right) => left - right;


        /******************** Embed In : Vector ********************/

        /// <inheritdoc cref="operator +(SparseVector, Vector)"/>
        public static Vector Add(SparseVector left, Vector right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector, Vector)"/>
        public static Vector Subtract(SparseVector left, Vector right) => left - right;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(SparseVector, double)"/>
        public static SparseVector Multiply(SparseVector operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, SparseVector)"/>
        public static SparseVector Multiply(double factor, SparseVector operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseVector, double)"/>
        public static SparseVector Divide(SparseVector operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="SparseVector"/> and the right <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(SparseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            double result = 0d;

            foreach ((int rowIndex, double leftValue) in left.NonZeros())
            {
                if (right.TryGetComponent(rowIndex, out double rightValue)) { result += leftValue * rightValue; }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="SparseVector"/> and the right <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            double result = 0d;

            foreach ((int rowIndex, double value) in left.NonZeros())
            {
                result += value * right[rowIndex];
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication between the transposed left <see cref="SparseVector"/> and the right <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to transpose, then multiply. </param>
        /// <param name="right"> Right <see cref="Vector"/> to multiply. </param>
        /// <returns> The scalar value resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static double TransposeMultiply(SparseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return DenseVector.TransposeMultiply(left, denseRight); }
            else if (right is SparseVector sparseRight) { return DenseVector.TransposeMultiply(left, sparseRight); }
            else
            {
                if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

                double[] otherComponents = right.ToArray();

                double result = 0d;
                foreach ((int rowIndex, double value) in left.NonZeros())
                {
                    result += value * otherComponents[rowIndex];
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
        public static SparseVector[] GramSchmidt(IEnumerable<SparseVector> vectors)
        {
            List<SparseVector> results = new List<SparseVector>();

            foreach (SparseVector denseVector in vectors)
            {
                SparseVector vector = new SparseVector(denseVector);

                for (int i_R = 0; i_R < results.Count; i_R++)
                {
                    SparseVector result = new SparseVector(results[i_R]);

                    double numerator = SparseVector.TransposeMultiply(vector, result);
                    double denominator = SparseVector.TransposeMultiply(result, result);

                    result *= numerator / denominator;

                    vector -= result;
                }

                double length = SparseVector.TransposeMultiply(vector, vector);
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
        /// Computes the addition of two <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the addition. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static SparseVector operator +(SparseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            Dictionary<int, double> result = new Dictionary<int, double>(left.NonZerosCount + right.NonZerosCount);

            foreach ((int rowIndex, double value) in left.NonZeros()) { result.Add(rowIndex, value); }

            foreach ((int rowIndex, double value) in right.NonZeros())
            {
                if (result.TryGetValue(rowIndex, out double val))
                {
                    if (value == -val) { result.Remove(rowIndex); }
                    else { result[rowIndex] = val + value; }
                }
                else { result.Add(rowIndex, value); }
            }

            return new SparseVector(left.Size, ref result);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static SparseVector operator -(SparseVector left, SparseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            Dictionary<int, double> result = new Dictionary<int, double>(left.NonZerosCount + right.NonZerosCount);

            foreach ((int rowIndex, double value) in left.NonZeros()) { result.Add(rowIndex, value); }

            foreach ((int rowIndex, double value) in right.NonZeros())
            {
                if (result.TryGetValue(rowIndex, out double val))
                {
                    if (value == val) { result.Remove(rowIndex); }
                    else { result[rowIndex] = val - value; }
                }
                else { result.Add(rowIndex, -value); }
            }

            return new SparseVector(left.Size, ref result);
        }


        /// <summary>
        /// Computes the opposite of the <see cref="SparseVector"/>.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> from which the opposite is computed. </param>
        /// <returns> The <see cref="SparseVector"/>, opposite of the initial one. </returns>
        public static SparseVector operator -(SparseVector operand)
        {
            int size = operand.Size;

            Dictionary<int, double> components = new Dictionary<int, double>(size);
            foreach ((int rowIndex, double value) in operand.NonZeros())
            {
                components.Add(rowIndex, -value);
            }

            return new SparseVector(size, ref components);
        }


        /******************** Embed In : DenseVector ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="SparseVector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator +(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left.TryGetComponent(i, out double value) ? value + right[i] : right[i];
            }

            return new DenseVector(ref components);
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="SparseVector"/> with a <see cref="DenseVector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the vectors must be the same. </exception>
        public static DenseVector operator -(SparseVector left, DenseVector right)
        {
            if (left.Size != right.Size) { throw new ArgumentException("The size of the vectors must be the same."); }

            int size = left.Size;

            double[] components = new double[size];
            for (int i = 0; i < size; i++)
            {
                components[i] = left.TryGetComponent(i, out double value) ? value - right[i] : -right[i];
            }

            return new DenseVector(ref components);
        }


        /******************** Embedding : Vector ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseVector"/> with a <see cref="Vector"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of left with right as a <see cref="Vector"/> is not implemented. 
        /// </exception>
        public static Vector operator +(SparseVector left, Vector right)
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
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of left with right as a <see cref="Vector"/> is not implemented. 
        /// </exception>
        public static Vector operator -(SparseVector left, Vector right)
        {
            if (right is DenseVector denseRight) { return left - denseRight; }
            else if (right is SparseVector sparseRight) { return left - sparseRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as {typeof(Vector)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="SparseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the right scalar multiplication. </returns>
        public static SparseVector operator *(SparseVector operand, double factor)
        {
            if (factor == 0d) { return new SparseVector(operand.Size); }

            int size = operand.Size;

            Dictionary<int, double> components = new Dictionary<int, double>(size);
            foreach ((int rowIndex, double value) in operand.NonZeros())
            {
                components.Add(rowIndex, value * factor);
            }

            return new SparseVector(size, ref components);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="SparseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the left scalar multiplication. </returns>
        public static SparseVector operator *(double factor, SparseVector operand)
        {
            if (factor == 0d) { return new SparseVector(operand.Size); }

            int size = operand.Size;

            Dictionary<int, double> components = new Dictionary<int, double>(size);
            foreach ((int rowIndex, double value) in operand.NonZeros())
            {
                components.Add(rowIndex, factor * value);
            }

            return new SparseVector(size, ref components);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="SparseVector"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseVector"/> resulting from the scalar division. </returns>
        /// <exception cref="DivideByZeroException"> The argument <paramref name="divisor"/> is equal to zero. </exception>
        public static SparseVector operator /(SparseVector operand, double divisor)
        {
            if (divisor == 0d) { throw new DivideByZeroException($"The argument {nameof(divisor)} is equal to zero."); }

            int size = operand.Size;

            Dictionary<int, double> components = new Dictionary<int, double>(size);
            foreach ((int key, double value) in operand.NonZeros())
            {
                components.Add(key, value / divisor);
            }

            return new SparseVector(size, ref components);
        }


        /******************** Equality Operators ********************/

        /// <summary>
        /// Evaluates whether two <see cref="SparseVector"/> are equal or not.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="SparseVector"/> are equal, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The equality comparison of a <see cref="SparseVector"/> with a <see cref="SparseVector"/> is not implemented. </exception>
        public static bool operator ==(SparseVector left, SparseVector right)
        {
            if (left is null) { return right is null; }
            else if (right is null) { return false; }
            else if (left.Size != right.Size) { return false; }
            else if (left.NonZerosCount != right.NonZerosCount) { return false; }

            foreach ((int rowIndex, double leftValue) in left.NonZeros())
            {
                if (!right.TryGetComponent(rowIndex, out double value) || leftValue != value) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Evaluates whether two <see cref="SparseVector"/> are different.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector"/> for the comparison. </param>
        /// <param name="right"> Right <see cref="SparseVector"/> for the comparison. </param>
        /// <returns> <see langword="true"/> if the two <see cref="SparseVector"/> are different, <see langword="false"/> otherwise. </returns>
        /// <exception cref="NotImplementedException"> The inequality comparison of a <see cref="SparseVector"/> with a <see cref="SparseVector"/> is not implemented. </exception>
        public static bool operator !=(SparseVector left, SparseVector right)
        {
            if (left is null) { return right != null; }
            else if (right is null) { return true; }
            else if (left.Size != right.Size) { return true; }
            else if (left.NonZerosCount != right.NonZerosCount) { return true; }

            foreach ((int rowIndex, double leftValue) in left.NonZeros())
            {
                if (!right.TryGetComponent(rowIndex, out double value) || leftValue != value) { return true; }
            }

            return false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the components at the given index.
        /// </summary>
        /// <param name="index"> The index of the component to get. </param>
        /// <param name="val"> Value containing the component at the given index if it was found, zero otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public bool TryGetComponent(int index, out double val)
        {
            return _components.TryGetValue(index, out val);
        }

        /// <summary>
        /// Returns an enumerator which reads through the non-zero components of the current <see cref="SparseVector"/>. <br/>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> represents is composed of the row index and the component value.
        /// </summary>
        /// <returns> The enumerator of the <see cref="SparseVector"/>. </returns>
        public IEnumerable<(int rowIndex, double value)> NonZeros()
        {
            var kvp_Enumerator = _components.GetEnumerator();
            try
            {
                while(kvp_Enumerator.MoveNext())
                {
                    var kvp = kvp_Enumerator.Current;
                    yield return (kvp.Key, kvp.Value);
                }
            }
            finally{ kvp_Enumerator.Dispose(); }
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(SparseVector other)
        {
            if (Size != other.Size) { return false; }

            if(NonZerosCount != other.NonZerosCount) { return false; }

            foreach((int rowIndex, double value) in NonZeros())
            {
                if(!other.TryGetComponent(rowIndex, out double otherValue) || otherValue != value) { return false; }
            }

            return true;
        }

        #endregion


        #region Overrides

        /******************** Vector ********************/

        /// <inheritdoc cref="DenseVector.ToArray()"/>
        public override double[] ToArray()
        {
            double[] result = new double[Size];
            foreach ((int rowIndex, double value) in NonZeros()) { result[rowIndex] = value; }

            return result;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
        /// <remarks> This method iterates over both zero and non-zero values. To iterate only on non-zero values use <see cref="SparseVector.NonZeros()"/>. </remarks>
        public override IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Size; i++) { yield return this[i]; }
        }


        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public override bool Equals(Vector other) => other is SparseVector sparse ? Equals(sparse) : base.Equals(other);


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
            return $"Sparse Vector (S:{Size})";
        }

        #endregion

        #region Explicit Implementations

        /******************** IDotProduct<DenseVector, double>, ********************/

        /// <inheritdoc cref="Alg_Meas.IMetric{TSelf}.DistanceTo(TSelf)"/>
        double Alg_Meas.IMetric<SparseVector>.DistanceTo(SparseVector other) => (this - other).Norm();

        /// <inheritdoc cref="Alg_Meas.IDotProduct{TSelf,TValue}.DotProduct(TSelf)"/>
        double Alg_Meas.IDotProduct<SparseVector, double>.DotProduct(SparseVector other) => TransposeMultiply(this, other);


        /******************** IGroupAction<Vector, double> ********************/

        /// <inheritdoc/>
        SparseVector Alg_Sets.IGroupAction<SparseVector, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        SparseVector Alg_Sets.IGroupAction<SparseVector, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
