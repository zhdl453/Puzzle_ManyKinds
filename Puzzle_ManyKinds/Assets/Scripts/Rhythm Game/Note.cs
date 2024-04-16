using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed;
    Image noteImage;
    //NoteManager.cs에서 노트들을 큐에 담아줄때 비활성화한 상태로 담아줘서 다시 꺼내다 쓸때 꺼져있는상태이다.
    //그래서 여기에서 다시 활성화 켜줌
    void OnEnable() 
    {
        if(noteImage ==null)//이미지 컴포넌트가 null때 이미지를 컴포넌트 가져와서 활성화시켜줌
        {
            noteImage = GetComponent<Image>();  
        }
        noteImage.enabled = true;  
    }
    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }
    public void HideNote()
    {
        noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}
