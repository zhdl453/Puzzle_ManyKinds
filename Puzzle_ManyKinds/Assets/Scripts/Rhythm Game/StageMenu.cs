using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMenu : MonoBehaviour
{
    [SerializeField] GameObject titleUI = null;
    public void BtnBack()
    {
        titleUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void BtnPlay()
    {
        GeneralManager.Instance.GameStart();
         this.gameObject.SetActive(false);
    }
}
