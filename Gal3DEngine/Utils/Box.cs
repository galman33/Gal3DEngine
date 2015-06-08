using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.Utils
{
    /// <summary>
    /// Holds the data of a 3D Box.
    /// </summary>
    public class Box
    {
        /// <summary>
        /// The X axis radius of the Box.
        /// </summary>
        public float radiusX;
        /// <summary>
        /// The Y axis radius of the Box.
        /// </summary>
        public float radiusY;
        /// <summary>
        /// The Z axis radius of the Box.
        /// </summary>
        public float radiusZ;

        /// <summary>
        /// The box origin position.
        /// </summary>
        public Vector3 origin;

        /// <summary>
        /// Initiallize a box from given axis radius and box origin position.
        /// </summary>
        /// <param name="radiusX"></param>
        /// <param name="radiusY"></param>
        /// <param name="radiusZ"></param>
        /// <param name="origin"></param>
        public Box(float radiusX, float radiusY, float radiusZ, Vector3 origin)
        {
            this.radiusX = radiusX;
            this.radiusY = radiusY;
            this.radiusZ = radiusZ;
            this.origin = origin;
        }

        private Vector3 GetMin()
        {
            return new Vector3(origin.X - radiusX, origin.Y - radiusY, origin.Z - radiusZ);
        }

        private Vector3 GetMax()
        {
            return new Vector3(origin.X + radiusX, origin.Y + radiusY, origin.Z + radiusZ);
        }
        /// <summary>
        /// Check for collision between two Boxes.
        /// </summary>
        /// <param name="a">The first Box to check for collision.</param>
        /// <param name="b">The second Box to check for collision.</param>
        /// <returns></returns>
        public static bool IsColliding(Box a, Box b)
        {
            bool collide = (a.GetMax().X >= b.GetMin().X && a.GetMin().X <= b.GetMax().X)
            && (a.GetMax().Y >= b.GetMin().Y && a.GetMin().Y <= b.GetMax().Y)
            && (a.GetMax().Z >= b.GetMin().Z && a.GetMin().Z <= b.GetMax().Z);
            if (collide)
                return collide;
            else return collide;
        }

        /// <summary>
        /// Checks wheter a 3D point is inside a given Box
        /// </summary>
        /// <param name="a">The box to check on.</param>
        /// <param name="point">The point to check on.</param>
        /// <returns></returns>
        public static bool IsPointContained(Box a, Vector3 point)
        {
            var min = a.GetMin();
            var max = a.GetMax();
            return  point.X >= min.X && point.X <= max.X &&
                    point.Y >= min.Y && point.Y <= max.Y &&
                    point.Z >= min.Z && point.Z <= max.Z;
        }

        
        public void Render(Screen screen, Matrix4 view, Matrix4 projection)
        {
            Render(screen, view, projection, new Color3(0, 255, 0));
        }
        /// <summary>
        /// Renders the current box into a given screen with view and projection matrices, and a given color to render with.
        /// </summary>
        /// <param name="screen">The screen to render on.</param>
        /// <param name="view">The view matrix to render with.</param>
        /// <param name="projection">The projection matrix to render with.</param>
        /// <param name="color">The color to render with.</param>
        public void Render(Screen screen, Matrix4 view, Matrix4 projection, Color3 color)
        {
            var min = GetMin();
            var max = GetMax();

            Vector3[] wireFrames = new Vector3[8];
            wireFrames[0] = new Vector3(min.X, min.Y, min.Z); // origin
            wireFrames[1] = new Vector3(max.X, min.Y, min.Z);
            wireFrames[2] = new Vector3(min.X, max.Y, min.Z);
            wireFrames[3] = new Vector3(min.X, min.Y, max.Z);
            wireFrames[4] = new Vector3(max.X, max.Y, min.Z);
            wireFrames[5] = new Vector3(max.X, min.Y, max.Z);
            wireFrames[6] = new Vector3(min.X, max.Y, max.Z);
            wireFrames[7] = new Vector3(max.X, max.Y, max.Z);

            for (int i = 0; i < wireFrames.Length; i++)
            {
                Vector4 tmp = new Vector4(wireFrames[i]);
                tmp.W = 1;
                ShaderHelper.TransformPosition(ref tmp, /*Matrix4.CreateTranslation(origin) **/ view * projection, screen);
                wireFrames[i] = new Vector3(tmp);
            }
            /*
                 .+------+ 
                 /|     /| 
                +-+----+ | 
                | |    | | 
                | +----+-+ 
                |/     |/
                +------+ 
             */
            //1-3
            screen.DrawLine(wireFrames[0], wireFrames[1], color);
            screen.DrawLine(wireFrames[0], wireFrames[2], color);
            screen.DrawLine(wireFrames[0], wireFrames[3], color);
            //4
            screen.DrawLine(wireFrames[1], wireFrames[4], color);
            screen.DrawLine(wireFrames[2], wireFrames[4], color);
            screen.DrawLine(wireFrames[7], wireFrames[4], color);
            //5
            screen.DrawLine(wireFrames[1], wireFrames[5], color);
            screen.DrawLine(wireFrames[3], wireFrames[5], color);
            screen.DrawLine(wireFrames[7], wireFrames[5], color);
            //6
            screen.DrawLine(wireFrames[2], wireFrames[6], color);
            screen.DrawLine(wireFrames[3], wireFrames[6], color);
            screen.DrawLine(wireFrames[7], wireFrames[6], color);
        }

    }
}
