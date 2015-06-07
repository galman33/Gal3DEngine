using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
	/// <summary>
	/// Representing the game's camera.
	/// </summary>
    public class Camera
    {
		/// <summary>
		/// The poisition of the camera in the world.
		/// </summary>
        public Vector3 Position;
		/// <summary>
		/// The rotation of the camera the world.
		/// </summary>
        public Quaternion Rotation = Quaternion.Identity;

		/// <summary>
		/// Computes the view matrix according to the position and rotation of the camera in the world.
		/// </summary>
		/// <returns>The computed view matrix.</returns>
        public Matrix4 GetViewMatrix()
        {
            return (Matrix4.CreateFromQuaternion(Rotation) *
                        Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z)
                        ).Inverted();
        }

    }
}