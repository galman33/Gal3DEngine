using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine;

namespace DanielFlappyGame
{
    class FlappyBird : Entity
    {
        private float jumpVelocity;
        private float velocityY = 0;
        private float velocityZ = 0;
        private float gravity = 0.005f;

        public FlappyBird(Matrix4 translation, Matrix4 rotation, Matrix4 scale, Model model , float velocityY , float velocityZ)
            : base(translation, rotation, scale, model)
        {
            this.jumpVelocity = velocityY;
            this.velocityZ = velocityZ;
        }

        public override void Update()
        {
            base.Update();
            UpdatePosition();
            this.translation = Matrix4.CreateTranslation(this.Position);
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
    }
}
