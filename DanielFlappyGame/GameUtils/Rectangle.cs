using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    /// <summary>
    /// Holds the data of a 2D rectangle.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// X axis of the rectangle origin.
        /// </summary>
        private int x;
        /// <summary>
        /// Y axis of the rectangle origin.
        /// </summary>
        private int y;
        /// <summary>
        /// The width of the Rectangle.
        /// </summary>
        private int width;
        /// <summary>
        /// The height of the Rectangle.
        /// </summary>
        private int height;        
        /// <summary>
        /// Initiallizes a rectangle from given x coordinate, y coordinate , width and height.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Checks whether a point is inside the rectangle.
        /// </summary>
        /// <param name="x">The points X coordinate.</param>
        /// <param name="y">The points Y coordinate.</param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return this.x <= x &&
            x < this.x + this.width &&
            this.y <= y &&
            y < this.y + this.height;
        }



        public bool Contains(Vector2 pt)
        {
            return Contains((int)pt.X, (int)pt.Y);
        }


        /// <summary>
        /// Checks wheter a Rectangle is inside the current Rectangle.
        /// </summary>
        /// <param name="rect">The Rectangle to check on.</param>
        /// <returns></returns>
        public bool Contains(Rectangle rect)
        {
            return (this.x <= rect.x) &&
            ((rect.x + rect.width) <= (this.x + this.width)) &&
            (this.y <= rect.y) &&
            ((rect.y + rect.height) <= (this.y + this.height));

        }
    }
}
