using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Faker.Tests
{
    [TestClass]
    public class FakerTests
    {
        private Faker faker;

        [TestInitialize]
        public void Setup()
        {
            faker = new Faker();
        }

        [TestMethod]
        public void Create_Int_ReturnsNonDefault()
        {
            int value = faker.Create<int>();
            Assert.AreNotEqual(0, value);
        }

        [TestMethod]
        public void Create_String_ReturnsNonEmpty()
        {
            string value = faker.Create<string>();
            Assert.IsNotNull(value);
            Assert.IsTrue(value.Length > 0);
        }

        [TestMethod]
        public void Create_User_PropertiesFilled()
        {
            User user = faker.Create<User>();
            Assert.IsNotNull(user.Name);
            Assert.AreNotEqual(0, user.Age);
        }

        [TestMethod]
        public void Create_ListOfUsers_ReturnsNonNull()
        {
            List<User> users = faker.Create<List<User>>();
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void Create_ArrayOfInts_ReturnsNonNull()
        {
            int[] array = faker.Create<int[]>();
            Assert.IsNotNull(array);
        }

        [TestMethod]
        public void RecursiveClass_NoStackOverflow()
        {
            A a = faker.Create<A>();
            Assert.IsNotNull(a);
            Assert.IsNull(a.B?.C?.A);
        }

        [TestMethod]
        public void ConstructorSelection_UsesLargestPossible()
        {
            MultiCtor obj = faker.Create<MultiCtor>();
            Assert.IsTrue(obj.UsedCtorWithMaxParams);
        }

        [TestMethod]
        public void StructWithCustomCtor_UsesCustomCtor()
        {
            MyStruct s = faker.Create<MyStruct>();
            Assert.IsTrue(s.CustomCtorUsed);
        }

        [TestMethod]
        public void CustomConfigForField_AppliesCustomGenerator()
        {
            var config = new FakerConfig();
            config.Add<Foo, string, FixedStringGenerator>(f => f.Bar);
            var customFaker = new Faker(config);
            Foo foo = customFaker.Create<Foo>();
            Assert.AreEqual("fixed", foo.Bar);
        }

        [TestMethod]
        public void CustomConfigForImmutablePerson_UsesForConstructorParameter()
        {
            var config = new FakerConfig();
            config.Add<Person, string, FixedStringGenerator>(p => p.Name);
            var customFaker = new Faker(config);
            Person person = customFaker.Create<Person>();
            Assert.AreEqual("fixed", person.Name);
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