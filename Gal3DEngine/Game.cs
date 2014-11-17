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

        private float scale = 0.5f;
        private float rotY = 0;
        private float rotX = MathHelper.Pi;
        private float transX = 0;
        private float transY = 0.25f;

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

        RefType<Vector4>[] vertices;
        RefType<Vector2>[] uvs;
        Color3[,] texture;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            model = new Model("Resources/Cat2.obj", "Resources/Cat2.png");

            vertices = new RefType<Vector4>[3];
            uvs = new RefType<Vector2>[3];

            vertices[0] = new RefType<Vector4>(new Vector4(-0.5f, 0.5f, -1, 1));
            uvs[0] = new RefType<Vector2>(new Vector2(0, 0));
            vertices[1] = new RefType<Vector4>(new Vector4(0.5f, 0.5f, -1, 1));
            uvs[1] = new RefType<Vector2>(new Vector2(1, 0));
            vertices[2] = new RefType<Vector4>(new Vector4(-0.5f, -0.5f, -1, 1));
            uvs[2] = new RefType<Vector2>(new Vector2(0, 1));

            texture = Texture.LoadTexture("Resources/TesTexture.png");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        Random rand = new Random();

        int ticks = 0;
        int lastSec = 0;

        float f = 0;

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            scale += e.Delta*0.01f;
        }

        protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Mouse.LeftButton == OpenTK.Input.ButtonState.Pressed)
            {
                rotY += -e.XDelta * 0.01f;
                rotX += e.YDelta * 0.01f;
            }
            if (e.Mouse.MiddleButton == OpenTK.Input.ButtonState.Pressed)
            {
                transX += -e.XDelta * 0.01f;
                transY += e.YDelta * 0.01f;
            }
        }

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

            Matrix4 world = Matrix4.CreateScale(scale) * Matrix4.CreateRotationY(rotY) * Matrix4.CreateRotationX(/*MathHelper.Pi*/ rotX) * Matrix4.CreateTranslation(transX, transY, 1);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 100);

            model.Render(screen, world, view, projection);

            /*ShaderUV.projection = ShaderUV.view = ShaderUV.world = Matrix4.Identity;
            ShaderUV.texture = texture;
            ShaderUV.Render(screen, vertices, uvs);*/

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
