using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class ShaderFlat
    {

        public static Matrix4 world;
        public static Matrix4 view;
        public static Matrix4 projection;
        public static Vector3 lightDirection;

        public static Color3[,] texture;

        private static Vector4[] positions;
        private static Vector2[] uvs;
        private static Vector3[] normals;

        public class IndexPositionUVNormal
        {
            public int position;
            public int uv;
            public int normal;

            public IndexPositionUVNormal(int position, int uv, int normal)
            {
                this.position = position;
                this.uv = uv;
                this.normal = normal;
            }

        }

        public static void SetVerticesPositions(Vector4[] positions)
        {
            ShaderFlat.positions = (Vector4[])positions.Clone();
        }

        public static void SetVerticesUvs(Vector2[] uvs)
        {
            ShaderFlat.uvs = uvs;
        }

        public static void SetVerticesNormals(Vector3[] normals)
        {
            ShaderFlat.normals = (Vector3[])normals.Clone();
        }

        public static void Render(Screen screen, IndexPositionUVNormal[] indices)
        {
            Matrix4 transformation = world * view * projection; // projection * view * world

            Vector4 v;

            for (int i = 0; i < positions.Length; i++)
            {
                v = Vector4.Transform(positions[i], transformation);

                // Returns points in Homogeneous ( -1 -> 1 ) and now tramsform them to (0 -> Width) and (0 -> Height)
                v.X = v.X / v.W * 0.5f * screen.Width + screen.Width * 0.5f;
                v.Y = v.Y / v.W * 0.5f * screen.Height + screen.Height * 0.5f;
                v.Z = v.Z / v.W;

                positions[i] = v;
            }

            Matrix4 normalTransform = world.Inverted(); // Same as: 
            normalTransform.Transpose();                // Matrix4 normalTransform = world.ClearScale().ClearProjection().ClearTranslation();

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.Transform(normals[i], normalTransform);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                DrawTriangle(screen, indices[i + 0], indices[i + 1], indices[i + 2]);
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

        private static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
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
        private static void ProcessScanLine(Screen screen, int y, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd, float brightness)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = positions[pa.position].Y != positions[pb.position].Y ? (y - positions[pa.position].Y) / (positions[pb.position].Y - positions[pa.position].Y) : 1;
            var gradient2 = positions[pc.position].Y != positions[pd.position].Y ? (y - positions[pc.position].Y) / (positions[pd.position].Y - positions[pc.position].Y) : 1;

            int sx = (int)Lerp(positions[pa.position].X, positions[pb.position].X, gradient1);
            int ex = (int)Lerp(positions[pc.position].X, positions[pd.position].X, gradient2);

            // starting Z & ending Z
            float z1 = Lerp(positions[pa.position].Z, positions[pb.position].Z, gradient1);
            float z2 = Lerp(positions[pc.position].Z, positions[pd.position].Z, gradient2);

            // starting uv & ending uv
            Vector2 uv1 = Lerp(uvs[pa.uv], uvs[pb.uv], gradient1);
            Vector2 uv2 = Lerp(uvs[pc.uv], uvs[pd.uv], gradient2);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Lerp(z1, z2, gradient);
                Vector2 uv = Lerp(uv1, uv2, gradient);

                int tx = (int)(texture.GetLength(0) * uv.X);
                if(tx >= texture.GetLength(0))
                    tx = texture.GetLength(0) - 1;
                int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
                if(ty >= texture.GetLength(1))
                    ty = texture.GetLength(1) - 1;
                Color3 c = texture[tx, ty];

                c.r = Convert.ToByte(c.r * brightness);
                c.g = Convert.ToByte(c.g * brightness);
                c.b = Convert.ToByte(c.b * brightness);

                screen.TryPutPixel(x, y, z, c);
            }
        }

        private static void DrawTriangle(Screen screen, IndexPositionUVNormal p1, IndexPositionUVNormal p2, IndexPositionUVNormal p3)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (positions[p1.position].Y > positions[p2.position].Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            if (positions[p2.position].Y > positions[p3.position].Y)
            {
                var temp = p2;
                p2 = p3;
                p3 = temp;
            }

            if (positions[p1.position].Y > positions[p2.position].Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            // inverse slopes
            float dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing inverse slopes
            if (positions[p2.position].Y - positions[p1.position].Y > 0)
                dP1P2 = (positions[p2.position].X - positions[p1.position].X) / (positions[p2.position].Y - positions[p1.position].Y);
            else
                dP1P2 = 0;

            if (positions[p3.position].Y - positions[p1.position].Y > 0)
                dP1P3 = (positions[p3.position].X - positions[p1.position].X) / (positions[p3.position].Y - positions[p1.position].Y);
            else
                dP1P3 = 0;

            // Lightning computing
            Vector3 normal = Vector3.Normalize((normals[p1.normal] + normals[p2.normal] + normals[p3.normal]) / 3.0f); // Compute face normal
            float brightness = Math.Max(0, Vector3.Dot(normal, lightDirection)); // Compute face brightness

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
                for (int y = (int)positions[p1.position].Y; y <= (int)positions[p3.position].Y; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p3, p1, p2, brightness);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p1, p3, p2, p3, brightness);
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
                for (int y = (int)positions[p1.position].Y; y <= (int)positions[p3.position].Y; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p2, p1, p3, brightness);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p2, p3, p1, p3, brightness);
                    }
                }
            }
        }

    }
}
