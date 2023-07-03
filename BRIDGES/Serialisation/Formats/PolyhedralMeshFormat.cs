using BRIDGES.Serialisation.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Specifies the file formats available for the meshes.
    /// </summary>
    public enum PolyhedralMeshSerialisationFormat : ushort
    {
        /// <summary>
        /// .JSON file format.
        /// </summary>
        Json = AvailableFormat.Json,
        /// <summary>
        /// .XML file format
        /// </summary>
        Xml = AvailableFormat.Xml,
        /// <summary>
        /// .OBJ file format.
        /// </summary>
        Obj = AvailableFormat.Obj,
    }
}
