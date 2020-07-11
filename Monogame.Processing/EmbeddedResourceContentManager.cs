using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame.Processing
{
    public class EmbeddedResourceContentManager : ContentManager
    {
        private class FakeGraphicsService : IGraphicsDeviceService
        {
            public FakeGraphicsService(GraphicsDevice graphicDevice) => GraphicsDevice = graphicDevice;

            public GraphicsDevice GraphicsDevice { get; }

#pragma warning disable 67
            public event EventHandler<EventArgs> DeviceCreated;
            public event EventHandler<EventArgs> DeviceDisposing;
            public event EventHandler<EventArgs> DeviceReset;
            public event EventHandler<EventArgs> DeviceResetting;
#pragma warning restore 67
        }

        private class FakeServiceProvider : IServiceProvider
        {
            readonly GraphicsDevice _graphicDevice;
            public FakeServiceProvider(GraphicsDevice graphicDevice) => _graphicDevice = graphicDevice;

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IGraphicsDeviceService))
                    return new FakeGraphicsService(_graphicDevice);

                throw new NotImplementedException();
            }
        }

        public EmbeddedResourceContentManager(GraphicsDevice graphicDevice)
            : base(new FakeServiceProvider(graphicDevice), "Content")
        { }

        protected override Stream OpenStream(string assetName)
        {
            var assembly = typeof(Processing).Assembly;
            return assembly.GetManifestResourceStream($"Monogame.Processing.Content.{assetName}.xnb");
        }
    }
}
