using System;

using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.ConstraintTypes
{

    /// <summary>
    /// Constraint enforcing a segment to be orthogonal to a variable direction. The list of variables for this constraint consists of:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>P<sub>s</sub></term>
    ///         <description> Variable representing the start point of the segment.</description>
    ///     </item>
    ///     <item> 
    ///         <term>P<sub>e</sub></term>
    ///         <description> Variable representing the end point of the segment.</description>
    ///     </item>
    ///     <item> 
    ///         <term>V</term>
    ///         <description> Variable representing the direction to which the segment must be orthogonal.</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <remarks> If the target direction is fixed, refer to the homonymous energy <see cref="EnergyTypes.SegmentOrthogonality"/>. </remarks>
    public class SegmentOrthogonality : ConstraintType
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SegmentLength"/> class.
        /// </summary>
        /// <param name="dimension"> Dimension of the variables representing the start and the end segment. </param>
        public SegmentOrthogonality(int dimension)
        {
            // ----- Define LocalHi ----- //

            int[] columnPointers = new int[(3 * dimension) + 1];
            int[] rowIndices = new int[(4 * dimension)];
            double[] values = new double[(4 * dimension)];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < dimension; i_C++)
            {
                columnPointers[i_C + 1] = i_C + 1;
                rowIndices[i_C] = (2 * dimension) + i_C;
                values[i_C] = -1;

                columnPointers[dimension + i_C + 1] = dimension + i_C + 1;
                rowIndices[dimension + i_C] = (2 * dimension) + i_C;
                values[dimension + i_C] = 1;

                columnPointers[(2 * dimension) + i_C + 1] = 2 * (dimension + i_C + 1);
                rowIndices[(2 * (dimension + i_C))] = i_C ; rowIndices[(2 * (dimension + i_C)) + 1] = dimension + i_C;
                values[(2 * (dimension + i_C))] = -1; values[(2 * (dimension + i_C)) + 1] = 1;
            }

            LocalHi = new CompressedColumn(3 * dimension, 3 * dimension, columnPointers, rowIndices, values);
            

            // ----- Define LocalBi ----- //

            LocalBi = null;


            // ----- Define Ci ----- //

            Ci = 0d;
        }
    }
}
