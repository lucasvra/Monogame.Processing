﻿// ----------------------
// Based on https://github.com/davidluzgouveia/blog-simple-synth
// ----------------------
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Monogame.Processing.Sound
{
    public abstract class Generator
    {
        private const int SampleRate = 44100;
        private const int SamplesInBuffer = 1000;
        private const int BytesPerSample = 2;
        private const int Channels = 2;
        private readonly DynamicSoundEffectInstance sound = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Stereo);
        private readonly float[,] floatBuffer = new float[Channels, SamplesInBuffer];
        private readonly byte[] byteBuffer = new byte[BytesPerSample * Channels * SamplesInBuffer];

        protected float offset = 0.0f;
 
        protected float time = 0.0f;

        protected Generator()
        {
            sound.BufferNeeded += Sound_BufferNeeded;
        }

        private void Sound_BufferNeeded(object sender, EventArgs e)
        {
            for (int i = 0; i < floatBuffer.GetLength(1); i++)
            {
                floatBuffer[0, i] = OscilatorFunction();
                time += 1f / SampleRate;
            }

            ConvertBuffer(floatBuffer, byteBuffer);
            sound.SubmitBuffer(byteBuffer);
        }

        private void ConvertBuffer(float[,] from, byte[] to)
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
        public void play() => sound.Play();

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
        public void amp(float amp) => sound.Volume = MathHelper.Clamp(amp, 0f, 1f);

        /// <summary>
        /// Offset the output of this generator by given value
        /// </summary>
        /// <param name="add"></param>
        public void add(float add) => offset = add;

        /// <summary>
        /// Move the sound in a stereo panorama.
        /// </summary>
        /// <param name="pos"></param>
        public void pan(float pos) => sound.Pan = MathHelper.Clamp(pos, -1f, 1f);

        /// <summary>
        /// Stop the oscillator.
        /// </summary>
        public void stop() => sound.Stop();
    }
}
