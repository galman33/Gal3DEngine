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
       protected Matrix4 rot , scale , translation;

       protected Matrix4 worldMatrix;

       protected Model model;

       public Entity(Matrix4 translation , Matrix4  rotation , Matrix4 scale , Model model)
       {
           this.translation = translation;
           this.scale = scale;
           this.rot = rotation;
           worldMatrix = scale * rotation * translation;
           this.model = model;
           this.Position = translation.ExtractTranslation();
       }

       public void Render(Screen screen)
       {
           ShaderPhong.ExtractData(model);
           ShaderPhong.world = worldMatrix;
           ShaderPhong.Render(screen);
       }

       public virtual void Update()
       {
           worldMatrix = scale * rot * translation;
       }
    }
}
