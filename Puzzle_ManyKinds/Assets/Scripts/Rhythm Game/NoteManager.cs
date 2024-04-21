using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm;
    double currentTime;
    bool noteActive =true;

    [SerializeField] Transform tfNoteAppear;
     TimingManager timingManager; //노트 생성한거 노트List에다가 넣어줘야함
     EffectManager effectManager;
     ComboManager comboManager;
     void Awake()
     {
        currentTime = 0d;
     }
     private void Start()
     {
        timingManager  = GetComponent<TimingManager>();
        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
     }

    // 60/120 = 1beat 당 0.5초 : 60s/bpm = 1beat시간
    void Update()
    {
        if(noteActive)
        {
            currentTime +=Time.deltaTime; //deltaTime을 더해주기 때문에 0.5초가 아니라 0.5100551..초가 됨.
            //실제로 시간이 소숫점 단위 만큼 더 흘렀지만,게임은 플레이 단위로 흘러간 시간을 체크할 수 있기때문에 생긴 일인거임
            //이걸 무지하고 초기화 currentTime=0으로 해버리면 그 오차만큼의 시간적 손실이 계속 발생함.
            //이 손실이 누적돼다보면 bpm과 노트생성 시간의 차이가 날수밖에 없음 => 오차를 잡아서 꼭 다 빼줘야함

            if(currentTime >=60d /bpm)
            {
            GameObject t_note = ObjectPool.instance.noteQueue.Dequeue(); //Dequeue(): 큐에있는걸 빼오는거임
            t_note.transform.position = tfNoteAppear.position;
            t_note.SetActive(true);
            timingManager.boxNoteList.Add(t_note);
            currentTime -= 60d/bpm; //0으로 안하고 굳이 -60/bpm으로 빼주는 이유 => deltaTime을 더해주기 때문에 0.5초가 아니라 0.5100551..초가 됨.
            }
        } 
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Note"))
        {
           if(other.GetComponent<Note>().GetNoteFlag())// *이미지가 보여질때만!!miss를 뜨게 한다
           {
                timingManager.MissRecord();
                effectManager.JudgementEffect(4); //Missed 연출뜨게 
                comboManager.ResetCombo();
           }
            timingManager.boxNoteList.Remove(other.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(other.gameObject); //다 쓴 노트들을 큐에다가 다시 담아줌
            other.gameObject.SetActive(false);
            
        }
    }
    public void RemoveNote()
    {
        noteActive =false;
        for (int i = 0; i < timingManager.boxNoteList.Count; i++)
        {
            timingManager.boxNoteList[i].SetActive(false); //끝났으면 안보이게 비활성화
            ObjectPool.instance.noteQueue.Enqueue(timingManager.boxNoteList[i]); //비활된거 다 큐에 다시 담아줌
        }
    }
}
