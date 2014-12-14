using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    class ShaderPhong : Shader
    {

        public static Matrix4 world;
        public static Matrix4 view;
        public static Matrix4 projection;
        public static Vector3 lightDirection;

        public static Color3[,] texture;

        private static Vector2[] uvs;
        private static Vector3[] normals;

        public static void SetVerticesUvs(Vector2[] uvs)
        {
            ShaderPhong.uvs = uvs;
        }

        public static void SetVerticesNormals(Vector3[] normals)
        {
            ShaderPhong.normals = (Vector3[])normals.Clone();
        }

        public static void Render(Screen screen, IndexPositionUVNormal[] indices)
        {
            Matrix4 transformation = world * view * projection;
            TransformData(TransformPosition, positions, transformation, screen);

            Matrix4 normalTransformation = world.Inverted();
            normalTransformation.Transpose();
            TransformData(TransformNormal, normals, normalTransformation, screen);
            
            Shader.DrawTriangles(indices, screen, MyProcessScanLine, ProcessPixel);
        }

        protected static void TransformPosition(ref Vector4 position, Matrix4 transformation, Screen screen)
        {
            position = Vector4.Transform(position, transformation); // projection * view * world

            position.X = position.X / position.W * 0.5f * screen.Width + screen.Width / 2;
            position.Y = position.Y / position.W * 0.5f * screen.Height + screen.Height / 2;
            position.Z = position.Z / position.W * 0.5f + 0.5f;
        }

        protected static void TransformNormal(ref Vector3 normal, Matrix4 transformation, Screen screen)
        {
            normal = Vector3.Transform(normal, transformation);
        }

        private struct LineData
        {
            public float z1, z2;
            public Vector2 uv1, uv2;
            public Vector3 n1, n2;
        }

        private static LineData MyProcessScanLine(float gradient1, float gradient2, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd)
        {
            LineData result = new LineData();

            // starting Z & ending Z
            result.z1 = Lerp(positions[pa.position].Z, positions[pb.position].Z, gradient1);
            result.z2 = Lerp(positions[pc.position].Z, positions[pd.position].Z, gradient2);

            // starting uv & ending uv
            result.uv1 = Lerp(uvs[pa.uv], uvs[pb.uv], gradient1);
            result.uv2 = Lerp(uvs[pc.uv], uvs[pd.uv], gradient2);

            result.n1 = Lerp(normals[pa.normal], normals[pb.normal], gradient1).Normalized();
            result.n2 = Lerp(normals[pc.normal], normals[pd.normal], gradient2).Normalized();

            return result;
        }

        private static void ProcessPixel(int x, int y, float gradient, ref LineData lineData, Screen screen)
        {
            var z = Lerp(lineData.z1, lineData.z2, gradient);
            Vector2 uv = Lerp(lineData.uv1, lineData.uv2, gradient);
            Vector3 n = Lerp(lineData.n1, lineData.n2, gradient).Normalized();

            float brightness = Math.Max(0, Vector3.Dot(n, -lightDirection));

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
