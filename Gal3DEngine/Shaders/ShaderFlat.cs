﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{


    public class ShaderFlat : Shader<IndexPositionUVNormal, ShaderFlat.TriangleData, ShaderFlat.LineData>
    {

        public struct TriangleData
        {
            public float brightness;
        }

        public struct LineData
        {
            public float z1, z2;
            public Vector2 uv1, uv2;
            public float brightness;
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

            SetVerticesUvs(model.UVs);
            SetVerticesNormals(model.Normals);

            texture = model.texture;

            SetIndices(model.indices);
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

        protected override TriangleData ProcessTriangle(IndexPositionUVNormal p1, IndexPositionUVNormal p2, IndexPositionUVNormal p3)
        {
            TriangleData result = new TriangleData();

            Vector3 normal = Vector3.Normalize((normals[p1.normal] + normals[p2.normal] + normals[p3.normal]) / 3.0f); // Compute face normal
            result.brightness = Math.Max(0, Vector3.Dot(normal, lightDirection)); // Compute face brightness
            if (result.brightness > 1)
                result.brightness = 1;
            if (result.brightness < ambientLight)
                result.brightness = ambientLight;

            return result;
        }

        protected override LineData MyProcessScanLine(float gradient1, float gradient2, IndexPositionUVNormal pa, IndexPositionUVNormal pb, IndexPositionUVNormal pc, IndexPositionUVNormal pd, TriangleData triData)
        {
            LineData result = new LineData();

            // starting Z & ending Z
            result.z1 = ShaderHelper.Lerp(positions[pa.position].Z, positions[pb.position].Z, gradient1);
            result.z2 = ShaderHelper.Lerp(positions[pc.position].Z, positions[pd.position].Z, gradient2);

            // starting uv & ending uv
            result.uv1 = ShaderHelper.Lerp(uvs[pa.uv], uvs[pb.uv], gradient1);
            result.uv2 = ShaderHelper.Lerp(uvs[pc.uv], uvs[pd.uv], gradient2);

            result.brightness = triData.brightness;

            return result;
        }

        protected override void ProcessPixel(int x, int y, float gradient, ref LineData lineData, Screen screen)
        {
            var z = ShaderHelper.Lerp(lineData.z1, lineData.z2, gradient);
            Vector2 uv = ShaderHelper.Lerp(lineData.uv1, lineData.uv2, gradient);

            int tx = (int)(texture.GetLength(0) * uv.X);
            if (tx >= texture.GetLength(0))
                tx = texture.GetLength(0) - 1;
            int ty = (int)(texture.GetLength(1) * (1 - uv.Y));
            if (ty >= texture.GetLength(1))
                ty = texture.GetLength(1) - 1;
            Color3 c = texture[tx, ty];

            c.r = Convert.ToByte(c.r * lineData.brightness);
            c.g = Convert.ToByte(c.g * lineData.brightness);
            c.b = Convert.ToByte(c.b * lineData.brightness);

            screen.TryPutPixel(x, y, z, c);
        }

    }
}