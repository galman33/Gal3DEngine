using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.Entities
{
    public class Earth : Entity
    {

        public Earth(Vector3 translation, Vector3 rotation, Vector3 scale, Model model) : base(translation , rotation , scale , model)
        {

        }

        public override void Update()
        {
            base.Update();
            if(Math.Abs(Program.world as FlapGame).flappyflappy.Position.Z- this.Position.Z > 10)
            {
                this.Position.Z += 10;
            }
        }

    }
}
