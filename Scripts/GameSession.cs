using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int currentScore = 0;

    private void Awake()
    {
        StartSingleton();
    }

    private void StartSingleton()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if (gameStatusCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddToScore(int scoreValue)
    {
        currentScore += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}
