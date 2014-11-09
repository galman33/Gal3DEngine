﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    public struct Color3
    {
        public byte r;
        public byte g;
        public byte b;

        public Color3(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color3(int r, int g, int b)
        {
            this.r = Convert.ToByte(r);
            this.g = Convert.ToByte(g);
            this.b = Convert.ToByte(b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Color3)
            {
                Color3 col = (Color3)obj;
                return r == col.r && g == col.g && b == col.b;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "Color:{R: " + r + ", G:" + g + ", B:" + b + "}";
        }

    }
}
