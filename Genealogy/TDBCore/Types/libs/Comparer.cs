using System;
using System.Collections.Generic;

namespace TDBCore.Types.libs
{
    public class Comparer<T> : IComparer<T>
    {
        private Func<T, T, int> _compareFn;

        public Comparer(Func<T, T, int> fn)
        {
            _compareFn = fn;
        }

        public int Compare(T x, T y)
        {
            return _compareFn(x, y);
        }

    }
}