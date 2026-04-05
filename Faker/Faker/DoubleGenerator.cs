using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class DoubleGenerator: IValueGenerator
    {
        private static readonly Random Random = new Random();
        public bool CanGenerate(Type type) => type == typeof(double);
        public object Generate(Type type, Faker faker, GenerationContext context) => Random.NextDouble();
    }
}
