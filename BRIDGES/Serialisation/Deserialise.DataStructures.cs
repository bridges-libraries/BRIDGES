using System;
using System.Numerics;
using System.Collections.Generic;

using Json = System.Text.Json;

using Meshes = BRIDGES.DataStructures.PolyhedralMeshes;


namespace BRIDGES.Serialisation
{
    public static partial class Deserialise
    {
        #region Polyhedral Meshes

        /// <summary>
        /// Deserialises a string representation to a <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="text"> String representation to deserialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> The deserialised <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> from the string representation. </returns>
        /// <exception cref="NotImplementedException"> The specified file format for the halfedge mesh deserialisation is not implemented. </exception>
        [Deserialiser(typeof(Meshes.HalfedgeMesh.Mesh<>))]
        public static Meshes.HalfedgeMesh.Mesh<TPosition> HalfedgeMesh<TPosition>(string text, Formats.PolyhedralMeshSerialisationFormat format)
            where TPosition : IEquatable<TPosition>,
                          IAdditionOperators<TPosition, TPosition, TPosition>,
                          IMultiplyOperators<TPosition, double, TPosition>, IDivisionOperators<TPosition, double, TPosition>
        {
            switch (format)
            {
                case Formats.PolyhedralMeshSerialisationFormat.Json:
                    return HalfedgeMeshFromJson<TPosition>(text);
                case Formats.PolyhedralMeshSerialisationFormat.Xml:
                    throw new NotImplementedException();
                case Formats.PolyhedralMeshSerialisationFormat.Obj:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException("The specified file format for the halfedge mesh deserialisation is not implemented.");
            }
        }

        /// <summary>
        /// Deserialises a string representation to a <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/>.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="text"> String representation to deserialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> The deserialised <see cref="Meshes.FaceVertexMesh.Mesh{TPosition}"/> from the string representation. </returns>
        [Deserialiser(typeof(Meshes.FaceVertexMesh.Mesh<>))]
        public static Meshes.FaceVertexMesh.Mesh<TPosition> FaceVertexMesh<TPosition>(string text, Formats.PolyhedralMeshSerialisationFormat format)
            where TPosition : IEquatable<TPosition>,
                          IAdditionOperators<TPosition, TPosition, TPosition>,
                          IMultiplyOperators<TPosition, double, TPosition>, IDivisionOperators<TPosition, double, TPosition>
        {
            switch (format)
            {
                case Formats.PolyhedralMeshSerialisationFormat.Json:
                    throw new NotImplementedException();
                case Formats.PolyhedralMeshSerialisationFormat.Xml:
                    throw new NotImplementedException();
                case Formats.PolyhedralMeshSerialisationFormat.Obj:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException("The specified file format for the face|vertex mesh deserialisation is not implemented.");
            }
        }


