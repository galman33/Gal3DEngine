using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Model
    {

        VertexColor[] vertices;
        int[] indices;

        public Model(string file)
        {
            Load(TestModel.Cat);
            //Load(System.IO.File.ReadAllText(file));
        }

        private Random rand = new Random();

        public void Load(string content)
        {
            List<VertexColor> verticesLst = new List<VertexColor>();
            List<int> indicesLst = new List<int>();


            string[] lines = content.Split(new char[]{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            int i ;
            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("v "))
                {
                    string[] args = lines[i].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                    verticesLst.Add(new VertexColor(new Vector4(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]), 1), new Color3(rand.Next(256), rand.Next(256), rand.Next(256))));
                }
            }

            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("f "))
                {
                    string[] args = lines[i].Split(' ');
                        
                    indicesLst.Add(int.Parse(args[1].Split('/')[0]) - 1);
                    indicesLst.Add(int.Parse(args[2].Split('/')[0]) - 1);
                    indicesLst.Add(int.Parse(args[3].Split('/')[0]) - 1);
                }
            }

            vertices = verticesLst.ToArray();
            indices = indicesLst.ToArray();
        }

        public void Render(Screen screen, Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            //screen.DrawTrianglesOutline(world, view, projection, vertices, indices);
            Shader.projection = projection;
            Shader.view = view;
            Shader.world = world;

            Shader.Color = new Color3(255, 128, 128);

            Shader.Render(screen, vertices, indices);
        }

    }
}
