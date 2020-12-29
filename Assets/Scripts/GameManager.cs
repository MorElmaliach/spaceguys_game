﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int p1_score = 0;
    public int p2_score = 0;
    private bool gameState = true;
    public Text p1ScoreText;
    public Text p2ScoreText;
    public Text latestBuff;
    public GameObject gameOver;
    public Text timerText;
    public Transform[] waypoints;
    public static int lives = 2;
    public GameObject pacman;
    public GameObject kakaman;
    public GameObject MapItem;
    public UnityEngine.Object coinPrefab;
    public float timeLeft = 12000;
    public GameObject[] Surprises;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        p1ScoreText.text = "P1 Score: " + p1_score;
        p2ScoreText.text = "P2 Score: " + p2_score;
    }

    // Update is called once per frame
    void Update()
    {
        float minutes = timeLeft < 0 ? 0 : Mathf.FloorToInt(timeLeft / 60);
        float seconds = timeLeft < 0 ? 0 : Mathf.FloorToInt(timeLeft % 60);

        timerText.text = "Time: " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if ((int)timeLeft % 15 == 0)
            {
                bringBackSurprises();
            }

        } /*else
        /*{
            gameState = false;
            gameOver.SetActive(true);
        }*/
        
    }

    public void IncrementScore(bool Player)
    {
        if (Player)
        {
            p1_score++;
            p1ScoreText.text = "P1 Score: " + p1_score;
        } else
        {
            p2_score++;
            p2ScoreText.text = "P1 Score: " + p2_score;
        }
        
    }
    public void DecreaseScore(bool Player)
    {
        if (Player)
        {
            if (p1_score > 0)
            {
                p1_score--;
            }
            p1ScoreText.text = "P1 Score: " + p1_score;
        }
        else
        {
            if (p2_score > 0)
            {
                p2_score--;
            }
            p2ScoreText.text = "P2 Score: " + p2_score;
        }
        

    }

    public void ConsumeSurprise(bool Player)
    {
        int score = Random.Range(1, 15);
        int speed = Random.Range(30, 71);
        int time = Random.Range(3, 8);
        GameObject p1GameObject = GameObject.FindGameObjectWithTag("player_1");
        GameObject p2GameObject = GameObject.FindGameObjectWithTag("player_2");
        switch (Random.Range(1, 7))
        {
            case 1:
                for (int i = 0; i < score; i++) {
                    IncrementScore(Player);
                }

                latestBuff.text = "Player " + (Player? "1" : "2") + " gained " + score + " points";
                break;
            case 2:
                for (int i = 0; i < score; i++)
                {
                    DecreaseScore(Player);
                }
                latestBuff.text = "Player " + (Player ? "1" : "2") + " lost " + score + " points";
                break;
            case 3:
                if (Player)
                {
                    p1GameObject.GetComponent<CandymanMoves>().ChangeSpeed(speed, time, true);
                } else
                {
                    p2GameObject.GetComponent<MovementP2>().ChangeSpeed(speed, time, true);
                }
                latestBuff.text = "Player " + (Player ? "1" : "2") + " is now faster for " + time + " seconds!";
                break;
            case 4:
                if (Player)
                {
                    p1GameObject.GetComponent<CandymanMoves>().ChangeSpeed(speed, time, false);
                }
                else
                {
                    p2GameObject.GetComponent<CandymanMoves>().ChangeSpeed(speed, time, false);
                }
                latestBuff.text = "Player " + (Player ? "1" : "2") + " is now slower for " + time + " seconds!";
                break;
            case 5:
                if (Player)
                {
                    p1GameObject.GetComponent<CandymanMoves>().FreezePlayer(time);
                }
                else
                {
                    p2GameObject.GetComponent<CandymanMoves>().FreezePlayer(time);
                }
                latestBuff.text = "Player " + (Player ? "1" : "2") + " is now frozen for " + time + " seconds!";
                break;
            case 6:
                if (Player)
                {
                    p2GameObject.GetComponent<CandymanMoves>().FreezePlayer(time);
                }
                else
                {
                    p1GameObject.GetComponent<CandymanMoves>().FreezePlayer(time);
                }
                latestBuff.text = "Player " + (Player ? "2" : "1") + " is now frozen for " + time + " seconds!";
                break;
            default:
                return;
        }
        
    }
    
    private void bringBackSurprises()
    {
        foreach(GameObject surprise in Surprises)
        {
            surprise.SetActive(true);
        }
    }

    public void Death()
    {
        waypoints[lives--].GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.black);
        if (lives >= 0)
        {
            pacman.GetComponent<CandymanMoves>().Restart();
            kakaman.GetComponent<Movement>().ResetPosition();
        }
        else
        {
            gameState = false;
            gameOver.SetActive(true);
        }
    }

    public bool isGameOver()
    {
        return gameState;
    }
}

