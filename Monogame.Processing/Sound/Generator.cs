// ----------------------
// Based on https://github.com/davidluzgouveia/blog-simple-synth
// ----------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Monogame.Processing.Sound
{
    public abstract class Generator
    {
        private static HashSet<Generator> _generators = new HashSet<Generator>();

        private static int SampleRate = 44100;
        private static int SamplesInBuffer = 1000;
        private static int BytesPerSample = 2;
        private static int Channels = 2;
        private static readonly DynamicSoundEffectInstance sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Stereo);
        private static readonly float[,] floatBuffer = new float[Channels, SamplesInBuffer];
        private static byte[] byteBuffer1 = new byte[BytesPerSample * Channels * SamplesInBuffer];

        protected float offset = 0.0f;
        protected float time = 0.0f;
        private float _pan = 0f;
        private float _amp = 0.2f;

        private static Task task;

        static Generator()
        {
            task = Task.Factory.StartNew(() => SoundLoop(Task.Factory.CancellationToken));
            sound.Play();
        }


        public static void SoundLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (sound.PendingBufferCount < 5) UpdateSound();
                else Thread.Sleep(1);
            }
            
        }

        private static void UpdateSound()
        {
            sound.SubmitBuffer(byteBuffer1);
            UpdateBuffer();
        }

        private static void UpdateBuffer()
        {

            for (var i = 0; i < floatBuffer.GetLength(1); i++)
            {
                floatBuffer[0, i] = 0;
                floatBuffer[1, i] = 0;
                lock (_generators)
                {
                    foreach (var generator in _generators)
                    {
                        var val = generator._amp * generator.OscilatorFunction();
                        var p = generator._pan;
                        float l = 1f, r = 1f;

                        if (p > 0)
                        {
                            r = p;
                            l = 1 - p;
                        }

                        if (p < 0)
                        {
                            l = -p;
                            r = 1 + p;
                        }

                        floatBuffer[0, i] += l * val;
                        floatBuffer[1, i] += r * val;
                        generator.time += 1f / SampleRate;
                    }
                }
            }

            ConvertBuffer(floatBuffer, byteBuffer1);
        }

        private static void ConvertBuffer(float[,] from, byte[] to)
        {
            for (var i = 0; i < from.GetLength(1); i++)
            {
                for (int c = 0; c < from.GetLength(0); c++)
                {
                    var floatSample = MathHelper.Clamp(from[c, i], -1.0f, 1.0f);
                    var shortSample = (short)(floatSample >= 0.0f ? floatSample * short.MaxValue : floatSample * short.MinValue * -1);

                    // Calculate the right index based on the PCM format of interleaved samples per channel [L-R-L-R]
                    var index = i * Channels * BytesPerSample + c * BytesPerSample;

                    if (!BitConverter.IsLittleEndian)
                    {
                        to[index] = (byte)(shortSample >> 8);
                        to[index + 1] = (byte)shortSample;
                    }
                    else
                    {
                        to[index] = (byte)shortSample;
                        to[index + 1] = (byte)(shortSample >> 8);
                    }
                }
            }
        }

        protected abstract float OscilatorFunction();

        /// <summary>
        /// Starts the oscillator
        /// </summary>
        public void play()
        {
            lock (_generators) _generators.Add(this);
        }

        /// <summary>
        /// Set multiple parameters at once
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="amp"></param>
        /// <param name="add"></param>
        /// <param name="pos"></param>
        public void set(float amp, float add, float pos)
        {
            this.amp(amp);
            this.add(add);
            this.pan(pos);
        }

        /// <summary>
        /// Change the amplitude/volume of this sound.
        /// </summary>
        /// <param name="amp"></param>
        public void amp(float amp) => _amp = MathHelper.Clamp(amp, 0f, 1f);

        /// <summary>
        /// Offset the output of this generator by given value
        /// </summary>
        /// <param name="add"></param>
        public void add(float add) => offset = add;

        /// <summary>
        /// Move the sound in a stereo panorama.
        /// </summary>
        /// <param name="pos"></param>
        public void pan(float pos) => _pan = MathHelper.Clamp(pos, -1f, 1f);

        /// <summary>
        /// Stop the oscillator.
        /// </summary>
        public void stop()
        {
            lock (_generators) _generators.Remove(this);
        }

        ~Generator()
        {
            lock (_generators) _generators.Remove(this);
        }
    }
}
