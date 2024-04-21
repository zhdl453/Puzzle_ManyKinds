using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerTrans = null; //카메라가 따라갈 플레이어의 위치
    [SerializeField] float followSpeed = 15f; //플레이어 따라갈 속도
    Vector3 playerDistance = new Vector3(); //카메라와 플레이어의 거리차
    float hitDistance = 0;
    [SerializeField] float zoomDistance = -1.25f;

    void Start()
    {
        playerDistance = transform.position - playerTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        //이동할 좌표값을 구한 뒤, 카메라 이동
        //아 내꺼 3d아니고 2d라 transform.forward(z축)이 효과 안먹힘 똑같음. 어쩔수 없이 어색해도 transform.up으로 위아래 흔들리게 해야겠다..
        Vector3 t_destPos = playerTrans.position + playerDistance+(transform.up*hitDistance);//대상의 정면방향(파란색 z축 화살표)
        //Vector3.Lerp(a,b,c): a와b사이의 값에서 c비율의 값을 추출 ex) Mathf.Lerp(0,10,0.5) => 5
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed*Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;
        yield return new WaitForSeconds(0.15f);
        hitDistance=0;
    }
}
