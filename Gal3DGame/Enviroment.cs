using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DGame
{
    class Enviroment
    {

        private static Color3[,] groundTexture;

        public Vector4[] positions;
        private static Vector2[] uvs;
        private static Vector3[] normals;
        private static IndexPositionUVNormal[] indices;

        public static void LoadContent()
        {
            groundTexture = Texture.LoadTexture("Resources/ground.jpg");
        }

        public Enviroment()
        {
            positions = new Vector4[4];
            positions[0] = new Vector4(-1, 0, -1, 1);
            positions[1] = new Vector4(1, 0, -1, 1);
            positions[2] = new Vector4(1, 0, 1, 1);
            positions[3] = new Vector4(1, 0, 1, 1);

            uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(1, 1);
            uvs[3] = new Vector2(0, 1);

            normals = new Vector3[1];
            normals[0] = new Vector3(0, 1, 0);
        }

        public void Render()
        {
            

        }

    }
}
