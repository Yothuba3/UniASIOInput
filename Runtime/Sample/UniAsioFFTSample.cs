using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;


namespace Yothuba.Asio.Runtime
{
    public class UniAsioFFTSample: MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnUniAsioInputevent(float[] buffer)
        {
            float[] samples = new float[buffer.Length / 2];
            samples = UniAsioUtility.FFT_NAudio(buffer);
            for (int i = 0; i < samples.Length; i++)
            {

            }
        }
    }
}