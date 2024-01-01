using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class MoleGameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;
    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [Header("-----------[Audio]")]
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum sfx { click, dead, Button, Over, deadHardHat }
    int sfxCursor;

    // Hardcoded variables you may want to tune.
    private float startingTime = 30f;

    // Global variables
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;

    // This is public so the play button can see it.
    public void StartGame()
    {
        // Hide/show the UI elements we don't/do want to see.
        sfxPlay(sfx.Button);
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);
        bgmPlayer.Play();
        // Hide all the visible moles.
        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        // Remove any old game state.
        currentMoles.Clear();
        // Start with 30 seconds.
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;
    }

    public void GameOver(int type)
    {
        // Show the message.
        if (type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else
        {
            bombText.SetActive(true);
        }
        // Hide all moles.
        foreach (Mole mole in moles)
        {
            mole.StopGame();
        }
        // Stop the game and show the start UI.
        playing = false;
        playButton.SetActive(true);
        bgmPlayer.Stop();
        sfxPlay(sfx.Over);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            // Update time.
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            // Check if we need to start any more moles.
            if (currentMoles.Count <= (score / 10))
            {
                // Choose a random mole.
                int index = Random.Range(0, moles.Count);
                // Doesn't matter if it's already doing something, we'll just try again next frame.
                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }
    }

    public void AddScore(int moleIndex)
    {
        // Add and update score.
        score += 1;
        scoreText.text = $"{score}";
        // Increase time by a little bit.
        timeRemaining += 1;
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        if (isMole)
        {
            // Decrease time by a little bit.
            timeRemaining -= 2;
        }
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }
    public void sfxPlay(sfx type)//{ click, dead, Button, Over }
    {
        switch (type)
        {
            case sfx.click:
                sfxPlayer[sfxCursor].clip = sfxClip[0];
                break;
            case sfx.dead:
                sfxPlayer[sfxCursor].clip = sfxClip[1];
                break;
            case sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[2];
                break;
            case sfx.Over:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case sfx.deadHardHat:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
        }
        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length; //계속 0,1,2가 될거임
    }
}
