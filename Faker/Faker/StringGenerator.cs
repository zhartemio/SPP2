using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public class StringGenerator: IValueGenerator
    {
        private static readonly Random Random = new Random();
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public bool CanGenerate(Type type) => type == typeof(string);

        public object Generate(Type type, Faker faker, GenerationContext context)
        {
            int length = Random.Next(5, 20);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(Chars[Random.Next(Chars.Length)]);
            return sb.ToString();
        }
    }
}
