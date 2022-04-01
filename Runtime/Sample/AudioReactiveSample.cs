using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveSample : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUniAsioInputEvent(float[] buffer)
    {
        foreach (var n in buffer)
        {
            cube.transform.position = new Vector3( cube.transform.position.x,n,  cube.transform.position.z);    
        }
    }
}
