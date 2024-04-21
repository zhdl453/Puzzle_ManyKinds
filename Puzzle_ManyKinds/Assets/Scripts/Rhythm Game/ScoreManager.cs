using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int increaseScore;
    [SerializeField] int comboBonusScore;
    [SerializeField] float[] weight;
    int currentScore;

    Animator scoreAnimator;
    ComboManager comboManager;


    private void Awake() {
        currentScore =0;
        scoreText.text = "0";
    }
    private void Start() {
        scoreAnimator = GetComponent<Animator>();
        comboManager = FindObjectOfType<ComboManager>();
    }
    public void Init()
    {
        currentScore =0;
        scoreText.text = "0";
    }
    public void IncreaseScore(int x)
    {
        //콤보증가
        comboManager.IncreaseCombo();

        //콤보 가중치계산
        int t_currentCombo = comboManager.GetCurrentCombo();
        //콤보구간 10~19=10점 / 20~29 = 20점
        int t_comboBonusScore = (int)(t_currentCombo / 10) * comboBonusScore;

        //판정 가중치계산
        int t_increaseScore = increaseScore+t_comboBonusScore;
        t_increaseScore = (int)(t_increaseScore * weight[x]);

        currentScore +=t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);

        scoreAnimator.SetTrigger("ScoreUp");
        
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
