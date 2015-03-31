using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    public class Animation
    {
        private List<AnimationData> animations;
        private Model frameModel;      

        private int index;
        private bool play;
        private string curAnimName;
        private double curTimePassed;

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

        public void Play(string animName)
        {
            play = true;
            curAnimName = animName;
        }

        public void Stop()
        {
            play = false;
        }

        public void Update()
        {
            if (play)
            {
                curTimePassed = Time.DeltaTime * 1000; // in miliseconds
                if (IsAnimationExists(curAnimName))
                {
                    if (curTimePassed >= 1000 / GetAnimation(curAnimName).frameRate)
                    {
                        Model entityModel = animations.First<AnimationData>(n => n.AnimationName == curAnimName).animationFrames[index];
                        frameModel = (entityModel);
                        index++;
                        if (index == GetAnimation(curAnimName).animationFrames.Length && GetAnimation(curAnimName).repeat) //end the anim cycle
                        {
                            index = 0;
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

        public void AddAnimation(string animName, Model[] animationModels, int frameRate, bool repeat)
        {
            animations.Add(new AnimationData() { frameRate = frameRate, AnimationName = animName, animationFrames = animationModels, repeat = repeat });
        }

        class AnimationData
        {
            public int frameRate;
            public bool repeat;
            public Model[] animationFrames;
            public string AnimationName;
        }

    }
}
