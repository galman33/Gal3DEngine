using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.UTILS
{
    public class Cube
    {
        public float radiusX;
        public float radiusY;
        public float radiusZ;

        public Vector3 origin;

        public Cube(float radiusX, float radiusY, float radiusZ, Vector3 origin)
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

        public static bool Collide(Cube a, Cube b)
        {
            bool collide = (a.GetMax().X >= b.GetMin().X && a.GetMin().X <= b.GetMax().X)
            && (a.GetMax().Y >= b.GetMin().Y && a.GetMin().Y <= b.GetMax().Y)
            && (a.GetMax().Z >= b.GetMin().Z && a.GetMin().Z <= b.GetMax().Z);
            if (collide)
                return collide;
            else return collide;
        }

        public static void DrawCube(Screen screen, Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            var cubeRadius = 0.3f / 2;
            Vector3 origin = Vector3.Zero;
            origin.Y += cubeRadius;
            Vector3[] wireFrames = new Vector3[8];
            wireFrames[0] = new Vector3(origin.X - cubeRadius, origin.Y - cubeRadius, origin.Z - cubeRadius); // origin
            wireFrames[1] = new Vector3(origin.X + cubeRadius, origin.Y - cubeRadius, origin.Z - cubeRadius);
            wireFrames[2] = new Vector3(origin.X - cubeRadius, origin.Y + cubeRadius, origin.Z - cubeRadius);
            wireFrames[3] = new Vector3(origin.X - cubeRadius, origin.Y - cubeRadius, origin.Z + cubeRadius);
            wireFrames[4] = new Vector3(origin.X + cubeRadius, origin.Y + cubeRadius, origin.Z - cubeRadius);
            wireFrames[5] = new Vector3(origin.X + cubeRadius, origin.Y - cubeRadius, origin.Z + cubeRadius);
            wireFrames[6] = new Vector3(origin.X - cubeRadius, origin.Y + cubeRadius, origin.Z + cubeRadius);
            wireFrames[7] = new Vector3(origin.X + cubeRadius, origin.Y + cubeRadius, origin.Z + cubeRadius);

            for (int i = 0; i < wireFrames.Length; i++)
            {
                Vector4 tmp = new Vector4(wireFrames[i]);
                tmp.W = 1;
                ShaderHelper.TransformPosition(ref tmp, world * view * projection, screen);
                wireFrames[i] = new Vector3(tmp);
            }
            //1-3
            screen.DrawLine(wireFrames[0], wireFrames[1], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[0], wireFrames[2], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[0], wireFrames[3], new Color3(0, 255, 0));
            //4
            screen.DrawLine(wireFrames[1], wireFrames[4], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[2], wireFrames[4], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[7], wireFrames[4], new Color3(0, 255, 0));
            //5
            screen.DrawLine(wireFrames[1], wireFrames[5], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[3], wireFrames[5], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[7], wireFrames[5], new Color3(0, 255, 0));
            //6
            screen.DrawLine(wireFrames[2], wireFrames[6], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[3], wireFrames[6], new Color3(0, 255, 0));
            screen.DrawLine(wireFrames[7], wireFrames[6], new Color3(0, 255, 0));


        }

    }
}
