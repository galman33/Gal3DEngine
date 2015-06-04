using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;
using Gal3DEngine.UTILS;
using DanielFlappyGame.Entities;
using DanielFlappyGame.Renders;

namespace DanielFlappyGame
{
    public class FlappyBird : Entity
    {

        private static Model modelS;
        private Animation flappyAnimation;       
        private static Model[] modelsS;

        private float jumpVelocity;
        private float velocityY = 0;
        private float velocityZ = 0;
        private float gravity = 0.0025f;

        public FlappyBird(Vector3 translation, Vector3 rotation, Vector3 lightDirection)
            : base(translation, rotation, new Vector3(0.12f, 0.12f, 0.12f) , lightDirection)
        {
            this.jumpVelocity = 0.06f;
            this.velocityZ = 0.015f;
            this.model = modelS;
            flappyAnimation = new Animation(model, modelsS, "Flying", 2, true);
            flappyAnimation.Play("Flying");
            
            AdjustHitBox();
        }

        private void AdjustHitBox()
        {
            this.hitBox = new Box(0.20f, 0.20f, 0.20f, Vector3.Add(this.Position , -new Vector3(0,0.00f, 0)));
        }
        public static void LoadModel()
        {
            modelsS = new Model[2];
            modelS = new Model("Resources\\flappy_bird.obj", "Resources\\flappyTexture.jpg");
            modelsS[0] = modelS;
            modelsS[1] = new Model("Resources\\flappy bird frame2.obj", "Resources\\flappyTexture.jpg");

        }

        public override void Update()
        {
            base.Update();
            flappyAnimation.Update();

            //this.model = flappyAnimation.frameModel;
            
            UpdatePosition();
            AdjustHitBox();
            if (CollideTunnels((Program.world as FlapGameWorld).GetTunnles())|| CollideFloor())
            {
                (Program.world as FlapGameWorld).GameOver();
            }
            
            var tunnles = CheckForPassedTunnels((Program.world as FlapGameWorld).GetTunnles());
            if(tunnles!= null)
            {
                (Program.world as FlapGameWorld).PassedTunnels(tunnles);
                (Program.world as FlapGameWorld).AddPoints();
            }
        }

        private bool CollideFloor()
        {
            return Box.IsColliding((Program.world as FlapGameWorld).floor.GetFloorHitBox(), this.hitBox);
        }

        private Pipe[] CheckForPassedTunnels(List<Pipe> entities)
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

        public bool CollideTunnels(List<Pipe> entities)
        {
            foreach (Pipe entity in entities)
            {
                 if (IsCollide(entity))
                    return true;
            }
            return false;
        }
        public bool IsCollide(Entity collideWith)
        { 
            Box thisCollide = this.hitBox;
            Box collideHitbox = collideWith.hitBox;

            bool collide = Box.IsColliding(thisCollide, collideHitbox);
            return collide;
        }     
    }
}
