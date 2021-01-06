using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameManager : MonoBehaviourPunCallbacks
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

    public Vector3 PlayerPositionsVector3;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, PlayerPositionsVector3, Quaternion.identity, 0);
        }
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

    public void IncrementScore()
    {
        PhotonNetwork.LocalPlayer.AddScore(1);
    }
    public void DecreaseScore()
    {
        PhotonNetwork.LocalPlayer.AddScore(-1);
    }

    public void ConsumeSurprise(bool Player)
    {
        //todo: Fix all RNG related shit
        int score = Random.Range(1, 15);
        int speed = Random.Range(30, 71);
        int time = Random.Range(3, 8);
        GameObject p1GameObject = GameObject.FindGameObjectWithTag("player_1");
        GameObject p2GameObject = GameObject.FindGameObjectWithTag("player_2");
        switch (Random.Range(1, 7))
        {
            case 1:
                for (int i = 0; i < score; i++) {
                    IncrementScore();
                }

                latestBuff.text = "Player " + (Player? "1" : "2") + " gained " + score + " points";
                break;
            case 2:
                for (int i = 0; i < score; i++)
                {
                    DecreaseScore();
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

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}