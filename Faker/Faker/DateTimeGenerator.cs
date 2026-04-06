using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public class DateTimeGenerator: IValueGenerator
    {
        private static readonly Random Random = new Random();
        private static readonly DateTime Start = new DateTime(1970, 1, 1);
        private static readonly DateTime End = new DateTime(2099, 12, 31);

        public bool CanGenerate(Type type) => type == typeof(DateTime);

        public object Generate(Type type, Faker faker, GenerationContext context)
        {
            int range = (End - Start).Days;
            return Start.AddDays(Random.Next(range));
        }
    }
}
