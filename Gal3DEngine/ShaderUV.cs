using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class ShaderUV
    {

        public static Matrix4 world;
        public static Matrix4 view;
        public static Matrix4 projection;

        public static Color3[,] texture;

        public static void Render(Screen screen, VertexUV[] vertices, int[] indices)
        {
            Matrix4 transformation = view * world * projection;

            VertexUV[] transformedVertices = new VertexUV[vertices.Length];

            Vector4 v;
            int i;

            for (i = 0; i < vertices.Length; i++)
            {
                v = Vector4.Transform(vertices[i].Position, view * world * projection); // projection * view * world

                v.X = v.X / v.W * 0.5f * screen.Width + screen.Width / 2;
                v.Y = v.Y / v.W * 0.5f * screen.Height + screen.Height / 2;
                v.Z = v.Z / v.W;

                transformedVertices[i].Position = v;
                transformedVertices[i].UV = vertices[i].UV;
            }

            for (i = 0; i < indices.Length; i += 3)
            {
                DrawTriangle(screen, transformedVertices[indices[i + 0]], transformedVertices[indices[i + 1]], transformedVertices[indices[i + 2]]);
            }
            

        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
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
        private static void ProcessScanLine(Screen screen, int y, VertexUV pa, VertexUV pb, VertexUV pc, VertexUV pd)
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
            Vector2 uv1 = Lerp(pa.UV, pb.UV, gradient1);
            Vector2 uv2 = Lerp(pc.UV, pd.UV, gradient2);



            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Lerp(z1, z2, gradient);
                Vector2 uv = Lerp(uv1, uv2, gradient);

                Color3 c = texture[(int) (texture.GetLength(0) * uv.X), (int)(texture.GetLength(1) * uv.Y)];

                screen.TryPutPixel(x, y, z, c);
            }
        }

        private static void DrawTriangle(Screen screen, VertexUV p1, VertexUV p2, VertexUV p3)
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
                        ProcessScanLine(screen, y, p1, p3, p1, p2);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p1, p3, p2, p3);
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
                        ProcessScanLine(screen, y, p1, p2, p1, p3);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p2, p3, p1, p3);
                    }
                }
            }
        }

    }
}
