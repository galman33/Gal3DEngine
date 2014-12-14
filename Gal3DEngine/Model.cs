using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    class Model
    {

        private Vector4[] vertices;
        private Vector2[] uvs;
        private Vector3[] normals;

        private IndexPositionUVNormal[] indices;

        private Color3[,] texture;

        public Model(string objPath, string texturePath)
        {
            Load(System.IO.File.ReadAllText(objPath));
            texture = Texture.LoadTexture(texturePath);
        }

        private Random rand = new Random();

        public void Load(string content)
        {
            List<Vector4> verticesLst = new List<Vector4>();
            List<Vector2> uvLst = new List<Vector2>();
            List<Vector3> normalsLst = new List<Vector3>();

            List<IndexPositionUVNormal> indicesLst = new List<IndexPositionUVNormal>();


            string[] lines = content.Split(new char[]{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            int i ;
            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("v "))
                {
                    string[] args = lines[i].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                    verticesLst.Add(new Vector4(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]), 1));
                }

                if (lines[i].StartsWith("vt "))
                {
                    string[] args = lines[i].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                    uvLst.Add(new Vector2(float.Parse(args[1]), float.Parse(args[2])));
                }

                if (lines[i].StartsWith("vn "))
                {
                    string[] args = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    normalsLst.Add(new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3])));
                }

                if (lines[i].StartsWith("f "))
                {
                    string[] args = lines[i].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                    if (args[1].Contains("//"))
                        continue;

                    indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[1].Split('/')[0]) - 1, int.Parse(args[1].Split('/')[1]) - 1, int.Parse(args[1].Split('/')[2]) - 1));
                    indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[2].Split('/')[0]) - 1, int.Parse(args[2].Split('/')[1]) - 1, int.Parse(args[2].Split('/')[2]) - 1));
                    indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[3].Split('/')[0]) - 1, int.Parse(args[3].Split('/')[1]) - 1, int.Parse(args[3].Split('/')[2]) - 1));

                    if (args.Length == 5) // 1 + 4
                    {
                        indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[3].Split('/')[0]) - 1, int.Parse(args[3].Split('/')[1]) - 1, int.Parse(args[3].Split('/')[2]) - 1));
                        indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[4].Split('/')[0]) - 1, int.Parse(args[4].Split('/')[1]) - 1, int.Parse(args[4].Split('/')[2]) - 1));
                        indicesLst.Add(new IndexPositionUVNormal(int.Parse(args[1].Split('/')[0]) - 1, int.Parse(args[1].Split('/')[1]) - 1, int.Parse(args[1].Split('/')[2]) - 1));
                    }
                }
            }

            vertices = verticesLst.ToArray();
            uvs = uvLst.ToArray();
            normals = normalsLst.ToArray();

            indices = indicesLst.ToArray();
        }

        public void Render(Screen screen, Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            ShaderPhong.projection = projection;
            ShaderPhong.view = view;
            ShaderPhong.world = world;
            ShaderPhong.lightDirection = Vector3.Normalize(new Vector3(0, 1, 0));

            ShaderPhong.texture = texture;

            ShaderPhong.SetVerticesPositions(vertices);
            ShaderPhong.SetVerticesUvs(uvs);
            ShaderPhong.SetVerticesNormals(normals);
            ShaderPhong.Render(screen, indices);
        }

    }
}
