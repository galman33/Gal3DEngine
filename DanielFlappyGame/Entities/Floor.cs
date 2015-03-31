using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine.IndicesTypes;
using Gal3DEngine.UTILS;
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

        Box hitBox; 

        public Floor(Vector3 translation ,Vector3 rotation , string url)
        {
            texture = Texture.LoadTexture(url);
            this.translation = translation;
            this.scale = new Vector3(4.5f, 1, 3.1f);
            this.rotation = rotation;
            InitFloor();
        }

        public void Update(Vector3 birdPosition)
        {
            this.translation.Z = birdPosition.Z -3  ;
            AdjustHitBox();
        }

        private void InitFloor()
        {
            vertices = new Vector4[4];
            /*
            0,0,-1    1,0,-1
            2_________3
            |*--------|
            |---*-----|
            0_________1
             0,0,0     1,0,0
            */
            vertices[0] = new Vector4(0, 0, 0.01f, 1);
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

            indices[0] = new IndexPositionUVNormal(3, 3, 0);
            indices[1] = new IndexPositionUVNormal(2, 2, 0);
            indices[2] = new IndexPositionUVNormal(1, 1, 0);
            indices[3] = new IndexPositionUVNormal(0, 0, 0);
            indices[4] = new IndexPositionUVNormal(1, 1, 0);
            indices[5] = new IndexPositionUVNormal(2, 2, 0);
            AdjustHitBox();
        }

        public void Render(Screen screen , int floorsAmount)
        {
            Matrix4[] worlds = new Matrix4[floorsAmount];
            
            for(int i = 0; i<floorsAmount; i++ )
            {
                worlds[i] = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation + new Vector3(0, 0, -3) * i);
                InnerRender(screen, worlds[i]);
            }
            
        }
        private void InnerRender(Screen screen , Matrix4 world)
        {
            ShaderFlat curShader = (Program.world as FlapGameWorld).curShader;
            curShader.SetVerticesNormals(normals);
            curShader.SetVerticesPositions(vertices);
            curShader.SetIndices(indices);
            curShader.SetVerticesUvs(uvs);
            curShader.texture = texture;
            curShader.lightDirection = -normals[0];//new Vector3(0, -1, 0);
            curShader.ambientLight = 0.5f;
            curShader.world = world; //Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);
            curShader.Render(screen);
            hitBox.Render(screen, curShader.view, curShader.projection);
        }
        private void AdjustHitBox()
        {
            this.hitBox = new Box(this.scale.X, 0.01f, this.scale.Z, this.translation);
        }

        public Box GetFloorHitBox()
        {
            return hitBox;
        }

    }
}
