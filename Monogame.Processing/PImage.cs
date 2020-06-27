using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Monogame.Processing
{
    public class PImage
    {
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;

        public int width => Texture.Width;
        public int height => Texture.Height;
        public Texture2D Texture { get; private set; }
        public Color[] pixels = new Color[0];

        private PImage(Texture2D img)
        {
            Texture = img;
        }

        public PImage(int w, int h)
        {
            Texture = new Texture2D(graphicsDevice, w, h);
        }

        public PImage(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);
            Texture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Dispose();
        }

        public void loadPixels()
        {
            pixels = new Color[Texture.Width*Texture.Height];
            Texture.GetData(pixels);
        }

        public void updatePixels()
        {
            Texture.SetData(pixels);
        }

        public void updatePixels(int x, int y, int w, int h)
        {
            for (var i = 0; i <= h; i++) Texture.SetData(pixels, (y + i) * width + x, w);
        }

        public PImage get() => this;

        public PImage get(int x, int y, int w, int h)
        {
            var aux = graphicsDevice.GetRenderTargets();
            var img = new RenderTarget2D(graphicsDevice, w, h, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            //graphicsDevice.SetRenderTarget(img);

            //spriteBatch.Begin();
            //spriteBatch.Draw(Texture, Vector2.Zero, new Rectangle(x, y, w, h), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //spriteBatch.End();

            //graphicsDevice.SetRenderTargets(aux);
            return new PImage(img);
        }

        public color get(int x, int y) => pixels[y * width + x];

        public void resize(int w, int h)
        {
            var aux = graphicsDevice.GetRenderTargets();
            var img = new RenderTarget2D(graphicsDevice, w, h, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            //graphicsDevice.SetRenderTarget(img);

            //spriteBatch.Begin();
            //spriteBatch.Draw(Texture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(w/(float)width, h/(float)height), SpriteEffects.None, 0);
            //spriteBatch.End();

            //graphicsDevice.SetRenderTargets(aux);
            Texture = img;
        }

        public void mask(color[] m)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                var aux = pixels[i];
                aux.A = m[i].A;
                pixels[i] = aux;
            }
        }

        public void mask(PImage img)
        {
            img.loadPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                var aux = pixels[i];
                aux.A = img.pixels[i].B;
                pixels[i] = aux;
            }
        }

        public void filter(int kind, float param)
        {

        }

        public PImage copy() => this;


        public void copy(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh) =>
            copy(this, sx, sy, sw, sh, dx, dy, dw, dh);

        public void copy(PImage src,int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh)
        {
            var aux = graphicsDevice.GetRenderTargets();
            var img = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            //graphicsDevice.SetRenderTarget(img);

            //spriteBatch.Begin();
            //spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            //spriteBatch.Draw(src.Texture, new Vector2(dx, dy), new Rectangle(sx, sy, sw, sh), Color.White, 0, Vector2.Zero, new Vector2(dw / (float) sw, dh / (float)sh), SpriteEffects.None, 0);
            //spriteBatch.End();

            //graphicsDevice.SetRenderTargets(aux);
            Texture = img;
        }

        public void blend(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, BlendMode mode) =>
            blend(this, sx, sy, sw, sh, dx, dy, dw, dh, mode);

        public void blend(PImage src, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, BlendMode mode)
        {
            var blend = mode switch
            {
                BlendMode.ADD => new BlendState
                {
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.DODGE => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.MULTIPLY => new BlendState
                {
                    ColorSourceBlend = Blend.DestinationColor,
                    ColorDestinationBlend = Blend.Zero,
                    ColorBlendFunction = BlendFunction.Add
                },
                BlendMode.LIGHTEST => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Max
                },
                BlendMode.DARKEST => new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.Min
                },
                BlendMode.SUBTRACT => new BlendState
                {
                    ColorBlendFunction = BlendFunction.Subtract
                },
                _ => new BlendState {ColorBlendFunction = BlendFunction.Add},
            };
            
            var aux = graphicsDevice.GetRenderTargets();
            var img = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            //graphicsDevice.SetRenderTarget(img);

            //spriteBatch.Begin(SpriteSortMode.Immediate);
            //spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            //spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.Immediate, blend);
            //spriteBatch.Draw(src.Texture, new Vector2(dx, dy), new Rectangle(sx, sy, sw, sh), Color.White, 0, Vector2.Zero, new Vector2(dw / (float)sw, dh / (float)sh), SpriteEffects.None, 0);
            //spriteBatch.End();

            //graphicsDevice.SetRenderTargets(aux);
            Texture = img;
        }

        public void save(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".jpg":
                    using (var stream = File.Create(filename))
                        Texture.SaveAsJpeg(stream, Texture.Width, Texture.Height);
                    break;
                case ".png":
                    using (var stream = File.Create(filename))
                        Texture.SaveAsPng(stream, Texture.Width, Texture.Height);
                    break;
                default:
                    throw new Exception("Only jpg and png are supported");
            }
        }
    }
}
