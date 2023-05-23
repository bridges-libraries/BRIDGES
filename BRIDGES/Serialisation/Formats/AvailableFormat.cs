using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Serialisation.Formats
{
    /// <summary>
    /// Specifies all the file formats available in the framework.
    /// </summary>
    internal enum AvailableFormat : ushort
    {
        /// <summary>
        /// .JSON file format.
        /// </summary>
        Json = 0,
        /// <summary>
        /// .XML file format
        /// </summary>
        Xml = 1,
        /// <summary>
        /// .OBJ file format.
        /// </summary>
        Obj = 100,
    }
}
