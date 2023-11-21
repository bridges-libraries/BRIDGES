using System;

using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.ConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a vector variable to equal a fixed target length (computed with euclidean norm). The list of variables of this constraint consists in:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>V</term>
    ///         <description> Variable representing the vector to resize. </description>
    ///     </item>
    /// </list>
    /// </summary>
    public class VectorLength : ConstraintType
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="VectorLength"/> class.
        /// </summary>
        /// <param name="length"> Target length of the vector. </param>
        /// <param name="dimension"> Dimension of the vector variable. </param>
        public VectorLength(double length, int dimension = 3)
        {
            // ----- Define LocalHi ********************/

            int[] columnPointers = new int[dimension + 1];
            int[] rowIndices = new int[dimension];
            double[] values = new double[dimension];

            columnPointers[0] = 0;
            for (int i = 0; i < dimension; i++)
            {
                columnPointers[i + 1] = i + 1;
                rowIndices[i] = i;
                values[i] = 2.0;
            }

            LocalHi = new CompressedColumn(dimension, dimension, columnPointers, rowIndices, values);


            // ----- Define LocalBi ----- //

            LocalBi = null;


            // ----- Define Ci ----- //

            Ci = - (length * length);
        }

        #endregion
    }
}
