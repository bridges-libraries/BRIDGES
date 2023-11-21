using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using GP = BRIDGES.Solvers.GuidedProjection;


namespace BRIDGES.Tests.Solvers.GuidedProjection
{
    /// <summary>
    /// Class testing the behavior of the <see cref="GP.Variable"/>, <see cref="GP.SubVariable"/>
    /// </summary>
    [TestClass]
    public class SubVariableTest
    {
        /// <summary>
        /// Epsilon value for the Tikhonov regularisation
        /// </summary>
        private static double Epsilon = 1e-4;


        #region SubVariable 1D

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.ScalarEquality"/>.
        /// </summary>
        [TestMethod("SubVariable 1D - ScalarEquality X")]
        public void SubVariable_1D_ScalarEquality_X()
        {
            // ----- Arrange ----- //
            double ztarget = 1d;
            double[] vector = new double[3] { 2d, 3d, 5d };

            int index = 0;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.SubVariable zSubVar = new GP.SubVariable(vectorVar, index, 1);

            GP.EnergyTypes.ScalarEquality energyType = new GP.EnergyTypes.ScalarEquality(ztarget);
            solver.AddEnergy(energyType, new GP.IVariable[1] { zSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            // ----- Assert ----- //

            Assert.AreEqual(vectorVar[index], zSubVar[0], Settings.AbsolutePrecision);

            Assert.AreEqual(ztarget, zSubVar[0], Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.ScalarEquality"/>.
        /// </summary>
        [TestMethod("SubVariable 1D - ScalarEquality Y")]
        public void SubVariable_1D_ScalarEquality_Y()
        {
            // ----- Arrange ----- //
            double ztarget = 1d;
            double[] vector = new double[3] { 2d, 3d, 5d };

            int index = 1;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.SubVariable zSubVar = new GP.SubVariable(vectorVar, index, 1);

            GP.EnergyTypes.ScalarEquality energyType = new GP.EnergyTypes.ScalarEquality(ztarget);
            solver.AddEnergy(energyType, new GP.IVariable[1] { zSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            // ----- Assert ----- //

            Assert.AreEqual(vectorVar[index], zSubVar[0], Settings.AbsolutePrecision);

            Assert.AreEqual(ztarget, zSubVar[0], Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.ScalarEquality"/>.
        /// </summary>
        [TestMethod("SubVariable 1D - ScalarEquality Z")]
        public void SubVariable_1D_ScalarEquality_Z()
        {
            // ----- Arrange ----- //
            double ztarget = 1d;
            double[] vector = new double[3] { 2d, 3d, 5d };

            int index = 2;

            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable vectorVar = new GP.Variable(vector);
            solver.TryAddVariable(vectorVar);

            GP.SubVariable zSubVar = new GP.SubVariable(vectorVar, index, 1);

            GP.EnergyTypes.ScalarEquality energyType = new GP.EnergyTypes.ScalarEquality(ztarget);
            solver.AddEnergy(energyType, new GP.IVariable[1] { zSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            // ----- Assert ----- //

            Assert.AreEqual(vectorVar[index], zSubVar[0], Settings.AbsolutePrecision);

            Assert.AreEqual(ztarget, zSubVar[0], Settings.AbsolutePrecision);
        }

        #endregion

        #region SubVariable 2D

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("SubVariable 2D - SegmentOrthogonality XY")]
        public void SubVariable_2D_SegmentOrthogonality_XY()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 1d, 2d };
            double[] endPoint = new double[3] { 2d, 6d, 5d };
            double[] orthogonalVector = new double[2] { 1d, 2d };

            int[] indices = new int[2] { 0, 1 };

            int dimension = orthogonalVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.SubVariable xyStartSubVar = new GP.SubVariable(startVar, indices);
            GP.SubVariable xyEndSubVar = new GP.SubVariable(endVar, indices);

            GP.EnergyTypes.SegmentOrthogonality energyType = new GP.EnergyTypes.SegmentOrthogonality(orthogonalVector);
            solver.AddEnergy(energyType, new GP.IVariable[2] { xyStartSubVar, xyEndSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = xyEndSubVar[i] - xyStartSubVar[i]; }

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * orthogonalVector[i]; }

            // ----- Assert ----- //
            for (int i = 0; i < indices.Length; i++)
            {
                Assert.AreEqual(startVar[indices[i]], xyStartSubVar[i], Settings.AbsolutePrecision);
                Assert.AreEqual(endVar[indices[i]], xyEndSubVar[i], Settings.AbsolutePrecision);
            }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("SubVariable 2D - SegmentOrthogonality YZ")]
        public void SubVariable_2D_SegmentOrthogonality_YZ()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 1d, 2d };
            double[] endPoint = new double[3] { 2d, 6d, 5d };
            double[] orthogonalVector = new double[2] { 2d, 1d };

            int[] indices = new int[2] { 1, 2 };

            int dimension = orthogonalVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.SubVariable xyStartSubVar = new GP.SubVariable(startVar, indices);
            GP.SubVariable xyEndSubVar = new GP.SubVariable(endVar, indices);

            GP.EnergyTypes.SegmentOrthogonality energyType = new GP.EnergyTypes.SegmentOrthogonality(orthogonalVector);
            solver.AddEnergy(energyType, new GP.IVariable[2] { xyStartSubVar, xyEndSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = xyEndSubVar[i] - xyStartSubVar[i]; }

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * orthogonalVector[i]; }

            // ----- Assert ----- //
            for (int i = 0; i < indices.Length; i++)
            {
                Assert.AreEqual(startVar[indices[i]], xyStartSubVar[i], Settings.AbsolutePrecision);
                Assert.AreEqual(endVar[indices[i]], xyEndSubVar[i], Settings.AbsolutePrecision);
            }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the class <see cref="GP.SubVariable"/> through the <see cref="GP.EnergyTypes.SegmentOrthogonality"/>.
        /// </summary>
        [TestMethod("SubVariable 2D - SegmentOrthogonality XZ")]
        public void SubVariable_2D_SegmentOrthogonality_XZ()
        {
            // ----- Arrange ----- //
            double[] startPoint = new double[3] { 2d, 1d, 2d };
            double[] endPoint = new double[3] { 2d, 6d, 5d };
            double[] orthogonalVector = new double[2] { 3d, 1d };

            int[] indices = new int[2] { 0, 2 };

            int dimension = orthogonalVector.Length;
            GP.Solver solver = new GP.Solver(Epsilon, 1);

            GP.Variable startVar = new GP.Variable(startPoint);
            GP.Variable endVar = new GP.Variable(endPoint);
            solver.TryAddVariable(startVar); solver.TryAddVariable(endVar);

            GP.SubVariable xyStartSubVar = new GP.SubVariable(startVar, indices);
            GP.SubVariable xyEndSubVar = new GP.SubVariable(endVar, indices);

            GP.EnergyTypes.SegmentOrthogonality energyType = new GP.EnergyTypes.SegmentOrthogonality(orthogonalVector);
            solver.AddEnergy(energyType, new GP.IVariable[2] { xyStartSubVar, xyEndSubVar }, 1d);

            // ----- Act ----- //
            solver.InitialiseX();
            for (int i = 0; i < solver.MaxIteration; i++)
            {
                solver.RunIteration(false);
            }

            double[] resultingSegment = new double[dimension];
            for (int i = 0; i < dimension; i++) { resultingSegment[i] = xyEndSubVar[i] - xyStartSubVar[i]; }

            double dotProduct = 0d;
            for (int i = 0; i < dimension; i++) { dotProduct += resultingSegment[i] * orthogonalVector[i]; }

            // ----- Assert ----- //
            for (int i = 0; i < indices.Length; i++)
            {
                Assert.AreEqual(startVar[indices[i]], xyStartSubVar[i], Settings.AbsolutePrecision);
                Assert.AreEqual(endVar[indices[i]], xyEndSubVar[i], Settings.AbsolutePrecision);
            }

            Assert.AreEqual(0d, dotProduct, Settings.AbsolutePrecision);
        }

        #endregion

    }
}
