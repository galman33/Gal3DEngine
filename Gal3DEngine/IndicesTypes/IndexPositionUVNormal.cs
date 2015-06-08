using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    /// <summary>
    /// Holds the data about a index position of a Vertex with a given UV and Normal.
    /// </summary>
    public class IndexPositionUVNormal : IndexPositionUV
    {
        /// <summary>
        /// The Normal of the index.
        /// </summary>
        public int normal;

        /// <summary>
        /// Initiallize an Index by a given position index , uv and normal.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="uv"></param>
        /// <param name="normal"></param>
        public IndexPositionUVNormal(int position, int uv, int normal) : base(position, uv)
        {
            this.normal = normal;
        }

    }
}
