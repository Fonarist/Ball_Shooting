using System;
using System.Collections.Generic;

namespace Ball.Core.AsyncSequence
{
    public class AsyncSequenceContext
    {
        private readonly Dictionary<Type, object> _data = new Dictionary<Type, object>();

        public bool TryGetData<T>(out T data)
        {
            data = default;
            if (_data.TryGetValue(typeof(T), out var contextData))
            {
                data = (T)contextData;

                return true;
            }

            return false;
        }

        public void SetData<T>(T data)
        {
            _data[data.GetType()] = data;
        }
    }
}