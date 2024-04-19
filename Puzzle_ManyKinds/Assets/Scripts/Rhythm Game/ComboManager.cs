using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] TMP_Text comboText;
    [SerializeField] GameObject comboImageObj;
    int currentCombo;
    [SerializeField] Animator comboAnimator;
    void Start()
    {
        //comboAnimator = GetComponent<Animator>();
        comboText.gameObject.SetActive(false);
        comboImageObj.SetActive(false);
    }

    public void IncreaseCombo(int p_num=1)
    {
        currentCombo +=p_num;
        comboText.text = string.Format("{0:#,##0}", currentCombo);

        if(currentCombo >2)
        {
            comboText.gameObject.SetActive(true);
            comboImageObj.SetActive(true);

            comboAnimator.SetTrigger("ComboUp"); 
        }
    }
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    public void ResetCombo()
    {
        currentCombo =0;
        comboText.text = "0";
        comboText.gameObject.SetActive(false);
        comboImageObj.SetActive(false);
    }
}
