using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StageManager : MonoBehaviour
{
   [SerializeField] GameObject stage = null;
   GameObject currentStage;
   public Transform[] plateTranses;
   [SerializeField] float offsetY = 5; //뻘판이 위에서 착착 붙어서 생기는것처럼
   [SerializeField] float plateSpeed = 10; //얼마나 빠르게 올라올지
   int stepCount = 0;
   int totalPlateCount = 0;

   public void RemoveStage()
   {
        if(currentStage !=null) //데이터가 있다는뜻
        {
            Destroy(currentStage); //있으면 없애라는거임
        }
   }
   public void SettingStage() //함수 호출할때만 스테이지 만들게끔
    {
        stepCount = 0;
        currentStage = Instantiate(stage, Vector3.zero, quaternion.identity);// Vector3.zero:자기 위치에 생성해주는거임
        plateTranses = currentStage.GetComponent<Stage>().plateTranses;
        totalPlateCount = plateTranses.Length;

        for (int i = 0; i < totalPlateCount; i++)
        {
            plateTranses[i].position = new Vector3(plateTranses[i].position.x,
                                                    plateTranses[i].position.y - offsetY,
                                                    plateTranses[i].position.z);
        }
    }

    public void ShowNextPlate()
    {
        if(stepCount<totalPlateCount)
        {
            StartCoroutine(MovePlateGo(stepCount++));
        }
        
    }

    IEnumerator MovePlateGo(int p_num)
    {
        plateTranses[p_num].gameObject.SetActive(true);
        //t_destPos는 start함수에서 더해줬던 offsetY만큼을 다시 빼준거니, 그냥 원래 원상태의 갑이다.
        Vector3 t_destPos = new Vector3(plateTranses[p_num].position.x,
                                        plateTranses[p_num].position.y+offsetY,
                                        plateTranses[p_num].position.z);
        
        while (Vector3.SqrMagnitude(plateTranses[p_num].position - t_destPos) >=0.001f)
        {
            plateTranses[p_num].position = Vector3.Lerp(plateTranses[p_num].position, t_destPos,plateSpeed*Time.deltaTime); 
            yield return null;  
        }
        plateTranses[p_num].position = t_destPos;
    }
}
