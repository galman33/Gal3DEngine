using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    /// <summary>
    /// Data structure of a vertex represented by a color and a position.
    /// </summary>
    public struct VertexColor
    {
        /// <summary>
        /// The 3D position of the vertex (W = 1).
        /// </summary>
        public Vector4 Position;
        /// <summary>
        /// The Color of the vertex
        /// </summary>
        public Color3 Color;

        /// <summary>
        /// Initiallize the vertex by given color and position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public VertexColor(Vector4 position, Color3 color)
        {
            this.Position = position;
            this.Color = color;
        }
    }
}
