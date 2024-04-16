using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    //생성된 노트를 담는 List => 판정범위에 있는지 모든 노트를 비교해야함
    public List<GameObject> boxNoteList;
    [SerializeField] Transform Center; //판정 범위에 중심을 알려주는 센터 변수를 선언
    [SerializeField] RectTransform[] timingRects; //다양한 판점 범위를 보여줄 RectTransform배열도 선언
    Vector2[] timingBoxes; //RectTransform[]실제값들을 여기에 넣어줄거임. 판정범위의 최소값x, 최대값y

    void Awake()
    {
        boxNoteList = new List<GameObject>();
        timingBoxes = new Vector2[timingRects.Length];
    }
    void Start()
    {
        //각각의 판정범위: 최소값=중심-(이미지의너비/2) ~ 최대값=중심+(이미지의너비/2)
        //타이밍 박스 설정
        for (int i = 0; i < timingRects.Length; i++)
        {
            timingBoxes[i].Set(Center.localPosition.x - timingRects[i].localPosition.x,
                                Center.localPosition.x + timingRects[i].localPosition.x);
        }
    }
    
    public void CheckTiming() //판정범위 최소값 <= 노트의 x값 <판정범위 최대값
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < timingBoxes.Length; x++) //판정범위에 있는지 확인
            {
                //인덱스 0부터 확인하므로 판정순서도 Perfect->Cool->Good->Bad
                if(timingBoxes[x].x <=t_notePosX && t_notePosX <=timingBoxes[x].y)
                {
                    Debug.Log("Hit :" +x);
                    return; //이미 판상 범위 있는 노트를 찾았으니까 의미없는 반복을 게속할 필요가 없음
                }
            }
        }
        
    }
}
