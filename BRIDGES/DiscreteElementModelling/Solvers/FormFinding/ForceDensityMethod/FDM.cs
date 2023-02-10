using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

using BRIDGES.DiscreteElementModelling.Model;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace ENPC.Numerics.DiscreteElementModelling.Solvers
{
    /// <summary>
    /// 
    /// </summary>
    public class FDM
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<Particle> Particles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ForceDensity> Elements { get; set; }

        private Dictionary<int, int> _connectivityIndices;      

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, int> ConnectivityIndices
        {
            get => _connectivityIndices;
            set
            {
                _connectivityIndices = value;
            }
        }
        #endregion
        
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FDM(List<Particle> particles, List<ForceDensity> elements)
        {
            Particles = particles;
            Elements = elements;
            ConnectivityIndices = IndicesDual(particles);
        }
        #endregion

        #region Methods

        private Dictionary<int, int> IndicesDual(List<Particle> particles)
        {
            Dictionary<int, int> dico = new Dictionary<int, int>();
            int s = 0;  //indices of the supports
            int n = 0;  //indices of the free nodes
            foreach (Particle p in particles.Where(i=> i.IsActive))
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
        /// 
        /// </summary>
        public void FDMSolve(ref List<string> debug)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            Matrix<double> Cf = ConnectivityMatrixFixed(Particles, Elements);

            Matrix<double> C = ConnectivityMatrixFree(Particles, Elements);


            Matrix<double> Q = ForceDensityMatrix(Elements);


            Matrix<double> D = C.Transpose() * Q * C;
            Matrix<double> Df = C.Transpose() * Q * Cf;

            Matrix<double> Xf = SupportNodes(Particles);

            Matrix<double> P = ExternalLoads(Particles);

            Matrix<double> temp = P - Df * Xf;
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            debug.Add(String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10));


            stopWatch.Restart();

            Matrix<double> X = D.Solve(temp);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            debug.Add(String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10));

            foreach (Particle p in Particles.Where(i=>i.IsActive))
            {
                if (!p.IsSupport)
                {
                    p.Position.Current = new BRIDGES.Geometry.Euclidean3D.Point(X[_connectivityIndices[p.Index], 0], X[_connectivityIndices[p.Index], 1], X[_connectivityIndices[p.Index], 2]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FDMSolve()
        {           
            Matrix<double> Cf = ConnectivityMatrixFixed(Particles, Elements);

            Matrix<double> C = ConnectivityMatrixFree(Particles, Elements);


            Matrix<double> Q = ForceDensityMatrix(Elements);


            Matrix<double> D = C.Transpose() * Q * C;
            Matrix<double> Df = C.Transpose() * Q * Cf;

            Matrix<double> Xf = SupportNodes(Particles);

            Matrix<double> P = ExternalLoads(Particles);

            Matrix<double> temp = P - Df * Xf;
           
            Matrix<double> X = D.Solve(temp);
         
            foreach (Particle p in Particles.Where(i => i.IsActive))
            {
                if (!p.IsSupport)
                {
                    p.Position.Current = new BRIDGES.Geometry.Euclidean3D.Point(X[_connectivityIndices[p.Index], 0], X[_connectivityIndices[p.Index], 1], X[_connectivityIndices[p.Index], 2]);
                }
            }
        }

        private Matrix<double> ExternalLoads(List<Particle> particles)
        {
            int Nfree = particles.Where(i => (!i.IsSupport && i.IsActive)).Count();
            Matrix<double> matrix = DenseMatrix.Create(Nfree, 3, 0.0);
            foreach (Particle p in particles.Where(i => (!i.IsSupport && i.IsActive)))
            {
                matrix[_connectivityIndices[p.Index], 0] = p.AppliedForce.X;
                matrix[_connectivityIndices[p.Index], 1] = p.AppliedForce.Y;
                matrix[_connectivityIndices[p.Index], 2] = p.AppliedForce.Z;
            }
            return matrix;
        }
        private Matrix<double> SupportNodes(List<Particle> particles)
        {
            int Nf = particles.Where(i => (i.IsSupport && i.IsActive)).Count();

            Matrix<double> matrix = DenseMatrix.Create(Nf, 3, 0.0);
            foreach (Particle p in particles.Where(i => (i.IsSupport && i.IsActive)))
            {
                matrix[_connectivityIndices[p.Index], 0] = p.Position.Current.X;
                matrix[_connectivityIndices[p.Index], 1] = p.Position.Current.Y;
                matrix[_connectivityIndices[p.Index], 2] = p.Position.Current.Z;
            }
            return matrix;
        }

        private Matrix<double> ForceDensityMatrix(List<ForceDensity> elements)
        {
            int N = elements.Count;
            Matrix<double> matrix = DenseMatrix.Create(N, N, 0.0);
            int n = 0;
            foreach (IElement f in elements)
            {
                matrix[n, n] = f.Rigidity;
                n++;
            }
            return matrix;
        }
        private Matrix<double> ConnectivityMatrixFixed(List<Particle> particles, List<ForceDensity> elements)
        {
            int Nf = particles.Where(i => (i.IsSupport && i.IsActive)).Count();

            Matrix<double> matrix = DenseMatrix.Create(elements.Count, Nf, 0.0);

            for (int i = 0; i < elements.Count(); i++)
            {
                if (particles[elements[i].Indices[0]].IsSupport) matrix[i, _connectivityIndices[elements[i].Indices[0]]] = 1;

                if (particles[elements[i].Indices[1]].IsSupport) matrix[i, _connectivityIndices[elements[i].Indices[1]]] = -1;
            }

            return matrix;
        }

        private Matrix<double> ConnectivityMatrixFree(List<Particle> particles, List<ForceDensity> elements)
        {
            int Nfree = particles.Where(i => (!i.IsSupport && i.IsActive)).Count();
            Matrix<double> matrix = DenseMatrix.Create(elements.Count(), Nfree, 0.0);

            for (int i = 0; i < elements.Count(); i++)
            {
                if (!particles[elements[i].Indices[0]].IsSupport) matrix[i, _connectivityIndices[elements[i].Indices[0]]] = 1;

                if (!particles[elements[i].Indices[1]].IsSupport) matrix[i, _connectivityIndices[elements[i].Indices[1]]] = -1;
            }
            return matrix;
        } 
        #endregion
    }
}
