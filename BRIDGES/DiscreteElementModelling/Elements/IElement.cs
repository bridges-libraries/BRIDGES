using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// Interface for an element for Dynamic Relaxation
    /// </summary>
    public interface IElement : ICloneable
    {
        /// <summary>
        /// True if the constraint affect the position of the element
        /// </summary>
        bool AffectsPosition { get; }

        /// <summary>
        /// True if the constraint affects the orientation of the element
        /// </summary>
        bool AffectsRotation { get; }

        /// <summary>
        /// Rigidity of the element used in the calculation of the fictitious mass of DR
        /// </summary>
        double Rigidity { get; set; }

        /// <summary>
        /// List of indices of particles connected to the element
        /// </summary>
        List<int> Indices { get; set; }


        /// <summary>
        /// Method implemented in Element class. Calculates all forces associated with the element.
        /// </summary>
        /// <param name="particles">List of the <see cref="Particle"/> of the model</param>
        void Calculate(List<Particle> particles);

        /// <summary>
        /// Applies calculated forces onto the affected particules
        /// </summary>
        /// <param name="particles"></param>
        void Apply(List<Particle> particles);



    }
}