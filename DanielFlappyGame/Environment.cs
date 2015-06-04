using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame
{
    public class Environment
    {
        public Model SunModel;
        public Model CloudModel;

        public Entity Sun;
        public Entity Cloud;
        public Environment()
        {
            LoadModels();
            Sun = new Entity(new Vector3(1.5f, 2f, 2f),Vector3.One ,Vector3.One, Vector3.Normalize(new Vector3(1, -0.25f, -1)));
        }

        private void LoadModels()
        {
            SunModel = new Model("Resources\\sun.obj" , "Resources\\SunText.jpg");
            CloudModel = new Model("Resources\\cloud.obj", "Resources\\anan.png");
        }

        public void Update()
        {
           
        }

        public void Render(Screen screen)
        {

        }
    }
}
