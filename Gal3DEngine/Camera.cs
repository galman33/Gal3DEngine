using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public Matrix4 GetViewMatrix()
        {
            return (Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) *
                        Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z)
                        ).Inverted();
        }

    }
}
