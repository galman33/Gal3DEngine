using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.IndicesTypes
{
    /// <summary>
    /// Holds the data about a index position of a Vertex
    /// </summary>
    public class IndexPosition
    {
        /// <summary>
        /// The index position of the vertex.
        /// </summary>
        public int position;

        /// <summary>
        /// Initiallize an Index by a given position index.
        /// </summary>
        /// <param name="position">The index of the position</param>
        public IndexPosition(int position)
        {
            this.position = position;
        }

    }
}
