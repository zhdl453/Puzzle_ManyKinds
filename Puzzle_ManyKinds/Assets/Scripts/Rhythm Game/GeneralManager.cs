using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    [SerializeField] GameObject[] gameUis = null;
    [SerializeField] GameObject titleUis = null;
    public static GeneralManager Instance;

    public bool isGameStart = false;

    private void Start() {
        Instance = this;
    }

    public void GameStart()
    {
        for (int i = 0; i < gameUis.Length; i++)
        {
            gameUis[i].SetActive(true);
        }
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
