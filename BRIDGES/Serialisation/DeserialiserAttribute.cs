using System;


namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Attribute informing that the method deserialises a given element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class DeserialiserAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Type deserialised by the deserialiser.
        /// </summary>
        private readonly Type deserialisedType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type deserialised by the deserialiser.
        /// </summary>
        public Type DeserialisedType { get => deserialisedType; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DeserialiserAttribute"/> attribute for a given type.
        /// </summary>
        /// <param name="deserialisedType"> Type deserialised. </param>
        public DeserialiserAttribute(Type deserialisedType)
        {
            this.deserialisedType = deserialisedType;
        }

        #endregion
    }
}
