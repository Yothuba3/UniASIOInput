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
        [SerializeField] private int chOffset = 0;
        [SerializeField] private int _inputChCount = 0;
        [SerializeField] private int _outputChCount = 0;
        private AsioOut _asioOut;
        private float[] _samples;
        
        private IWaveProvider _provider;
        private WaveProvider32 _waveProvider32;
        
        public int BufferSizePerCh => bufferSizePerCh;
        public string DriverName => driverName;
        public int InputChCount => _inputChCount;
        public int OuputChCount => _outputChCount;
        private int numberOfInput, numberOfOutput;
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
            
            _inputChCount = _asioOut.DriverInputChannelCount;
            _outputChCount = _asioOut.DriverOutputChannelCount;
            _asioOut.InputChannelOffset = chOffset;
            _asioOut.InitRecordAndPlayback(_waveProvider32, _inputChCount, sampleRate);
            _asioOut.AudioAvailable += OnReceive;
            _asioOut.Play();
        }
        
        private void OnDestroy()
        {
            _asioOut?.Stop();
            _asioOut?.Dispose();
        }
        
        public string[]  GetInputChannelsName()
        {
            if (nameOfChannels == null && !EditorApplication.isPlaying)
            {
                var driver = AsioDriver.GetAsioDriverByName(driverName);
            
                int inNum, outNum;
                driver.GetChannels(out inNum, out outNum);
                _inputChCount = inNum;
                _outputChCount = outNum;
                var names = new string[_inputChCount];
                for (int i = 0; i < _inputChCount; i++)
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