using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
	/// <summary>
	/// Responsible of transforming and drawing triangles (vertices and indices) into screen pixels.
	/// </summary>
	/// <typeparam name="IndexData">The index data type.</typeparam>
	/// <typeparam name="TriangleData">The triangle data type.</typeparam>
	/// <typeparam name="LineData">The line data type.</typeparam>
    public class Shader<IndexData, TriangleData, LineData> where IndexData : IndexPosition
    {

        protected Vector4[] positions;

		/// <summary>
		/// Set the vertices positions data.
		/// </summary>
		/// <param name="positions"></param>
        public void SetVerticesPositions(Vector4[] positions)
        {
            this.positions = (Vector4[])positions.Clone();
        }

		/// <summary>
		/// Extracts the model data into the shader.
		/// </summary>
		/// <param name="model">The model to extract data from.</param>
        public virtual void ExtractData(Model model)
        {
            SetVerticesPositions(model.Vertices);
        }

		/// <summary>
		/// A delegate to a transform method.
		/// </summary>
        public delegate void TransformMethod<InData, InTransformation>(ref InData data, InTransformation transformation, Screen screen);
		/// <summary>
		/// Apply batch transformation to multiple data using a transformation method.
		/// For example: Applying normal's transformation to all of the normals.
		/// </summary>
		/// <param name="transformMethod">A delegate to the transform method.</param>
		/// <param name="data">The data.</param>
		/// <param name="transformation">The transform.</param>
		/// <param name="screen">The screen.</param>
        protected static void TransformData<InData, InTransformation>(TransformMethod<InData, InTransformation> transformMethod, InData[] data, InTransformation transformation, Screen screen)
        {
            for (int i = 0; i < data.Length; i++)
            {
                transformMethod(ref data[i], transformation, screen);
            }
        }

		/// <summary>
		/// The current screen.
		/// </summary>
		protected Screen screen;

		/// <summary>
		/// Render the loaded data into the screen.
		/// </summary>
		/// <param name="screen"></param>
        public virtual void Render(Screen screen)
        {
			this.screen = screen;
        }

		/// <param name="p1">The first index of the triangle.</param>
		protected IndexData p1;
		/// <param name="p2">The second index of the triangle.</param>
		protected IndexData p2;
		/// <param name="p3">The third index of the triangle.</param>
		protected IndexData p3;

		/// <summary>
		/// Draws every triangle.
		/// </summary>
		/// <param name="indices">The indexes of the triangles in the data. (Each 3 indices represent a triangle).</param>
		/// <param name="screen"></param>
        protected virtual void DrawTriangles(IndexData[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
				p1 = indices[i + 0];
				p2 = indices[i + 1];
				p3 = indices[i + 2];
				if (ShaderHelper.ShouldRender(positions[p1.position], positions[p2.position], positions[p3.position], screen.Width, screen.Height, screen.ClippingEnabled))
                {
                    DrawTriangle();
                }
            }
        }

		/// <summary>
		/// The current triangle data.
		/// </summary>
		protected TriangleData triangleData;

		/// <summary>
		/// The current line data.
		/// </summary>
		protected LineData lineData;

		/// <summary>
		/// Draws a specific triangle (3 indices).
		/// </summary>
        private void DrawTriangle()
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

            triangleData = ProcessTriangle();


            int maxY = Math.Min(screen.Width / 2, (int)positions[p3.position].Y);

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
                for (var y = Math.Max((int)positions[p1.position].Y, -screen.Height / 2); y <= maxY; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(y, p1, p3, p1, p2);
                    }
                    else
                    {
                        ProcessScanLine(y, p1, p3, p2, p3);
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
                for (var y = Math.Max((int)positions[p1.position].Y, - screen.Height / 2); y <= maxY; y++)
                {
                    if (y < positions[p2.position].Y)
                    {
                        ProcessScanLine(y, p1, p2, p1, p3);
                    }
                    else
                    {
                        ProcessScanLine(y, p2, p3, p1, p3);
                    }
                }
            }
        }

		/// <summary>
		/// drawing line between 2 points from left to right
		/// papb -> pcpd
		/// pa, pb, pc, pd must then be sorted before
		/// </summary>
		/// <param name="y">The y of the process line.</param>
		/// <param name="pa"></param>
		/// <param name="pb"></param>
		/// <param name="pc"></param>
		/// <param name="pd"></param>
        private void ProcessScanLine(int y, IndexData pa, IndexData pb, IndexData pc, IndexData pd)
        {
            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = positions[pa.position].Y != positions[pb.position].Y ? (y - positions[pa.position].Y) / (positions[pb.position].Y - positions[pa.position].Y) : 1;
            var gradient2 = positions[pc.position].Y != positions[pd.position].Y ? (y - positions[pc.position].Y) / (positions[pd.position].Y - positions[pc.position].Y) : 1;

            int sx = (int)ShaderHelper.Lerp(positions[pa.position].X, positions[pb.position].X, gradient1);
            int ex = (int)ShaderHelper.Lerp(positions[pc.position].X, positions[pd.position].X, gradient2);

            /*sx = Math.Max(sx, 0);
            ex = Math.Min(ex, screen.Width);*/

            int endX = Math.Min(ex, screen.Width / 2);

            lineData = MyProcessScanLine(gradient1, gradient2, pa, pb, pc, pd);

            // drawing a line from left (sx) to right (ex) 
            for (var x = Math.Max(sx, -screen.Width / 2); x < endX; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                ProcessPixel(x, y, gradient);
            }
        }

		/// <summary>
		/// Process three indices into triangle data.
		/// </summary>
		/// <returns>The triangle data</returns>
        protected virtual TriangleData ProcessTriangle()
        {
            return default(TriangleData);
        }

		/// <summary>
		/// Process a triangle and interpolations into a process line data.
		/// </summary>
		/// <param name="gradient1"></param>
		/// <param name="gradient2"></param>
		/// <param name="pa"></param>
		/// <param name="pb"></param>
		/// <param name="pc"></param>
		/// <param name="pd"></param>
		/// <param name="triangleData"></param>
		/// <returns>The scan linedata.</returns>
        protected virtual LineData MyProcessScanLine(float gradient1, float gradient2, IndexData pa, IndexData pb, IndexData pc, IndexData pd)
        {
            return default(LineData);
        }

		/// <summary>
		/// Process and draw a screen pixel.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="gradient"></param>
		/// <param name="lineData"></param>
		/// <param name="screen"></param>
        protected virtual void ProcessPixel(int x, int y, float gradient)
        {

        }

    }
}
