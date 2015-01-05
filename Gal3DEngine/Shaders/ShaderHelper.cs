using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    public static class ShaderHelper
    {

        public static void TransformPosition(ref Vector4 position, Matrix4 transformation, Screen screen)
        {
            position = Vector4.Transform(position, transformation); // projection * view * world

            position.X = position.X / position.W * 0.5f * screen.Width + screen.Width / 2;
            position.Y = position.Y / position.W * 0.5f * screen.Height + screen.Height / 2;
            position.Z = position.Z / position.W * 0.5f + 0.5f;
        }

        public static void TransformNormal(ref Vector3 normal, Matrix4 transformation, Screen screen)
        {
            normal = Vector3.Transform(normal, transformation);
        }

        public static bool ShouldRender(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height)
        {
            return !(ShouldClip(p1, p2, p3, width, height) || ShouldCull(p1, p2, p3));
        }

        // back face Culling check
        private static bool ShouldCull(Vector4 p1 , Vector4 p2 , Vector4 p3)
        {
            return ((p1.X - p2.X) * (p3.Y - p2.Y) - (p1.Y - p2.Y) * (p3.X - p2.X)) > 0;
        }

        // Clipping check
        private static bool ShouldClip(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height)
        {
            return  ((p1.X < 0 || p1.X > width) || (p1.Y < 0 || p1.Y > height) || p1.Z < 0 || p1.Z > 1) &&
                    ((p2.X < 0 || p2.X > width) || (p2.Y < 0 || p2.Y > height) || p2.Z < 0 || p2.Z > 1) &&
                    ((p3.X < 0 || p3.X > width) || (p3.Y < 0 || p3.Y > height) || p3.Z < 0 || p3.Z > 1);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
        }

        public static Color3 ColorLerp(Color3 a, Color3 b, float t)
        {
            Color3 result;
            result.r = (byte)(a.r + (b.r - a.r) * t);
            result.g = (byte)(a.g + (b.g - a.g) * t);
            result.b = (byte)(a.b + (b.b - a.b) * t);
            return result;
        }

    }
}
