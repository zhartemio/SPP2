using System;
using System.Collections.Generic;

namespace Faker
{
    class Program
    {
        static void Main(string[] args)
        {
            var faker = new Faker();

            
            int intValue = faker.Create<int>();
            long longValue = faker.Create<long>();
            double doubleValue = faker.Create<double>();
            float floatValue = faker.Create<float>();
            string stringValue = faker.Create<string>();
            DateTime dateValue = faker.Create<DateTime>();

            Console.WriteLine($"int:    {intValue}");
            Console.WriteLine($"long:   {longValue}");
            Console.WriteLine($"double: {doubleValue}");
            Console.WriteLine($"float:  {floatValue}");
            Console.WriteLine($"string: {stringValue}");
            Console.WriteLine($"DateTime: {dateValue}");

            
            User user = faker.Create<User>();
            Console.WriteLine($"\nUser: Name = {user.Name}, Age = {user.Age}");

            
            List<int> intList = faker.Create<List<int>>();
            Console.WriteLine($"\nList<int> count: {intList.Count}");
            if (intList.Count > 0)
                Console.WriteLine($"  Первые 3 элемента: {string.Join(", ", intList.GetRange(0, Math.Min(3, intList.Count)))}");

            User[] userArray = faker.Create<User[]>();
            Console.WriteLine($"User[] length: {userArray.Length}");

            Console.WriteLine("\nГенерация завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}