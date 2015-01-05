using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    public class ShaderFlat : Shader
    {

        public static Matrix4 world;
        public static Matrix4 view;
        public static Matrix4 projection;
        public static Vector3 lightDirection;
        public static float ambientLight;

        public static Color3[,] texture;

        private static Vector2[] uvs;
        private static Vector3[] normals;
        private static IndexPositionUVNormal[] indices;

        public static void SetVerticesUvs(Vector2[] uvs)
        {
            ShaderFlat.uvs = uvs;
        }

        public static void SetVerticesNormals(Vector3[] normals)
        {
            ShaderFlat.normals = (Vector3[])normals.Clone();
        }

        public static void SetIndices(IndexPositionUVNormal[] indices)
        {
            ShaderFlat.indices = indices;
        }

        public static void ExtractData(Model model)
        {
            Shader.ExtractData(model);

            ShaderFlat.SetVerticesUvs(model.UVs);
            ShaderFlat.SetVerticesNormals(model.Normals);

            ShaderFlat.texture = model.texture;

            ShaderFlat.SetIndices(model.indices);
        }

        public static void Render(Screen screen)
        {
            Matrix4 transformation = world * view * projection;
            TransformData(TransformPosition, positions, transformation, screen);

            Matrix4 normalTransformation = world.Inverted();
            normalTransformation.Transpose();
            TransformData(TransformNormal, normals, normalTransformation, screen);

            Shader.DrawTriangles(indices, screen, MyProcessScanLine, ProcessPixel);
        }

        protected static void TransformNormal(ref Vector3 normal, Matrix4 transformation, Screen screen)
        {
            normal = Vector3.Transform(normal, transformation);
        }

        private struct TriangleData
        {
            public float brightness;
        }

        private struct LineData
        {
            public float z1, z2;
            public Vector2 uv1, uv2;
            public float brightness;
        }

        private static TriangleData MyProcessTriangle(IndexPositionUVNormal p1, IndexPositionUVNormal p2, IndexPositionUVNormal p3)
        {
            TriangleData result = new TriangleData();

            Vector3 normal = Vector3.Normalize((normals[p1.normal] + normals[p2.normal] + normals[p3.normal]) / 3.0f); // Compute face normal
            result.brightness = Math.Max(0, Vector3.Dot(normal, lightDirection)); // Compute face brightness

            return result;
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

            return result;
        }

        private static void ProcessPixel(int x, int y, float gradient, ref LineData lineData, Screen screen)
        {
            var z = Lerp(lineData.z1, lineData.z2, gradient);
            Vector2 uv = Lerp(lineData.uv1, lineData.uv2, gradient);
            Vector3 n = Lerp(lineData.n1, lineData.n2, gradient).Normalized();

            float brightness = Vector3.Dot(n, -lightDirection);
            if (brightness < ambientLight) brightness = ambientLight;
            if (brightness > 1) brightness = 1;

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
