using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    /// <summary>
    /// Holds the data about a index position of a Vertex with a given UV.
    /// </summary>
    public class IndexPositionUV : IndexPosition
    {
        /// <summary>
        /// The UV of the index.
        /// </summary>
        public int uv;

        /// <summary>
        /// Initiallize an Index by a given position index and uv.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="uv"></param>
        public IndexPositionUV(int position, int uv) : base(position)
        {
            this.uv = uv;
        }

    }
}
