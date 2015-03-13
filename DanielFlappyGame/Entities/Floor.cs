using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine.IndicesTypes;
namespace DanielFlappyGame
{
    public class Floor
    {
        Color3[,] texture;
        public Vector3 translation;
        Vector3 scale;
        Vector3 rotation;

        Vector4[] vertices;
        Vector2[] uvs;
        Vector3[] normals;
        IndexPositionUVNormal[] indices;

        public Floor(Vector3 translation ,Vector3 scale , Vector3 rotation , string url)
        {
            texture = Texture.LoadTexture(url);
            this.translation = translation;
            this.scale = scale;
            this.rotation = rotation;
            InitFloor();
        }

        public void Update(Vector3 translation)
        {
            this.translation = translation;
        }

        private void InitFloor()
        {
            vertices = new Vector4[4];
            vertices[0] = new Vector4(0, 0, 0, 1);
            vertices[1] = new Vector4(1, 0, 0, 1);
            vertices[2] = new Vector4(0, 0, -1, 1);
            vertices[3] = new Vector4(1, 0, -1, 1);
            uvs = new Vector2[4];
            uvs[0] = new Vector2(0,0);
            uvs[1] = new Vector2(1,0);
            uvs[2] = new Vector2(0,1);
            uvs[3] = new Vector2(1,1);
            normals= new Vector3[1];

           normals[0] = new Vector3(0,1,0);
           

            indices = new IndexPositionUVNormal[6];
            indices[0] = new IndexPositionUVNormal(0, 0, 0);
            indices[1] = new IndexPositionUVNormal(1, 1, 0);
            indices[2] = new IndexPositionUVNormal(2, 2, 0);
            indices[3] = new IndexPositionUVNormal(3, 3, 0);
            indices[4] = new IndexPositionUVNormal(2, 2, 0);
            indices[5] = new IndexPositionUVNormal(1, 1, 0);
        }

        public void Render(Screen screen)
        {           
            (Program.world as FlapGameWorld).curShader.SetVerticesNormals(normals);
            (Program.world as FlapGameWorld).curShader.SetVerticesPositions(vertices);
            (Program.world as FlapGameWorld).curShader.SetIndices(indices);
            (Program.world as FlapGameWorld).curShader.SetVerticesUvs(uvs);
            (Program.world as FlapGameWorld).curShader.texture = texture;
            (Program.world as FlapGameWorld).curShader.lightDirection = -normals[0];//new Vector3(0, -1, 0);
            (Program.world as FlapGameWorld).curShader.ambientLight = 0.5f;
            (Program.world as FlapGameWorld).curShader.world =  Matrix4.CreateTranslation(translation);
            (Program.world as FlapGameWorld).curShader.Render(screen);
        }

    }
}
