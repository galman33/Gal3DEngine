﻿using Gal3DEngine;
using Gal3DEngine.IndicesTypes;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    public class CubeMap
    {
        private Box box;
        private Color3[,] texture;
        public CubeMap(Vector3 radius ,Color3[,] texture)
        {
            box = new Box(radius.X, radius.Y, radius.Z, Vector3.Zero);
            this.texture = texture;
            InitCubeMap();
        }
        Vector4[] wireFrames;
       
        Vector2[] uvs;
        IndexPositionUVNormal[] indices;
        Vector3[] normals;

        private Vector3 GetMin()
        {
            return new Vector3(box.origin.X - box.radiusX, box.origin.Y - box.radiusY, box.origin.Z - box.radiusZ);
        }

        private Vector3 GetMax()
        {
            return new Vector3(box.origin.X + box.radiusX, box.origin.Y + box.radiusY, box.origin.Z + box.radiusZ);
        }


        private void InitCubeMap()
        {
            var min = GetMin();
            var max = GetMax();

            wireFrames = new Vector4[8];
            wireFrames[0] = new Vector4(min.X, min.Y, min.Z, 1); // origin
            wireFrames[1] = new Vector4(max.X, min.Y, min.Z, 1);
            wireFrames[2] = new Vector4(min.X, max.Y, min.Z, 1);
            wireFrames[3] = new Vector4(min.X, min.Y, max.Z, 1);
            wireFrames[4] = new Vector4(max.X, max.Y, min.Z, 1);
            wireFrames[5] = new Vector4(max.X, min.Y, max.Z, 1);
            wireFrames[6] = new Vector4(min.X, max.Y, max.Z, 1);
            wireFrames[7] = new Vector4(max.X, max.Y, max.Z, 1);
            normals = new Vector3[1] { Vector3.One };
            /*
                .2------4 
                /|     /| 
               6-+----7 | 
               | |    | | 
               | 0----+-1 
               |/     |/
               3------5 
            */
            uvs = new Vector2[wireFrames.Length];
            /*uvs[0] = new Vector2(1f / 2f,   2f / 3f);
            uvs[1] = new Vector2(1f / 2f,   1f / 3f);
            uvs[2] = new Vector2(1f / 4f,   2f / 3f);
            uvs[3] = new Vector2(3f / 4f,   2f / 3f);
            uvs[4] = new Vector2(1f / 4f,   1f / 3f);
            uvs[5] = new Vector2(3f / 4f,   1f / 3f);
            uvs[6] = new Vector2(0f     ,   1f / 3f);
            uvs[7] = new Vector2(0f     ,   1f / 3f);*/
            uvs[0] = new Vector2(1f / 2f, 1f / 3f);
            uvs[1] = new Vector2(1f / 2f, 1f / 3f);
            uvs[2] = new Vector2(1f / 4f, 2f / 3f);
            uvs[3] = new Vector2(3f / 4f, 2f / 3f);

            List<IndexPositionUVNormal> indicesLst = new List<IndexPositionUVNormal>();

            addRectIndices(indicesLst, 0, 3, 5, 1); // floor
            addRectIndices(indicesLst, 2, 4, 7, 6); // ceil
            addRectIndices(indicesLst, 4, 1, 5, 7); // right
            addRectIndices(indicesLst, 2, 6, 3, 0); // left
            addRectIndices(indicesLst, 4, 2, 0, 1); // front
            addRectIndices(indicesLst, 6, 7, 5, 3); // back

            indices = indicesLst.ToArray();

            return;
            
            
            //indices = new IndexPositionUVNormal[24];
            
            
            //floor rec
            indices[0] = new IndexPositionUVNormal(1, 1 , 0);
            indices[1] = new IndexPositionUVNormal(0, 0 , 0);
            indices[2] = new IndexPositionUVNormal(5, 5 , 0);
            indices[3] = new IndexPositionUVNormal(3, 3 , 0);
            indices[4] = new IndexPositionUVNormal(5, 5 , 0);
            indices[5] = new IndexPositionUVNormal(0, 0 , 0);

            //ceiling rec
            indices[6] = new IndexPositionUVNormal(2, 2 , 0);
            indices[7] = new IndexPositionUVNormal(4, 4 , 0);
            indices[8] = new IndexPositionUVNormal(7, 7 , 0);
            indices[9] = new IndexPositionUVNormal(6, 6 , 0);
            indices[10] = new IndexPositionUVNormal(2, 2 , 0);
            indices[11] = new IndexPositionUVNormal(7, 7 , 0);
            /*
            0,0,-1    1,0,-1
            2_________0
            |*--------|
            |---*-----|
            6_________3
             0,0,0     1,0,0
            */
            //side rec

            indices[12] = new IndexPositionUVNormal(1, 1 , 0);
            indices[13] = new IndexPositionUVNormal(4, 4 , 0);
            indices[14] = new IndexPositionUVNormal(7, 7 , 0);
            indices[15] = new IndexPositionUVNormal(5, 5 , 0);
            indices[16] = new IndexPositionUVNormal(1, 1, 0);
            indices[17] = new IndexPositionUVNormal(7, 7, 0);

            //side 2 rec

            indices[18] = new IndexPositionUVNormal(2, 2, 0);
            indices[19] = new IndexPositionUVNormal(3, 3, 0);
            indices[20] = new IndexPositionUVNormal(0, 0, 0);
            indices[21] = new IndexPositionUVNormal(2, 2, 0);
            indices[22] = new IndexPositionUVNormal(6, 6, 0);
            indices[23] = new IndexPositionUVNormal(3, 3, 0);
        }


        private void addRectIndices(List<IndexPositionUVNormal> indicesLst, int a, int b, int c, int d)
        {
            indicesLst.Add(new IndexPositionUVNormal(a, a, 0));
            indicesLst.Add(new IndexPositionUVNormal(b, b, 0));
            indicesLst.Add(new IndexPositionUVNormal(c, c, 0));

            indicesLst.Add(new IndexPositionUVNormal(a, a, 0));
            indicesLst.Add(new IndexPositionUVNormal(c, c, 0));
            indicesLst.Add(new IndexPositionUVNormal(d, d, 0));
        }

        public void Render(Screen screen , Matrix4 view , Matrix4 projection)
        {
            //for (int i = 0; i < wireFrames.Length; i++)
            //{
            //    Vector4 tmp = new Vector4(wireFrames[i]);
            //    tmp.W = 1;
            //    ShaderHelper.TransformPosition(ref tmp, /*Matrix4.CreateTranslation(origin) **/ view * projection, screen);
            //    wireFrames[i] = new Vector4(tmp);
            //}
            Matrix4 world = Matrix4.CreateTranslation(0, 0, -5);//Matrix4.CreateScale(5);           

            ShaderFlat curShader = (Program.world as FlapGameWorld).curShader;
           
            curShader.SetVerticesPositions(wireFrames);
            curShader.SetVerticesNormals(normals);
            curShader.SetIndices(indices);
            curShader.SetVerticesUvs(uvs);
            curShader.texture = this.texture;           
            curShader.ambientLight = 1f;
            curShader.world = world; //Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);
            curShader.Render(screen);
            
        }
    }
}