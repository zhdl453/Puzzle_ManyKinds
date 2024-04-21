using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPressKey = true;
    [Header("이동")]
    [SerializeField] float moveSpeed;
    Vector3 dir; //어느방향으로 갈지
    public Vector3 destPos; //목적지
    Vector3 originPos = new Vector3();

    [Header("회전")]
    [SerializeField] float spinSpeed;
    Vector3 rotDir; //어디 방향으로 회전할지
    Quaternion destRot; //얼마만큼 회전할지 목표치
    [Header("반동")]
    [SerializeField] float vibePosY; //들썩이게 하는 효과
    [SerializeField] float vibeSpeed; //들썩이게 하는 효과
    bool canMove = true; //플레이어가 움직이게 할수 있는 만드는 변수
    bool isFalling = false; //플레이어가 떨어지는지 판단 변수

    [Header("기타")]
    [SerializeField] Transform fakeCube = null; //가짜 큐브를 먼저 돌려놓고, 그 돌아간 만큼의 값을 목표 회전값으로 삼음 =>그걸 destRot에 넣어줄거임
    [SerializeField] Transform realCube = null; //가짜 큐브를 먼저 돌려놓고, 그 돌아간 만큼의 값을 목표 회전값으로 삼음 =>그걸 destRot에 넣어줄거임
    TimingManager timingManager;
    [SerializeField] CameraController cameraController;
    Rigidbody playerRigid;
    private void Awake() {
        moveSpeed = 3;
        spinSpeed = 270;
        dir = new Vector3();
        destPos = new Vector3();
        rotDir = new Vector3();
        destRot = new Quaternion();
        vibePosY = 0.25f;
        vibeSpeed=1.5f;
    }
    private void Start()
     {
        timingManager  = FindObjectOfType<TimingManager>();
        playerRigid = GetComponentInChildren<Rigidbody>();
        originPos = transform.position;
        //cameraController  = FindObjectOfType<CameraController>();
     }
    void Update()
    {
        if(GeneralManager.Instance.isGameStart)
        {
            CheckFalling();
            if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.W))
            {
                if(canMove &&s_canPressKey&&!isFalling)
                {
                    Calc(); //타이밍 체크하기전에 잘 방향맞게 가는지 계산부터 하는거임.
                    if(timingManager.CheckTiming())
                    {
                        StartAction();
                        Debug.Log($"StartAction()실행: {timingManager.CheckTiming()}");
                    }
                    else
                    {
                        Debug.Log($"CheckTiming: {timingManager.CheckTiming()}");
                    }
                }
            }
        }
        
    }
    void Calc()
    {
        //방향 계산
        dir.Set(Input.GetAxisRaw("Vertical"),0,Input.GetAxisRaw("Horizontal")); //상하로는 안움직이게 0

        //이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x,0,dir.z);

        //회전 목표값 계산
        rotDir = new Vector3(-dir.z,0,-dir.x);
        //RotateAround(): 태양 주변을 공전하는 지구등을 구현할때 사용:RotateAround(공전 대상, 최전축, 회전값)을 이용한 <편법 회전 구현>
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }
    void StartAction()
    {
        

        StartCoroutine(MoveGo());
        StartCoroutine(SpinGo());
        StartCoroutine(VibeGo());
        StartCoroutine(cameraController.ZoomCam());

    }

    IEnumerator MoveGo()
    {
        canMove = false;
        //목적지 도달할때까지 while문으로 계속 돌릴거임
        //while (Vector3.Distance(transform.position,destPos) !=0)//Vector3.Distance: A좌표와 B좌표간의 거리차를 반환
        while (Vector3.SqrMagnitude(transform.position-destPos) >=0.001)//Vector3.SqrMagnitude:이게 distance보다 가벼움. 제곱근을 리턴함 ex) SqrMagnitude(4) =2
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed*Time.deltaTime);
            yield return null;
        }
        transform.position = destPos; //0.001로 작은 오차 남을수 있으니 안전하게 딱 목적지로 찍어주는거임
        canMove = true;
    }
    IEnumerator SpinGo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot) >0.5f) //a,b의 회전 차이값
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation,destRot,spinSpeed*Time.deltaTime);
            yield return null;
        }
        realCube.rotation = destRot;
    }
    IEnumerator VibeGo()
    {
        while (realCube.position.y<vibePosY) //Y축으로 최대로 올라가기 전까지 올라감
        {
            realCube.position += new Vector3(0, vibeSpeed * Time.deltaTime,0);
            yield return null;
        }
        while (realCube.position.y>0)//Y축으로 최대치찍고 아래로 내려감 0될때까지 
        {
            realCube.position -= new Vector3(0, vibeSpeed * Time.deltaTime,0);
            yield return null;
        }
        realCube.localPosition = new Vector3(0,0,0);
    }

    void CheckFalling() //레이저 쏴서 plate와 충돌안하면 낭떠러지임
    {
        if(!isFalling && canMove)//canMove넣은이유: 움직임이 완전 멈춘 상태에서 떨어지는 상태일지아닐지 판단해주고 싶음
        {
            if(!Physics.Raycast(transform.position, Vector3.down,1.1f))
            {
                Falling();
            }
        }
        
    }
    void Falling()
    {
        isFalling = true;
        playerRigid.useGravity = true;
        playerRigid.isKinematic = false;
    }

    public void ResetFalling()
    {
        isFalling = false;
        playerRigid.useGravity = false;
        playerRigid.isKinematic= true;
        transform.position = originPos;
        realCube.localPosition = new Vector3(0,0,0); //rigidbody가 없는 부모객체는 낭떠러지위에, 자식객체(그래픽)은 추락중이니 이것도 위로 올려줘야함
    }
}
