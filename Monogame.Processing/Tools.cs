using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Monogame.Processing
{
    internal static class Tools
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(this Vector2 value1, Vector2 value2) => value1.X * value2.Y - value1.Y * value2.X;
    }
}
