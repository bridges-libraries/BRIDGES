using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using GP = BRIDGES.Solvers.GuidedProjection;


namespace BRIDGES.Tests.Solvers.GuidedProjection
{
    /// <summary>
    /// Class testing <see cref="GP.Abstracts.ConstraintType"/> in <see cref="GP.ConstraintTypes"/>.
    /// </summary>
    [TestClass]
    public class ConstraintTypesTest
    {
        /// <summary>
        /// Epsilon value for the Tikhonov regularisation
        /// </summary>
        private static double Epsilon = 1e-4;


        #region Lower Bound

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.LowerBound"/>.
        /// </summary>
        [TestMethod("Lower Bound")]
        public void LowerBound()
        {
            // ----- Arrange ----- //
            double lowerBound = 2d;
            double initialValue = -3d ;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable valueVar = new GP.Variable(initialValue);
            GP.Variable dummyVar = new GP.Variable(0d);
            solver.TryAddVariable(valueVar); solver.TryAddVariable(dummyVar);

            GP.ConstraintTypes.LowerBound constraintType = new GP.ConstraintTypes.LowerBound(lowerBound);
            solver.AddConstraint(constraintType, new GP.IVariable[2] { valueVar, dummyVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double resultingValue = valueVar[0];

            // ----- Assert ----- //

            Assert.IsTrue(lowerBound - Settings.AbsolutePrecision <= resultingValue,
                $"The resulting value ({resultingValue}) smaller than the lower bound specified ({lowerBound})");
        }

        #endregion

        #region Upper Bound

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.LowerBound"/>.
        /// </summary>
        [TestMethod("Upper Bound")]
        public void UpperBound()
        {
            // ----- Arrange ----- //
            double upperBound = -2.3;
            double initialValue = 6.5;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable valueVar = new GP.Variable(initialValue);
            GP.Variable dummyVar = new GP.Variable(0d);
            solver.TryAddVariable(valueVar); solver.TryAddVariable(dummyVar);

            GP.ConstraintTypes.UpperBound constraintType = new GP.ConstraintTypes.UpperBound(upperBound);
            solver.AddConstraint(constraintType, new GP.IVariable[2] { valueVar, dummyVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double resultingValue = valueVar[0];

            // ----- Assert ----- //

            Assert.IsTrue(resultingValue <= upperBound + Settings.AbsolutePrecision, 
                $"The resulting value ({resultingValue}) larger than the upper bound specified ({upperBound})");
        }

        #endregion

        #region Upper And Lower Bound

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.LowerBound"/> and <see cref="GP.ConstraintTypes.UpperBound"/>..
        /// </summary>
        [TestMethod("Upper And Lower Bound")]
        public void UpperAndLowerBound()
        {
            // ----- Arrange ----- //
            double lowerBound = -4.8;
            double upperBound = lowerBound;
            double initialValue = 6.5;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable valueVar = new GP.Variable(initialValue);
            GP.Variable dummyVar_Low = new GP.Variable(0d);
            GP.Variable dummyVar_Up = new GP.Variable(0d);
            solver.TryAddVariable(valueVar); solver.TryAddVariable(dummyVar_Low); solver.TryAddVariable(dummyVar_Up);

            GP.ConstraintTypes.UpperBound lowerConstraintType = new GP.ConstraintTypes.UpperBound(upperBound);
            solver.AddConstraint(lowerConstraintType, new GP.IVariable[2] { valueVar, dummyVar_Low }, 1d);

            GP.ConstraintTypes.UpperBound upperConstraintType = new GP.ConstraintTypes.UpperBound(upperBound);
            solver.AddConstraint(upperConstraintType, new GP.IVariable[2] { valueVar, dummyVar_Up }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double resultingValue = valueVar[0];

            // ----- Assert ----- //

            Assert.IsTrue(lowerBound - Settings.AbsolutePrecision <= resultingValue,
                $"The resulting value ({resultingValue}) smaller than the lower bound specified ({lowerBound})");
            Assert.IsTrue(resultingValue <= upperBound + Settings.AbsolutePrecision,
                $"The resulting value ({resultingValue}) larger than the upper bound specified ({upperBound})");
        }

        #endregion


        #region Vector Length

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.VectorLength"/>.
        /// </summary>
        [TestMethod("Vector Length 1D")]
        public void VectorLength_1D()
        {
            // ----- Arrange ----- //
            double targetLength = 5d;
            double[] vector = new double[1] { 2d };

            int dimension = vector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 5);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.ConstraintTypes.VectorLength constraintType = new GP.ConstraintTypes.VectorLength(targetLength, dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[1] { vectorVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingVector = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingVector[i] = vectorVar[i]; }

            // ----- Assert ----- //

            double resultingLength = 0d;
            for (int i = 0; i < dimension; i++) { resultingLength += resultingVector[i] * resultingVector[i]; }
            resultingLength = Math.Sqrt(resultingLength);

            Assert.AreEqual(targetLength, resultingLength, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.VectorLength"/>.
        /// </summary>
        [TestMethod("Vector Length 2D")]
        public void VectorLength_2D()
        {
            // ----- Arrange ----- //
            double targetLength = 5.5;
            double[] vector = new double[3] { 8d, 3d, 0d };

            int dimension = vector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 5);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.ConstraintTypes.VectorLength constraintType = new GP.ConstraintTypes.VectorLength(targetLength, dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[1] { vectorVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingVector = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingVector[i] = vectorVar[i]; }

            // ----- Assert ----- //

            double resultingLength = 0d;
            for (int i = 0; i < dimension; i++) { resultingLength += resultingVector[i] * resultingVector[i]; }
            resultingLength = Math.Sqrt(resultingLength);

            Assert.AreEqual(targetLength, resultingLength, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.VectorLength"/>.
        /// </summary>
        [TestMethod("Vector Length 3D")]
        public void VectorLength_3D()
        {
            // ----- Arrange ----- //
            double targetLength = 1d;
            double[] vector = new double[3] { 2d, 3d, 0d };

            int dimension = vector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 5);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.ConstraintTypes.VectorLength constraintType = new GP.ConstraintTypes.VectorLength(targetLength, dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[1] { vectorVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingVector = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingVector[i] = vectorVar[i]; }

            // ----- Assert ----- //

            double resultingLength = 0d;
            for (int i = 0; i < dimension; i++) { resultingLength += resultingVector[i] * resultingVector[i]; }
            resultingLength = Math.Sqrt(resultingLength);

            Assert.AreEqual(targetLength, resultingLength, Settings.AbsolutePrecision);
        }

        #endregion


        #region Coherent Length

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.CoherentLength"/>.
        /// </summary>
        [TestMethod("Coherent Length 2D")]
        public void CoherentLength_2D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[2] { 2d, 3d };
            double[] endPoint = new double[2] { 1d, 4d };
            double initialLengthValue = 3.2;

            int dimension = startPoint.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 4);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable lengthVar = new GP.Variable(initialLengthValue);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(lengthVar);

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
            double resultingLength = lengthVar[0];

            // ----- Assert ----- //

            double segmentLength = 0d;
            for (int i = 0; i < dimension; i++) { segmentLength += resultingSegment[i] * resultingSegment[i]; }
            segmentLength = Math.Sqrt(segmentLength);

            Assert.AreEqual(resultingLength, segmentLength, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.CoherentLength"/>.
        /// </summary>
        [TestMethod("Coherent Length 3D")]
        public void CoherentLength_3D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 3d, 2d };
            double[] endPoint = new double[3] { 1d, 4d, 0d };
            double initialLengthValue = 12.5;

            int dimension = startPoint.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 4);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable lengthVar = new GP.Variable(initialLengthValue);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(lengthVar);

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
            double resultingLength = lengthVar[0];

            // ----- Assert ----- //

            double segmentLength = 0d;
            for (int i = 0; i < dimension; i++) { segmentLength += resultingSegment[i] * resultingSegment[i]; }
            segmentLength = Math.Sqrt(segmentLength);

            Assert.AreEqual(resultingLength, segmentLength, Settings.AbsolutePrecision);
        }

        #endregion

        #region Segment Length

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.SegmentLength"/>.
        /// </summary>
        [TestMethod("Segment Length 2D")]
        public void SegmentLength_2D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[2] { 2d, 3d };
            double[] endPoint = new double[2] { 1d, 4d };
            double targetLength = 3.2;

            int dimension = startPoint.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 5);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.ConstraintTypes.SegmentLength constraintType = new GP.ConstraintTypes.SegmentLength(dimension, targetLength);
            solver.AddConstraint(constraintType, new GP.IVariable[2] { startVar, endVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double length = 0d;
            for (int i = 0; i < dimension; i++) { length += resultingSegment[i] * resultingSegment[i]; }
            length = Math.Sqrt(length);

            Assert.AreEqual(targetLength, length, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.SegmentLength"/>.
        /// </summary>
        [TestMethod("Segment Length 3D")]
        public void SegmentLength_3D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 3d, 2d };
            double[] endPoint = new double[3] { 1d, 4d, 0d };
            double targetLength = 9.5;

            int dimension = startPoint.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 5);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.ConstraintTypes.SegmentLength constraintType = new GP.ConstraintTypes.SegmentLength(dimension, targetLength);
            solver.AddConstraint(constraintType, new GP.IVariable[2] { startVar, endVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = endVar[i] - startVar[i]; }

            // ----- Assert ----- //

            double length = 0d;
            for (int i = 0; i < dimension; i++) { length += resultingSegment[i] * resultingSegment[i]; }
            length = Math.Sqrt(length);

            Assert.AreEqual(targetLength, length, Settings.AbsolutePrecision);
        }

        #endregion

        #region Segment Orthogonality

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("SegmentOrthogonality 2D")]
        public void SegmentOrthogonality_2D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[2] { 1d, 1d };
            double[] endPoint = new double[2] { 2d, 4d };
            double[] vector = new double[2] { 1d, 1d };

            int dimension = vector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 3);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(vectorVar);

            GP.ConstraintTypes.SegmentOrthogonality constraintType = new GP.ConstraintTypes.SegmentOrthogonality(dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[3] { startVar, endVar, vectorVar }, 1d);

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
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * vectorVar[i]; }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the constraint <see cref="GP.ConstraintTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("SegmentOrthogonality 3D")]
        public void SegmentOrthogonality_3D()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 3d, 0d };
            double[] endPoint = new double[3] { 2d, 4d, 0d };
            double[] vector = new double[3] { 1d, 1d, 1d };

            int dimension = vector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 3);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar); solver.TryAddVariable(vectorVar);

            GP.ConstraintTypes.SegmentOrthogonality constraintType = new GP.ConstraintTypes.SegmentOrthogonality(dimension);
            solver.AddConstraint(constraintType, new GP.IVariable[3] { startVar, endVar, vectorVar }, 1d);

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
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * vectorVar[i]; }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        #endregion
    }
}
