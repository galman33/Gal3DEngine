using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;
using Gal3DEngine.UTILS;

namespace DanielFlappyGame
{
    public class FlappyBird : Entity
    {
        private float jumpVelocity;
        private float velocityY = 0;
        private float velocityZ = 0;
        private float gravity = 0.005f;

        public FlappyBird(Vector3 translation, Vector3 rotation, Vector3 scale, Model model, float velocityY, float velocityZ , Vector3 lightDirection)
            : base(translation, rotation, scale, model , lightDirection)
        {
            this.jumpVelocity = velocityY;
            this.velocityZ = velocityZ;
        }

        public override void Update()
        {
            base.Update();
            UpdatePosition();
            if (collide((Program.world as FlapGameWorld).GetTunnles()))
            {
                (Program.world as FlapGameWorld).GameOver();
            }
            
            var tunnles = CheckForPassedTunnels((Program.world as FlapGameWorld).GetTunnles());
            if(tunnles!= null)
            {
                (Program.world as FlapGameWorld).PassedTunnles(tunnles);
                (Program.world as FlapGameWorld).AddPoints();
            }
        }

        private Entity[] CheckForPassedTunnels(List<Entity> entities)
        {            
            for (int i = 0; i < entities.Count; i += 2) // the list contains tuple of to tunnles
            {
                if (entities[i].Position.Z > this.Position.Z)//meaning its close to the camera
                {
                    return new []{entities[i] , entities[i+1]};
                }
            }
            return null;
        }



        public void UpdatePosition()
        {
            velocityY -= gravity;
            this.Position.Z -= velocityZ;
            this.Position.Y += velocityY;
        }

        public void Jump()
        {
            velocityY = jumpVelocity;
        }

        public bool collide(List<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                 if (IsCollide(entity))
                    return true;
            }
            return false;
        }
        public bool IsCollide(Entity collideWith)
        {   
           

            Box thisCollide = this.entityCube;
            Box collideHitbox = collideWith.entityCube;///new Cube(0.15f, 0.15f, 0.15f, new Vector3(collideWith.Position.X , collideWith.Position.Y+ 0.15f/2 , collideWith.Position.Z));

            bool collide = Box.IsColliding(thisCollide, collideHitbox);
            return collide;
        }     
    }
}
