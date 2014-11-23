using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Model
    {

        Vector4[] vertices;
        Vector2[] uvs;
        Vector3[] normals;

        ShaderFlat.IndexPositionUVNormal[] indices;

        Color3[,] texture;

        public Model(string file, string textureFile)
        {
            Load(System.IO.File.ReadAllText(file));
            texture = Texture.LoadTexture(textureFile);
        }

        private Random rand = new Random();

        public void Load(string content)
        {
            List<Vector4> verticesLst = new List<Vector4>();
            List<Vector2> uvLst = new List<Vector2>();
            List<Vector3> normalsLst = new List<Vector3>();

            List<ShaderFlat.IndexPositionUVNormal> indicesLst = new List<ShaderFlat.IndexPositionUVNormal>();


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

                    indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[1].Split('/')[0]) - 1, int.Parse(args[1].Split('/')[1]) - 1, int.Parse(args[1].Split('/')[2]) - 1));
                    indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[2].Split('/')[0]) - 1, int.Parse(args[2].Split('/')[1]) - 1, int.Parse(args[2].Split('/')[2]) - 1));
                    indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[3].Split('/')[0]) - 1, int.Parse(args[3].Split('/')[1]) - 1, int.Parse(args[3].Split('/')[2]) - 1));

                    if (args.Length == 5) // 1 + 4
                    {
                        indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[3].Split('/')[0]) - 1, int.Parse(args[3].Split('/')[1]) - 1, int.Parse(args[3].Split('/')[2]) - 1));
                        indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[4].Split('/')[0]) - 1, int.Parse(args[4].Split('/')[1]) - 1, int.Parse(args[4].Split('/')[2]) - 1));
                        indicesLst.Add(new ShaderFlat.IndexPositionUVNormal(int.Parse(args[1].Split('/')[0]) - 1, int.Parse(args[1].Split('/')[1]) - 1, int.Parse(args[1].Split('/')[2]) - 1));
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
            ShaderFlat.projection = projection;
            ShaderFlat.view = view;
            ShaderFlat.world = world;
            ShaderFlat.lightDirection = Vector3.Normalize(new Vector3(1, 0, -1));

            ShaderFlat.texture = texture;

            ShaderFlat.SetVerticesPositions(vertices);
            ShaderFlat.SetVerticesUvs(uvs);
            ShaderFlat.SetVerticesNormals(normals);
            ShaderFlat.Render(screen, indices);
        }

    }
}
