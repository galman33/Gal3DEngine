using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Model
    {

        List<Vector4> vertices = new List<Vector4>();
        List<int> indices = new List<int>();

        public Model(string file)
        {
            Load(TestModel.Cat);
            //Load(System.IO.File.ReadAllText(file));
        }

        public void Load(string content)
        {
            string[] lines = content.Split(new char[]{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            int i ;
            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("v "))
                {
                    string[] args = lines[i].Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                    vertices.Add(new Vector4(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]), 1));
                }
            }

            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("f "))
                {
                    string[] args = lines[i].Split(' ');
                        
                    indices.Add(int.Parse(args[1].Split('/')[0]) - 1);
                    indices.Add(int.Parse(args[2].Split('/')[0]) - 1);
                    indices.Add(int.Parse(args[3].Split('/')[0]) - 1);
                }
            }
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
