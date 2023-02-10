using System.Linq;
using System.Collections.Generic;
using BRIDGES.Geometry.Euclidean3D;
//using BRIDGES.DataStructures.PolyhedralMeshes;
using BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// A linear element with a force density 
    /// </summary>
    public class ForceDensity : Element, IElement
    {
        #region Properties
        /// <summary>
        /// Rigidity of the Element "Force Density" is equal to the force density value K=q=F/x
        /// </summary>
        public override double Rigidity
        {
            get => ForceDensityValue; set => ForceDensityValue = value;
        }

        /// <summary>
        /// Force density value of the element.
        /// </summary>
        public double ForceDensityValue { get; set; }

        /// <summary>
        /// Direction of the element. 0 for longitudinal, 1 for transversal.
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// Length of the element.
        /// </summary>
        public double Length { get; private set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="ForceDensity"/> class
        /// </summary>
        public ForceDensity()
        {
            ForceDensityValue = new double();
            Indices = new List<int>();
            Length = new double();
        }

        /// <summary>
        /// Construct a Force Density Element from two indices of Particles and a force density value.
        /// </summary>
        /// <param name="index0">Index of first particle.</param>
        /// <param name="index1">Index of second element.</param>
        /// <param name="forceDensity">Value of the force density of the element.</param>
        /// <param name="length">Length of the element.</param>       
        public ForceDensity(int index0, int index1, double forceDensity, double length)
        {
            ForceDensityValue = forceDensity;
            Indices.Add(index0);
            Indices.Add(index1);
            Length = length;
        }

        /// <summary>
        /// Construct a Force Density Element directly from a mesh with a <see cref="HeMesh"/> data structure.
        /// </summary>
        /// <param name="mesh">The <see cref="HeMesh"/> in the model.</param>
        /// <param name="halfEdge">The <see cref="HeHalfedge"/> used for creating the element.</param>
        /// <param name="forceDensityValue">the force density value of the element.</param>
        public ForceDensity(Mesh<Point> mesh, Halfedge<Point> halfEdge, double forceDensityValue)
        {
            ForceDensityValue = forceDensityValue;
            Indices.Add(mesh.GetVertices().ToList().FindIndex(x => x == halfEdge.StartVertex));
            Indices.Add(mesh.GetVertices().ToList().FindIndex(x => x == halfEdge.EndVertex));
            Length = ((Vector)(halfEdge.EndVertex.Position - halfEdge.StartVertex.Position)).Length();           
        }

        /// <summary>
        /// Construct a Force Density Element directly from a mesh with a <see cref="HeMesh"/> data structure.
        /// </summary>
        /// <param name="mesh">The <see cref="HeMesh"/> in the model.</param>
        /// <param name="halfEdge">The <see cref="HeHalfedge"/> used for creating the element.</param>
        /// <param name="forceDensityValue">the force density value of the element.</param>
        /// <param name="direction"></param>
        public ForceDensity(Mesh<Point> mesh, Edge<Point> halfEdge, double forceDensityValue, int direction)
        {
            ForceDensityValue = forceDensityValue;
            Indices.Add(mesh.GetVertices().ToList().FindIndex(x => x == halfEdge.StartVertex));
            Indices.Add(mesh.GetVertices().ToList().FindIndex(x => x == halfEdge.EndVertex));
            Length = ((Vector)(halfEdge.EndVertex.Position - halfEdge.StartVertex.Position)).Length();
            Direction = direction;
        }

        #endregion

        #region Inherited Methods
        /// <summary>
        /// Calculate forces applied to the particles connected to the element.
        /// </summary>
        /// <param name="particles">List of particles in the model.</param>
        /// <param name="indices">indices of the particles of the model connected to the element.</param>
        /// <param name="_appliedForces">List of forces applied to the particles connected to the element.</param>
        /// <returns>True if the calculation was successful.</returns>
        protected override bool Calculate(List<Particle> particles, List<int> indices, List<Vector> _appliedForces)
        {
            int n = indices.Count;
            if (n > 2) return (false);

            _appliedForces.Clear();
            _appliedForces.Add(ForceDensityValue * (new Vector(particles[indices[0]].Position.Current, particles[indices[1]].Position.Current)));
            _appliedForces.Add(ForceDensityValue * (new Vector(particles[indices[1]].Position.Current, particles[indices[0]].Position.Current)));

            return true;
        }

        /// <summary>
        /// Creates a new Element that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ForceDensity forceDensity = new ForceDensity();

            forceDensity.ForceDensityValue = ForceDensityValue;
            forceDensity.Indices.Add(Indices[0]);
            forceDensity.Indices.Add(Indices[1]);
            forceDensity.Length = Length;
            forceDensity.Direction = Direction;

            return forceDensity;
        }
        #endregion
    }
}
