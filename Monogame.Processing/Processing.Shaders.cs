using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    partial class Processing
    {
        private PShader _activeShader;

        /// <summary>
        /// Loads a MonoGame effect as a Processing shader.
        /// </summary>
        public PShader loadShader(string fragFilename) => LoadEffectShader(fragFilename);

        /// <summary>
        /// Loads a MonoGame effect as a Processing shader. MonoGame effects already contain
        /// their vertex and pixel programs, so the fragment filename is loaded when a separate
        /// vertex filename is supplied for Processing compatibility.
        /// </summary>
        public PShader loadShader(string vertFilename, string fragFilename) => LoadEffectShader(fragFilename ?? vertFilename);

        public void shader(PShader shader)
        {
            _activeShader = shader ?? throw new ArgumentNullException(nameof(shader));
        }

        public void resetShader()
        {
            _activeShader = null;
        }

        private Effect ActiveSpriteBatchEffect
        {
            get
            {
                var effect = _activeShader?.Effect;
                ApplyCommonShaderParameters(effect);
                return effect;
            }
        }

        private Effect ActivePrimitiveEffect
        {
            get
            {
                var effect = _activeShader?.Effect ?? _basicEffect;
                ApplyCommonShaderParameters(effect);
                return effect;
            }
        }

        private PShader LoadEffectShader(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Shader filename cannot be empty.", nameof(filename));

            var assetName = Path.ChangeExtension(filename, null)?.Replace('\\', '/');

            try
            {
                return new PShader(Content.Load<Effect>(assetName));
            }
            catch
            {
                if (!File.Exists(filename)) throw;

                return new PShader(new Effect(GraphicsDevice, File.ReadAllBytes(filename)));
            }
        }

        private void ApplyCommonShaderParameters(Effect effect)
        {
            if (effect == null) return;

            SetEffectParameter(effect, "World", _world);
            SetEffectParameter(effect, "View", Matrix.Identity);
            SetEffectParameter(effect, "Projection", Matrix.Identity);
            SetEffectParameter(effect, "WorldViewProjection", _world);
            SetEffectParameter(effect, "MatrixTransform", _matrix);
        }

        private static void SetEffectParameter(Effect effect, string name, Matrix value)
        {
            var parameter = effect.Parameters[name];
            if (parameter != null) parameter.SetValue(value);
        }
    }
}
