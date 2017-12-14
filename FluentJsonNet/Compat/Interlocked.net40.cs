using System;
using System.Collections.Generic;
using System.Text;
#if NET40
namespace System.Threading
{
    class Interlocked
    {
        public static void MemoryBarrier()
        {
            Thread.MemoryBarrier();
        }
    }
}
#endif