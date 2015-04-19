using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame
{
    public class Renderer
    {
        public Model model;
        public Renderer(Model model)
        {
            this.model = model;
        }

        public void Render(Screen screen , Matrix4 world , Vector3 lightDirection , Box hitBox)
        {            
            (Program.world as FlapGameWorld).curShader.ExtractData(model);
            (Program.world as FlapGameWorld).curShader.world = world;
            (Program.world as FlapGameWorld).curShader.lightDirection = lightDirection;
            (Program.world as FlapGameWorld).curShader.Render(screen);
            hitBox.Render(screen, (Program.world as FlapGameWorld).curShader.view, (Program.world as FlapGameWorld).curShader.projection);
        }
    }
}
