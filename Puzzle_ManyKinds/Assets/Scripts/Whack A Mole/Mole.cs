using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite moleHardHat;
    [SerializeField] private Sprite moleHatBroken;
    [SerializeField] private Sprite moleHit;
    [SerializeField] private Sprite moleHatHit;

    [Header("GameManager")]
    [SerializeField] private GameManager gameManager;

    //The offset of the sprite to hide it
    private Vector2 StartPosition = new Vector2(0f, -2.56f);

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
