using System;
using System.Reflection;

using Json = System.Text.Json;


namespace BRIDGES.Serialisation
{
    /// <summary>
    /// Static class providing methods for the serialisation of objects.
    /// </summary>
    public static partial class Serialise
    {
        /// <summary>
        /// Serialises an object into a given file format.
        /// </summary>
        /// <param name="obj"> Object to serialise. </param>
        /// <param name="format"> Format of the serialisation. </param>
        /// <returns> A string representation of the object. </returns>
        /// <exception cref="MissingMethodException"> The object is marked as serialisable but the serialiser method could not be found. </exception>
        public static string Object(object obj, Formats.ObjectFormat format)
        {
            Type objType = obj.GetType();

            Type serialisedType = Helpers.FindClosestAttributeUsage(objType, typeof(SerialisableAttribute));

            if (serialisedType is null)
            {
                if(format == Formats.ObjectFormat.Json)
                { 
                    Json.JsonSerializerOptions options = new Json.JsonSerializerOptions() { WriteIndented = false };
                    return Json.JsonSerializer.Serialize(obj, options);
                }
                else { throw new NotImplementedException(); }
            }
            else
            {
                MethodInfo serialiser = FindSerialiser(serialisedType);

                if (serialiser is null) { throw new MissingMethodException("The object is marked as serialisable but the serialiser method could not be found."); }
                else
                {

                    object[] parameters = new object[2] { obj, (ushort)format };

                    return (string)serialiser.Invoke(typeof(Serialise), parameters);
                }
            }
        }


        /// <summary>
        /// Find the metadata of the serialiser method for the serialisable type.
        /// </summary>
        /// <param name="serialisableType"> Type whose serialiser method is being searched. </param>
        /// <returns> The method metadata if it exists, <see langword="null"/> otherwise. </returns>
        private static MethodInfo FindSerialiser(Type serialisableType)
        {
            // Go through the public methods of the Serialise class
            Type serialiserClassType = typeof(Serialise);
            foreach (MethodInfo publicMethods in serialiserClassType.GetMethods())
            {
                SerialiserAttribute serialiserAttribute = (SerialiserAttribute)publicMethods.GetCustomAttribute(typeof(SerialiserAttribute));
                if (serialiserAttribute?.SerialisedType == serialisableType) { return publicMethods; }
            }

            // Go through the assembly defining the type on which the attribute is defined.
            Assembly serialisedTypeAssembly = Assembly.GetAssembly(serialisableType);
            foreach(Type type in serialisedTypeAssembly.GetTypes())
            {
                foreach(MethodInfo publicMethods in type.GetMethods())
                {
                    SerialiserAttribute serialiserAttribute = (SerialiserAttribute)publicMethods.GetCustomAttribute(typeof(SerialiserAttribute));
                    if (serialiserAttribute?.SerialisedType == serialisableType) { return publicMethods; }
                }
            }

            return null;
        }
    }
}
