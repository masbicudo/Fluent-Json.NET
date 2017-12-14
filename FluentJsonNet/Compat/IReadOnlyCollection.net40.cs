using System;
using System.Collections.Generic;
using System.Text;

#if NET40
namespace System.Collections.Generic
{
    public interface IReadOnlyCollection<T> : IEnumerable<T>
    {
        int Count { get; }
    }
}
#endif