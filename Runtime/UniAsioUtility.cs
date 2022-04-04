using System;
using System.Collections;
using System.Collections.Generic;
using NAudio.Dsp;
using UnityEngine;

namespace Yothuba.Asio.Runtime
{
    public static class UniAsioUtility
    {
        public static float[] FFT_NAudio(float[] bufferData)
        {
            var fftSample = new Complex[bufferData.Length];
            var result = new float[bufferData.Length / 2];

            for (int i = 0; i < bufferData.Length; i++)
            {
                fftSample[i].X = (float) (bufferData[i] * FastFourierTransform.HammingWindow(i, bufferData.Length));
                fftSample[i].Y = 0;
            }

            var m = (int)Math.Log(fftSample.Length, 2);
            FastFourierTransform.FFT(true, m, fftSample);

            for (int i = 0; i < bufferData.Length / 2; i++)
            {
                result[i] = 10 *
                         (float) Math.Log(Math.Sqrt(fftSample[i].X * fftSample[i].X + fftSample[i].Y * fftSample[i].Y));
            }

            return result;
        }
    }
}