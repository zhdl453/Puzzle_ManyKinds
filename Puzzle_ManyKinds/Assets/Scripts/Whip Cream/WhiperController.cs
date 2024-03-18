using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhiperController : MonoBehaviour
{
    Touch touch;
    [Header("Whipper 관련 UI")]
    [SerializeField] float whipperSpeed; //whipper움직이는거 빠르기 속도 <=인스펙터에서 속도조절
    [SerializeField] Transform whipperTrans;
    bool isMoving; //whipper가 움직이고 있는동안은 터치해도 작동안되게 Boolean 변수<
    [Header("게이지 바")]
    [SerializeField] Slider slider;
    [SerializeField] Image sliderFillImg;
    [SerializeField] float FillAmount; //증가량. 게이지 안떨어짐. 계속 증가만
    float currentFill; //currentFill +=FillAmount <=이 값이 슬라이더 value 정해줌
    [SerializeField] float MaxFill;// MaxFill를 넘으면 게임 종료.


    private void Start()
    {
        currentFill = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // PC와 모바일에서 모두 작동하도록 입력 처리
#if UNITY_EDITOR || UNITY_STANDALONE // PC에서 실행되는 경우
        if (Input.GetMouseButtonDown(0)) //마우스 버튼을 누른 순간
        {
            Debug.Log("마우스 클릭 감지했습니다");

            IncreaseFill();
        }
#else // 모바일 장치에서 실행되는 경우
        if (Input.touchCount > 0)
        {
            Debug.Log("터치 감지했습니다");
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                IncreaseFill();
            }
        }
#endif
    }

    void IncreaseFill()
    {
        currentFill += FillAmount;
        slider.value = currentFill / MaxFill;
        if (slider.value == 1)
        {
            StartCoroutine(AllCompleteAndHide());
        }
    }
    IEnumerator AllCompleteAndHide()
    {
        Debug.Log("반죽이 완성되었습니다! 짝짝짝");
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        slider.value = 0;
        currentFill = 0;
    }

}
