using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;

namespace DanielFlappyGame
{
    public class FlappyBird : Entity
    {
        private float jumpVelocity;
        private float velocityY = 0;
        private float velocityZ = 0;
        private float gravity = 0.005f;

        public FlappyBird(Vector3 translation, Vector3 rotation, Vector3 scale, Model model, float velocityY, float velocityZ)
            : base(translation, rotation, scale, model)
        {
            this.jumpVelocity = velocityY;
            this.velocityZ = velocityZ;
        }

        public override void Update()
        {
            base.Update();
            UpdatePosition();
            if (collide((Program.world as FlapGame).GetTunnles()))
            {
                (Program.world as FlapGame).GameOver();
            }

            var tunnles = CheckForPassedTunnels((Program.world as FlapGame).GetTunnles());
            if(tunnles!= null)
            {
                (Program.world as FlapGame).PassedTunnles(tunnles);
                (Program.world as FlapGame).AddPoints();
            }
        }

        private Entity[] CheckForPassedTunnels(List<Entity> entities)
        {            
            for (int i = 0; i < entities.Count; i += 2) // the list contains tuple of to tunnles
            {
                if (GetCollisionHitBox(entities[i].GetModel())[0].Z > this.Position.Z)//meaning its close to the camera
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
            Model modelEntity = collideWith.GetModel();
            Vector3[] CollideVectices = GetCollisionHitBox(modelEntity);
            Vector3[] thisModelVectices = GetCollisionHitBox(this.model);
            Cube thisHitbox = new Cube(thisModelVectices[0], thisModelVectices[1]);
            Cube collideHitbox = new Cube(CollideVectices[0], CollideVectices[1]);

            return Cube.Collide(thisHitbox , collideHitbox);
        }

        private Vector3[] GetCollisionHitBox(Model model)
        {
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float minZ = float.PositiveInfinity;
            foreach (Vector4 vector in model.Vertices)
            {
                if (minX >= vector.X)
                {
                    minX = vector.X;
                    if (minY >= vector.Y)
                    {
                        minY = vector.Y;
                        if (minZ >= vector.Z)
                        {
                            minZ = vector.Z;
                        }
                    }
                }
            }

            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
            float maxZ = float.NegativeInfinity;
            foreach (Vector4 vector in model.Vertices)
            {
                if (maxX <= vector.X)
                {
                    maxX = vector.X;
                    if (maxY <= vector.Y)
                    {
                        maxY = vector.Y;
                        if (maxZ <= vector.Z)
                        {
                            maxZ = vector.Z;
                        }
                    }
                }
            }

            Vector3[] hitBox = new Vector3[2];
            hitBox[0] = new Vector3(minZ, minY, minZ);
            hitBox[1] = new Vector3(maxX, maxY, maxZ);
            return hitBox;
        }

      
    }
}
