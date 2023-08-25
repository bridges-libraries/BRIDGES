using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.EnergyTypes
{
    /// <summary>
    /// Energy enforcing a segment to be orthogonal to a fixed direction <em>V</em>. The list of variables for this energy consists of:
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
    public class SegmentOrthogonality : EnergyType
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SegmentOrthogonality"/> class.
        /// </summary>
        /// <param name="direction"> Coordinates of the direction vector to which the segment must be orthogonal. </param>
        public SegmentOrthogonality(double[] direction)
        {
            /******************** Unitise the direction ********************/

            bool isZero = true;
            double length = 0d;
            for (int i = 0; i < direction.Length; i++)
            {
                if (isZero & Settings.AbsolutePrecision < Math.Abs(direction[i])) { isZero = false; }

                length += direction[i] * direction[i];
            }
            length = Math.Sqrt(length);

            if (isZero) { throw new DivideByZeroException("The length of the target direction must be different from zero."); }

            for (int i = 0; i < direction.Length; i++)
            {
                direction[i] = direction[i] / length;
            }

            /******************** Define LocalKi ********************/

            Dictionary<int, double> components = new Dictionary<int, double>((2 * direction.Length));
            for (int i = 0; i < direction.Length; i++)
            {
                components.Add(i, -direction[i]);
                components.Add(direction.Length + i, direction[i]);
            }

            LocalKi = new SparseVector(2 * direction.Length, ref components);


            /******************** Define Si ********************/

            Si = 0d;
        }

        #endregion
    }
}
