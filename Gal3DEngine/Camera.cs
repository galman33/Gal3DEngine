using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public Matrix4 GetViewMatrix()
        {
            return (Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z)) *
                        Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z)
                        ).Inverted();
        }

    }
}
