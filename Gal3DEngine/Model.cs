﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    class Model
    {

        public Vector4[] Vertices { private set; get; }
        public Vector2[] UVs { private set; get; }
        public Vector3[] Normals { private set; get; }

        public Color3[,] texture { private set; get; }

        public IndexPositionUVNormal[] indices { private set; get; }

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

            Vertices = verticesLst.ToArray();
            UVs = uvLst.ToArray();
            Normals = normalsLst.ToArray();

            indices = indicesLst.ToArray();
        }

    }
}
