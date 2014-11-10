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

        public static void Render(Screen screen, List<Vector4> vertices, List<int> indices)
        {
            Matrix4 transformation = view * world * projection;

            Vector4[] transformedVertices = new Vector4[vertices.Count];

            Vector4 v;
            int i;

            for (i = 0; i < vertices.Count; i++)
            {
                v = Vector4.Transform(vertices[i], view * world * projection); // projection * view * world

                v.X = v.X / v.W * 0.5f * screen.Width + screen.Width / 2;
                v.Y = v.Y / v.W * 0.5f * screen.Height + screen.Height / 2;
                v.Z = v.Z / v.W;

                transformedVertices[i] = v;
            }

            for (i = 0; i < indices.Count; i += 3)
            {
                if( (i / 3) % 2 == 0)
                {
                    DrawTriangle(screen, transformedVertices[indices[i + 0]], transformedVertices[indices[i + 1]], transformedVertices[indices[i + 2]], Color);
                }
                else
                {
                    //screen.DrawTriangleOutline( transformedVertices[indices[i + 0]], transformedVertices[indices[i + 1]], transformedVertices[indices[i + 2]], Color);
                }
            }
            

        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        private static void ProcessScanLine(Screen screen, int y, Vector4 pa, Vector4 pb, Vector4 pc, Vector4 pd, Color3 color)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Lerp(pa.X, pb.X, gradient1);
            int ex = (int)Lerp(pc.X, pd.X, gradient2);

            // starting Z & ending Z
            float z1 = Lerp(pa.Z, pb.Z, gradient1);
            float z2 = Lerp(pc.Z, pd.Z, gradient2);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Lerp(z1, z2, gradient);

                Color3 c = color;
                c.r = (byte)(c.r * ((z - 1) * 2));
                c.g = (byte)(c.g * ((z - 1) * 2));
                c.b = (byte)(c.b * ((z - 1) * 2));

                screen.PutPixel(x, y, z, c);
            }
        }

        private static void DrawTriangle(Screen screen, Vector4 p1, Vector4 p2, Vector4 p3, Color3 color)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            if (p2.Y > p3.Y)
            {
                var temp = p2;
                p2 = p3;
                p3 = temp;
            }

            if (p1.Y > p2.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            // inverse slopes
            float dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing inverse slopes
            if (p2.Y - p1.Y > 0)
                dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            else
                dP1P2 = 0;

            if (p3.Y - p1.Y > 0)
                dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
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
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
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
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
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

    }
}
