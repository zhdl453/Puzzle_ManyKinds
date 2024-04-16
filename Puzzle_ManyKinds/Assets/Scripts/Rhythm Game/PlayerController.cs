using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    TimingManager timingManager;
    private void Start()
     {
        timingManager  = FindObjectOfType<TimingManager>();
     }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //판정체크를 위한 스크립트를 호출해줄거임
            timingManager.CheckTiming();
        }
    }
}
