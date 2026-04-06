
using System;
using System.Collections.Generic;


namespace Faker
{
    public class GenerationContext
    {
        private readonly Stack<Type> _typeStack = new Stack<Type>();

        public bool IsCyclic(Type type) => _typeStack.Contains(type);

        public IDisposable PushType(Type type)
        {
            _typeStack.Push(type);
            return new PopOnDispose(_typeStack);
        }

        private class PopOnDispose : IDisposable
        {
            private readonly Stack<Type> _stack;
            public PopOnDispose(Stack<Type> stack) => _stack = stack;
            public void Dispose() => _stack.Pop();
        }
    }
}
