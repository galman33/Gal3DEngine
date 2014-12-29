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
        public Quaternion Rotation;

        public Matrix4 GetViewMatrix()
        {
            return (Matrix4.CreateFromQuaternion(Rotation) *
                        Matrix4.CreateTranslation(Position.X, Position.Y, Position.Z)
                        ).Inverted();
        }

    }
}