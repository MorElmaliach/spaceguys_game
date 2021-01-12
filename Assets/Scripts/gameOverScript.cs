using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class gameOverScript : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private Text gameOverMessage;


    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 3)
        {
            object[] data = (object[])photonEvent.CustomData;
            gameOverMessage.text = $"Game over! The winner is {data[0]} with {data[1]} points!";
        }

        if (eventCode == 4)
        {
            gameOverMessage.text = "";
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