        /// <summary>
        /// Deserialises a json representation to a <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>.
        /// </summary>
        /// <typeparam name="TPosition"> Type of the vertex position in the <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/>. </typeparam>
        /// <param name="text"> Json representation to deserialise. </param>
        /// <returns> The deserialised <see cref="Meshes.HalfedgeMesh.Mesh{TPosition}"/> from the json representation. </returns>
        private static Meshes.HalfedgeMesh.Mesh<TPosition> HalfedgeMeshFromJson<TPosition>(string text)
            where TPosition : IEquatable<TPosition>,
                          IAdditionOperators<TPosition, TPosition, TPosition>,
                          IMultiplyOperators<TPosition, double, TPosition>, IDivisionOperators<TPosition, double, TPosition>
        {
            // Initialisation
            int newVertexIndex = 0, newHalfedgeIndex = 0, newFaceIndex = 0;
            Dictionary<int, Meshes.HalfedgeMesh.Vertex<TPosition>> vertices = new Dictionary<int, Meshes.HalfedgeMesh.Vertex<TPosition>>();
            Dictionary<int, Meshes.HalfedgeMesh.Halfedge<TPosition>> halfedges = new Dictionary<int, Meshes.HalfedgeMesh.Halfedge<TPosition>>();
            Dictionary<int, Meshes.HalfedgeMesh.Face<TPosition>> faces = new Dictionary<int, Meshes.HalfedgeMesh.Face<TPosition>>();

            Dictionary<int, int> unresolvedVerticesRef = new Dictionary<int, int>();
            Dictionary<int, string[]> unresolvedHalfedgesRef = new Dictionary<int, string[]>();


            // Parse text to a Json object for the queries
            using (Json.JsonDocument document = Json.JsonDocument.Parse(text))
            {
                Json.JsonElement root = document.RootElement;

                /******************** Add Vertices ********************/

                Json.JsonElement jVertices = root.GetProperty("Vertices");

                foreach (Json.JsonElement jVertex in jVertices.EnumerateArray())
                {
                    if (jVertex.ValueKind == Json.JsonValueKind.Null) { continue; }

                    // Add vertex to the Dictionary
                    jVertex.TryGetProperty("Index", out Json.JsonElement jIndex);
                    jIndex.TryGetInt32(out int index);

                    jVertex.TryGetProperty("Position", out Json.JsonElement jPosition);
                    TPosition position = Deserialise.Object<TPosition>(jPosition.GetRawText(), Formats.ObjectFormat.Json);

                    vertices.Add(index, new Meshes.HalfedgeMesh.Vertex<TPosition>(index, position));

                    // Store outgoing halfedge reference
                    if (TryGetPropertyReferenceIndex(jVertex, "OutgoingHalfedge", out int outgoingHeIndex))
                    {
                        unresolvedVerticesRef.Add(index, outgoingHeIndex);
                    }

                    // Manage new vertex index
                    if (!(index < newVertexIndex)) { newVertexIndex = index + 1; }
                }


                /******************** Add Halfedges ********************/

                Json.JsonElement jHalfedges = root.GetProperty("Halfedges");

                foreach (Json.JsonElement jHalfedge in jHalfedges.EnumerateArray())
                {
                    if (jHalfedge.ValueKind == Json.JsonValueKind.Null) { continue; }

                    // Add halfedge to the Dictionary
                    jHalfedge.TryGetProperty("Index", out Json.JsonElement jIndex);
                    jIndex.TryGetInt32(out int index);

                    TryGetPropertyReferenceIndex(jHalfedge, "StartVertex", out int startIndex);
                    Meshes.HalfedgeMesh.Vertex<TPosition> startVertex = vertices[startIndex];

                    TryGetPropertyReferenceIndex(jHalfedge, "EndVertex", out int endIndex);
                    Meshes.HalfedgeMesh.Vertex<TPosition> endVertex = vertices[endIndex];

                    halfedges.Add(index, new Meshes.HalfedgeMesh.Halfedge<TPosition>(index, startVertex, endVertex));

                    // Store outgoing halfedge reference

                    string[] indices = new string[4];
                    indices[0] = TryGetPropertyReferenceId(jHalfedge, "PrevHalfedge", out string prevHeIndex) ? prevHeIndex : null;
                    indices[1] = TryGetPropertyReferenceId(jHalfedge, "NextHalfedge", out string nextHeIndex) ? nextHeIndex : null;
                    indices[2] = TryGetPropertyReferenceId(jHalfedge, "PairHalfedge", out string pairHeIndex) ? pairHeIndex : null;
                    indices[3] = TryGetPropertyReferenceId(jHalfedge, "AdjacentFace", out string AdjFaceIndex) ? AdjFaceIndex : null;

                    unresolvedHalfedgesRef.Add(index, indices);

                    // Manage new halfedge index
                    if (!(index < newHalfedgeIndex)) { newHalfedgeIndex = index + 1; }
                }


                /******************** Add Faces ********************/

                Json.JsonElement jFaces = root.GetProperty("Faces");

                foreach (Json.JsonElement jFace in jFaces.EnumerateArray())
                {
                    if (jFace.ValueKind == Json.JsonValueKind.Null) { continue; }

                    // Add halfedge to the Dictionary
                    jFace.TryGetProperty("Index", out Json.JsonElement jIndex);
                    jIndex.TryGetInt32(out int index);

                    TryGetPropertyReferenceIndex(jFace, "FirstHalfedge", out int firstHeIndex);
                    Meshes.HalfedgeMesh.Halfedge<TPosition> firstHe = halfedges[firstHeIndex];

                    faces.Add(index, new Meshes.HalfedgeMesh.Face<TPosition>(index, firstHe));

                    // Manage new face index
                    if (!(index < newFaceIndex)) { newFaceIndex = index + 1; }
                }


                /******************** Resolve Vertices References ********************/

                foreach (KeyValuePair<int, int> kvp in unresolvedVerticesRef)
                {
                    vertices[kvp.Key].OutgoingHalfedge = halfedges[kvp.Value];
                }


                /******************** Resolve Halfedges References ********************/

                foreach (KeyValuePair<int, string[]> kvp in unresolvedHalfedgesRef)
                {
                    Meshes.HalfedgeMesh.Halfedge<TPosition> halfedge = halfedges[kvp.Key];

                    halfedge.PrevHalfedge = kvp.Value[0] is null ? null : halfedges[Convert.ToInt32(kvp.Value[0])];
                    halfedge.NextHalfedge = kvp.Value[1] is null ? null : halfedges[Convert.ToInt32(kvp.Value[1])];
                    halfedge.PairHalfedge = kvp.Value[2] is null ? null : halfedges[Convert.ToInt32(kvp.Value[2])];
                    halfedge.AdjacentFace = kvp.Value[3] is null ? null : faces[Convert.ToInt32(kvp.Value[3])];
                }
            }

            return new Meshes.HalfedgeMesh.Mesh<TPosition>(vertices, halfedges, faces, newVertexIndex, newHalfedgeIndex, newFaceIndex);


            /******************** Nested Methods ********************/

            bool TryGetPropertyReferenceId(Json.JsonElement element, string propertyName, out string referenceId)
            {
                if (element.TryGetProperty(propertyName, out Json.JsonElement jProperty)
                    && jProperty.ValueKind == Json.JsonValueKind.Object
                    && jProperty.TryGetProperty("$ref", out Json.JsonElement jRef))
                {
                    referenceId = jRef.GetString()?.Substring(1);

                    if (referenceId is null) { return false; }
                    else { return true; }
                }
                else { referenceId = null; return false; }
            }

            bool TryGetPropertyReferenceIndex(Json.JsonElement element, string propertyName, out int referenceIndex)
            {
                if (TryGetPropertyReferenceId(element, propertyName, out string referenceId))
                {
                    referenceIndex = Convert.ToInt32(referenceId);
                    return true;
                }
                else { referenceIndex = -1; return false; }
            }
        }

        #endregion
    }
}
