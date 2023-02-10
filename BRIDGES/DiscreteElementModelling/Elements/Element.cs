using System;
using System.Collections.Generic;
using System.Text;
using BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// An abstract class of an element for Dynamic Relaxation
    /// </summary>
    public abstract class Element : IElement
    {
        #region Properties
        /// <summary>
        /// List of forces applied by the element to the particles.
        /// </summary>
        private List<Vector> _appliedForces = new List<Vector>();

        /// <summary>
        /// List of indices of the particles connected to the element.
        /// </summary>
        private List<int> _indices = new List<int>();

        /// <summary>
        /// number of particles connected to the element.
        /// </summary>
        private int _count
        {
            get
            {
                return (_indices.Count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _apply = true;

        /// <summary>
        /// List of indices of the particles connected to the element
        /// </summary>
        public List<int> Indices
        {
            get { return _indices; }
            set { }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Calculate forces applied to the particles connected to the element and test if the calculation is correct.
        /// </summary>
        /// <param name="particles">List of particles of the model.</param>
        public void Calculate(List<Particle> particles)
        {
            _apply = Calculate(particles, _indices, _appliedForces);
        }

        /// <summary>
        /// Abstract method implemented in the specific element class to calculate the forces applied to the particles connected to the element.
        /// </summary>
        /// <param name="particles">The list of particles of the model</param>
        /// <param name="indices"></param>
        /// <param name="_appliedForces"></param>
        /// <returns></returns>
        protected abstract bool Calculate(List<Particle> particles, List<int> indices, List<Vector> _appliedForces);

        /// <summary>
        /// Apply the internal force of the element on the particles.
        /// </summary>
        /// <param name="particles">List of particles of the model.</param>
        public void Apply(List<Particle> particles)
        {
            if (!_apply) return;

            for (int i = 0; i < _count; i++)
                particles[_indices[i]].ApplyInternalForce(_appliedForces[i]);
        }
        #endregion

        #region Inherited Properties
        /// <summary>
        /// 
        /// </summary>
        bool IElement.AffectsPosition
        {
            get { return true; }
        }
        /// <summary>
        /// 
        /// </summary>
        bool IElement.AffectsRotation
        {
            get { return false; }
        }

        /// <summary>
        /// Rigidity of the Element implemented in the lower instance of the element type
        /// </summary>
        public abstract double Rigidity { get; set; }

        #endregion

        #region ICloneable

        /// <summary>
        /// Create a new element that is a copy of the current instance
        /// </summary>
        public abstract object Clone();


        #endregion
    }
}