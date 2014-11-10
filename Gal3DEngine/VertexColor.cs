using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    public struct VertexColor
    {
        public Vector4 Position;
        public Color3 Color;

        public VertexColor(Vector4 position, Color3 color)
        {
            this.Position = position;
            this.Color = color;
        }
    }
}
