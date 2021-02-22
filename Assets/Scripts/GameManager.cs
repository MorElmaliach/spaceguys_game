using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameManager : MonoBehaviourPunCallbacks
{
    private bool gameState = true;
    public Text timerText;
    [SerializeField] private float _timeLeft = 120;
    public GameObject[] Surprises;
    public Vector3 PlayerPositionsVector3;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Missing playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, PlayerPositionsVector3, Quaternion.identity, 0);
        }
        PlayerExt.SetScore(PhotonNetwork.LocalPlayer, 0);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(4, "", raiseEventOptions, SendOptions.SendReliable);
    }

    // Update is called once per frame
    void Update()
    {
        float minutes = _timeLeft < 0 ? 0 : Mathf.FloorToInt(_timeLeft / 60);
        float seconds = _timeLeft < 0 ? 0 : Mathf.FloorToInt(_timeLeft % 60);

        timerText.text = "Time: " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            if ((int)_timeLeft % 15 == 0)
            {
                bringBackSurprises();
            }

        } 

        else if (gameState)
        {
            StartCoroutine(TimedEffect(1));
            gameState = false;
        }
    }


    private void bringBackSurprises()
    {
        foreach(GameObject surprise in Surprises)
        {
            surprise.SetActive(true);
        }
    }

    IEnumerator TimedEffect(int time)
    {
        yield return new WaitForSeconds(time);
        string winner = "";
        var score = 0;
        if (PhotonNetwork.PlayerList[0].GetScore() >
            PhotonNetwork.PlayerList[1].GetScore())
        {
            winner = PhotonNetwork.PlayerList[0].NickName;
            score = PhotonNetwork.PlayerList[0].GetScore();
        }
        else
        {
            winner = PhotonNetwork.PlayerList[1].NickName;
            score = PhotonNetwork.PlayerList[1].GetScore();
        }

        object[] content = new object[] { winner, score };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(3, content, raiseEventOptions, SendOptions.SendReliable);
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