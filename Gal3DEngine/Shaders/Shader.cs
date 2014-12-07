using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    class Shader
    {

        public delegate void TransformMethod<InData, InTransformation>(ref InData data, InTransformation transformation, Screen screen);
        public delegate OutLineData ProcessScanLineMethod<OutLineData, InIndex>(float gradient1, float gradient2, InIndex pa, InIndex pb, InIndex pc, InIndex pd);
        public delegate void ProcessPixelMethod<InLineData>(int x, int y, float gradient, ref InLineData lineData, Screen screen);

        protected static Vector4[] positions;

        public static void SetVerticesPositions(Vector4[] positions)
        {
            ShaderPhong.positions = (Vector4[])positions.Clone();
        }

        protected static void TransformData<InData, InTransformation>(TransformMethod<InData, InTransformation> transformMethod, InData[] data, InTransformation transformation, Screen screen)
        {
            for (int i = 0; i < data.Length; i++)
            {
                transformMethod(ref data[i], transformation, screen);
            }
        }

        protected static void DrawTriangles<InIndex, InLineData>(InIndex[] indices, Screen screen, ProcessScanLineMethod<InLineData, InIndex> processScanLine, ProcessPixelMethod<InLineData> processPixel) where InIndex : IndexPosition
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                if (Shader.ShouldRender(positions[indices[i + 0].position], positions[indices[i + 1].position], positions[indices[i + 2].position], screen.Width, screen.Height))
                {
                    DrawTriangle(screen, indices[i + 0], indices[i + 1], indices[i + 2],
                        processScanLine, processPixel);
                }
            }
        }

        private static void DrawTriangle<InIndex, InLineData>(Screen screen, InIndex p1, InIndex p2, InIndex p3, ProcessScanLineMethod<InLineData, InIndex> processScanLine, ProcessPixelMethod<InLineData> processPixel) where InIndex : IndexPosition
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
                for (var y = (int)positions[p1.position].Y; y <= (int)positions[p3.position].Y; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p3, p1, p2, processScanLine, processPixel);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p1, p3, p2, p3, processScanLine, processPixel);
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
                for (var y = (int)positions[p1.position].Y; y <= (int)positions[p3.position].Y; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p2, p1, p3, processScanLine, processPixel);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p2, p3, p1, p3, processScanLine, processPixel);
                    }
                }
            }
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        private static void ProcessScanLine<InIndex, InLineData>(Screen screen, int y, InIndex pa, InIndex pb, InIndex pc, InIndex pd, ProcessScanLineMethod<InLineData, InIndex> processScanLine, ProcessPixelMethod<InLineData> processPixel) where InIndex : IndexPosition
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = positions[pa.position].Y != positions[pb.position].Y ? (y - positions[pa.position].Y) / (positions[pb.position].Y - positions[pa.position].Y) : 1;
            var gradient2 = positions[pc.position].Y != positions[pd.position].Y ? (y - positions[pc.position].Y) / (positions[pd.position].Y - positions[pc.position].Y) : 1;

            int sx = (int)Lerp(positions[pa.position].X, positions[pb.position].X, gradient1);
            int ex = (int)Lerp(positions[pc.position].X, positions[pd.position].X, gradient2);

            InLineData lineData = processScanLine(gradient1, gradient2, pa, pb, pc, pd);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                processPixel(x, y, gradient, ref lineData, screen);
            }
        }

        public static bool ShouldRender(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height)
        {
            return !(ShouldClip(p1, p2, p3, width, height) || ShouldCull(p1, p2, p3));
        }

        // back face Culling check
        private static bool ShouldCull(Vector4 p1 , Vector4 p2 , Vector4 p3)
        {
            return ((p1.X - p2.X) * (p3.Y - p2.Y) - (p1.Y - p2.Y) * (p3.X - p2.X)) < 0;
        }

        // Clipping check
        private static bool ShouldClip(Vector4 p1, Vector4 p2, Vector4 p3, int width, int height)
        {
            // W??
            return  ((p1.X < 0 || p1.X > width) || (p1.Y < 0 || p1.Y > height) || p1.W > 0) &&
                    ((p2.X < 0 || p2.X > width) || (p2.Y < 0 || p2.Y > height) || p2.W > 0) &&
                    ((p3.X < 0 || p3.X > width) || (p3.Y < 0 || p3.Y > height) || p3.W > 0);
        }

        protected static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        protected static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        protected static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
        }

        protected static Color3 ColorLerp(Color3 a, Color3 b, float t)
        {
            Color3 result;
            result.r = (byte)(a.r + (b.r - a.r) * t);
            result.g = (byte)(a.g + (b.g - a.g) * t);
            result.b = (byte)(a.b + (b.b - a.b) * t);
            return result;
        }

    }
}
