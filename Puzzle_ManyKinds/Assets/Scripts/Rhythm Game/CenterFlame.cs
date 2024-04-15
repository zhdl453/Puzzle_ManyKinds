using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    AudioSource myAudio;
    bool musicStart = false;
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Note") && !musicStart)
        {
            myAudio.Play();  //우와..여기에다가 뮤직 플레이할 생각은 못했는데..! 굿
            musicStart = true;
        }
    }
}
