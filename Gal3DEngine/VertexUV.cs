using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    public struct VertexUV
    {
        public Vector4 Position;
        public Vector2 UV;

        public VertexUV(Vector4 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }
    }
}
