using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    //생성된 노트를 담는 List => 판정범위에 있는지 모든 노트를 비교해야함
    public List<GameObject> boxNoteList;

    int[] judgementRecord = new int[5]; //퍈정기록에다 보여줄 기록들
    [SerializeField] Transform Center; //판정 범위에 중심을 알려주는 센터 변수를 선언
    [SerializeField] RectTransform[] timingRects; //다양한 판점 범위를 보여줄 RectTransform배열도 선언
    Vector2[] timingBoxes; //RectTransform[]실제값들을 여기에 넣어줄거임. 판정범위의 최소값x, 최대값y
    EffectManager effectManager;
    ScoreManager scoreManager;
    ComboManager comboManager;
    StageManager stageManager;
    PlayerController playerController;

    void Awake()
    {
        boxNoteList = new List<GameObject>();
        timingBoxes = new Vector2[timingRects.Length];
        effectManager = FindObjectOfType<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        stageManager = FindObjectOfType<StageManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    public void Init()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
        {
            judgementRecord[i] = 0;
        }
    }
    void Start()
    {
        //각각의 판정범위: 최소값=중심-(이미지의너비/2) ~ 최대값=중심+(이미지의너비/2)
        //타이밍 박스 설정
        //timingBoxes =  new Vector2[timingRects.Length];
        for (int i = 0; i < timingRects.Length; i++)
        {
            //x:최소값 ~ y:최대값
            timingBoxes[i].Set(Center.localPosition.x - timingRects[i].rect.width/2,
                                Center.localPosition.x + timingRects[i].rect.width/2);
        }
    }

    public bool CheckTiming() //판정범위 최소값 <= 노트의 x값 <판정범위 최대값
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < timingBoxes.Length; x++) //판정범위에 있는지 확인
            {
                //인덱스 0부터 확인하므로 판정순서도 Perfect->Cool->Good->Bad
                if(timingBoxes[x].x <=t_notePosX && t_notePosX <=timingBoxes[x].y)
                {
                    //노트제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);//스페이스바 누르면 없애주는거임

                    //이펙트연출
                    if(x<timingBoxes.Length -1)//Hit 애니 효과가 Bad때는 안뜨게 하기 위함
                    {
                        effectManager.NoteHitEffect(); 
                    }           
                    

                    //새로운 빨판 나올때만 점수 증가하게
                    if(CheckCanNextPlate())
                    {
                        //점수 증가
                        scoreManager.IncreaseScore(x);
                        stageManager.ShowNextPlate(); //다음 빨판 나옴
                        effectManager.JudgementEffect(x);
                        judgementRecord[x]++; //판정기록
                    }
                    else//새로운 빨판 안 밟으면 normal 판정뜨게
                    {
                        effectManager.JudgementEffect(5);
                    }        
                    return true; //올바른 판정값이 되면 true
                    //이미 판상 범위 있는 노트를 찾았으니까 의미없는 반복을 게속할 필요가 없음
                }
            }
        }
        comboManager.ResetCombo();
        effectManager.JudgementEffect(timingBoxes.Length); //timingBoxes.Length는 5니까 Missed가 뜰거임!
        MissRecord(); //miss기록도 올라감판정기록
        return false; //miss 판정값이 되면 false반환
    }
    bool CheckCanNextPlate()
    {
        //목적지 좌표에서 아래로 발사 =>충돌한 BasicPlate를 t_hitInfo에 획득한다.
        if(Physics.Raycast(playerController.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if(t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if(t_plate.flag)
                {
                    t_plate.flag = false; //이미 밟은 발판을 또 출연하게 하면 안되니까 밟은건 false로 처리 해주는거임
                    return true;
                }
            }
        }
        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;//판정기록
    }
}
