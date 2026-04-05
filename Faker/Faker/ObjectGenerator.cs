using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class ObjectGenerator: IValueGenerator
    {
        public bool CanGenerate(Type type) => true;

        public object Generate(Type type, Faker faker, GenerationContext context)
        {
            if (context.IsCyclic(type))
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            using (context.PushType(type))
            {
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .ToArray();

                object instance = null;
                ConstructorInfo usedCtor = null;
                object[] usedArgs = null;

                foreach (var ctor in constructors)
                {
                    try
                    {
                        ParameterInfo[] parameters = ctor.GetParameters();
                        object[] args = new object[parameters.Length];
                        bool canCreate = true;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Type paramType = parameters[i].ParameterType;
                            IValueGenerator customGen = faker.GetCustomGeneratorForParameter(type, parameters[i].Name, paramType);
                            if (customGen != null)
                            {
                                args[i] = customGen.Generate(paramType, faker, context);
                            }
                            else
                            {
                                args[i] = faker.Generate(paramType, context);
                            }

                            if (args[i] == null && paramType.IsValueType && Nullable.GetUnderlyingType(paramType) == null)
                            {
                                canCreate = false;
                                break;
                            }
                        }

                        if (canCreate)
                        {
                            instance = ctor.Invoke(args);
                            usedCtor = ctor;
                            usedArgs = args;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }

                if (instance == null && type.IsValueType)
                {
                    instance = Activator.CreateInstance(type);
                }
                else if (instance == null)
                {
                    return null;
                }

                List<string> initializedMembers = new List<string>();
                if (usedCtor != null && usedArgs != null)
                {
                    ParameterInfo[] parameters = usedCtor.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        initializedMembers.Add(parameters[i].Name);
                    }
                }

                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (initializedMembers.Contains(field.Name, StringComparer.OrdinalIgnoreCase))
                        continue;

                    IValueGenerator customGen = faker.GetCustomGeneratorForMember(type, field);
                    object value = customGen != null
                        ? customGen.Generate(field.FieldType, faker, context)
                        : faker.Generate(field.FieldType, context);
                    if (value != null || !field.FieldType.IsValueType || Nullable.GetUnderlyingType(field.FieldType) != null)
                        field.SetValue(instance, value);
                }

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && p.SetMethod.IsPublic)
                    .ToArray();
                foreach (var prop in properties)
                {
                    if (initializedMembers.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                        continue;

                    IValueGenerator customGen = faker.GetCustomGeneratorForMember(type, prop);
                    object value = customGen != null
                        ? customGen.Generate(prop.PropertyType, faker, context)
                        : faker.Generate(prop.PropertyType, context);
                    if (value != null || !prop.PropertyType.IsValueType || Nullable.GetUnderlyingType(prop.PropertyType) != null)
                        prop.SetValue(instance, value);
                }

                return instance;
            }
        }
    }
}
