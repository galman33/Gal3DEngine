using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Shader
    {

        public static Matrix4 world;
        public static Matrix4 view;
        public static Matrix4 projection;

        public static Color3 Color;

        public static void Render(Screen screen, VertexColor[] vertices, int[] indices)
        {
            Matrix4 transformation = world * view * projection;

            VertexColor[] transformedVertices = new VertexColor[vertices.Length];

            Vector4 v;
            int i;

            for (i = 0; i < vertices.Length; i++)
            {
                v = Vector4.Transform(vertices[i].Position, view * world * projection); // projection * view * world

                v.X = v.X / v.W * 0.5f * screen.Width + screen.Width / 2;
                v.Y = v.Y / v.W * 0.5f * screen.Height + screen.Height / 2;
                v.Z = v.Z / v.W;

                transformedVertices[i].Position = v;
                transformedVertices[i].Color = vertices[i].Color;
            }

            for (i = 0; i < indices.Length; i += 3)
            {
                if (!Shader.ShouldCull(transformedVertices[indices[i + 0]].Position, transformedVertices[indices[i + 1]].Position, transformedVertices[indices[i + 2]].Position))
                    DrawTriangle(screen, transformedVertices[indices[i + 0]], transformedVertices[indices[i + 1]], transformedVertices[indices[i + 2]], Color);
            }
            

        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private static Color3 ColorLerp(Color3 a, Color3 b, float t)
        {
            Color3 result;
            result.r = (byte)(a.r + (b.r - a.r) * t);
            result.g = (byte)(a.g + (b.g - a.g) * t);
            result.b = (byte)(a.b + (b.b - a.b) * t);
            return result;
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        private static void ProcessScanLine(Screen screen, int y, VertexColor pa, VertexColor pb, VertexColor pc, VertexColor pd, Color3 color)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Position.Y != pb.Position.Y ? (y - pa.Position.Y) / (pb.Position.Y - pa.Position.Y) : 1;
            var gradient2 = pc.Position.Y != pd.Position.Y ? (y - pc.Position.Y) / (pd.Position.Y - pc.Position.Y) : 1;

            int sx = (int)Lerp(pa.Position.X, pb.Position.X, gradient1);
            int ex = (int)Lerp(pc.Position.X, pd.Position.X, gradient2);

            // starting Z & ending Z
            float z1 = Lerp(pa.Position.Z, pb.Position.Z, gradient1);
            float z2 = Lerp(pc.Position.Z, pd.Position.Z, gradient2);

            // starting Z & ending Z
            Color3 c1 = ColorLerp(pa.Color, pb.Color, gradient1);
            Color3 c2 = ColorLerp(pc.Color, pd.Color, gradient2);



            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Lerp(z1, z2, gradient);
                Color3 c = ColorLerp(c1, c2, gradient);

                screen.TryPutPixel(x, y, z, c);
            }
        }

        private static void DrawTriangle(Screen screen, VertexColor p1, VertexColor p2, VertexColor p3, Color3 color)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (p1.Position.Y > p2.Position.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            if (p2.Position.Y > p3.Position.Y)
            {
                var temp = p2;
                p2 = p3;
                p3 = temp;
            }

            if (p1.Position.Y > p2.Position.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            // inverse slopes
            float dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing inverse slopes
            if (p2.Position.Y - p1.Position.Y > 0)
                dP1P2 = (p2.Position.X - p1.Position.X) / (p2.Position.Y - p1.Position.Y);
            else
                dP1P2 = 0;

            if (p3.Position.Y - p1.Position.Y > 0)
                dP1P3 = (p3.Position.X - p1.Position.X) / (p3.Position.Y - p1.Position.Y);
            else
                dP1P3 = 0;

            // First case where triangles are like that:
            // P1
            // -
            // -- 
            // - -
            // -  -
            // -   - P2
            // -  -
            // - -
            // -
            // P3
            if (dP1P2 > dP1P3)
            {
                for (var y = (int)p1.Position.Y; y <= (int)p3.Position.Y; y++)
                {
                    if (y < p2.Position.Y)
                    {
                        ProcessScanLine(screen, y, p1, p3, p1, p2, color);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p1, p3, p2, p3, color);
                    }
                }
            }
            // First case where triangles are like that:
            //       P1
            //        -
            //       -- 
            //      - -
            //     -  -
            // P2 -   - 
            //     -  -
            //      - -
            //        -
            //       P3
            else
            {
                for (var y = (int)p1.Position.Y; y <= (int)p3.Position.Y; y++)
                {
                    if (y < p2.Position.Y)
                    {
                        ProcessScanLine(screen, y, p1, p2, p1, p3, color);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p2, p3, p1, p3, color);
                    }
                }
            }
        }

        //back face Culling check
        public static bool ShouldCull(Vector4 p1 , Vector4 p2 , Vector4 p3)
        {
            return ((p1.X - p2.X) * (p3.Y - p2.Y) - (p1.Y - p2.Y) * (p3.X - p2.X)) < 0;
        }

        public static bool ShouldClip(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height)
        {
            return  ((p1.X < 0 || p1.X > width) || (p1.Y < 0 || p1.Y > height) || p1.Z < 0) &&
                    ((p2.X < 0 || p2.X > width) || (p2.Y < 0 || p2.Y > height) || p2.Z < 0) &&
                    ((p3.X < 0 || p3.X > width) || (p3.Y < 0 || p3.Y > height) || p3.Z < 0);
        }

    }
}
