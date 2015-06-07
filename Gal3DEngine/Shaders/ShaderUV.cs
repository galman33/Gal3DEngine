using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{


	public class ShaderUV : Shader<IndexPositionUV, ShaderUV.TriangleData, ShaderUV.LineData>
	{

		public struct TriangleData
		{
			
		}

		public struct LineData
		{
			public float w1, w2;
			public float z1, z2;
			public Vector2 uv1, uv2;
		}

		public Matrix4 world;
		public Matrix4 view;
		public Matrix4 projection;

		public Color3[,] texture;

		private Vector2[] uvs;
		private IndexPositionUV[] indices;

		public void SetVerticesUvs(Vector2[] uvs)
		{
			this.uvs = uvs;
		}

		public void SetIndices(IndexPositionUV[] indices)
		{
			this.indices = indices;
		}

		public override void ExtractData(Model model)
		{
			base.ExtractData(model);

			SetVerticesUvs(model.UVs);

			texture = model.texture;

			SetIndices(model.indices);
		}

		public override void Render(Screen screen)
		{
			Matrix4 transformation = world * view * projection;
			TransformData(ShaderHelper.TransformPosition, positions, transformation, screen);

			DrawTriangles(indices);
		}

		protected override TriangleData ProcessTriangle()
		{
			return new TriangleData();
		}

		protected override LineData MyProcessScanLine(float gradient1, float gradient2, IndexPositionUV pa, IndexPositionUV pb, IndexPositionUV pc, IndexPositionUV pd)
		{
			LineData result = new LineData();

			// perspective
			result.w1 = ShaderHelper.Lerp(1 / positions[pa.position].W, 1 / positions[pb.position].W, gradient1);
			result.w2 = ShaderHelper.Lerp(1 / positions[pc.position].W, 1 / positions[pd.position].W, gradient2);

			// starting Z & ending Z
			result.z1 = ShaderHelper.Lerp(positions[pa.position].Z / positions[pa.position].W, positions[pb.position].Z / positions[pb.position].W, gradient1);
			result.z2 = ShaderHelper.Lerp(positions[pc.position].Z / positions[pc.position].W, positions[pd.position].Z / positions[pd.position].W, gradient2);

			// starting uv & ending uv
			result.uv1 = ShaderHelper.Lerp(uvs[pa.uv] / positions[pa.position].W, uvs[pb.uv] / positions[pb.position].W, gradient1);
			result.uv2 = ShaderHelper.Lerp(uvs[pc.uv] / positions[pc.position].W, uvs[pd.uv] / positions[pd.position].W, gradient2);

			return result;
		}

		protected override void ProcessPixel(int x, int y, float gradient)
		{
			var w = 1 / ShaderHelper.Lerp(currentLineData.w1, currentLineData.w2, gradient);
			var z = ShaderHelper.Lerp(currentLineData.z1, currentLineData.z2, gradient) * w;
			Vector2 uv = ShaderHelper.Lerp(currentLineData.uv1, currentLineData.uv2, gradient) * w;

			int tx = (int)(texture.GetLength(0) * uv.X);
			if (tx >= texture.GetLength(0))
				tx = texture.GetLength(0) - 1;
			int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
			if (ty >= texture.GetLength(1))
				ty = texture.GetLength(1) - 1;

			if (tx < 0) tx = 0;
			if (ty < 0) ty = 0;
			Color3 c = texture[tx, ty];

			screen.TryPutPixel(x, y, z, c);
		}

	}
}