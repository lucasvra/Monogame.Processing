using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Monogame.Processing
{
    public class PImage
    {
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;
        public static RenderTarget2D currentTarget;

        public int width => texture.Width;
        public int height => texture.Height;
        public Texture2D texture { get; set; }
        public Color[] pixels = new Color[0];

        private PImage(Texture2D img)
        {
            texture = img;
        }

        public PImage(int w, int h)
        {
            texture = new Texture2D(graphicsDevice, w, h);
        }

        public PImage(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);
            texture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Dispose();
        }

        public void loadPixels()
        {
            pixels = new Color[texture.Width*texture.Height];
            texture.GetData(pixels);
        }

        public void updatePixels()
        {
            texture.SetData(pixels);
        }

        public void updatePixels(int x, int y, int w, int h)
        {
            for (var i = 0; i <= h; i++) 
                texture.SetData(pixels, (y + i) * width + x, w);
        }

        public PImage get() => this;

        public PImage get(int x, int y, int w, int h)
        {
            var img = new RenderTarget2D(graphicsDevice, w, h, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(img);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(x, y, w, h), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            graphicsDevice.SetRenderTargets(currentTarget);
            return new PImage(img);
        }

        public color get(int x, int y) => pixels[y * width + x];

        public void resize(int w, int h)
        {
            var img = new RenderTarget2D(graphicsDevice, w, h, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(img);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(w/(float)width, h/(float)height), SpriteEffects.None, 0);
            spriteBatch.End();

            graphicsDevice.SetRenderTargets(currentTarget);
            texture = img;
        }

        public void mask(color[] m)
        {
            for (var i = 0; i < pixels.Length; i++)
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

        private void ApplyBoxBlur(Color[] p, int width, int height, int radius)
        {
            Color[] output = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int redSum = 0;
                    int greenSum = 0;
                    int blueSum = 0;
                    int count = 0;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int posX = x + kx;
                            int posY = y + ky;

                            if (posX >= 0 && posX < width && posY >= 0 && posY < height)
                            {
                                Color neighbor = p[posY * width + posX];
                                redSum += neighbor.R;
                                greenSum += neighbor.G;
                                blueSum += neighbor.B;
                                count++;
                            }
                        }
                    }

                    if (count > 0)
                    {
                        output[y * width + x] = new Color(redSum / count, greenSum / count, blueSum / count);
                    }
                }
            }

            Array.Copy(output, p, p.Length);
        }

        // Apply erode
        private void ApplyErosion(Color[] p, int width, int height, int radius)
        {
            Color[] output = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int minIntensity = 255;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int posX = x + kx;
                            int posY = y + ky;

                            if (posX >= 0 && posX < width && posY >= 0 && posY < height)
                            {
                                Color neighbor = p[posY * width + posX];
                                int intensity = (neighbor.R + neighbor.G + neighbor.B) / 3;
                                minIntensity = Math.Min(minIntensity, intensity);
                            }
                        }
                    }

                    output[y * width + x] = new Color(minIntensity, minIntensity, minIntensity);
                }
            }

            Array.Copy(output, p, p.Length);
        }

        // Appy Dilation
        private void ApplyDilation(Color[] p, int width, int height, int radius)
        {
            Color[] output = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int maxIntensity = 0;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int posX = x + kx;
                            int posY = y + ky;

                            if (posX >= 0 && posX < width && posY >= 0 && posY < height)
                            {
                                Color neighbor = p[posY * width + posX];
                                int intensity = (neighbor.R + neighbor.G + neighbor.B) / 3;
                                maxIntensity = Math.Max(maxIntensity, intensity);
                            }
                        }
                    }

                    output[y * width + x] = new Color(maxIntensity, maxIntensity, maxIntensity);
                }
            }

            Array.Copy(output, p, p.Length);
        }

        public void filter(Filter kind, float param = -1f)
        {
            var p = new Color[texture.Width * texture.Height];
            texture.GetData(p);

            switch (kind)
            {
                case Filter.THRESHOLD:
                    if (param < 0f || param > 1f) param = 0.5f;
                    p = p.AsParallel().AsOrdered()
                        .Select(c => (grey: (int) ((0.3 * c.R) + (0.59 * c.G) + (0.11 * c.B)), c.A))
                        .Select(t => (grey: t.grey / 255.0 < param ? 255 : 0, t.A))
                        .Select(t => Color.FromNonPremultiplied(t.grey, t.grey, t.grey, t.A)).ToArray();
                    break;
                case Filter.GRAY:
                    p = p.AsParallel().AsOrdered()
                        .Select(c => (grey: (int) ((0.3 * c.R) + (0.59 * c.G) + (0.11 * c.B)), c.A))
                        .Select(t => Color.FromNonPremultiplied(t.grey, t.grey, t.grey, t.A)).ToArray();
                    break;
                case Filter.OPAQUE:
                    p = p.AsParallel().AsOrdered().Select(c => Color.FromNonPremultiplied(c.R, c.G, c.B, 255))
                        .ToArray();
                    break;
                case Filter.INVERT:
                    p = p.AsParallel().AsOrdered()
                        .Select(c => Color.FromNonPremultiplied(255 - c.R, 255 - c.G, 255 - c.B, c.A)).ToArray();
                    break;
                case Filter.POSTERIZE:
                    if (param < 2f) param = 2f;
                    if (param > 255f) param = 255f;
                    p = p.AsParallel().AsOrdered()
                        .Select(c => Color.FromNonPremultiplied((int)Math.Min(c.R, param), (int)Math.Min(c.G, param), (int)Math.Min(c.B, param), c.A)).ToArray();
                    break;
                case Filter.BLUR:
                    ApplyBoxBlur(p, texture.Width, texture.Height, 3);
                    break;
                case Filter.ERODE:
                    ApplyErosion(p, texture.Width, texture.Height, 3);
                    break;
                case Filter.DILATE:
                    ApplyDilation(p, texture.Width, texture.Height, 3);
                    break;
                    
            }

            texture.SetData(p);
        }

        public PImage copy() => this;


        public void copy(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh) =>
            copy(this, sx, sy, sw, sh, dx, dy, dw, dh);

        public void copy(PImage src,int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh)
        {
            var img = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(img);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(src.texture, new Vector2(dx, dy), new Rectangle(sx, sy, sw, sh), Color.White, 0, Vector2.Zero, new Vector2(dw / (float) sw, dh / (float)sh), SpriteEffects.None, 0);
            spriteBatch.End();

            graphicsDevice.SetRenderTargets(currentTarget);
            texture = img;
        }

        public void blend(int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, BlendMode mode) =>
            blend(this, sx, sy, sw, sh, dx, dy, dw, dh, mode);

        public void blend(PImage src, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, BlendMode mode)
        {
            var blend = mode switch
            {
                BlendMode.ADD => BlendState.Additive,
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
                    ColorSourceBlend = Blend.One,
                    AlphaSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    AlphaDestinationBlend = Blend.One,
                    ColorBlendFunction = BlendFunction.ReverseSubtract,
                    AlphaBlendFunction = BlendFunction.ReverseSubtract
                },
                _ => BlendState.NonPremultiplied,
            };
            
            var aux = graphicsDevice.GetRenderTargets();
            var img = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(img);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate);
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, blend);
            spriteBatch.Draw(src.texture, new Vector2(dx, dy), new Rectangle(sx, sy, sw, sh), Color.White, 0, Vector2.Zero, new Vector2(dw / (float)sw, dh / (float)sh), SpriteEffects.None, 0);
            spriteBatch.End();

            graphicsDevice.SetRenderTargets(currentTarget);
            texture = img;
        }

        public void save(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".jpg":
                    using (var stream = File.Create(filename))
                        texture.SaveAsJpeg(stream, texture.Width, texture.Height);
                    break;
                case ".png":
                    using (var stream = File.Create(filename))
                        texture.SaveAsPng(stream, texture.Width, texture.Height);
                    break;
                default:
                    throw new Exception("Only jpg and png are supported");
            }
        }
    }
}
