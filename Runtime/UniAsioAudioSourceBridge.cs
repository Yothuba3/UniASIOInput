using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yothuba.Asio.Runtime;


public class UniAsioAudioSourceBridge : MonoBehaviour
{
    public AudioSource _AudioSource;

    private int[] MaxBufferSize =
    {
        256, 512, 1024, 2048, 4096, 8192, 44100, 48000
    };
   
    private const int MAX_BUFF_SIZE = 1024;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private float[] audioSourceBuffer = new float[MAX_BUFF_SIZE];
    private int tmpBufferSize = 0;
    public void OnUniAsioInputEvent(float[] buffer, int sampleRate)
    {
        /*
        var clip = AudioClip.Create("clip",sampleRate,1,sampleRate, false);
        clip.SetData(buffer, 0);
        _AudioSource.clip = clip;

        _AudioSource.Play();
*/
        
        var tmp = tmpBufferSize;
        if (tmp >= MAX_BUFF_SIZE)
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
                if (tmp + i >= MAX_BUFF_SIZE) break;
                audioSourceBuffer[tmp + i] = buffer[i];
            }
            tmpBufferSize += buffer.Length;
        }
        
        
    }
}
