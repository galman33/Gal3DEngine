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
       
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
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



        public bool Contains(Rectangle rect)
        {
            return (this.x <= rect.x) &&
            ((rect.x + rect.width) <= (this.x + this.width)) &&
            (this.y <= rect.y) &&
            ((rect.y + rect.height) <= (this.y + this.height));

        }
    }
}
