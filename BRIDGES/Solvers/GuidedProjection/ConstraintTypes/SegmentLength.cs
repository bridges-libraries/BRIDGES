using System;

using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.ConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a segment to equal a fixed target length. The list of variables for this constraint consists of:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>P<sub>s</sub></term>
    ///         <description> Variable representing the start point of the segment.</description>
    ///     </item>
    ///     <item> 
    ///         <term>P<sub>e</sub></term>
    ///         <description> Variable representing the end point of the segment.</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <remarks> If the target length is a variable, refer to the constraint <see cref="CoherentLength"/>. </remarks>
    public class SegmentLength : ConstraintType
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SegmentLength"/> class.
        /// </summary>
        /// <param name="dimension"> Dimension of the variables representing the start and the end segment. </param>
        /// <param name="length"> Value of the target lenght. </param>
        public SegmentLength(int dimension, double length)
        {
            // ----- Define LocalHi ----- //

            int[] columnPointers = new int[(2 * dimension) + 1];
            int[] rowIndices = new int[(4 * dimension)];
            double[] values = new double[(4 * dimension)];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < dimension; i_C++)
            {
                columnPointers[i_C + 1] = 2 * (i_C + 1);
                rowIndices[(2 * i_C)] = i_C ; rowIndices[(2 * i_C) + 1] = dimension + i_C;
                values[(2 * i_C)] = 2d ; values[(2 * i_C) + 1] = -2d ;

                columnPointers[dimension + i_C + 1] = 2 * (dimension + i_C + 1);
                rowIndices[(2 * (dimension + i_C))] = i_C; rowIndices[(2 * (dimension + i_C)) + 1] = dimension + i_C;
                values[(2 * (dimension + i_C))] = -2d; values[(2 * (dimension + i_C)) + 1] = 2d;
            }

            LocalHi = new CompressedColumn((2 * dimension), (2 * dimension), columnPointers, rowIndices, values);


            // ----- Define LocalBi ----- //

            LocalBi = null;


            // ----- Define Ci ----- //

            Ci = - (length * length);
        }
    }
}
