using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BRIDGES.DiscreteElementModelling.Model;
using System.Diagnostics;

using CSparse;
using CSparse.Double;
using CSparse.Double.Factorization;
using CSparse.IO;

using MatrX = MathNet.Numerics.LinearAlgebra.Matrix<double>;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
using SparseMatrix = CSparse.Double.SparseMatrix;

namespace ENPC.Numerics.DiscreteElementModelling.Solvers
{
    /// <summary>
    /// Implementation of the Force Density Method as described in "The force density method for form finding and computation of general networks" 
    /// by Schek, doi:10.1016/0045-7825(74)90045-0
    /// </summary>
    public class ForceDensityMethod
    {
        #region fields
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, int> _connectivityIndices;

        /// <summary>
        /// 
        /// </summary>
        private CSparse.Factorization.SparseQR<double> _dSparseQR;

        /// <summary>
        /// 
        /// </summary>
        private CSparse.Storage.CompressedColumnStorage<double> _dfSparse;


        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<Particle> Particles { get; set; }

        /// <summary>
        /// The number of free particles in the current model.
        /// </summary>
        private int Nfree { get; }


        /// <summary>
        /// The number of fixed particles in the current model.
        /// </summary>
        private int Nsupport { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> Exit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, int> ConnectivityIndices
        {
            get => _connectivityIndices;
            set
            {
                _connectivityIndices = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private CSparse.Factorization.SparseQR<double> DSparseQR
        {
            get => _dSparseQR;
            set
            {
                _dSparseQR = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private CSparse.Storage.CompressedColumnStorage<double> DfSparse
        {
            get => _dfSparse;
            set
            {
                _dfSparse = value;
            }
        }

        private SparseMatrix ConnectivityMatrixFreeNodes { get; set; }

        private SparseMatrix ConnectivityMatrixSupport { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ForceDensityMethod(List<Particle> particles, List<ForceDensity> elements)
        {
            Particles = particles;
            ConnectivityIndices = IndicesDual(particles);
            Nfree = Particles.Where(i => !i.IsSupport && i.IsActive).Count();
            Nsupport = Particles.Where(i => i.IsSupport && i.IsActive).Count();

            //Create connectivity matrices Ca and Cb for free nodes and supports
            ConnectivityMatrix(Particles, elements, out SparseMatrix Ca, out SparseMatrix Cb);
            ConnectivityMatrixFreeNodes = Ca;
            ConnectivityMatrixSupport = Cb;
        }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="particles"></param>
        /// <returns></returns>
        protected Dictionary<int, int> IndicesDual(List<Particle> particles)
        {
            Dictionary<int, int> dico = new Dictionary<int, int>();
            int s = 0;  //indices of the supports
            int n = 0;  //indices of the free nodes
            foreach (Particle p in particles.Where(i => i.IsActive))
            {
                if (p.IsSupport)
                {
                    dico.Add(p.Index, s);
                    s++;
                }
                else
                {
                    dico.Add(p.Index, n);
                    n++;
                }
            }
            return dico;
        }

        /// <summary>
        /// Initialise all matrices that are independent of the particle positions
        /// </summary>
        public void InitialiseMatrices(List<ForceDensity> elements)
        {
            //Create a table of all force densities
            double[] q = elements.Select(i => i.ForceDensityValue).ToArray();

            //Create a diagonal sparse matrix from the force densities table
            var Qsparse = SparseMatrix.OfDiagonalArray(q);

            var dSparse = ConnectivityMatrixFreeNodes.Transpose().Multiply(Qsparse).Multiply(ConnectivityMatrixFreeNodes);
            _dSparseQR = SparseQR.Create(dSparse, ColumnOrdering.MinimumDegreeAtA);

            _dfSparse = ConnectivityMatrixFreeNodes.Transpose().Multiply(Qsparse).Multiply(ConnectivityMatrixSupport);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FDMSolve()
        {
            //Create Matrix of support nodes positions
            var xf = new double[Nsupport];
            var yf = new double[Nsupport];
            var zf = new double[Nsupport];

            //Create Matrix of external forces
            var Px = new double[Nfree];
            var Py = new double[Nfree];
            var Pz = new double[Nfree];

            foreach (Particle p in Particles.Where(i => i.IsActive))
            {
                if (p.IsSupport)
                {
                    xf[_connectivityIndices[p.Index]] = p.Position.Current.X;
                    yf[_connectivityIndices[p.Index]] = p.Position.Current.Y;
                    zf[_connectivityIndices[p.Index]] = p.Position.Current.Z;
                }
                else
                {
                    Px[_connectivityIndices[p.Index]] = p.AppliedForce.X;
                    Py[_connectivityIndices[p.Index]] = p.AppliedForce.Y;
                    Pz[_connectivityIndices[p.Index]] = p.AppliedForce.Z;
                }
            }

            double[] X = new double[Nfree];
            double[] Y = new double[Nfree];
            double[] Z = new double[Nfree];

            //Calculate C'QCsXs
            DfSparse.Multiply(xf, X);
            DfSparse.Multiply(yf, Y);
            DfSparse.Multiply(zf, Z);

            //Calculate (P - C'QCsXs)
            X = Enumerable.Range(0, Nfree).Select(i => Px[i] - X[i]).ToArray();
            Y = Enumerable.Range(0, Nfree).Select(i => Py[i] - Y[i]).ToArray();
            Z = Enumerable.Range(0, Nfree).Select(i => Pz[i] - Z[i]).ToArray();

            //Solve Ax = b
            DSparseQR.Solve(X, Px);
            DSparseQR.Solve(Y, Py);
            DSparseQR.Solve(Z, Pz);


            //Move particles position
            UpdateParticlePosition(Px, Py, Pz);
        }

        /// <summary>
        /// Update particle position
        /// </summary>
        /// <param name="Px"></param>
        /// <param name="Py"></param>
        /// <param name="Pz"></param>
        private void UpdateParticlePosition(double[] Px, double[] Py, double[] Pz)
        {
            foreach (Particle p in Particles.Where(i => i.IsActive))
            {
                if (!p.IsSupport)
                    p.Position.Current = new BRIDGES.Geometry.Euclidean3D.Point(Px[_connectivityIndices[p.Index]], Py[_connectivityIndices[p.Index]], Pz[_connectivityIndices[p.Index]]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="particles"></param>
        /// <param name="elements"></param>
        /// <param name="C"></param>
        /// <param name="Cf"></param>
        private void ConnectivityMatrix(List<Particle> particles, List<ForceDensity> elements, out SparseMatrix C, out SparseMatrix Cf)
        {
            int count = elements.Count();

            List<double> valuesC = new List<double>();
            List<double> valuesCf = new List<double>();

            Dictionary<int, int> Cfree = new Dictionary<int, int>();
            Dictionary<int, int> Csupport = new Dictionary<int, int>();

            List<int> rowIndicesC = new List<int>();
            List<int> rowIndicesCf = new List<int>();

            int[] columnPointersC = new int[Nfree + 1];
            int[] columnPointersCf = new int[Nsupport + 1];

            columnPointersC[0] = 0;
            columnPointersCf[0] = 0;

            int element = 0;


            foreach (Particle p in particles.Where(i => i.IsActive))
            {

                for (int e = 0; e < count; e++)
                {
                    if (elements[e].Indices.Contains(p.Index))
                    {
                        int i = Enumerable.Range(0, 2).Where(f => elements[e].Indices[f] == p.Index).First();

                        if (!p.IsSupport)
                        {
                            valuesC.Add(-2 * i + 1);
                            rowIndicesC.Add(e);
                            //columnPointersC[_connectivityIndices[j] + 1]++;
                            for (int k = _connectivityIndices[p.Index]; k < Nfree; k++) columnPointersC[k + 1]++;
                        }

                        else
                        {
                            valuesCf.Add(-2 * i + 1);
                            rowIndicesCf.Add(e);
                            //columnPointersCf[_connectivityIndices[j] + 1]++;
                            for (int k = _connectivityIndices[p.Index]; k < Nsupport; k++) columnPointersCf[k + 1]++;
                        }

                    }
                }

            }

            C = new SparseMatrix(count, Nfree, valuesC.ToArray(), rowIndicesC.ToArray(), columnPointersC);
            Cf = new SparseMatrix(count, Nsupport, valuesCf.ToArray(), rowIndicesCf.ToArray(), columnPointersCf);
        }
        #endregion
    }
}
