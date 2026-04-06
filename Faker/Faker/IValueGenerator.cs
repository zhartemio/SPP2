using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public interface IValueGenerator
    {
        bool CanGenerate(Type type);
        object Generate(Type type, Faker faker, GenerationContext context);
    }
}
