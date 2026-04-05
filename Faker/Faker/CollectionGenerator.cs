using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class CollectionGenerator: IValueGenerator
    {
        public bool CanGenerate(Type type)
        {
            if (type.IsArray)
                return true;
            if (type.IsGenericType)
            {
                Type generic = type.GetGenericTypeDefinition();
                return generic == typeof(List<>) ||
                       generic == typeof(IEnumerable<>) ||
                       generic == typeof(IList<>) ||
                       generic == typeof(ICollection<>);
            }
            return false;
        }

        public object Generate(Type type, Faker faker, GenerationContext context)
        {
            Type elementType;
            if (type.IsArray)
                elementType = type.GetElementType();
            else
                elementType = type.GetGenericArguments()[0];

            Type collectionType;
            if (type.IsArray)
                collectionType = typeof(List<>).MakeGenericType(elementType);
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                collectionType = typeof(List<>).MakeGenericType(elementType);
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
                collectionType = typeof(List<>).MakeGenericType(elementType);
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                collectionType = typeof(List<>).MakeGenericType(elementType);
            else
                collectionType = type;

            IList list = (IList)Activator.CreateInstance(collectionType);
            int count = new Random().Next(0, 10);
            for (int i = 0; i < count; i++)
            {
                object item = faker.Generate(elementType, context);
                list.Add(item);
            }

            if (type.IsArray)
            {
                Array array = Array.CreateInstance(elementType, list.Count);
                list.CopyTo(array, 0);
                return array;
            }

            return list;
        }
    }
}
