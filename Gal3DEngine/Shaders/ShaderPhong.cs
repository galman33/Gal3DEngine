using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{

	/// <summary>
	/// Renders a mesh with interpolated brightness at every mesh.
	/// </summary>
    public class ShaderPhong : Shader<IndexPositionUVNormal, object, ShaderPhong.LineData>
    {

        public struct LineData
        {
            public float w1, w2;
            public float z1, z2;
            public Vector2 uv1, uv2;
            public Vector3 n1, n2;
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

            this.SetVerticesUvs(model.UVs);
            this.SetVerticesNormals(model.Normals);

            this.texture = model.texture;

            this.SetIndices(model.indices);
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
        
        protected override LineData GenerateScanLineData(float gradient1, float gradient2, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd)
        {
            LineData result = new LineData();

            // perspective
            result.w1 = 1 / ShaderHelper.Lerp(positions[pa.position].W, positions[pb.position].W, gradient1);
            result.w2 = 1 / ShaderHelper.Lerp(positions[pc.position].W, positions[pd.position].W, gradient2);

            // starting Z & ending Z
            result.z1 = ShaderHelper.Lerp(positions[pa.position].Z / positions[pa.position].W, positions[pb.position].Z / positions[pb.position].W, gradient1);
            result.z2 = ShaderHelper.Lerp(positions[pc.position].Z / positions[pc.position].W, positions[pd.position].Z / positions[pd.position].W, gradient2);

            // starting uv & ending uv
            result.uv1 = ShaderHelper.Lerp(uvs[pa.uv] / positions[pa.position].W, uvs[pb.uv] / positions[pb.position].W, gradient1);
            result.uv2 = ShaderHelper.Lerp(uvs[pc.uv] / positions[pc.position].W, uvs[pd.uv] / positions[pd.position].W, gradient2);

            result.n1 = ShaderHelper.Lerp(normals[pa.normal] / positions[pa.position].W, normals[pb.normal] / positions[pb.position].W, gradient1).Normalized();
            result.n2 = ShaderHelper.Lerp(normals[pc.normal] / positions[pc.position].W, normals[pd.normal] / positions[pd.position].W, gradient2).Normalized();

            return result;
        }

        protected override void ProcessPixel(int x, int y, float gradient)
        {
            var w = ShaderHelper.Lerp(lineData.w1, lineData.w2, gradient);
            var z = ShaderHelper.Lerp(lineData.z1, lineData.z2, gradient) / w;
            Vector2 uv = ShaderHelper.Lerp(lineData.uv1, lineData.uv2, gradient) / w;
            Vector3 n = ShaderHelper.Lerp(lineData.n1, lineData.n2, gradient).Normalized() / w;

            float brightness = Vector3.Dot(n, -lightDirection);
            if(brightness < ambientLight) brightness = ambientLight;
            if(brightness > 1) brightness = 1;

            int tx = (int)(texture.GetLength(0) * uv.X);
            if (tx >= texture.GetLength(0))
                tx = texture.GetLength(0) - 1;
            int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
            if (ty >= texture.GetLength(1))
                ty = texture.GetLength(1) - 1;
            if (tx < 0) tx = 0;
            if (ty < 0) ty = 0;

            Color3 c = texture[tx, ty];

            c.r = Convert.ToByte(c.r * brightness);
            c.g = Convert.ToByte(c.g * brightness);
            c.b = Convert.ToByte(c.b * brightness);

            screen.TryPutPixel(x, y, z, c);
        }

    }
}
