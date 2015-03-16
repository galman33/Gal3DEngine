using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    public class Rectangle
    {
        private int x;
        private int y;
        private int width;
        private int height;



        
        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref="System.Drawing.Rectangle">
        ///       class with the specified location and size.
        ///    </see></para>
        /// </devdoc>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            public bool Contains(int x, int y) {
            return this.x <= x &&
            x < this.x + this.width &&
            this.y <= y &&
            y < this.y + this.height;
        }
  
        
        /// <devdoc>
        ///    <para>
        ///       Determines if the specfied point is contained within the
        ///       rectangular region defined by this <see cref="System.Drawing.Rectangle"> .
        ///    </see></para>
        /// </devdoc>
        public bool Contains(Vector2 pt) {
            return Contains((int)pt.X, (int)pt.Y);
        }
 
        
        /// <devdoc>
        ///    <para>
        ///       Determines if the rectangular region represented by
        ///    <paramref name="rect"> is entirely contained within the rectangular region represented by
        ///       this <see cref="System.Drawing.Rectangle"> .
        ///    </see></paramref></para>
        /// </devdoc>
        public bool Contains(Rectangle rect) {
            return(this.x <= rect.x) &&
            ((rect.x + rect.width) <= (this.x + this.width)) &&
            (this.y <= rect.y) &&
            ((rect.y + rect.height) <= (this.y + this.height));
        }
        }
    }
}
