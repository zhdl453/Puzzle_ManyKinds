using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject resultUI;
    [SerializeField] TMP_Text[] txtCount = null;
    [SerializeField] TMP_Text txtCoin = null;
    [SerializeField] TMP_Text txtScore = null;
    [SerializeField] TMP_Text txtMaxCombo = null;

    ScoreManager scoreManager;
    ComboManager comboManager;
    TimingManager timingManager;

    private void Start()
    {
        resultUI.SetActive(false);
        timingManager = FindObjectOfType<TimingManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }
    public void ShowResult()
    {
        resultUI.SetActive(true);
        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }
         txtCoin.text = "0";
         txtScore.text = "0";
         txtMaxCombo.text = "0";

         int[] t_judgement = timingManager.GetJudgementRecord(); //판정들 가져오는거
         int t_currentScore = scoreManager.GetCurrentScore();//점수 가져오는거
         int t_maxCombo = comboManager.GetMaxCombo();//최고콤보 가져오는거
         int t_coin = t_currentScore / 50; //동전 갯수 가져오기

         for (int i = 0; i < txtCount.Length; i++)
         {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
         }
         txtScore.text = string.Format("{0:#,##0}", t_currentScore);
         txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
         txtCoin.text = string.Format("{0:#,##0}", t_coin);
    }
}
