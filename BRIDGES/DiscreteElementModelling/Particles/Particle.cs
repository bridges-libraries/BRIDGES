using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Particle : ICloneable
    {
        #region Properties

        /// <summary>
        /// The mass of the particle
        /// </summary>
        public double Mass { get; set; } = 1.0;

        /// <summary>
        /// A boolean that keeps track of the change of position of the particle. Used for recalculation of the mesh influence areas.
        /// </summary>
        public bool Moved { get; set; } = true;

        /// <summary>
        /// The inverse of the mass
        /// </summary>
        public double MassInv
        {
            get
            {
                return (1 / Mass);
            }
        }

        /// <summary>
        /// particle velocity
        /// </summary>
        // public Vector3D Velocity { get; set; }


        public List<Vector> Velocity { get; set; }

        /// <summary>
        /// The position of the particle in the Euclidean space
        /// </summary>
        public ParticlePosition Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Point> PositionHistory { get; set; }

        /// <summary>
        /// The orientation of the particle in the Euclidean space
        /// </summary>
        public ParticleOrientation Orientation { get; set; }

        /// <summary>
        /// The forces applied to the particle. It is the dual value of Position
        /// </summary>
        public Vector AppliedForce { get; set; }

        /// <summary>
        /// The torques applied to the particle. It is the dual value of Orientation
        /// </summary>
        public Vector AppliedTorque { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSupport { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector ResidualForce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ForceDensity> NeighbourElements { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public Vector Normal { get; set; }

        /// <summary>
        /// Damping coefficient of the particle
        /// </summary>
        public double Damping { get; set; } = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of the class <see cref="Particle"/>.
        /// </summary>
        public Particle()
        {
        }

        /// <summary>
        /// Construct a particle from its position, orientation and applied forces
        /// </summary>
        /// <param name="particlePosition"></param>
        /// <param name="particleOrientation"></param>
        /// <param name="appliedForce"></param>
        /// <param name="appliedTorque"></param>
        public Particle(ParticlePosition particlePosition, ParticleOrientation particleOrientation, Vector appliedForce, Vector appliedTorque)
        {
            Position = particlePosition;
            Orientation = particleOrientation;
            AppliedForce = appliedForce;
            AppliedTorque = appliedTorque;
            ResidualForce = appliedForce;
        }
        /// <summary>
        /// Construct a particle from a point, the applied force, its support condition and its index in the meshVertex list
        /// </summary>
        /// <param name="point"></param>
        /// <param name="appliedForce"></param>
        /// <param name="isSupport"></param>
        /// <param name="index"></param>
        public Particle(Point point, Vector appliedForce, bool isSupport, int index)
        {
            Position = new ParticlePosition(point);
            PositionHistory = new List<Point> { point };
            IsSupport = isSupport;
            AppliedForce = appliedForce;
            // if (!isSupport) AppliedForce = appliedForce;
            //else AppliedForce = Vector3D.Zero;


            ResidualForce = AppliedForce;
            Index = index;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Apply an internal force from an element to the particle.
        /// </summary>
        /// <param name="internalForce"></param>
        public void ApplyInternalForce(Vector internalForce)
        {         
                ResidualForce += internalForce;
        }

        /// <summary>
        /// Clear the residual forces on the particle
        /// </summary>
        public void ClearResidualForce()
        {
            ResidualForce = AppliedForce;
        }

        /// <summary>
        /// Set the virtual mass of the particle to Zero
        /// </summary>
        public void ClearMass()
        {
            Mass = 0;
        }

        /// <summary>
        /// Initialise particle velocity
        /// </summary>
        public void InitialiseVelocity(double Dt, int integrationScheme)
        {
            List<Vector> velocities = new List<Vector>();

            if (!IsSupport)
                velocities.Add(Dt / 2 * ResidualForce * MassInv);

            //Add possibility for supports to move normal to the mesh            
            else
                velocities.Add(Dt / 2 * ResidualForce * Normal * MassInv * Normal);

            velocities.Add(-velocities.First());
            for (int i = 2; i < integrationScheme; i++) velocities.Add(Vector.Zero);

            Velocity = velocities;
        }

        /// <summary>
        /// Update particle position based on its velocity
        /// </summary>
        public void Update(double Dt)
        {
            if (!IsSupport)
            {
                var instantVelocity = Damping * Velocity[0] + Dt * MassInv * ResidualForce;

                for (int i = Velocity.Count() - 1; i > 0; i--)
                {
                    Velocity[i] = Velocity[i - 1];
                }
                Velocity[0] = instantVelocity;

                UpdatePosition(Dt);
            }

            else
            {
                var instantVelocity = Damping * Velocity[0] * Normal + Dt * MassInv * ResidualForce * Normal;

                for (int i = Velocity.Count() - 1; i > 0; i--)
                {
                    Velocity[i] = Velocity[i - 1];
                }
                //Velocity[0] = instantVelocity*Normal;
                Velocity[0] = Vector.Zero;
                UpdatePosition(Dt);
            }


        }

        /// <summary>
        /// Update position of the particle.
        /// </summary>
        /// <param name="Dt">The time step of the dynamic relaxation algorithm.</param>
        public void UpdatePosition(double Dt)
        {
            Position.Current += Dt * Velocity.First();
        }

        /// <summary>
        /// Update particle position at the peak
        /// </summary>
        /// <param name="peakTime"></param>
        /// <param name="Dt"></param>
        public void UpdateAtPeak(double peakTime, double Dt)
        {
            Position.Current += Dt * Velocity[0];//Calculate the position at t+Dt (after the peak)
            Position.Current = Position.Current - Dt * Velocity[0] - peakTime * Dt * Velocity[1];
        }
        #endregion

        #region ICloneable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Particle particle = new Particle();

            particle.Position = (ParticlePosition)Position.Clone();
            //particle.Orientation = (ParticleOrientation) Orientation.Clone(); 
            particle.AppliedForce = AppliedForce;
            particle.AppliedTorque = AppliedTorque;
            particle.ResidualForce = ResidualForce;
            particle.IsActive = IsActive;
            particle.NeighbourElements = NeighbourElements;
            particle.Index = Index;
            particle.IsSupport = IsSupport;
            particle.Velocity = Velocity;
            particle.Mass = Mass;
            particle.Normal = Normal;
            particle.Damping = Damping;

            if (!(PositionHistory is null))
                particle.PositionHistory = PositionHistory.Select(i => new Point(i)).ToList();


            return particle;
        }
        #endregion

    }
}
