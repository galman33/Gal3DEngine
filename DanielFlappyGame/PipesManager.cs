using DanielFlappyGame.Entities;
using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame
{
    /// <summary>
    /// Responsibles for the Pipes manufacturing logic.
    /// </summary>
    public class PipesManager
    {
        /// <summary>
        /// The current Pipes in the frame.
        /// </summary>
        private List<Pipe> Pipes = new List<Pipe>();
        /// <summary>
        /// The passed pipes who are still on screen.
        /// </summary>
        private List<Pipe> passedPipes = new List<Pipe>();

        /// <summary>
        /// The time between the creation of two pipes.
        /// </summary>
        private TimeSpan delta = new TimeSpan(0, 0, 0, 0, 700);
        /// <summary>
        /// The current passed time from the last creation of a Pipe.
        /// </summary>
        private TimeSpan curDelta = TimeSpan.Zero;

        /// <summary>
        /// The Y axis distnace between two Pipes.
        /// </summary>
        float radius = 0.95f;

        /// <summary>
        /// Initiallizes a new Pipes manager.
        /// </summary>
        public PipesManager()
        {
            Pipe.LoadModel();
            rand = new Random();
        }

        /// <summary>
        /// Initiallizes the data and logic for the manufaturing of the Pipes.
        /// </summary>
        public void Init()
        {
            //clear all tunnles in the game for game to start
            Pipes.Clear();
            passedPipes.Clear();

            Vector3[] positions = GetRandomPoint(-3f + (Program.world as FlapGameWorld).flappyflappy.Position.Z);
            Pipes.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Pipes.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Pipes[Pipes.Count - 1].DestroyEntity += EntityDestroyed;
            Pipes[Pipes.Count - 2].DestroyEntity += EntityDestroyed;
        }

        /// <summary>
        /// Updates the manager every frame.
        /// </summary>
        public void Update()
        {
            foreach (Entity entity in Pipes)
            {
                entity.Update();
            }
            for (int i = 0; i < passedPipes.Count; i++ )
            {
                Pipe entity = passedPipes[i];
                entity.Update();
            }

            curDelta = curDelta.Add(TimeSpan.FromMilliseconds(16 / 4));
            if (curDelta.TotalMilliseconds > delta.TotalMilliseconds)//Math.Abs(this.flappyflappy.Position.Z - this.Tunnels[Tunnels.Count-1].Position.Z) <3) //curDelta.TotalMilliseconds > delta.TotalMilliseconds)
            {
                curDelta = TimeSpan.Zero;

                Vector3[] positions = GetRandomPoint(-3f + (Program.world as FlapGameWorld).flappyflappy.Position.Z);
                if (recycleTunnels.Count == 0)
                {
                    Pipes.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
                    Pipes.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
                    Pipes[Pipes.Count - 1].DestroyEntity += EntityDestroyed;
                    Pipes[Pipes.Count - 2].DestroyEntity += EntityDestroyed;
                }
                else
                {
                    recycleTunnels[0].Position = positions[0];
                    recycleTunnels[1].Position = positions[1];
                    Pipes.Add(recycleTunnels[0]);
                    Pipes.Add(recycleTunnels[1]);
                    recycleTunnels.RemoveRange(0, 2);
                }
            }
        }
        /// <summary>
        /// Renders all the pipes into a given Screen.
        /// </summary>
        /// <param name="screen">The screen to render to.</param>
        public void RenderPipes(Screen screen)
        {
            foreach (Pipe entity in Pipes)
            {
                entity.Render(screen);
            }
            foreach (Pipe entity in passedPipes)
            {
                entity.Render(screen);
            }
        }
        /// <summary>
        /// Returns the current unpassed Pipes.
        /// </summary>
        /// <returns></returns>
        public List<Pipe> GetPipes()
        {
            return this.Pipes;
        }

        /// <summary>
        /// Update the pipes by given Pipes who passed the Object (usually FlappyBird)activate the function.
        /// </summary>
        /// <param name="tunnles"></param>
        public void PassedPipes(Pipe[] tunnles)
        {
            foreach (Pipe tunnel in tunnles)
            {
                Pipes.Remove(tunnel);
                passedPipes.Add(tunnel);
            }
        }
        /// <summary>
        /// A list of passed pipes who are recycleable. for performence issues.
        /// </summary>
        private List<Pipe> recycleTunnels = new List<Pipe>();       
        Random rand;
        /// <summary>
        /// Get Random position for the new Upcoming Pipes.
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private Vector3[] GetRandomPoint(float z)
        {

            float centerY = rand.Next(-5, 5) / 10.0f;           

            return new[] { new Vector3(0, centerY + radius, z), new Vector3(0, centerY - radius, z) };
        }

        /// <summary>
        /// Fires when a Pipe is destroyed.
        /// </summary>
        /// <param name="entity">The destoryed Pipe.</param>
        private void EntityDestroyed(Entity entity)
        {
            passedPipes.Remove((entity as Pipe));
            recycleTunnels.Add((entity as Pipe));
        }
    }
}
