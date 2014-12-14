using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Gal3DEngine
{
    class Screen
    {
        public int Width;
        public int Height;
        
        private Color3[] colorBuffer;
        private float[] zBuffer;

        public Screen()
        {

        }

        public Screen(int width, int height)
        {
            Init(width, height);
        }

        public void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.colorBuffer = new Color3[width * height];
            this.zBuffer = new float[width * height];
        }

        public void PutPixel(int x, int y, Color3 color)
        {
            if(x < Width && x >= 0 && y < Height && y >=0)
                colorBuffer[x + y * Width] = color;
        }

        public void TryPutPixel(int x, int y, float z, Color3 color)
        {
            if (x < Width && x >= 0 && y < Height && y >= 0)
            {
                if (zBuffer[x + y * Width] > z)
                {
                    colorBuffer[x + y * Width] = color;
                    zBuffer[x + y * Width] = z;
                }
            }
        }

        public Color3 ReadPixel(int x, int y)
        {
            if (x < Width && x >= 0 && y < Height && y >= 0)
                return colorBuffer[x + y * Width];
            else
                throw new Exception("Pixel not in screen!");
        }

        public void Clear(Color3 clearColor)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    PutPixel(x, y, clearColor);
                    zBuffer[x + y * Width] = float.PositiveInfinity;
                }
            }
        }

        public void Render()
        {
            GL.DrawPixels(Width, Height, PixelFormat.Rgb, PixelType.UnsignedByte, colorBuffer);
        }

        public void DrawLine(Point2i p1, Point2i p2, Color3 color)
        {
            DrawLine(p1.X, p1.Y, p2.X, p2.Y, color);
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color3 color)
        {
            DrawLine((int)p1.X, (int) p1.Y, (int) p2.X, (int) p2.Y, color);
        }

        public void DrawLine(Vector3 p1, Vector3 p2, Color3 color)
        {
            DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color);
        }

        public void DrawLine(Vector4 p1, Vector4 p2, Color3 color)
        {
            DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color3 color)
        {
            // Bresenham's_line_algorithm
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
                t = x2; // swap x2 and y2
                x2 = y2;
                y2 = t;
            }
            if (x1 > x2)
            {
                int t;
                t = x1; // swap x1 and x2
                x1 = x2;
                x2 = t;
                t = y1; // swap y1 and y2
                y1 = y2;
                y2 = t;
            }
            int dx = x2 - x1;
            int dy = Math.Abs(y2 - y1);
            int error = dx / 2;
            int ystep = (y1 < y2) ? 1 : -1;
            int y = y1;
            for (int x = x1; x <= x2; x++)
            {
                PutPixel((steep ? y : x), (steep ? x : y), color);
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        public void DrawTriangleOutline(Vector4 p1, Vector4 p2, Vector4 p3, Color3 color)
        {
            DrawLine(p1, p2, color);
            DrawLine(p1, p3, color);
            DrawLine(p2, p3, color);
        }

        public void DrawTriangleOutline(Vector3 p1, Vector3 p2, Vector3 p3, Color3 color)
        {
            DrawLine(p1, p2, color);
            DrawLine(p1, p3, color);
            DrawLine(p2, p3, color);
        }

        public void DrawTriangleOutline(Vector2 p1, Vector2 p2, Vector2 p3, Color3 color)
        {
            DrawLine(p1, p2, color);
            DrawLine(p1, p3, color);
            DrawLine(p2, p3, color);
        }


        public void DrawTriangleOutline(Point2i p1, Point2i p2, Point2i p3, Color3 color)
        {
            DrawLine(p1, p2, color);
            DrawLine(p1, p3, color);
            DrawLine(p2, p3, color);
        }


        public void DrawTrianglesOutline(Matrix4 world, Matrix4 view, Matrix4 projection, List<Vector4> vertices, List<int> indices)
        {
            Vector4[] transformedVertices = new Vector4[vertices.Count];

            Vector4 v;

            for (int i = 0; i < vertices.Count; i++)
            {
                v = Vector4.Transform(vertices[i], view * world * projection); // projection * view * world

                v.X = v.X / v.W * 0.5f * Width + Width / 2;
                v.Y = v.Y / v.W * 0.5f * Height + Height / 2;
                v.Z = v.Z / v.W;

                transformedVertices[i] = v;
            }

            for (int i = 0; i < indices.Count; i += 3)
            {
                DrawTriangleOutline(transformedVertices[indices[i + 0]], transformedVertices[indices[i + 1]], transformedVertices[indices[i + 2]], new Color3(255, 255, 255));
            }
        }
    }
}
