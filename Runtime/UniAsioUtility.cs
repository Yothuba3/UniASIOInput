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
            var fftsample = new Complex[bufferData.Length];
            var res = new float[bufferData.Length / 2];

            for (int i = 0; i < bufferData.Length; i++)
            {
                fftsample[i].X = (float) (bufferData[i] * FastFourierTransform.HammingWindow(i, bufferData.Length));
                fftsample[i].Y = 0;
            }

            var m = (int)Math.Log(fftsample.Length, 2);
            FastFourierTransform.FFT(true, m, fftsample);

            for (int i = 0; i < bufferData.Length / 2; i++)
            {
                res[i] = 10 *
                         (float) Math.Log(Math.Sqrt(fftsample[i].X * fftsample[i].X + fftsample[i].Y * fftsample[i].Y));
            }

            return res;
        }
    }
}