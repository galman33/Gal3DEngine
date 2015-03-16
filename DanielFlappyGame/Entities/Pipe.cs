using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.Entities
{
    public class Pipe : Entity
    {
        private static Model modelS;
        public Pipe(Vector3 Position, Vector3 rotation, Vector3 lightDirection)
            : base(Position, rotation, new Vector3(0.1f, 0.1f, 0.1f), lightDirection)
        {
            this.model = modelS;
            AdjustHitBox();
        }

        public static void LoadModel()
        {
            modelS = new Model("Resources\\tunnel.obj", "Resources\\TunnelT.jpg");
        }

        private void AdjustHitBox()
        {
            this.entityCube = new Box(0.15f, 1f, 0.15f, this.Position);
        }

        public override void Update()
        {
            base.Update();
            AdjustHitBox();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
