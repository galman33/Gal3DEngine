using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    /// <summary>
    /// Responsible for animation logic.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Holds the data of the animations.
        /// </summary>
        private List<AnimationData> animations;

        /// <summary>
        /// The current frame of the current playing animation.
        /// </summary>
        public Model frameModel; 
        /// <summary>
        /// The index of the current frame in the current playing animation.
        /// </summary>
        private int index;
        /// <summary>
        /// Should the animation run in the current frame.
        /// </summary>
        private bool play;
        /// <summary>
        /// The current playing animation
        /// </summary>
        private string curAnimName;
        /// <summary>
        /// Time passed since the start of the current frame
        /// </summary>
        private double curTimePassed;

        /// <summary>
        /// Initiallize an animation from given models , animation name, frame rate, and if repeat.
        /// </summary>
        /// <param name="frameModel">The current frame of the model.</param>
        /// <param name="models">The models of the animation.</param>
        /// <param name="animName">The animation name.</param>
        /// <param name="framesRate">The animation frame rate.</param>
        /// <param name="repeat">Should the animation repeat itself once done.</param>
        public Animation(Model frameModel , Model[] models, string animName, int framesRate, bool repeat)
        {
            animations = new List<AnimationData>();
            AddAnimation(animName, models, framesRate, repeat);
            curAnimName = animName;
            index = 0;
            play = false;
            this.frameModel = frameModel;             
            curTimePassed = 0.0;
            Model entityModel = animations.First<AnimationData>(n => n.AnimationName == curAnimName).animationFrames[index];
            this.frameModel = entityModel;           
        }

        
        /// <summary>
        /// Activates an animation
        /// </summary>
        /// <param name="animName">The animation name to play</param>
        public void Play(string animName)
        {
            play = true;
            curAnimName = animName;
        }

        /// <summary>
        /// Stops the current animation.
        /// </summary>
        public void Stop()
        {
            play = false;
        }


        /// <summary>
        /// Update the animation (every frame of the engine)
        /// </summary>
        public void Update()
        {
            if (play)
            {
                
                if (IsAnimationExists(curAnimName))
                {
                    curTimePassed += Time.DeltaTime; // in miliseconds
                    if (curTimePassed >= 1/ GetAnimation(curAnimName).frameRate)
                    {
                        index++;
                        Model entityModel = animations.First<AnimationData>(n => n.AnimationName == curAnimName).animationFrames[index];
                        frameModel = (entityModel);
                        
                        if (index == GetAnimation(curAnimName).animationFrames.Length - 1) //end the anim cycle
                        {
                            if (GetAnimation(curAnimName).repeat)
                                index = -1;
                            else
                                play = false; // stops the animation
                        }

                        curTimePassed = 0.0f;
                    }
                }
            }
        }


        private bool IsAnimationExists(string name)
        {
            return  (animations.Count<AnimationData>(n => n.AnimationName == curAnimName) > 0);
        }

        private AnimationData GetAnimation(string name)
        {
            if (animations.Count<AnimationData>(n => n.AnimationName == curAnimName) > 0)
            return animations.First<AnimationData>(n => n.AnimationName == curAnimName);

            return null;
        }      

        /// <summary>
        /// Adds a new animation.
        /// </summary>
        /// <param name="animName">The new animation name.</param>
        /// <param name="animationModels">The new animation frames(models).</param>
        /// <param name="frameRate">The new animation frame rate.</param>
        /// <param name="repeat">If the new animation should repeat itself once done.</param>
        public void AddAnimation(string animName, Model[] animationModels, int frameRate, bool repeat)
        {
            animations.Add(new AnimationData() { frameRate = frameRate, AnimationName = animName, animationFrames = animationModels, repeat = repeat });
        }

        /// <summary>
        /// Holds the animation data.
        /// </summary>
        class AnimationData
        {
            /// <summary>
            /// The animation frame rate.
            /// </summary>
            public int frameRate;
            /// <summary>
            /// If The animation should repeat itself one done.
            /// </summary>
            public bool repeat;
            /// <summary>
            /// The animation frames.
            /// </summary>
            public Model[] animationFrames;
            /// <summary>
            /// The animation name.
            /// </summary>
            public string AnimationName;
        }

    }
}
