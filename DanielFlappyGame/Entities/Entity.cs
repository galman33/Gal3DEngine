using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;
using Gal3DEngine.UTILS;
namespace DanielFlappyGame
{
    public delegate void DestroyDelegate(Entity entity);
   public class Entity
    {
       public Vector3 Position;
       protected Vector3 rot, scale, translation;
       protected Matrix4 worldMatrix;
       protected Model model;
       protected Vector3 light;

       public Box entityCube;

       public event DestroyDelegate DestroyEntity;

       public Entity(Vector3 translation, Vector3 rotation, Vector3 scale, Vector3 lightDirection)
       {
           this.translation = translation;
           this.scale = scale;
           this.rot = rotation;           
           
           this.Position = translation;
           this.light = lightDirection;
           this.entityCube = makeCube(this.Position, 1);
       }

       public void Render(Screen screen)
       {
           //update the world matrix
           GetMatrix(this.translation, this.rot, this.scale, out this.worldMatrix);
           (Program.world as FlapGameWorld).curShader.ExtractData(model);
           (Program.world as FlapGameWorld).curShader.world = worldMatrix;
           (Program.world as FlapGameWorld).curShader.lightDirection = light;
           (Program.world as FlapGameWorld).curShader.Render(screen);
           this.entityCube.Render(screen, (Program.world as FlapGameWorld).curShader.view, (Program.world as FlapGameWorld).curShader.projection);
           
       }

       private Box makeCube(Vector3 position, float size)
       {              
           return new Box(size/3, size/3 , size, position);
       }

       private void GetMatrix(Vector3 translation , Vector3 rotation , Vector3  scale , out Matrix4 world )
       {
           world = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(translation);
       }

       public virtual void Update()
       {
           //update translation
           this.translation = this.Position;
           if (this.Position.Z - (Program.world as FlapGameWorld).flappyflappy.Position.Z > 1)
           {
               Destroy();
           }
       }
       public Model GetModel()
       {
           return model;
       }

       public virtual void Destroy()
       {
           if(DestroyEntity!= null)
           {
               DestroyEntity(this);
           }
       }       
    }    
}
