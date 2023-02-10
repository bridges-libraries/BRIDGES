using BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;
using BRIDGES.Geometry.Euclidean3D;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// An class describing a model used for Dynamic relaxation
    /// </summary>
    public class Model : ICloneable
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the class <see cref="Model"/>.
        /// </summary>
        public Model()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="particles"></param>
        /// <param name="elements"></param>
        public Model(List<Particle> particles, List<ForceDensity> elements)
        {
            Particles = particles;

            Elements = elements.Select(i => (IElement)i.Clone()).ToList();

            InitialPoints = particles.Select(i => i.Position.Current).ToList();
        }

        /// <summary>
        /// Construct a model from a mesh, a list of supports, a force density value and a uniform external load
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="supports"></param>
        /// <param name="forceDensityValue"></param>
        /// <param name="externalLoad"></param>
        public Model(Mesh<Point> mesh, List<Point> supports, double forceDensityValue, double externalLoad)    //todo[Implementation]: create External load class and support class
        {
            InitialMesh = mesh;
            Mesh = (Mesh<Point>)mesh.Clone();

            List<Point> points = new List<Point>();
            foreach (Vertex<Point> p in mesh.GetVertices())
                points.Add(p.Position);

            SurfaceDensity = new Vector(0, 0, externalLoad);
            bool[] isSupport = GetSupports(points, supports);


            List<Particle> particles = new List<Particle>();
            for (int i = 0; i < points.Count; i++)
            {
                Particle particle = new Particle(points[i], 1 * SurfaceDensity, isSupport[i], i);
                particles.Add(particle);
            }
            Particles = particles;
            AreaWeight = CalculateAreaWeight(mesh);
            InitialPoints = particles.Select(i => i.Position.Current).ToList();

            UpdateExternalForces();

            //Creation of elements from the mesh (no connectivity matrix)
            List<IElement> forceDensityElements = new List<IElement>();
            List<Halfedge<Point>> isVisited = new List<Halfedge<Point>>();

            foreach (Halfedge<Point> he in mesh.GetHalfedges())
            {
                if (!isVisited.Contains(he) && !(supports.Contains(he.EndVertex.Position) && supports.Contains(he.StartVertex.Position)))
                //if (!isVisited.Contains(he))
                {
                    forceDensityElements.Add(new ForceDensity(mesh, he, forceDensityValue));
                    isVisited.Add(he);
                    isVisited.Add(he.PairHalfedge);
                }
            }
            isVisited.Clear();
            Vector[] normals = CalculateNormals(Mesh);

            for (int j = 0; j < Particles.Count; j++) Particles[j].Normal = normals[j];

            Elements = forceDensityElements;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The list of Particles of the model
        /// </summary>
        public List<Particle> Particles { get; set; }

        /// <summary>
        /// The list of Elements of the model
        /// </summary>
        public List<IElement> Elements { get; set; }

        /// <summary>
        ///The loading area corresponding to each particle 
        /// </summary>
        public List<double> AreaWeight { get; set; }

        /// <summary>
        /// The thickness of the shell
        /// </summary>
        public double ShellThickness { get; set; }

        /// <summary>
        /// The loading area of each particle corresponding to the projected mesh on XY plane.
        /// </summary>
        public List<double> AreaWeightProjected { get; set; }

        /// <summary>
        /// Halfedhe Mesh of the model
        /// </summary>
        public Mesh<Point> Mesh { get; set; }

        /// <summary>
        /// Points of the initial mesh
        /// </summary>
        public List<Point> InitialPoints { get; set; }

        /// <summary>
        /// Initial position of all vertices of the mesh
        /// </summary>
        public List<Point> InitialMeshPosition
        {
            get
            {
                List<Point> points = new List<Point>();
                for (int i = 0; i < Mesh.GetVertices().Count(); i++)
                {
                    points.Add(InitialMesh.GetVertices()[i].Position);
                }
                return points;
            }
        }

        /// <summary>
        /// Initial Mesh of the model.
        /// </summary>
        public Mesh<Point> InitialMesh { get; set; }

        /// <summary>
        /// The surfacic load applied to the structure
        /// </summary>
        public Vector SurfaceDensity { get; set; } = new Vector(0, 0, 1);

        /// <summary>
        /// 
        /// </summary>
        public bool InEquilibrium
        {
            get
            {
                double total = 0;
                foreach (Particle p in Particles.Where(i => i.IsActive))
                {
                    if (!p.IsSupport) total += p.ResidualForce.SquaredLength();
                }
                return (total < 0.5);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double Anchor { get; set; } = 500;
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidualForces()
        {
            foreach (Particle p in Particles.Where(i => (i.IsActive))) p.ClearResidualForce();

            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Calculate(Particles);
            }
            for (int i = 0; i < Elements.Count; i++) Elements[i].Apply(Particles);
        }

        /// <summary>
        /// Returns an array of bool stating if particle i is support
        /// </summary>
        /// <param name="points">All vertices of the mesh</param>
        /// <param name="supports">Points of the mesh to be set as supports</param>
        /// <returns></returns>
        public bool[] GetSupports(List<Point> points, List<Point> supports)
        {
            bool[] isSupport = new bool[points.Count];

            for (int i = 0; i < points.Count; i++) isSupport[i] = supports.Contains(points[i]) ? true : false;

            return isSupport;
        }

        /// <summary>
        /// Reset particles at the position on the initial mesh
        /// </summary>
        public void ReinitialisePaticles()
        {
            for (int i = 0; i < Mesh.GetVertices().Count(); i++)
            {
                Particles[i].Position.Current = InitialMeshPosition[i];
                Particles[i].Moved = true;
            }
        }

        /// <summary>
        /// Reset particles at the position of the current Mesh
        /// </summary>
        public void UpdateParticlePosition()
        {
            for (int i = 0; i < Mesh.GetVertices().Count(); i++)
            {
                Particles[i].Position.Current = Mesh.GetVertices()[i].Position;
            }
            CalculateAreaWeight(Mesh);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetInitialMesh()
        {
            InitialMesh = (Mesh<Point>)Mesh.Clone();
        }

        /// <summary>
        /// Calculate the area of influence of each vertex of the mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="projected">true if the mesh is projected (and all particles have moved virtually)</param>
        /// <returns></returns>
        public List<double> CalculateAreaWeight(Mesh<Point> mesh, bool projected = false)
        {
            List<double> influenceAreas = new List<double>();

            foreach (Vertex<Point> v in mesh.GetVertices())
            {
                if (projected || Particles[v.Index].Moved)
                    influenceAreas.Add(v.InfluenceArea());

                else influenceAreas.Add(AreaWeight[v.Index]);

                Particles[v.Index].Moved = false;
            }
            return influenceAreas;
        }

        /// <summary>
        /// Create a line representation of a model
        /// </summary>
        /// <returns></returns>
        public List<Line> ModelToLine()
        {
            List<Line> allLines = new List<Line>();

            foreach (ForceDensity e in Elements.Where(i => i is ForceDensity))
            {
                Line tempLine = new Line(Particles[e.Indices[0]].Position.Current, Particles[e.Indices[1]].Position.Current);
                allLines.Add(tempLine);
            }

            return (allLines);
        }

        /// <summary>
        /// Set new values to the model force densities
        /// </summary>
        /// <param name="forceDensities"></param>
        public void UpdateForceDensities(List<double> forceDensities)
        {
            int i = 0;
            foreach (ForceDensity e in Elements.Where(j => j is ForceDensity))
            {
                e.Rigidity = forceDensities[i];
                i++;
            }
        }

        /// <summary>
        /// Update external forces based on the new area of influence of each vertex
        /// </summary>
        public void UpdateExternalForces()
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                //if (!Particles[i].IsSupport) Particles[i].AppliedForce = SurfaceDensity * AreaWeight[i];
                Particles[i].AppliedForce = SurfaceDensity * AreaWeight[i];
            }
        }

        /// <summary>
        /// Return an array of all force densities in the model
        /// </summary>
        /// <returns></returns>
        public double[] ForceDensities()
        {
            return (from e in Elements.Where(i => i is ForceDensity) select e.Rigidity).ToArray();    //todo[implementation]: Move ForceDensity related methods to an subclass or other interface.
        }

        /// <summary>
        /// Calculate the normals of the mesh at the vertices
        /// </summary>
        /// <param name="mesh">the mesh on which normal are calculated</param>
        /// <returns></returns>
        public Vector[] CalculateNormals(Mesh<Point> mesh)
        {
            List<Vector> normals = new List<Vector>();

            foreach (Vertex<Point> v in mesh.GetVertices())
            {
                normals.Add(v.Normal<Vertex<Point>>(true));
            }

            return normals.ToArray();
        }

        /// <summary>
        /// Update the position of the mesh from the new position of the particles
        /// </summary>
        public virtual void UpdateMesh()
        {

            foreach (Particle p in Particles.Where(i => i.IsActive))
            {
                Mesh.GetVertices()[p.Index].Position = p.Position.Current;
                p.Moved = true;
            }
            AreaWeight = CalculateAreaWeight(Mesh);

            Vector[] normals = CalculateNormals(Mesh);
            for (int j = 0; j < Mesh.GetVertices().Count; j++) Particles[j].Normal = normals[j];
        }

        /// <summary>
        /// Project a point on a Mesh along the normal of the mesh at the original point.
        /// </summary>
        public void ProjectMeshAlongNormal()
        {
            for (int i = 0; i < InitialMeshPosition.Count(); i++)
            {
                Vector drift = new Vector(InitialMeshPosition[i], Mesh.GetVertices()[i].Position);
                Vector projNormal = (drift * InitialMesh.GetVertices()[i].Normal<Vertex<Point>>()) * InitialMesh.GetVertices()[i].Normal<Vertex<Point>>();

                Vector proj = drift - projNormal;
                Particles[i].Position.Current = InitialMeshPosition[i] + proj;
            }
        }
        #endregion

        #region ICloneable

        /// <summary>
        /// Create a new Model which is a copy of the current instance.
        /// </summary>
        public virtual object Clone()
        {
            Model model = new Model();

            List<Particle> particles = new List<Particle>();
            foreach (Particle p in Particles) particles.Add((Particle)p.Clone());
            model.Particles = particles;

            List<IElement> elements = new List<IElement>();
            foreach (IElement e in Elements) elements.Add((IElement)e.Clone());
            model.Elements = elements;

            model.Mesh = (Mesh<Point>)Mesh.Clone();
            model.InitialMesh = (Mesh<Point>)Mesh.Clone();
            model.SurfaceDensity = SurfaceDensity;
            model.InitialPoints = InitialPoints;

            return model;
        }
        #endregion

    }
}
