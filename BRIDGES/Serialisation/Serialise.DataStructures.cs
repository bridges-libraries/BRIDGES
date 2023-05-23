using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Json = System.Text.Json;

using Meshes = BRIDGES.DataStructures.PolyhedralMeshes;

namespace BRIDGES.Serialisation
{
    public static partial class Serialise
    {
        #region Polyhedral Meshes

        /// <summary>
        /// Serialises a <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> into a given file format.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="mesh"> <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> to serialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> A string representation of the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </returns>
        [Serialiser(typeof(Meshes.HalfedgeMesh.Mesh<>))]
        public static string HalfedgeMesh<TPosition>(Meshes.HalfedgeMesh.Mesh<TPosition> mesh, PolyhedralMeshSerialisationFormat format)
            where TPosition : IEquatable<TPosition>,
                          Algebra.Fundamentals.IAddable<TPosition> /* To Do : Remove */,
                          Algebra.Sets.IGroupAction<TPosition, double>
        {
            switch (format)
            {
                case PolyhedralMeshSerialisationFormat.Json:
                    return HalfedgeMeshToJson(mesh);
                case PolyhedralMeshSerialisationFormat.Xml:
                    throw new NotImplementedException();
                case PolyhedralMeshSerialisationFormat.Obj:
                    return PolyhedralMeshToObj(mesh);
                default:
                    throw new NotImplementedException("The specified file format for the halfedge mesh serialisation is not implemented.");
            }
        }

        /// <summary>
        /// Serialises a <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/> into a given file format.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="mesh"> <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/> to serialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> A string representation of the <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/>. </returns>
        [Serialiser(typeof(Meshes.FaceVertexMesh.Mesh<>))]
        public static string FaceVertexMesh<TPosition>(Meshes.FaceVertexMesh.Mesh<TPosition> mesh, PolyhedralMeshSerialisationFormat format)
            where TPosition : IEquatable<TPosition>,
                          Algebra.Fundamentals.IAddable<TPosition> /* To Do : Remove */,
                          Algebra.Sets.IGroupAction<TPosition, double>
        {
            switch (format)
            {
                case PolyhedralMeshSerialisationFormat.Json:
                    throw new NotImplementedException();
                case PolyhedralMeshSerialisationFormat.Xml:
                    throw new NotImplementedException();
                case PolyhedralMeshSerialisationFormat.Obj:
                    return PolyhedralMeshToObj(mesh);
                default:
                    throw new NotImplementedException("The specified file format for the face|vertex mesh serialisation is not implemented.");
            }
        }


        /// <summary>
        /// Serialises a <see cref="Meshes.IMesh{TPosition}"/> into a given file format.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.IMesh{TPosition}"/>. </typeparam>
        /// <param name="mesh"> <see cref="Meshes.IMesh{TPosition}"/> to serialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> A string representation of the <see cref="Meshes.IMesh{TPosition}"/>. </returns>
        public static string PolyhedralMesh<TPosition>(Meshes.IMesh<TPosition> mesh, PolyhedralMeshSerialisationFormat format)
            where TPosition : IEquatable<TPosition>,
                          Algebra.Fundamentals.IAddable<TPosition> /* To Do : Remove */,
                          Algebra.Sets.IGroupAction<TPosition, double>
        {
            switch (format)
            {
                case PolyhedralMeshSerialisationFormat.Json:
                    throw new NotImplementedException();
                case PolyhedralMeshSerialisationFormat.Xml:
                    throw new NotImplementedException();
                case PolyhedralMeshSerialisationFormat.Obj:
                    return PolyhedralMeshToObj(mesh);
                default:
                    throw new NotImplementedException("The specified file format for the polyhedral mesh serialisation is not implemented.");
            }
        }



        /// <summary>
        /// Serialises a halfedge mesh into a "json" formatted string.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="mesh"> <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> to serialise. </param>
        /// <returns> A json representation of the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </returns>
        private static string HalfedgeMeshToJson<TPosition>(Meshes.HalfedgeMesh.Mesh<TPosition> mesh)
            where TPosition : IEquatable<TPosition>,
                          Algebra.Fundamentals.IAddable<TPosition> /* To Do : Remove */,
                          Algebra.Sets.IGroupAction<TPosition, double>
        {
            var options = new Json.JsonWriterOptions { Indented = true };

            MemoryStream stream = new MemoryStream();
            Json.Utf8JsonWriter writer = new Json.Utf8JsonWriter(stream, options);


            /******************** Start Object ********************/

            writer.WriteStartObject();

            /******************** Write Vertices ********************/

            writer.WritePropertyName("Vertices");
            writer.WriteStartArray();

            foreach (Meshes.HalfedgeMesh.Vertex<TPosition> vertex in mesh.GetVertices())
            {
                // Write Key
                // writer.WritePropertyName(vertex.Index.ToString());

                // Write Value : the Vertex
                if (vertex is null) { writer.WriteNullValue(); continue; }

                writer.WriteStartObject();
                // Reference Id
                writer.WritePropertyName("$id");
                writer.WriteStringValue(String.Format("v{0}", vertex.Index));
                // Index
                writer.WritePropertyName("Index");
                writer.WriteNumberValue(vertex.Index);
                // OutgoingEdge
                writer.WritePropertyName("OutgoingHalfedge");
                if (vertex.OutgoingHalfedge is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("h{0}", vertex.OutgoingHalfedge.Index));
                    writer.WriteEndObject();
                }
                // Position
                writer.WritePropertyName("Position");
                writer.WriteRawValue(Serialise.Object(vertex.Position, Formats.ObjectFormat.Json));
                writer.WriteEndObject();
            }

