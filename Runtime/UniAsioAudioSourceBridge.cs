using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yothuba.Asio.Runtime;

enum BufferSize:uint
{
    Size256 = 256,
    Size512 = 512,
    Size1024 = 1024,
    Size2048 = 2048,
    Size4096 = 4096,
    Size8192 = 8192,
    Size44100 = 44100,
    Size48000 = 48000
}

public class UniAsioAudioSourceBridge : MonoBehaviour
{
    public AudioSource _AudioSource;
    [SerializeField] private BufferSize _bufferSize;
    private float[] audioSourceBuffer;
    private int tmpBufferSize = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSourceBuffer = new float[(uint)_bufferSize];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    public void OnUniAsioInputEvent(float[] buffer, int sampleRate)
    {
        /*
        var clip = AudioClip.Create("clip",sampleRate,1,sampleRate, false);
        clip.SetData(buffer, 0);
        _AudioSource.clip = clip;

        _AudioSource.Play();
*/
        
        var tmp = tmpBufferSize;
        if (tmp >= (uint)_bufferSize)
        {
            var clip = AudioClip.Create("clip",sampleRate,1,sampleRate, false);
            clip.SetData(audioSourceBuffer, 0);
            _AudioSource.clip = clip;
            _AudioSource.PlayOneShot(clip);
            tmpBufferSize = 0;
        }
        else
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (tmp + i >= (uint)_bufferSize) break;
                audioSourceBuffer[tmp + i] = buffer[i];
            }
            tmpBufferSize += buffer.Length;
        }
        
        
    }
}
