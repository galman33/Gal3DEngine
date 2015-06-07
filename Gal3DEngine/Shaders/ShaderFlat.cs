using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{

	/// <summary>
	/// Renders a mesh with each triangle at a single flat brightness according to the ligh direction.
	/// </summary>
    public class ShaderFlat : Shader<IndexPositionUVNormal, ShaderFlat.TriangleData, ShaderFlat.LineData>
    {

        public struct TriangleData
        {
            public float brightness;
        }

        public struct LineData
        {
            public float w1, w2;
            public float z1, z2;
            public Vector2 uv1, uv2;
            public float brightness;
        }

		/// <summary>
		/// The world matrix.
		/// </summary>
        public Matrix4 world;
		/// <summary>
		/// The view matrix.
		/// </summary>
        public Matrix4 view;
		/// <summary>
		/// The projection matrix.
		/// </summary>
        public Matrix4 projection;
		/// <summary>
		/// The light direction.
		/// </summary>
        public Vector3 lightDirection;
		/// <summary>
		/// The minimum brightness.
		/// </summary>
        public float ambientLight;

		/// <summary>
		/// The texture.
		/// </summary>
        public Color3[,] texture;

        private Vector2[] uvs;
        private Vector3[] normals;

		/// <summary>
		/// Set the vertices UVs.
		/// </summary>
		/// <param name="uvs"></param>
        public void SetVerticesUvs(Vector2[] uvs)
        {
            this.uvs = uvs;
        }

		/// <summary>
		/// Set the vertices normals.
		/// </summary>
		/// <param name="normals"></param>
        public void SetVerticesNormals(Vector3[] normals)
        {
            this.normals = (Vector3[])normals.Clone();
        }

		/// <summary>
		/// Extracts the model data into the shader.
		/// </summary>
		/// <param name="model">The model to extract data from.</param>
        public override void ExtractData(Model model)
        {
            base.ExtractData(model);

            SetVerticesUvs(model.UVs);
            SetVerticesNormals(model.Normals);

            texture = model.texture;

            SetIndices(model.indices);
        }

		/// <summary>
		/// Render the loaded data into the screen.
		/// </summary>
		/// <param name="screen">The screen</param>
        public override void Render(Screen screen)
        {
			base.Render(screen);

            Matrix4 transformation = world * view * projection;
            TransformData(ShaderHelper.TransformPosition, positions, transformation, screen);

            Matrix4 normalTransformation = world.Inverted();
            normalTransformation.Transpose();
            TransformData(ShaderHelper.TransformNormal, normals, normalTransformation, screen);

            DrawTriangles(indices);
        }

        protected override TriangleData GenerateTriangleData()
        {
            TriangleData result = new TriangleData();

            Vector3 normal = Vector3.Normalize((normals[p1.normal] + normals[p2.normal] + normals[p3.normal]) / 3.0f); // Compute face normal
            result.brightness = Math.Max(0, Vector3.Dot(normal, -lightDirection)); // Compute face brightness
            if (result.brightness > 1)
                result.brightness = 1;
            if (result.brightness < ambientLight)
                result.brightness = ambientLight;

            return result;
        }

        protected override LineData GenerateScanLineData(float gradient1, float gradient2, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd)
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

            result.brightness = triangleData.brightness;

            return result;
        }

        protected override void ProcessPixel(int x, int y, float gradient)
        {
            var w = 1 / ShaderHelper.Lerp(lineData.w1, lineData.w2, gradient);
            var z = ShaderHelper.Lerp(lineData.z1, lineData.z2, gradient) * w;
            Vector2 uv = ShaderHelper.Lerp(lineData.uv1, lineData.uv2, gradient) * w;

            int tx = (int)(texture.GetLength(0) * uv.X);
            if (tx >= texture.GetLength(0))
                tx = texture.GetLength(0) - 1;
            int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
            if (ty >= texture.GetLength(1))
                ty = texture.GetLength(1) - 1;

            if (tx < 0) tx = 0;
            if (ty < 0) ty = 0;
            Color3 c = texture[tx, ty];

            c.r = Convert.ToByte(c.r * lineData.brightness);
            c.g = Convert.ToByte(c.g * lineData.brightness);
            c.b = Convert.ToByte(c.b * lineData.brightness);

            screen.TryPutPixel(x, y, z, c);
        }

    }
}