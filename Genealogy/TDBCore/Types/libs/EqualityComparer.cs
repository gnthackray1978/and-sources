using System;
using System.Collections.Generic;

namespace TDBCore.Types.libs
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> _equalsFn;
        private Func<T, int> _getHashCodefn;

        public EqualityComparer(Func<T, T, bool> equalsFn, Func<T, int> getHashCodefn)
        {
            _equalsFn = equalsFn;
            _getHashCodefn = getHashCodefn;
        }

        public bool Equals(T x, T y)
        {
            return _equalsFn(x, y);
        }

        public int GetHashCode(T obj)
        {
          //  Debug.WriteLine(_getHashCodefn(obj));
            return _getHashCodefn(obj);
        }
    }  
}
