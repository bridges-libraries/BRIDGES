using System;
using System.Reflection;

using Json = System.Text.Json;



namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Static class providing methods for the deserialisation of objects.
    /// </summary>
    public static partial class Deserialise
    {
        /// <summary>
        /// Deserialises a string representation to an <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T"> Type of the object to deserialise. </typeparam>
        /// <param name="text"> String representation to deserialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> The deserialised object  from the string representation. </returns>
        public static T Object<T>(string text, Formats.ObjectFormat format)
        {
            Type serialisedType = Helpers.FindClosestAttributeUsage(typeof(T), typeof(SerialisableAttribute));

            if (serialisedType is null)
            {
                if (format == Formats.ObjectFormat.Json)
                {
                    return Json.JsonSerializer.Deserialize<T>(text);
                }
                else { throw new NotImplementedException(); }
            }
            else
            {
                MethodInfo serialiser = FindDeserialiser(serialisedType);

                if (serialiser is null) { throw new MissingMethodException("The object is marked as serialisable but the serialiser method could not be found."); }
                else
                {

                    object[] parameters = new object[2] { text, (ushort)format };

                    return (T)serialiser.Invoke(typeof(Deserialise), parameters);
                }
            }
        }


        /// <summary>
        /// Find the metadata of the deserialiser method for the deserialisable type.
        /// </summary>
        /// <param name="deserialisableType"> Type whose deserialiser method is being searched. </param>
        /// <returns> The method metadata if it exists, <see langword="null"/> otherwise. </returns>
        private static MethodInfo FindDeserialiser(Type deserialisableType)
        {
            // Go through the public methods of the Deserialise class
            Type deserialiserClassType = typeof(Deserialise);
            foreach (MethodInfo publicMethods in deserialiserClassType.GetMethods())
            {
                DeserialiserAttribute deserialiserAttribute = (DeserialiserAttribute)publicMethods.GetCustomAttribute(typeof(DeserialiserAttribute));
                if (deserialiserAttribute?.DeserialisedType == deserialisableType) { return publicMethods; }
            }

            // Go through the assembly defining the type on which the attribute is defined.
            Assembly deserialisedTypeAssembly = Assembly.GetAssembly(deserialisableType);
            foreach (Type type in deserialisedTypeAssembly.GetTypes())
            {
                foreach (MethodInfo publicMethods in type.GetMethods())
                {
                    DeserialiserAttribute deserialiserAttribute = (DeserialiserAttribute)publicMethods.GetCustomAttribute(typeof(DeserialiserAttribute));
                    if (deserialiserAttribute?.DeserialisedType == deserialisableType) { return publicMethods; }
                }
            }

            return null;
        }
    }
}
