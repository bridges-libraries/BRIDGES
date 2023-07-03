using System;


namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Attribute attesting that a serialisation method is implemented for this element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    internal class SerialisableAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SerialisableAttribute"/> attribute for a given class or struct.
        /// </summary>
        public SerialisableAttribute()
        {
            /* Do Nothing */
        }

        #endregion
    }
}
