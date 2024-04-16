using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //이걸해야 인스펙터에서 클래스 수정할수 있음
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count; //필요한 만큼의 갯수가 몇개인지. 나는 인스펙터에서 적당히 15개로 설정할거임. 부족하면 오류뜰거임
    public Transform poolParentTrans; //누구 부모 밑에 생성할지
}
//ObjectPool클래스를 언제 어디서든 쓰고 반납할수 있도록 공유자원으로 만듦
public class ObjectPool : MonoBehaviour 
{
    [SerializeField] ObjectInfo[] objectInfos;
    public static ObjectPool instance; //공유자원 instance를 통해 어디서든 public 변수,함수에 접근이 가능
    public Queue<GameObject> noteQueue; //Queue: 선입선출 자료형(가장 먼저 들어간 데이터가 가장 먼저 빠져나옴)

    void Awake()
    {
        noteQueue = new Queue<GameObject>();
    }
    void Start()
    {
        instance = this; // 이게 없으면 값이 없는상태가 되니까 자기자신 ObjectPool 타입 객체를 넣어줌
        noteQueue = InserQueue(objectInfos[0]); 
        // noteQueue = InserQueue(objectInfos[1]); //생성파괴가 자주 일어나는 객체가 있다 싶으면 다른 큐에 넣어주면 됨
        // noteQueue = InserQueue(objectInfos[2]); //생성파괴가 자주 일어나는 객체가 있다 싶으면 다른 큐에 넣어주면 됨
    }

    Queue<GameObject> InserQueue(ObjectInfo p_objecInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for (int i = 0; i < p_objecInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objecInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if(p_objecInfo.poolParentTrans != null) //부모객체가 있으면 그 객체 밑으로 클론펩 생성
            {
                t_clone.transform.SetParent(p_objecInfo.poolParentTrans);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }
            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }
}
