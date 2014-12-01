using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    public class IndexPositionUV
    {
        public int position;
        public int uv;

        public IndexPositionUV(int position, int uv)
        {
            this.position = position;
            this.uv = uv;
        }

    }
}
