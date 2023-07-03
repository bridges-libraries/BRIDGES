using System;


namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Attribute informing that the method serialises a given element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class SerialiserAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Type serialised by the serialiser.
        /// </summary>
        private readonly Type serialisedType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type serialised by the serialiser.
        /// </summary>
        public Type SerialisedType { get => serialisedType; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SerialiserAttribute"/> attribute for a given type.
        /// </summary>
        /// <param name="serialisedType"> Type serialised. </param>
        public SerialiserAttribute(Type serialisedType)
        {
            this.serialisedType = serialisedType;
        }

        #endregion
    }
}
