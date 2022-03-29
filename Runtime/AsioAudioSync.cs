using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Wave.Asio;
using UnityEngine;
using UnityEngine.Serialization;
using Yothuba.Asio.Runtime;

public class AsioAudioSync : MonoBehaviour
{ 
    
    [SerializeField] internal AsioManager _asioDriver;
    [SerializeField] private int channelIndex;
    [SerializeField] private string channelName;
    public Transform cube;
    private float min = 0, max = 0;
    ConcurrentQueue<float[]> queue = new ConcurrentQueue<float[]>();
    private void Start()
    {
        if (!_asioDriver) return; 
        
        _asioDriver.OnReceive += OnAsioOutAudioAvailable;
    }
    
    
    private float[] _samples;
    private float[] _buffer;
    void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
    {
#pragma  warning disable CS0618
        _samples = e.GetAsInterleavedSamples();
#pragma warning restore CS0618
        _buffer = new float[_asioDriver.BufferSizePerCh];
        for (var i = 0; i < _asioDriver.BufferSizePerCh; i++)
        {
            var val = _samples[channelIndex + _asioDriver.InputChCount* i];
            _buffer[i] = remap(val, -1, 1, 0, 1);
        }
        queue.Enqueue(_buffer);
    }

    void Update()
    {
        float[] popBuffer;
        if (queue.TryDequeue(out popBuffer))
        {
            float tmp = 0;
            foreach (var n in popBuffer)
            {
                cube.position = new Vector3(cube.position.x,n, cube.position.z);    
            }
            
        }
    }
    public float remap(float val, float in1, float in2, float out1, float out2)
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

}
 