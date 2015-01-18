using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;
namespace DanielFlappyGame
{
   public class Entity
    {
       public Vector3 Position;
       protected Vector3 rot, scale, translation;
       protected Matrix4 worldMatrix;
       protected Model model;

       private Cube entityCube;

       public Entity(Vector3 translation, Vector3 rotation, Vector3 scale, Model model)
       {
           this.translation = translation;
           this.scale = scale;
           this.rot = rotation;           
           this.model = model;
           this.Position = translation;
           this.entityCube = makeCube(this.Position, 2);
       }

       public void Render(Screen screen)
       {
           //update the world matrix
           GetMatrix(this.translation, this.rot, this.scale, out this.worldMatrix);
           ShaderPhong.ExtractData(model);
           ShaderPhong.world = worldMatrix;
           ShaderPhong.Render(screen);
       }

       private Cube makeCube(Vector3 position, float size)
       {           
           Vector3 min;
           Vector3 max;
           min = new Vector3(position.X - size, position.Y - size, position.Z - size);
           max = new Vector3(position.X + size, position.Y + size, position.Z + size);
           return new Cube(min, max);
       }

       private void GetMatrix(Vector3 translation , Vector3 rotation , Vector3  scale , out Matrix4 world )
       {
           world = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(translation);
       }

       public virtual void Update()
       {
           //update translation
           this.translation = this.Position;          
       }
       public Model GetModel()
       {
           return this.model;
       }

       public class Cube
       {
           public Vector3 min;
           public Vector3 max;

           public Cube(Vector3 min, Vector3 max)
           {
               this.min = min;
               this.max = max;
           }

           public static bool Collide(Cube a, Cube b)
           {
               return (a.max.X >= b.min.X && a.min.X <= b.max.X)
               && (a.max.Y >= b.min.Y && a.min.Y <= b.max.Y)
               && (a.max.Z >= b.min.Z && a.min.Z <= b.max.Z);
           }

           public static void DrawCube(Screen screen, Matrix4 world , Matrix4 view , Matrix4 projection)
           {
               var cubeWidth = 0.3f;
               Vector3 origin = Vector3.Zero;
               Vector3[] wireFrames = new Vector3[8];
                wireFrames[0] = origin; // origin
               wireFrames[1] = new Vector3(origin.X + cubeWidth, origin.Y, origin.Z);
               wireFrames[2] = new Vector3(origin.X, origin.Y + cubeWidth, origin.Z);
               wireFrames[3] = new Vector3(origin.X, origin.Y, origin.Z + cubeWidth);
               wireFrames[4] = new Vector3(origin.X + cubeWidth, origin.Y + cubeWidth, origin.Z);
               wireFrames[5] = new Vector3(origin.X + cubeWidth, origin.Y, origin.Z + cubeWidth);
               wireFrames[6] = new Vector3(origin.X, origin.Y + cubeWidth, origin.Z + cubeWidth);
               wireFrames[7] = new Vector3(origin.X + cubeWidth, origin.Y + cubeWidth, origin.Z + cubeWidth);
              
               for (int i = 0; i < wireFrames.Length; i++)
               {
                   Vector4 tmp = new Vector4(wireFrames[i]);
                   tmp.W = 1;
                   Shader.TransformPosition(ref tmp , world * view * projection, screen);
                   wireFrames[i] = new Vector3(tmp);                  
               }
               //1-3
               screen.DrawLine(wireFrames[0], wireFrames[1], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[0], wireFrames[2], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[0], wireFrames[3], new Color3(0, 255, 0));
               //4
               screen.DrawLine(wireFrames[1], wireFrames[4], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[2], wireFrames[4], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[7], wireFrames[4], new Color3(0, 255, 0));
               //5
               screen.DrawLine(wireFrames[1], wireFrames[5], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[3], wireFrames[5], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[7], wireFrames[5], new Color3(0, 255, 0));
               //6
               screen.DrawLine(wireFrames[2], wireFrames[6], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[3], wireFrames[6], new Color3(0, 255, 0));
               screen.DrawLine(wireFrames[7], wireFrames[6], new Color3(0, 255, 0));


           }
       }
    }
}
