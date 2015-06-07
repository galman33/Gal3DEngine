using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
	/// <summary>
	/// Holding references to the singletons of the available shaders.
	/// </summary>
    public static class AvailableShaders
    {

        public static readonly ShaderPhong ShaderPhong = new ShaderPhong();
        public static readonly ShaderFlat ShaderFlat = new ShaderFlat();

    }
}
