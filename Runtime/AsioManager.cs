using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.Asio;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yothuba.Asio.Runtime
{


    public class AsioManager : MonoBehaviour
    {
        [SerializeField] internal string[] nameOfChannels;
        [SerializeField] private string driverName;
        [SerializeField] private int bufferSizePerCh = 512;
        [SerializeField] private int sampleRate = 44100;
        [SerializeField] private int inputChCount = 0;
        [SerializeField] private int outputChCount = 0;
        
        private AsioOut _asioOut;

        
        public int BufferSizePerCh => bufferSizePerCh;
        public string DriverName => driverName;
        public int InputChCount => inputChCount;
        public int OuputChCount => outputChCount;
        /// <summary>
        /// OnEnableより後にイベントを登録してください
        /// </summary>
       public event EventHandler<AsioAudioAvailableEventArgs> OnReceive;

        private void OnEnable()
        {
        }
        
        void Start()
        {
            _asioOut = new AsioOut(driverName);
            
            inputChCount = _asioOut.DriverInputChannelCount;
            _asioOut.InitRecordAndPlayback(null, inputChCount, sampleRate);
            _asioOut.AudioAvailable += OnReceive;
            _asioOut.Play();
        }
        
        private void OnDestroy()
        {
            _asioOut?.Stop();
            _asioOut?.Dispose();
        }

        public void InitDriverConfig(AsioOut driver)
        {
            //driver.
        }
        
        internal string[]  GetInputChannelsName()
        {
            if (nameOfChannels == null && !EditorApplication.isPlaying)
            {
                var driver = AsioDriver.GetAsioDriverByName(driverName);
                
                driver.GetChannels(out int inNum, out int outNum);
                inputChCount = inNum;
                outputChCount = outNum;
                var names = new string[inputChCount];
                for (int i = 0; i < inputChCount; i++)
                {
                    names[i] = driver.GetChannelInfo(i,false).name;
                }

                nameOfChannels = names;
                driver.ReleaseComAsioDriver();
            }
            return nameOfChannels;
        }
    }
    
}