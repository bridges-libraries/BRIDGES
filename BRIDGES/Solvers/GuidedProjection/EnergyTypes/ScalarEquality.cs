using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.EnergyTypes
{

    /// <summary>
    /// Energy enforcing a numeric variable to be equal to a fixed value. The list of variables of this energy consists in:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>V</term>
    ///         <description> Scalar variable. which should match the specified fixed value. </description>
    ///     </item>
    /// </list>
    /// </summary>
    public class ScalarEquality : EnergyType
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ScalarEquality"/> class.
        /// </summary>
        /// <param name="value"> Target value for the numeric variable. </param>
        public ScalarEquality(double value)
        {
            // ----- Define Ki ----- //

            int[] rowIndices = new int[1] { 0 };
            double[] values = new double[1] { 1d };

            LocalKi = new SparseVector(1, rowIndices, values);

            // ----- Define Si ----- //

            Si = value;
        }

        #endregion
    }
}
