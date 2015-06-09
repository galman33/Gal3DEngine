using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.Entities
{
    /// <summary>
    /// Responsible for a Pipe entity logic.
    /// </summary>
    public class Pipe : Entity
    {
        /// <summary>
        /// The model represting the Pipe entities.
        /// </summary>
        private static Model modelS;
        /// <summary>
        /// Initiallizes a pipe from given position, rotation and light direction.
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="rotation"></param>
        /// <param name="lightDirection"></param>
        public Pipe(Vector3 Position, Vector3 rotation, Vector3 lightDirection)
            : base(Position, rotation, new Vector3(0.1f, 0.1f, 0.1f), lightDirection)
        {
            this.model = modelS;
            AdjustHitBox();
        }

        /// <summary>
        /// Loads The model representing the Pipe entities.
        /// </summary>
        public static void LoadModel()
        {
            modelS = new Model("Resources\\tunnel.obj", "Resources\\TunnelT.jpg");
        }

        /// <summary>
        /// Adjusts the hitbox of the pipe according to its location.
        /// </summary>
        private void AdjustHitBox()
        {
            this.hitBox = new Box(0.15f, 0.35f, 0.15f,this.Position + new Vector3(0,0.1f,0));
        }

        /// <summary>
        /// Updates the pipe.
        /// </summary>
        public override void Update()
        {
            base.Update();
            AdjustHitBox();
            if (this.Position.Z - (Program.world as FlapGameWorld).flappyflappy.Position.Z > 2)
            {
                Destroy();
            }
        }      
    }
}
