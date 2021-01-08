using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameManager : MonoBehaviourPunCallbacks
{
    private bool gameState = true;
    public GameObject gameOver;
    public Text timerText;
    public Transform[] waypoints;
    public static int lives = 2;
    public GameObject pacman;
    public GameObject kakaman;
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
            GameObject obj = PhotonNetwork.Instantiate(this.playerPrefab.name, PlayerPositionsVector3, Quaternion.identity, 0);
            obj.name = PhotonNetwork.LocalPlayer.UserId;
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