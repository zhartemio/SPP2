using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Faker.Tests
{
    [TestFixture]
    public class FakerTests
    {
        private Faker faker;

        [SetUp]
        public void Setup()
        {
            faker = new Faker();
        }

        [Test]
        public void Create_Int_ReturnsNonDefault()
        {
            int value = faker.Create<int>();
            Assert.That(value, Is.Not.EqualTo(0));
        }

        [Test]
        public void Create_String_ReturnsNonEmpty()
        {
            string value = faker.Create<string>();
            Assert.That(value, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Create_User_PropertiesFilled()
        {
            User user = faker.Create<User>();
            Assert.That(user.Name, Is.Not.Null);
            Assert.That(user.Age, Is.Not.EqualTo(0));
        }

        [Test]
        public void Create_ListOfUsers_ReturnsNonNull()
        {
            List<User> users = faker.Create<List<User>>();
            Assert.That(users, Is.Not.Null);
        }

        [Test]
        public void Create_ArrayOfInts_ReturnsNonNull()
        {
            int[] array = faker.Create<int[]>();
            Assert.That(array, Is.Not.Null);
        }

        [Test]
        public void RecursiveClass_NoStackOverflow()
        {
            A a = faker.Create<A>();
            Assert.That(a, Is.Not.Null);
            Assert.That(a.B?.C?.A, Is.Null);
        }

        [Test]
        public void ConstructorSelection_UsesLargestPossible()
        {
            MultiCtor obj = faker.Create<MultiCtor>();
            Assert.That(obj.UsedCtorWithMaxParams, Is.True);
        }

        [Test]
        public void StructWithCustomCtor_UsesCustomCtor()
        {
            MyStruct s = faker.Create<MyStruct>();
            Assert.That(s.CustomCtorUsed, Is.True);
        }

        [Test]
        public void CustomConfigForField_AppliesCustomGenerator()
        {
            var config = new FakerConfig();
            config.Add<Foo, string, FixedStringGenerator>(f => f.Bar);
            var customFaker = new Faker(config);
            Foo foo = customFaker.Create<Foo>();
            Assert.That(foo.Bar, Is.EqualTo("fixed"));
        }

        [Test]
        public void CustomConfigForImmutablePerson_UsesForConstructorParameter()
        {
            var config = new FakerConfig();
            config.Add<Person, string, FixedStringGenerator>(p => p.Name);
            var customFaker = new Faker(config);
            Person person = customFaker.Create<Person>();
            Assert.That(person.Name, Is.EqualTo("fixed"));
        }

        
        public class User { public string Name { get; set; } public int Age { get; set; } }
        public class A { public B B { get; set; } }
        public class B { public C C { get; set; } }
        public class C { public A A { get; set; } }
        public class MultiCtor
        {
            public bool UsedCtorWithMaxParams { get; }
            public MultiCtor() => UsedCtorWithMaxParams = false;
            public MultiCtor(int x) => UsedCtorWithMaxParams = false;
            public MultiCtor(int x, string y) => UsedCtorWithMaxParams = true;
        }
        public struct MyStruct
        {
            public bool CustomCtorUsed { get; }
            public MyStruct(int x) : this() { CustomCtorUsed = true; }
        }
        public class Foo { public string Bar { get; set; } }
        public class Person { public string Name { get; } public Person(string name) => Name = name; }
        public class FixedStringGenerator : IValueGenerator
        {
            public bool CanGenerate(Type type) => type == typeof(string);
            public object Generate(Type type, Faker faker, GenerationContext context) => "fixed";
        }
    }
}