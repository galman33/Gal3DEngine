using Gal3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.Renders
{
    public class AnimationRender : Renderer
    {
        public AnimationRender(Model frameModel) : base(frameModel)
        {

        }

        public void SetModel(Model frameModel)
        {
            this.model = frameModel;
        }
    }
}