            writer.WriteEndArray();


            /******************** Write Edges ********************/

            writer.WritePropertyName("Halfedges");
            writer.WriteStartArray();

            foreach (Meshes.HalfedgeMesh.Halfedge<TPosition> halfedge in mesh.GetHalfedges())
            {
                // Write Key
                // writer.WritePropertyName(halfedge.Index.ToString());

                // Write Value : the Halfedge
                if (halfedge is null) { writer.WriteNullValue(); continue; }

                writer.WriteStartObject();
                // Reference Id
                writer.WritePropertyName("$id");
                writer.WriteStringValue(String.Format("h{0}", halfedge.Index));

                // Index
                writer.WritePropertyName("Index");
                writer.WriteNumberValue(halfedge.Index);

                // Start Vertex 
                writer.WritePropertyName("StartVertex");

                writer.WriteStartObject();
                writer.WritePropertyName("$ref");
                writer.WriteStringValue(String.Format("v{0}", halfedge.StartVertex.Index));
                writer.WriteEndObject();

                // End Vertex
                writer.WritePropertyName("EndVertex");

                writer.WriteStartObject();
                writer.WritePropertyName("$ref");
                writer.WriteStringValue(String.Format("v{0}", halfedge.EndVertex.Index));
                writer.WriteEndObject();

                // Previous Halfedge
                writer.WritePropertyName("PrevHalfedge");
                if (halfedge.PrevHalfedge is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("h{0}", halfedge.PrevHalfedge.Index));
                    writer.WriteEndObject();
                }
                // Next Halfedge
                writer.WritePropertyName("NextHalfedge");
                if (halfedge.NextHalfedge is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("h{0}", halfedge.NextHalfedge.Index));
                    writer.WriteEndObject();
                }
                // Pair Edge
                writer.WritePropertyName("PairHalfedge");
                if (halfedge.PairHalfedge is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("h{0}", halfedge.PairHalfedge.Index));
                    writer.WriteEndObject();
                }
                // Adjacent Face
                writer.WritePropertyName("AdjacentFace");
                if (halfedge.AdjacentFace is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("f{0}", halfedge.AdjacentFace.Index));
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();


            /******************** Write Faces ********************/

            writer.WritePropertyName("Faces");
            writer.WriteStartArray();

            foreach (Meshes.HalfedgeMesh.Face<TPosition> face in mesh.GetFaces())
            {
                // Write Key
                // writer.WritePropertyName(face.Index.ToString());

                // Write Value : the Face
                if (face is null) { writer.WriteNullValue(); continue; }

                writer.WriteStartObject();

                // Reference Id
                writer.WritePropertyName("$id");
                writer.WriteStringValue(String.Format("f{0}", face.Index));

                // Index
                writer.WritePropertyName("Index");
                writer.WriteNumberValue(face.Index);

                // FirstEdge
                writer.WritePropertyName("FirstHalfedge");
                if (face.FirstHalfedge is null) { writer.WriteNullValue(); }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("$ref");
                    writer.WriteStringValue(String.Format("h{0}", face.FirstHalfedge.Index));
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();


            /******************** End Object ********************/

            writer.WriteEndObject();

            /******************** End Document ********************/
            writer.Flush();


            string json = Encoding.UTF8.GetString(stream.ToArray());

            return json;
        }


        /// <summary>
        /// Serialises a halfedge mesh into an "obj" formatted string.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="mesh"> <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> to serialise. </param>
        /// <returns> An "obj" representation of the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </returns>
        private static string PolyhedralMeshToObj<TPosition>(Meshes.IMesh<TPosition> mesh)
            where TPosition : IEquatable<TPosition>,
                          Algebra.Fundamentals.IAddable<TPosition> /* To Do : Remove */,
                          Algebra.Sets.IGroupAction<TPosition, double>
        {
            StringWriter sw = new StringWriter();

            // Title
            sw.WriteLine("o mesh");
            sw.WriteLine();

            // Storing vertices
            Dictionary<int, int> converter = new Dictionary<int, int>(mesh.VertexCount);
            foreach (Meshes.IVertex<TPosition> vertex in mesh.GetVertices())
            {
                converter.Add(vertex.Index, converter.Count + 1);

                sw.Write("v");

                Geometry.Kernel.IAnalytic<double> analytic = (Geometry.Kernel.IAnalytic<double>)vertex.Position ??
                    throw new ArithmeticException("The vertex position could not be converted to an analytic element (i.e. with coordinates");

                foreach (double coordinate in analytic.GetCoordinates())
                {
                    sw.Write(" {0}", coordinate);
                }
                sw.WriteLine();
            }
            sw.WriteLine();

            // Storing faces
            foreach (Meshes.IFace<TPosition> face in mesh.GetFaces())
            {
                sw.Write("f");
                foreach (Meshes.IVertex<TPosition> faceVertex in face.FaceVertices())
                {
                    sw.Write(" {0}", converter[faceVertex.Index]);
                }
                sw.WriteLine();
            }

            return sw.ToString();
        }

        #endregion
    }
}
