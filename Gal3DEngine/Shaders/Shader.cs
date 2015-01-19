using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    public class Shader<IndexData, TriangleData, LineData> where IndexData : IndexPosition
    {

        protected Vector4[] positions;

        public void SetVerticesPositions(Vector4[] positions)
        {
            this.positions = (Vector4[])positions.Clone();
        }

        public virtual void ExtractData(Model model)
        {
            SetVerticesPositions(model.Vertices);
        }

        public delegate void TransformMethod<InData, InTransformation>(ref InData data, InTransformation transformation, Screen screen);
        protected static void TransformData<InData, InTransformation>(TransformMethod<InData, InTransformation> transformMethod, InData[] data, InTransformation transformation, Screen screen)
        {
            for (int i = 0; i < data.Length; i++)
            {
                transformMethod(ref data[i], transformation, screen);
            }
        }

        public virtual void Render(Screen screen)
        {

        }

        protected virtual void DrawTriangles(IndexData[] indices, Screen screen)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                if (ShaderHelper.ShouldRender(positions[indices[i + 0].position], positions[indices[i + 1].position], positions[indices[i + 2].position], screen.Width, screen.Height))
                {
                    DrawTriangle(screen, indices[i + 0], indices[i + 1], indices[i + 2]);
                }
            }
        }

        private void DrawTriangle(Screen screen, IndexData p1, IndexData p2, IndexData p3)
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

            TriangleData triData = ProcessTriangle(p1, p2, p3);


            int maxY = Math.Min(screen.Height, (int) positions[p3.position].Y);

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
                for (var y = Math.Max((int)positions[p1.position].Y, 0); y <= maxY; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p3, p1, p2, triData);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p1, p3, p2, p3, triData);
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
                for (var y = Math.Max((int)positions[p1.position].Y, 0); y <= maxY; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(screen, y, p1, p2, p1, p3, triData);
                    }
                    else
                    {
                        ProcessScanLine(screen, y, p2, p3, p1, p3, triData);
                    }
                }
            }
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        private void ProcessScanLine(Screen screen, int y, IndexData pa, IndexData pb, IndexData pc, IndexData pd, TriangleData triData)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = positions[pa.position].Y != positions[pb.position].Y ? (y - positions[pa.position].Y) / (positions[pb.position].Y - positions[pa.position].Y) : 1;
            var gradient2 = positions[pc.position].Y != positions[pd.position].Y ? (y - positions[pc.position].Y) / (positions[pd.position].Y - positions[pc.position].Y) : 1;

            int sx = (int)ShaderHelper.Lerp(positions[pa.position].X, positions[pb.position].X, gradient1);
            int ex = (int)ShaderHelper.Lerp(positions[pc.position].X, positions[pd.position].X, gradient2);

            sx = Math.Max(sx, 0);
            ex = Math.Min(ex, screen.Width);

            LineData lineData = MyProcessScanLine(gradient1, gradient2, pa, pb, pc, pd, triData);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                ProcessPixel(x, y, gradient, ref lineData, screen);
            }
        }

        protected virtual TriangleData ProcessTriangle(IndexData p1, IndexData p2, IndexData p3)
        {
            return default(TriangleData);
        }

        protected virtual LineData MyProcessScanLine(float gradient1, float gradient2, IndexData pa, IndexData pb, IndexData pc, IndexData pd, TriangleData triangleData)
        {
            return default(LineData);
        }

        protected virtual void ProcessPixel(int x, int y, float gradient, ref LineData lineData, Screen screen)
        {

        }

    }
}
