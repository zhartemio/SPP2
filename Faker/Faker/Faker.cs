using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public class Faker
    {
        private readonly List<IValueGenerator> _generators;
        private readonly FakerConfig _config;

        public Faker(FakerConfig config = null)
        {
            _config = config;
            _generators = new List<IValueGenerator>
        {
            new IntGenerator(),
            new LongGenerator(),
            new DoubleGenerator(),
            new FloatGenerator(),
            new StringGenerator(),
            new DateTimeGenerator(),
            new CollectionGenerator(),
            new ObjectGenerator()
        };
        }

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        public object Create(Type type)
        {
            return Generate(type, new GenerationContext());
        }

        internal object Generate(Type type, GenerationContext context)
        {
            foreach (var generator in _generators)
            {
                if (generator.CanGenerate(type))
                    return generator.Generate(type, this, context);
            }
            throw new InvalidOperationException($"No generator found for type {type}");
        }

        internal IValueGenerator GetCustomGeneratorForMember(Type targetType, MemberInfo member)
        {
            return _config?.GetCustomGenerator(targetType, member);
        }

        internal IValueGenerator GetCustomGeneratorForParameter(Type targetType, string paramName, Type paramType)
        {
            return _config?.GetCustomGeneratorForParameter(targetType, paramName, paramType);
        }
    }
}
