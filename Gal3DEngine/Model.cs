using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Model
    {

        RefType<Vector4>[] vertices;
        RefType<Vector2>[] uvs;
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
            List<RefType<Vector4>> indicesLst = new List<RefType<Vector4>>();
            List<RefType<Vector2>> indicesUVLst = new List<RefType<Vector2>>();


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

                if (lines[i].StartsWith("f "))
                {
                    string[] args = lines[i].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                    if (args[1].Contains("//"))
                        continue;

                    indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[1].Split('/')[0]) - 1]));
                    indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[2].Split('/')[0]) - 1]));
                    indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[3].Split('/')[0]) - 1]));

                    indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[1].Split('/')[1]) - 1]));
                    indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[2].Split('/')[1]) - 1]));
                    indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[3].Split('/')[1]) - 1]));

                    if (args.Length == 5) // 1 + 4
                    {
                        indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[3].Split('/')[0]) - 1]));
                        indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[4].Split('/')[0]) - 1]));
                        indicesLst.Add(new RefType<Vector4>(verticesLst[int.Parse(args[1].Split('/')[0]) - 1]));

                        indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[3].Split('/')[1]) - 1]));
                        indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[4].Split('/')[1]) - 1]));
                        indicesUVLst.Add(new RefType<Vector2>(uvLst[int.Parse(args[1].Split('/')[1]) - 1]));
                    }

                    
                }
            }

            /*vertices = new VertexUV[verticesLst.Count];
            for(i = 0; i < verticesLst.Count; i++)
            {
                vertices[i].Position = verticesLst[i];
                vertices[i].UV = uvLst[i];
            }*/

            /*vertices = verticesLst.ToArray();
            uvs = uvLst.ToArray();*/
            vertices = indicesLst.ToArray();
            uvs = indicesUVLst.ToArray();
        }

        public void Render(Screen screen, Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            //screen.DrawTrianglesOutline(world, view, projection, vertices, indices);
            ShaderUV.projection = projection;
            ShaderUV.view = view;
            ShaderUV.world = world;

            ShaderUV.texture = texture;

            ShaderUV.Render(screen, vertices, uvs);
        }

    }
}
