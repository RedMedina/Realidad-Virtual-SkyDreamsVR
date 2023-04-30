using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Sound : MonoBehaviour
{

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0.175f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            audio.volume = 0.175f;
        }
        else 
        {
            audio.volume = 0.07f;
        }
    }
}
