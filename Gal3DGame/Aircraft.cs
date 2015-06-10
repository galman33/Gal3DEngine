﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;

namespace Gal3DGame
{
	/// <summary>
	/// The in-game aircraft controlled by the player.
	/// </summary>
    class Aircraft
    {
        private static Model aircrafModel;

        private Quaternion rotation;
        private Vector3 position;

        private float rotateX;
        private float rotateY;

        private ShaderPhong shader = AvailableShaders.ShaderPhong;

		/// <summary>
		/// The collision Box of the aircraft.
		/// </summary>
        public Gal3DEngine.Utils.Box CollisionBox { get; private set; }

        public Aircraft()
        {
            rotation = Quaternion.Identity;
            position = new Vector3();

            rotateX = rotateY = 0;

            CollisionBox = new Gal3DEngine.Utils.Box(0.15f, 0.15f, 0.15f, position);

            Reset();
        }

		/// <summary>
		/// Load into memory the resources used by the Aircraft.
		/// </summary>
        public static void LoadContent()
        {
            aircrafModel = new Model("Resources/Blue Ship_3d.obj", "Resources/Ship_3d_texture.png");
        }

		/// <summary>
		/// Update the Aircraft state.
		/// </summary>
        public void Update()
        {
            rotation = rotation * Quaternion.FromAxisAngle(Vector3.UnitX, rotateX * (float) Time.DeltaTime);
            rotation = rotation * Quaternion.FromAxisAngle(Vector3.UnitY, rotateY * (float)Time.DeltaTime);

            position += Forward * (float) Time.DeltaTime;

            CollisionBox.origin = position;
        }

		/// <summary>
		/// Render the Aircraft.
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="projection"></param>
		/// <param name="view"></param>
        public void Render(Screen screen, Matrix4 projection, Matrix4 view)
        {
            shader.world = Matrix4.CreateScale(0.001f) *
                Matrix4.CreateTranslation(0, -0.1f, 0) *
                Matrix4.CreateRotationY(-MathHelper.PiOver2) *
                Matrix4.CreateFromQuaternion(rotation) * 
                Matrix4.CreateTranslation(position);
            shader.view = view;
            shader.projection = projection;

            shader.lightDirection = (new Vector3(-1, -1, -1)).Normalized();
            shader.ambientLight = 0.3f;

            shader.ExtractData(aircrafModel);

            shader.Render(screen);

            Gal3DEngine.Gizmos.AxisGizmo.Render(screen, Matrix4.CreateFromQuaternion(rotation) *
                Matrix4.CreateTranslation(position), view, projection);

            //CollisionBox.DrawCube(screen, Matrix4.Identity, view, projection);
        }

		/// <summary>
		/// Handle a keydown event.
		/// </summary>
		/// <param name="e"></param>
        public void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Up)
                rotateX = 1;
            if (e.Key == OpenTK.Input.Key.Down)
                rotateX = -1;

            if (e.Key == OpenTK.Input.Key.Right)
                rotateY = -1;
            if (e.Key == OpenTK.Input.Key.Left)
                rotateY = 1;
        }

		/// <summary>
		/// Handle a keyUp event.
		/// </summary>
		/// <param name="e"></param>
        public void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Up)
                rotateX = 0;
            if (e.Key == OpenTK.Input.Key.Down)
                rotateX = 0;

            if (e.Key == OpenTK.Input.Key.Right)
                rotateY = 0;
            if (e.Key == OpenTK.Input.Key.Left)
                rotateY = 0;
        }

		/// <summary>
		/// Reset the Aircraft position and rotation.
		/// </summary>
        public void Reset()
        {
            position.X = 14f;
            position.Y = 4.5f;
            position.Z = 14f;
            rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 45);
        }

		/// <summary>
		/// The position of the Aircraft.
		/// </summary>
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

		/// <summary>
		/// The rotation of the Aircraft.
		/// </summary>
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

		/// <summary>
		/// The forward vector of the Aircraft.
		/// </summary>
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(-Vector3.UnitZ, rotation).Normalized();
            }
        }

    }
}