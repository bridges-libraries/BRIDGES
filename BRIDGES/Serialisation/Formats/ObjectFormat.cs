using System;


namespace BRIDGES.Serialisation.Formats
{
    /// <summary>
    /// Specifies the file formats available for any object in BRIDGES framework.
    /// </summary>
    public enum ObjectFormat : ushort
    {
        /// <summary>
        /// .JSON file format.
        /// </summary>
        Json = AvailableFormat.Json,
        /// <summary>
        /// .XML file format
        /// </summary>
        Xml = AvailableFormat.Xml,
    }
}
