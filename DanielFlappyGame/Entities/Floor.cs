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

    /// <summary>
    /// Handles The Floor entity logic.
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// The color Texture (pixels array).
        /// </summary>
        Color3[,] texture;
        /// <summary>
        /// The floor position.
        /// </summary>
        public Vector3 translation;
        /// <summary>
        /// The floor scale.
        /// </summary>
        Vector3 scale;
        /// <summary>
        /// The floor rotation.
        /// </summary>
        Vector3 rotation;
        /// <summary>
        /// The floor vertecis.
        /// </summary>
        Vector4[] vertices;
        /// <summary>
        /// The floor Uvs.
        /// </summary>
        Vector2[] uvs;
        /// <summary>
        /// The floor normals.
        /// </summary>
        Vector3[] normals;
        /// <summary>
        /// The floor indecis
        /// </summary>
        IndexPositionUVNormal[] indices;
        /// <summary>
        /// The floor hitbox(for collision).
        /// </summary>
        Box hitBox; 

        /// <summary>
        /// Initiallizes a floor from given translation , rotation and texture image path.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <param name="url"></param>
        public Floor(Vector3 translation ,Vector3 rotation , string url)
        {
            texture = Texture.LoadTexture(url);
            this.translation = translation;
            this.scale = new Vector3(10f, 1, 3.1f);
            this.rotation = rotation;
            InitFloor();
        }

        /// <summary>
        /// Updates the floor every frame according to the Flappy bird entity.
        /// </summary>
        /// <param name="birdPosition">The flappy bird entity position.</param>
        public void Update(Vector3 birdPosition)
        {
            this.translation.Z = birdPosition.Z -3  ;
            AdjustHitBox();
        }

        /// <summary>
        /// Initiallizes the floor.
        /// </summary>
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

        /// <summary>
        /// Renders a given amout of floors into a Screen.
        /// </summary>
        /// <param name="screen">The screen to render to.</param>
        /// <param name="floorsAmount">Amount of floors to render.</param>
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
            curShader.lightDirection = -normals[0];
            curShader.ambientLight = 0.5f;
            curShader.world = world; //Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);
            curShader.Render(screen);
            hitBox.Render(screen, curShader.view, curShader.projection);
        }
        /// <summary>
        /// Adjusts the floor hitbox according to its translation.
        /// </summary>
        private void AdjustHitBox()
        {
            this.hitBox = new Box(this.scale.X, 0.01f, this.scale.Z, this.translation);
        }

        /// <summary>
        /// Returns the floor HitBox(for collision purposes).
        /// </summary>
        /// <returns></returns>
        public Box GetFloorHitBox()
        {
            return hitBox;
        }

    }
}
