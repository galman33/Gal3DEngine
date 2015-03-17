using DanielFlappyGame.Entities;
using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame
{
    public class PipesManager
    {
        private List<Pipe> Tunnels = new List<Pipe>();
        private List<Pipe> passedTunnles = new List<Pipe>();

        private TimeSpan delta = new TimeSpan(0, 0, 0, 0, 700);
        private TimeSpan curDelta = TimeSpan.Zero;

        public PipesManager()
        {
            Pipe.LoadModel();
            rand = new Random();
        }

        public void Init()
        {
            //clear all tunnles in the game for game to start
            Tunnels.Clear();
            passedTunnles.Clear();

            Vector3[] positions = GetRandomPoint(-3f + (Program.world as FlapGameWorld).flappyflappy.Position.Z);
            Tunnels.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Tunnels.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Tunnels[Tunnels.Count - 1].DestroyEntity += EntityDestroyed;
            Tunnels[Tunnels.Count - 2].DestroyEntity += EntityDestroyed;
        }

        public void Update()
        {
            foreach (Entity entity in Tunnels)
            {
                entity.Update();
            }
            foreach (Entity entity in passedTunnles)
            {
                entity.Update();
            }

            curDelta = curDelta.Add(TimeSpan.FromMilliseconds(16 / 4));
            if (curDelta.TotalMilliseconds > delta.TotalMilliseconds)//Math.Abs(this.flappyflappy.Position.Z - this.Tunnels[Tunnels.Count-1].Position.Z) <3) //curDelta.TotalMilliseconds > delta.TotalMilliseconds)
            {
                curDelta = TimeSpan.Zero;

                Vector3[] positions = GetRandomPoint(-3f + (Program.world as FlapGameWorld).flappyflappy.Position.Z);
                if (recycleTunnels.Count == 0)
                {
                    Tunnels.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
                    Tunnels.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
                    Tunnels[Tunnels.Count - 1].DestroyEntity += EntityDestroyed;
                    Tunnels[Tunnels.Count - 2].DestroyEntity += EntityDestroyed;
                }
                else
                {
                    recycleTunnels[0].Position = positions[0];
                    recycleTunnels[1].Position = positions[1];
                    Tunnels.Add(recycleTunnels[0]);
                    Tunnels.Add(recycleTunnels[1]);
                    recycleTunnels.RemoveRange(0, 2);
                }
            }
        }

        public void RenderPipes(Screen screen)
        {
            foreach (Pipe entity in Tunnels)
            {
                entity.Render(screen);
            }
            foreach (Pipe entity in passedTunnles)
            {
                entity.Render(screen);
            }
        }

        public List<Pipe> GetTunnles()
        {
            return this.Tunnels;
        }

        public void PassedTunnles(Pipe[] tunnles)
        {
            foreach (Pipe tunnel in tunnles)
            {
                Tunnels.Remove(tunnel);
                passedTunnles.Add(tunnel);
            }
        }
        private List<Pipe> recycleTunnels = new List<Pipe>();       
        Random rand;
        private Vector3[] GetRandomPoint(float z)
        {

            float centerY = rand.Next(-5, 5) / 10.0f;
            int maxRadius = (int)((1.9 - Math.Abs(centerY)) * 10);
            float radius = rand.Next(8, maxRadius) / 10.0f;

            return new[] { new Vector3(0, centerY + radius, z), new Vector3(0, centerY - radius, z) };
        }

        private void EntityDestroyed(Entity entity)
        {
            recycleTunnels.Add((entity as Pipe));
        }
    }
}
