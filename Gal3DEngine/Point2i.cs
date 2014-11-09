using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    class Point2i
    {
        private int x;
        private int y;

        public Point2i(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

    }
}
