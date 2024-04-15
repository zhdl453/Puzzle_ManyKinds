using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm;
    double currentTime;

    [SerializeField] Transform tfNoteAppear;
     [SerializeField] GameObject goNote;
     void Awake()
     {
        currentTime = 0d;
     }

    // 60/120 = 1beat 당 0.5초 : 60s/bpm = 1beat시간
    void Update()
    {
        currentTime +=Time.deltaTime; //deltaTime을 더해주기 때문에 0.5초가 아니라 0.5100551..초가 됨.
        //실제로 시간이 소숫점 단위 만큼 더 흘렀지만,게임은 플레이 단위로 흘러간 시간을 체크할 수 있기때문에 생긴 일인거임
        //이걸 무지하고 초기화 currentTime=0으로 해버리면 그 오차만큼의 시간적 손실이 계속 발생함.
        //이 손실이 누적돼다보면 bpm과 노트생성 시간의 차이가 날수밖에 없음 => 오차를 잡아서 꼭 다 빼줘야함

        if(currentTime >=60d /bpm)
        {
            GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
            t_note.transform.SetParent(this.transform);
            currentTime -= 60d/bpm; //0으로 안하고 굳이 -60/bpm으로 빼주는 이유 => deltaTime을 더해주기 때문에 0.5초가 아니라 0.5100551..초가 됨.
        }
    }
}
