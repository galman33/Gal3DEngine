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

       public Entity(Vector3 translation, Vector3 rotation, Vector3 scale, Model model)
       {
           this.translation = translation;
           this.scale = scale;
           this.rot = rotation;           
           this.model = model;
           this.Position = translation;
       }

       public void Render(Screen screen)
       {
           //update the world matrix
           GetMatrix(this.translation, this.rot, this.scale, out this.worldMatrix);
           ShaderPhong.ExtractData(model);
           ShaderPhong.world = worldMatrix;
           ShaderPhong.Render(screen);
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
    }
}
