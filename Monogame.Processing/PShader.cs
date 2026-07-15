using System;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    /// <summary>
    /// Processing-style wrapper around a MonoGame <see cref="Effect"/>.
    /// </summary>
    public sealed class PShader : IDisposable
    {
        public PShader(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
        }

        public Effect Effect { get; }

        public void Dispose() => Effect.Dispose();
    }
}
