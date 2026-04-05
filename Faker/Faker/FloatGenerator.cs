using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class FloatGenerator: IValueGenerator
    {
        private static readonly Random Random = new Random();
        public bool CanGenerate(Type type) => type == typeof(float);
        public object Generate(Type type, Faker faker, GenerationContext context) => (float)Random.NextDouble();
    }
}
