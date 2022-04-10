using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.Asio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Yothuba.Asio.Runtime;

public class UniAsioInputReceiver : MonoBehaviour
{

    [SerializeField] public AsioManager asioManager;
    [SerializeField] public UnityEvent<float[], int> OnUniAsioInputEvent;
    [SerializeField] private int channelIndex;
    [SerializeField] private string channelName;
    private ConcurrentQueue<float[]> queue = new ConcurrentQueue<float[]>();
    private int sampleRate = 0;
    private void Awake()
    {
        if (!asioManager) return;

        asioManager.OnReceive += OnAsioOutAudioAvailable;
    }

    private void Start()
    {
        sampleRate = asioManager.SampleRate;
    }

    private float[] _samples;
    private float[] _buffer;

   
    //ここで直接Unity機能を利用するとこのイベントが速攻で呼ばれなくなるので，スレッドセーフなqueueを利用
    void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
    {
#pragma warning disable CS0618
        _samples = e.GetAsInterleavedSamples();
        
#pragma warning restore CS0618
        _buffer = new float[asioManager.BufferSizePerCh];
        for (var i = 0; i < asioManager.BufferSizePerCh; i++)
        {
            _buffer[i] = _samples[channelIndex + asioManager.InputChCount * i];
        }
        queue.Enqueue(_buffer);
    }

    //Update内でtryDequeueし，何かしらデータが入っていたらUnityイベント発火
    void Update()
    {
        if (queue.TryDequeue(out float[] popBuffer))
        {
            if (popBuffer.Sum() != 0)
            {
                OnUniAsioInputEvent?.Invoke(popBuffer, sampleRate);
            }
        }
    }
}
 