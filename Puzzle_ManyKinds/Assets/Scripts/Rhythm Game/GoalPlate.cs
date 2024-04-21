using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource goalAudioSource; //골인지점 도착하면 빵빠레 소리 나게끔;
    NoteManager noteManager;
    CenterFlame centerFlame;

    Result result;
    void Start()
    {
        goalAudioSource = GetComponent<AudioSource>();
        noteManager = FindObjectOfType<NoteManager>();
        centerFlame = FindObjectOfType<CenterFlame>();
        result = FindObjectOfType<Result>();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            result.ShowResult();
            Debug.Log("빵빠레 소리");
            centerFlame.centerAudio.Stop();
            goalAudioSource.Play();
            PlayerController.s_canPressKey = false; //이렇게 되면 플레이어가 도착할때 움직일수 없게됨
            noteManager.RemoveNote();
        }
    }
}
