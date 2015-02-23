﻿using System;
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

        private float BlockSize = 1.0f;
        private int CityLength = 15;

        private static Color3[,] groundTexture;

        public Vector4[] positions;
        private static Vector2[] uvs;
        private static Vector3[] normals;
        private static IndexPositionUVNormal[] indices;

        private bool[,] buildingsArray;

        private ShaderFlat shader = AvailableShaders.ShaderFlat;

        public static void LoadContent()
        {
            groundTexture = Texture.LoadTexture("Resources/City.png");
        }

        public Enviroment()
        {
            GenerateBuildings();

            List<Vector4> positionsLst = new List<Vector4>();
            List<IndexPositionUVNormal> indicesLst = new List<IndexPositionUVNormal>();

            Setup(positionsLst);

            SetupGroundIndices(indicesLst);

            SetupBuildings(positionsLst, indicesLst);

            positions = positionsLst.ToArray();
            indices = indicesLst.ToArray();
        }

        private void GenerateBuildings()
        {
            Random r = new Random();

            buildingsArray = new bool[CityLength, CityLength];

            for (int x = 0; x < buildingsArray.GetLength(0); x++)
            {
                for (int y = 0; y < buildingsArray.GetLength(1); y++)
                {
                    if (IsOutlineBuilding(x, y))
                    {
                        buildingsArray[x, y] = true;
                    }
                    else
                    {
                        buildingsArray[x, y] = r.Next(15) == 0; ;
                    }
                }
            }
        }

        private void Setup(List<Vector4> positionsLst)
        {
            uvs = new Vector2[12];
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(0.5f, 0);
            uvs[2] = new Vector2(0.5f, 0.5f);
            uvs[3] = new Vector2(0, 0.5f);

            uvs[4] = new Vector2(0.5f, 0);
            uvs[5] = new Vector2(1.0f, 0);
            uvs[6] = new Vector2(1.0f, 0.5f);
            uvs[7] = new Vector2(0.5f, 0.5f);

            uvs[8] = new Vector2(0, 0.5f);
            uvs[9] = new Vector2(0.5f, 0.5f);
            uvs[10] = new Vector2(0.5f, 1.0f);
            uvs[11] = new Vector2(0, 1.0f);

            normals = new Vector3[5];
            normals[0] = new Vector3(0, 1, 0);
            normals[1] = new Vector3(-1, 0, 0);
            normals[2] = new Vector3(1, 0, 0) ;
            normals[3] = new Vector3(0, 0, -1);
            normals[4] = new Vector3(0, 0, 1);

            positions = new Vector4[(buildingsArray.GetLength(0) + 1) * (buildingsArray.GetLength(1) + 1)];
            for (int y = 0; y < CityLength + 1; y++)
                for (int x = 0; x < CityLength + 1; x++)
                    positionsLst.Add(new Vector4(x * BlockSize, 0, y * BlockSize, 1));
        }

        private void SetupGroundIndices(List<IndexPositionUVNormal> indicesLst)
        {
            for (int x = 0; x < CityLength; x++)
            {
                for (int y = 0; y < CityLength; y++)
                {
                    if (!buildingsArray[x, y])
                    {
                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x + 1, y), 0, 0));
                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x, y), 1, 0));
                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x, y + 1), 2, 0));

                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x, y + 1), 2, 0));
                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x + 1, y + 1), 3, 0));
                        indicesLst.Add(new IndexPositionUVNormal(GetGroundPositionIndex(x + 1, y), 0, 0));
                    }
                }
            }
        }

        private void SetupBuildings(List<Vector4> positionsLst, List<IndexPositionUVNormal> indicesLst)
        {
            Random r = new Random();
            for (int y = 0; y < CityLength; y++)
            {
                for (int x = 0; x < CityLength; x++)
                {
                    if (buildingsArray[x, y])
                    {
                        int a = GetGroundPositionIndex(x, y); // -x -y -z
                        int b = GetGroundPositionIndex(x + 1 , y); // +x -y -z
                        int c = GetGroundPositionIndex(x + 1, y + 1); // +x -y +z
                        int d = GetGroundPositionIndex(x, y + 1); // -x -y +z

                        int floors = r.Next(1, 3); // 1 or 2

                        if(IsOutlineBuilding(x, y))
                            floors = 2;

                        float buildingHeight = 2.0f * floors;

                        int e = positionsLst.Count; // -x +y -z
                        positionsLst.Add(new Vector4(x * BlockSize, buildingHeight, y * BlockSize, 1));
                        int f = positionsLst.Count; // +x +y -z
                        positionsLst.Add(new Vector4((x + 1) * BlockSize, buildingHeight, y * BlockSize, 1));
                        int g = positionsLst.Count; // +x +y +z
                        positionsLst.Add(new Vector4((x + 1) * BlockSize, buildingHeight, (y + 1) * BlockSize, 1));
                        int h = positionsLst.Count; // -x +y +z
                        positionsLst.Add(new Vector4(x * BlockSize, buildingHeight, (y + 1) * BlockSize, 1));

                        //ROOF
                        AddRect(e, f, g, h, 4, 5, 6, 7, 4, indicesLst);

                        //Wall -X
                        AddRect(e, h, d, a, 11, 10, 9, 8, 1, indicesLst);

                        //Wall +X
                        AddRect(g, f, b, c, 11, 10, 9, 8, 2, indicesLst);

                        //Wall -Z
                        AddRect(a, b, f, e, 8, 9, 10, 11, 3, indicesLst);

                        //Wall +Z
                        AddRect(c, d, h, g, 8, 9, 10, 11, 4, indicesLst);
                    }
                }
            }
        }

        private void AddRect(int pa, int pb, int pc, int pd,
                                int ta, int tb, int tc, int td,
                                int n, List<IndexPositionUVNormal> indicesLst)
        {
            indicesLst.Add(new IndexPositionUVNormal(pb, tb, n));
            indicesLst.Add(new IndexPositionUVNormal(pa, ta, n));
            indicesLst.Add(new IndexPositionUVNormal(pd, td, n));

            indicesLst.Add(new IndexPositionUVNormal(pd, td, n));
            indicesLst.Add(new IndexPositionUVNormal(pc, tc, n));
            indicesLst.Add(new IndexPositionUVNormal(pb, tb, n));
        }

        private int GetGroundPositionIndex(int x, int y)
        {
            return y * (CityLength + 1) + x;
        }

        public void Render(Screen screen, Matrix4 view, Matrix4 projection)
        {
            shader.world = Matrix4.Identity;
            shader.view = view;
            shader.projection = projection;
            shader.texture = groundTexture;
            shader.ambientLight = 0.3f;
            shader.lightDirection = (new Vector3(-1, -1, -1)).Normalized();

            shader.SetVerticesPositions(positions);
            shader.SetVerticesUvs(uvs);
            shader.SetVerticesNormals(normals);

            shader.SetIndices(indices);

            shader.Render(screen);
        }

        private bool IsOutlineBuilding(int x, int y)
        {
            return x == 0 || y == 0 || x == buildingsArray.GetLength(0) - 1 || y == buildingsArray.GetLength(1) - 1;
        }

    }
}
