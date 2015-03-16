﻿using System;
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

        private static Model modelS;

        private float jumpVelocity;
        private float velocityY = 0;
        private float velocityZ = 0;
        private float gravity = 0.0035f;

        public FlappyBird(Vector3 translation, Vector3 rotation, Vector3 lightDirection)
            : base(translation, rotation, new Vector3(0.5f, 0.5f, 0.5f) , lightDirection)
        {
            this.jumpVelocity = 0.07f;
            this.velocityZ = 0.015f;
            this.model = modelS;
            AdjustHitBox();
        }

        private void AdjustHitBox()
        {
            this.entityCube = new Box(0.25f, 0.25f, 0.25f, this.Position);
        }
        public static void LoadModel()
        {
            modelS = new Model("Resources\\Cat2.obj", "Resources\\Cat2.png");            
        }

        public override void Update()
        {
            base.Update();
            UpdatePosition();
            AdjustHitBox();
            if (CollideTunnels((Program.world as FlapGameWorld).GetTunnles()))
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
            Box thisCollide = this.entityCube;
            Box collideHitbox = collideWith.entityCube;

            bool collide = Box.IsColliding(thisCollide, collideHitbox);
            return collide;
        }     
    }
}
