using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Gal3DEngine
{
    class Game : GameWindow
    {

        public int GameWidth = 640;
        public int GameHeight = 480;

        private Screen screen;

        private Model model;

        public Game()
            : base()
        {
            Width = GameWidth;
            Height = GameHeight;

            VSync = VSyncMode.Off;

            WindowBorder = OpenTK.WindowBorder.Fixed;
            screen = new Screen();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            screen.Init(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            model = new Model("Resources/Sasuke.obj");
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        Random rand = new Random();

        int ticks = 0;
        int lastSec = 0;

        float f = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            f += 0.04f;

            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            screen.Clear(new Color3());

            int camX = 0, camY = 0, camZ = 0;
            float rot = 0;
            Matrix4 view = (Matrix4.CreateTranslation(camX, camY, camZ)).Inverted();

            Matrix4 world = Matrix4.CreateScale(0.005f) * Matrix4.CreateRotationY(f) * Matrix4.CreateRotationX(MathHelper.Pi) * Matrix4.CreateTranslation(0, 0.25f, 1);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 100);

            /*List<Vector3> vertices3 = new List<Vector3>();
            CreateCube(vertices3);*/
            /*List<Vector3> vertices3 = modelVertices;

            List<Vector4> vertices = new List<Vector4>();
            foreach(Vector3 vert in vertices3)
                vertices.Add(new Vector4(vert, 1));*/
            /*List<Vector4> vertices = new List<Vector4>(modelVertices);

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Vector4.Transform(vertices[i], view * world * projection); // projection * view * world

                Vector4 v = vertices[i];
                v.X = v.X / v.W * 0.5f * Width + Width / 2;
                v.Y = v.Y / v.W * 0.5f * Height + Height / 2;
                v.Z = v.Z / v.W;
                vertices[i] = v;
            }

            for (int i = 0; i < vertices.Count; i+=3)
            {
                screen.DrawTriangleOutline(vertices[i + 0], vertices[i + 1], vertices[i + 2], new Color3(255, 255, 255));
            }*/
            model.Render(screen, world, view, projection);

            screen.Render();

            SwapBuffers();

            ticks++;
            if (DateTime.Now.Second != lastSec)
            {
                Console.WriteLine(ticks + " FPS");

                ticks = 0;
                lastSec = DateTime.Now.Second;
            }
        }

        private void CreateCube(List<Vector3> vertices)
        {
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f)); // triangle 1 : begin
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f)); // triangle 1 : end
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f)); // triangle 2 : begin
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f)); // triangle 2 : end
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
        }

    }
}
