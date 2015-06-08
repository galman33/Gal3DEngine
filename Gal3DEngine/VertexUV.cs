using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    /// <summary>
    /// Data structure of a vertex represented by a UV and a position.
    /// </summary>
    public class VertexUV
    {
        /// <summary>        
        /// The 3D position of the vertex (W = 1).        
        /// </summary>
        public Vector4 Position;
        /// <summary>
        /// The UV of the vertex
        /// </summary>
        public Vector2 UV;

        /// <summary>
        /// Initiallize the vertex by given UV and position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="uv"></param>
        public VertexUV(Vector4 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }
    }
}
