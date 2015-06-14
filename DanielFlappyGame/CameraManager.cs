using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame
{
    /// <summary>
    /// Handles the camera logic of the game.
    /// </summary>
    class CameraManager
    {
        /// <summary>
        /// The Camera of the game.
        /// </summary>
        public Camera gameCam;

        private FlappyBird bird;

        /// <summary>
        /// Camera prespective is changing.
        /// </summary>
        public bool stateChange = false;
        /// <summary>
        /// The camera prespective.
        /// </summary>
        private bool front = false;
        
        /// <summary>
        /// Initiallizes a new Camera manager with a given FlappyBird.
        /// </summary>
        /// <param name="bird"></param>
        public CameraManager(FlappyBird bird)
        {
            
            this.bird = bird;
        }

        /// <summary>
        /// Starts the camera change of view angle.
        /// </summary>
        public void ChangeState()
        {
             front = !front;
             
             stateChange = true;

        }

        /// <summary>
        /// Init the current Camera Manager.
        /// </summary>
        public void Init()
        {
            gameCam = new Camera();
            front = true;  

        }

        /// <summary>
        /// Updates the Camera according to the FlappyBird player.
        /// </summary>
        public void UpdateCamera()
        {
            ChangeStateOfCamara();
            if (front)
            {
                gameCam.Position.Z = bird.Position.Z + 2.3f;
                gameCam.Position.Y = bird.Position.Y;

            }
            else
            {
                if (!stateChange)
                {
                    gameCam.Position.Z = bird.Position.Z;
                    gameCam.Position.X = 5;
                }
            }
        }

        private float cameraChangeT = 0;
        /// <summary>
        /// The radius
        /// </summary>
        float radius = 5.0f;
        /// <summary>
        /// Change the camera prespective(if needed)
        /// </summary>
        private void ChangeStateOfCamara()
        {
            
            if (stateChange)
            {
                
                HandelCameraChange(front? -1: 1);

                Quaternion frontRotation = Quaternion.Identity;
                Quaternion sideRotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.PiOver2);// *Quaternion.FromAxisAngle(Vector3.UnitX, -0.5f);

                gameCam.Rotation = Quaternion.Slerp(frontRotation, sideRotation, cameraChangeT);
                gameCam.Position.X = (float)(bird.Position.X + Math.Sin(cameraChangeT * Math.PI / 2) * radius);
                gameCam.Position.Z = (float)(bird.Position.Z + Math.Cos(cameraChangeT * Math.PI / 2) * radius);
            }
          
        }
        /// <summary>
        /// Handles the camera change logic.
        /// </summary>
        /// <param name="dChange"></param>
        private void HandelCameraChange(float dChange)
        {
            cameraChangeT += (float)Time.DeltaTime * 0.5f * dChange;
            if (cameraChangeT < 0)
                cameraChangeT = 0;
            if (cameraChangeT > 1)
                cameraChangeT = 1;
            
        }

    }
}
