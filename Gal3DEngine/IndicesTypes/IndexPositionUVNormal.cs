using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    public class IndexPositionUVNormal
    {
        public int position;
        public int uv;
        public int normal;

        public IndexPositionUVNormal(int position, int uv, int normal)
        {
            this.position = position;
            this.uv = uv;
            this.normal = normal;
        }

    }
}
