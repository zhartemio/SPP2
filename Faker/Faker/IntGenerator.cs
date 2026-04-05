using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class IntGenerator : IValueGenerator
    {
        private static readonly Random Random = new Random();
        public bool CanGenerate(Type type) => type == typeof(int);
        public object Generate(Type type, Faker faker, GenerationContext context) => Random.Next();
    }
}
