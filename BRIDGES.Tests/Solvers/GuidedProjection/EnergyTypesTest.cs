using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using GP = BRIDGES.Solvers.GuidedProjection;


namespace BRIDGES.Tests.Solvers.GuidedProjection
{
    /// <summary>
    /// Class testing <see cref="GP.Abstracts.EnergyType"/> in <see cref="GP.EnergyTypes"/>.
    /// </summary>
    [TestClass]
    public class EnergyTypesTest
    {
        /// <summary>
        /// Epsilon value for the Tikhonov regularisation
        /// </summary>
        private static double Epsilon = 1e-4;


        #region ScalarEquality

        /// <summary>
        /// Tests the constraint <see cref="GP.EnergyTypes.ScalarEquality"/>.
        /// </summary>
        [TestMethod("Scalar Equality")]
        public void ScalarEquality()
        {
            // ----- Arrange ----- //
            double scalar = 5.2;
            double targetValue = 1.4;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable scalarVar = new GP.Variable(scalar);
            solver.TryAddVariable(scalarVar);

            GP.EnergyTypes.ScalarEquality energyType = new GP.EnergyTypes.ScalarEquality(targetValue);
            solver.AddEnergy(energyType, new GP.IVariable[1] { scalarVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double result = scalarVar[0];

            // ----- Assert ----- //

            Assert.AreEqual(targetValue, result, Settings.AbsolutePrecision);
        }

        #endregion


        #region Segment Parallelity

        /// <summary>
        /// Tests the method <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("Segment Parallelity 2D")]
        public void SegmentParallelity_2D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[2] { 1d, 1d };
            double[] endPoint = new double[2] { 2d, 4d };
            double initialLengthValue = 5d;

            double[] parallelVector = new double[2] { 1d, 1d };

            int dimension = parallelVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 10);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable lengthVar = new GP.Variable(initialLengthValue);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(lengthVar);

            GP.EnergyTypes.SegmentParallelity energyType = new GP.EnergyTypes.SegmentParallelity(parallelVector);
            solver.AddEnergy(energyType, new GP.IVariable[3] { startVar, endVar, lengthVar }, 1d);

            GP.ConstraintTypes.CoherentLength constraintType = new GP.ConstraintTypes.CoherentLength(dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[3] { startVar, endVar, lengthVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double segmentLength = 0d;
            for (int i = 0; i < dimension; i++) { segmentLength += resultingSegment[i] * resultingSegment[i]; }
            segmentLength = Math.Sqrt(segmentLength);


            double vectorLength = 0d;
            for (int i = 0; i < dimension; i++) { vectorLength += parallelVector[i] * parallelVector[i]; }
            vectorLength = Math.Sqrt(vectorLength);

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * parallelVector[i]; }
            dotProduct /= (segmentLength * vectorLength);

            Assert.AreEqual(1d, dotProduct, Settings.AbsolutePrecision);
        }


        /// <summary>
        /// Tests the method <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("Segment Parallelity 3D")]
        public void SegmentParallelity_3D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 3d, 3d };
            double[] endPoint = new double[3] { 2d, 4d, 1d };
            double initialLengthValue = 6.4;

            double[] parallelVector = new double[3] { 1d, 1d, 1d };

            int dimension = parallelVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 12);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable lengthVar = new GP.Variable(initialLengthValue);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(lengthVar);

            GP.EnergyTypes.SegmentParallelity energyType = new GP.EnergyTypes.SegmentParallelity(parallelVector);
            solver.AddEnergy(energyType, new GP.IVariable[3] { startVar, endVar, lengthVar }, 1d);

            GP.ConstraintTypes.CoherentLength constraintType = new GP.ConstraintTypes.CoherentLength(dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[3] { startVar, endVar, lengthVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double segmentLength = 0d;
            for (int i = 0; i < dimension; i++) { segmentLength += resultingSegment[i] * resultingSegment[i]; }
            segmentLength = Math.Sqrt(segmentLength);


            double vectorLength = 0d;
            for (int i = 0; i < dimension; i++) { vectorLength += parallelVector[i] * parallelVector[i]; }
            vectorLength = Math.Sqrt(vectorLength);

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * parallelVector[i]; }
            dotProduct /= (segmentLength * vectorLength);

            Assert.AreEqual(1d, dotProduct, Settings.AbsolutePrecision);
        }


        #endregion

        #region Segment Orthogonality

        /// <summary>
        /// Tests the method <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("Segment Orthogonality 2D")]
        public void SegmentOrthogonality_2D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[2] { 1d, 1d };
            double[] endPoint = new double[2] { 2d, 4d };
            double[] orthogonalVector = new double[2] { 1d, 1d };

            int dimension = orthogonalVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.EnergyTypes.SegmentOrthogonality energyType = new GP.EnergyTypes.SegmentOrthogonality(orthogonalVector);
            solver.AddEnergy(energyType, new GP.IVariable[2] { startVar, endVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * orthogonalVector[i]; }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the method <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("Segment Orthogonality 3D")]
        public void SegmentOrthogonality_3D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 3d, 0d };
            double[] endPoint = new double[3] { 2d, 4d, 0d };
            double[] orthogonalVector = new double[3] { 1d, 1d, 1d };

            int dimension = orthogonalVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.EnergyTypes.SegmentOrthogonality energyType = new GP.EnergyTypes.SegmentOrthogonality(orthogonalVector);
            solver.AddEnergy(energyType, new GP.IVariable[2] { startVar, endVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * orthogonalVector[i]; }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        #endregion
    }
}
