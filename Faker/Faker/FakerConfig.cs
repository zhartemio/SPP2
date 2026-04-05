using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    internal class FakerConfig
    {
        private readonly Dictionary<Type, Dictionary<MemberInfo, IValueGenerator>> _config = new Dictionary<Type, Dictionary<MemberInfo, IValueGenerator>>();

        public void Add<TTarget, TProp, TGenerator>(Expression<Func<TTarget, TProp>> memberSelector) where TGenerator : IValueGenerator, new()
        {
            var memberExpression = memberSelector.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Expression must be a member access (property or field).");

            MemberInfo member = memberExpression.Member;
            Type targetType = typeof(TTarget);

            if (!_config.ContainsKey(targetType))
                _config[targetType] = new Dictionary<MemberInfo, IValueGenerator>();

            _config[targetType][member] = new TGenerator();
        }

        public IValueGenerator GetCustomGenerator(Type targetType, MemberInfo member)
        {
            if (_config.TryGetValue(targetType, out var memberDict))
            {
                if (memberDict.TryGetValue(member, out var generator))
                    return generator;
            }
            return null;
        }

        public IValueGenerator GetCustomGeneratorForParameter(Type targetType, string paramName, Type paramType)
        {
            var members = targetType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => (m is PropertyInfo || m is FieldInfo) &&
                            string.Equals(m.Name, paramName, StringComparison.OrdinalIgnoreCase) &&
                            ((m is PropertyInfo p && p.PropertyType == paramType) ||
                             (m is FieldInfo f && f.FieldType == paramType)))
                .ToList();

            if (members.Count == 1)
            {
                return GetCustomGenerator(targetType, members[0]);
            }
            return null;
        }
    }
}
