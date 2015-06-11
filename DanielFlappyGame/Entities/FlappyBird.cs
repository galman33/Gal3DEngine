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
    /// <summary>
    /// Responsibles for the logic of the Flappy Bird entity.
    /// </summary>
    public class FlappyBird : Entity
    {
        /// <summary>
        /// The model represting the Pipe entity.
        /// </summary>
        private static Model modelS;
        /// <summary>
        /// The animation of the Flappy Bird.
        /// </summary>
        private Animation flappyAnimation;
        /// <summary>
        /// The models represting the animation of the Flappy Bird.
        /// </summary>
        private static Model[] modelsS;

        /// <summary>
        /// The Y axis velocity when "Jump" accurs.
        /// </summary>
        private float jumpVelocity;
        /// <summary>
        /// Current Y axis velocity.
        /// </summary>
        private float velocityY = 0;
        /// <summary>
        /// Current Z axis velocity.
        /// </summary>
        private float velocityZ = 0;
        /// <summary>
        /// The gravity acceleration trigger on the Flappy Bird.
        /// </summary>
        private float gravity = 0.0025f;

        /// <summary>
        /// Initiallizes a Flappy Bird from given position, rotation and light direction.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <param name="lightDirection"></param>
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

        /// <summary>
        /// Adjusts the hitbox of the pipe according to its location.
        /// </summary>
        private void AdjustHitBox()
        {
            this.hitBox = new Box(0.20f, 0.20f, 0.20f, Vector3.Add(this.Position , -new Vector3(0,0.00f, 0)));
        }
        /// <summary>
        /// Loads The models representing the Flappy Bird entity.
        /// </summary>
        public static void LoadModel()
        {
            modelsS = new Model[2];
            modelS = new Model("Resources\\flappy_bird.obj", "Resources\\flappyTexture.jpg");
            modelsS[0] = modelS;
            modelsS[1] = new Model("Resources\\flappy bird frame2.obj", "Resources\\flappyTexture.jpg");

        }
        /// <summary>
        /// Updates the pipe.
        /// </summary>
        public override void Update()
        {
            base.Update();
            flappyAnimation.Update();

            //this.model = flappyAnimation.frameModel;
            
            UpdatePosition();
            AdjustHitBox();
            if (CollidePipes((Program.world as FlapGameWorld).GetPipes())|| CollideFloor())
            {
                (Program.world as FlapGameWorld).GameOver();
            }
            
            var tunnles = CheckForPassedTunnels((Program.world as FlapGameWorld).GetPipes());
            if(tunnles!= null)
            {
                (Program.world as FlapGameWorld).PassedPipes(tunnles);
                (Program.world as FlapGameWorld).AddPoints();
            }
        }

        /// <summary>
        /// Checks for collision with the floor.
        /// </summary>
        /// <returns></returns>
        private bool CollideFloor()
        {
            return Box.IsColliding((Program.world as FlapGameWorld).floor.GetFloorHitBox(), this.hitBox);
        }

        /// <summary>
        /// Returns the Pipes passed in the current frame, from given list of pipes.
        /// </summary>
        /// <param name="entities">The list of pipes to check from.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Update the position of the Flappy Bird according to velocity in the varius Axises.
        /// </summary>
        public void UpdatePosition()
        {
            velocityY -= gravity;
            this.Position.Z -= velocityZ;
            this.Position.Y += velocityY;
        }

        /// <summary>
        /// Trigger a jump of the Bird.
        /// </summary>
        public void Jump()
        {
            velocityY = jumpVelocity;
        }

        /// <summary>
        /// Checks wheter the current Flappy Bird is colliding with any Pipes from given list of Pipes.
        /// </summary>
        /// <param name="entities">The list of pipes to check collision from.</param>
        /// <returns></returns>
        public bool CollidePipes(List<Pipe> entities)
        {
            foreach (Pipe entity in entities)
            {
                 if (IsCollide(entity))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Check wheter the current Flappy Bird is colliding with another given entity.
        /// </summary>
        /// <param name="collideWith">The entity to check collision with</param>
        /// <returns></returns>
        public bool IsCollide(Entity collideWith)
        { 
            Box thisCollide = this.hitBox;
            Box collideHitbox = collideWith.hitBox;

            bool collide = Box.IsColliding(thisCollide, collideHitbox);
            return collide;
        }     
    }
}
