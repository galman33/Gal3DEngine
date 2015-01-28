using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{

    public class ShaderPhong : Shader<IndexPositionUVNormal, object, ShaderPhong.LineData>
    {

        public struct LineData
        {
            public float z1, z2;
            public Vector2 uv1, uv2;
            public Vector3 n1, n2;
        }

        public Matrix4 world;
        public Matrix4 view;
        public Matrix4 projection;
        public Vector3 lightDirection;
        public float ambientLight;

        public Color3[,] texture;

        private Vector2[] uvs;
        private Vector3[] normals;
        private IndexPositionUVNormal[] indices;

        public void SetVerticesUvs(Vector2[] uvs)
        {
            this.uvs = uvs;
        }

        public void SetVerticesNormals(Vector3[] normals)
        {
            this.normals = (Vector3[])normals.Clone();
        }

        public void SetIndices(IndexPositionUVNormal[] indices)
        {
            this.indices = indices;
        }

        public override void ExtractData(Model model) 
        {
            base.ExtractData(model);

            this.SetVerticesUvs(model.UVs);
            this.SetVerticesNormals(model.Normals);

            this.texture = model.texture;

            this.SetIndices(model.indices);
        }

        public override void Render(Screen screen)
        {
            Matrix4 transformation = world * view * projection;
            TransformData(ShaderHelper.TransformPosition, positions, transformation, screen);

            Matrix4 normalTransformation = world.Inverted();
            normalTransformation.Transpose();
            TransformData(ShaderHelper.TransformNormal, normals, normalTransformation, screen);
            
            DrawTriangles(indices, screen);
        }
        
        protected override LineData MyProcessScanLine(float gradient1, float gradient2, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd, object triangleData)
        {
            LineData result = new LineData();

            // starting Z & ending Z
            result.z1 = ShaderHelper.Lerp(positions[pa.position].Z, positions[pb.position].Z, gradient1);
            result.z2 = ShaderHelper.Lerp(positions[pc.position].Z, positions[pd.position].Z, gradient2);

            // starting uv & ending uv
            result.uv1 = ShaderHelper.Lerp(uvs[pa.uv], uvs[pb.uv], gradient1);
            result.uv2 = ShaderHelper.Lerp(uvs[pc.uv], uvs[pd.uv], gradient2);

            result.n1 = ShaderHelper.Lerp(normals[pa.normal], normals[pb.normal], gradient1).Normalized();
            result.n2 = ShaderHelper.Lerp(normals[pc.normal], normals[pd.normal], gradient2).Normalized();

            return result;
        }

        protected override void ProcessPixel(int x, int y, float gradient, ref LineData lineData, Screen screen)
        {
            var z = ShaderHelper.Lerp(lineData.z1, lineData.z2, gradient);
            Vector2 uv = ShaderHelper.Lerp(lineData.uv1, lineData.uv2, gradient);
            Vector3 n = ShaderHelper.Lerp(lineData.n1, lineData.n2, gradient).Normalized();

            float brightness = Vector3.Dot(n, -lightDirection);
            if(brightness < ambientLight) brightness = ambientLight;
            if(brightness > 1) brightness = 1;

            int tx = (int)(texture.GetLength(0) * uv.X);
            if (tx >= texture.GetLength(0))
                tx = texture.GetLength(0) - 1;
            int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
            if (ty >= texture.GetLength(1))
                ty = texture.GetLength(1) - 1;
            Color3 c = texture[tx, ty];

            c.r = Convert.ToByte(c.r * brightness);
            c.g = Convert.ToByte(c.g * brightness);
            c.b = Convert.ToByte(c.b * brightness);

            screen.TryPutPixel(x, y, z, c);
        }

    }
}
