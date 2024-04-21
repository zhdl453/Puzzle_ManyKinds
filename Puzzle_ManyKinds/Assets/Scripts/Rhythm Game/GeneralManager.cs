using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    [SerializeField] GameObject[] gameUis = null;
    [SerializeField] GameObject titleUis = null;
    public static GeneralManager Instance;

    public bool isGameStart = false;
    ComboManager comboManager;
    TimingManager timingManager;
    ScoreManager scoreManager;
    PlayerController playerController;
    StageManager stageManager;

    private void Start()
    {
        Instance = this;

        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        playerController = FindObjectOfType<PlayerController>();
        stageManager = FindObjectOfType<StageManager>();
        timingManager = FindObjectOfType<TimingManager>();
    }

    public void GameStart()
    {
        for (int i = 0; i < gameUis.Length; i++)
        {
            gameUis[i].SetActive(true);
        }
        stageManager.RemoveStage(); //있는것들 있으면 다 파괴시키고
        stageManager.SettingStage(); //다시 세팅깔아주는 거임 스테이지 빨판

        comboManager.ResetCombo();
        scoreManager.Init();
        timingManager.Init();
        playerController.Init();

        isGameStart = true;
    }
    public void MainMenu()
    {
        for (int i = 0; i < gameUis.Length; i++)
        {
            gameUis[i].SetActive(false);
        }
        titleUis.SetActive(true);
    }
}
