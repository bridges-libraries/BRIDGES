using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.Serialisation
{
    internal class Helpers
    {
        /// <summary>
        /// Finds the closest type decorated with a given attribute type, by going back through the ancestors.
        /// </summary>
        /// <param name="elementType"> Element type to inspect. </param>
        /// <param name="attributeType"> Attribute type to find. </param>
        /// <returns> The first ancestor type on wich the attribute is defined if it is defined , <see langword="null"/> otherwise. </returns>
        /// <exception cref="ArgumentNullException"> Either <paramref name="elementType"/> or <paramref name="attributeType"/> is <see langword="null"/>. </exception>
        /// <exception cref="ArgumentException">
        /// Either <paramref name="elementType"/> is not derived from <see cref="object"/>, or <paramref name="attributeType"/> is not derived from <see cref="Attribute"/>. 
        /// </exception>
        internal static Type FindClosestAttributeUsage(Type elementType, Type attributeType)
        {
            if (elementType == null) { throw new ArgumentNullException(nameof(elementType)); }
            if (attributeType == null) { throw new ArgumentNullException(nameof(attributeType)); }

            if (!(elementType is object)) { throw new ArgumentException("The element to inspect should derive from the object class", nameof(elementType)); }
            if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
            {
                throw new ArgumentException("The attribute type to look for should derive from the attribute class", nameof(attributeType));
            }

            Type type = elementType;
            while (type != typeof(object))
            {
                if (Attribute.IsDefined(type, attributeType, false))
                {
                    return type;
                }
                else { type = type.BaseType; }
            }

            return null;
        }
    }
}
