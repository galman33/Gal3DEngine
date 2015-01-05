﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    public class IndexPositionUV : IndexPosition
    {
        public int uv;

        public IndexPositionUV(int position, int uv) : base(position)
        {
            this.uv = uv;
        }

    }
}