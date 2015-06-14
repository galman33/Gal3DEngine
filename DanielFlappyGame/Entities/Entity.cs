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
    /// <summary>
    /// Responsibles for the game entity logic.
    /// </summary>
   public class Entity
    {
       /// <summary>
       /// The current position of the Entity.
       /// </summary>
       public Vector3 Position;
       /// <summary>
       /// The world matrix as rotation scale and translation.
       /// </summary>
       protected Vector3 rot, scale, translation;
       /// <summary>
       /// The world matrix of the entity.
       /// </summary>
       protected Matrix4 worldMatrix;
       /// <summary>
       /// The model of the entity.
       /// </summary>
       protected Model model;
       /// <summary>
       /// The light normal vector of the entity (for rendering).
       /// </summary>
       protected Vector3 light;
      
       /// <summary>
       /// The hitbox of the entity(for collision detection).
       /// </summary>
       public Box hitBox;

       /// <summary>
       /// Handles the "after destroyed" event.
       /// </summary>
       public event DestroyDelegate DestroyEntity;

       /// <summary>
       /// Initiallizes an entity with a given translation, rotation scale and light direction.
       /// </summary>
       /// <param name="translation"></param>
       /// <param name="rotation"></param>
       /// <param name="scale"></param>
       /// <param name="lightDirection"></param>
       public Entity(Vector3 translation, Vector3 rotation, Vector3 scale, Vector3 lightDirection)
       {
           this.translation = translation;
           this.scale = scale;
           this.rot = rotation;           
           
           this.Position = translation;
           this.light = lightDirection;
           this.hitBox = makeCube(this.Position, 1);
       }
       /// <summary>
       /// Handles the rendering of the entity to a given Screen.
       /// </summary>
       /// <param name="screen">The screenn to render too.</param>
       public void Render(Screen screen)
       {
           //update the world matrix
           Entity.GetMatrix(this.translation, this.rot, this.scale, out this.worldMatrix);
           (Program.world as FlapGameWorld).curShader.ExtractData(model);
           (Program.world as FlapGameWorld).curShader.world = worldMatrix;
           (Program.world as FlapGameWorld).curShader.lightDirection = light;
           (Program.world as FlapGameWorld).curShader.Render(screen);
           //this.hitBox.Render(screen, (Program.world as FlapGameWorld).curShader.view, (Program.world as FlapGameWorld).curShader.projection);
           
       }

       private Box makeCube(Vector3 position, float size)
       {              
           return new Box(size/3, size/3 , size, position);
       }
       /// <summary>
       /// Translates given translation, rotation and scale into a world Matrix.
       /// </summary>
       /// <param name="translation">The translation of the world matrix.</param>
       /// <param name="rotation">The rotation of the world matrix.</param>
       /// <param name="scale">The scale of the world matrix.</param>
       /// <param name="world">The world matrix.</param>
       private static void GetMatrix(Vector3 translation , Vector3 rotation , Vector3  scale , out Matrix4 world )
       {
           world = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateTranslation(translation);
       }
       /// <summary>
       /// Handles the update of the entity on every frame.
       /// </summary>
       public virtual void Update()
       {
           //update translation
           this.translation = this.Position;
          
       }

       /// <summary>
       /// Returns the model of the entity.
       /// </summary>
       /// <returns></returns>
       public Model GetModel()
       {
           return model;
       }
       /// <summary>
       /// Sets a new model as the entity model.
       /// </summary>
       /// <param name="model">The new model to change to.</param>
       public void SetModel(Model model)
       {
           this.model = model;
       }

       /// <summary>
       /// Destroy the entity.
       /// </summary>
       public virtual void Destroy()
       {
           if(DestroyEntity!= null)
           {
               DestroyEntity(this);
           }
       }       
    }    
}
