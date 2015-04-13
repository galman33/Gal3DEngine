using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    public class IndexPositionUVNormal : IndexPositionUV
    {
        public int normal;

        public IndexPositionUVNormal(int position, int uv, int normal) : base(position, uv)
        {
            this.normal = normal;
        }

    }
}
