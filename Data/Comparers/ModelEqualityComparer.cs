using Data.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Data.Comparers
{
    public abstract class ModelEqualityComparer<T> : IEqualityComparer<T> where T : IModel
    {
        public virtual bool Equals(T x, T y)
        {
            if (x is null && y is null)
                return true;
            if (x is null || y is null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] T obj) => obj.Id;
    }
}
