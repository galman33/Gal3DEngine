using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    class RefType<T> where T : struct
    {
        public T Value { get; set; }

        public RefType(T value)
        {
            this.Value = value;
        }
    }
}
