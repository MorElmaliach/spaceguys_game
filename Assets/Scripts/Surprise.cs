using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utilities;
using UnityEngine.UI;


public class Surprise : MonoBehaviour
{
    public void setBuffsMessage(string message)
    {
        object[] content = new object[] {message};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(1, content, raiseEventOptions, SendOptions.SendReliable);
    }

    void OnTriggerExit2D(Collider2D co)
    {
            gameObject.SetActive(false);
            if (co.gameObject.GetPhotonView().Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                ConsumeSurprise(PhotonNetwork.LocalPlayer.IsMasterClient? 0 : 1);
            }

    }

    public void ConsumeSurprise(int playerIndex)
    {
        //todo: Fix all RNG related shit
        int score = Random.Range(1, 15);
        int speed = Random.Range(30, 71);
        int time = Random.Range(3, 8);
        switch (Random.Range(1, 7))
        {
            case 1:
                ModifyScore(playerIndex, score);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} gained {score} points");
                break;
            case 2:
                ModifyScore(playerIndex, -score);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} lost {score} points");
                break;
            case 3:
                GameObject.Find(PhotonNetwork.PlayerList[playerIndex].UserId).GetComponent<PlayerControls>().ChangeSpeed(speed, time, true);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} is now faster for {time} seconds!");
                break;
            case 4:
                GameObject.Find(PhotonNetwork.PlayerList[playerIndex].UserId).GetComponent<PlayerControls>().ChangeSpeed(speed, time, false);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} is now slower for {time} seconds!");
                break;
            case 5:
                GameObject.Find(PhotonNetwork.PlayerList[playerIndex].UserId).GetComponent<PlayerControls>().FreezePlayer(time);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} is now frozen for {time} seconds!");
                break;
            case 6:
                if (playerIndex != 0) playerIndex = 1;
                GameObject.Find(PhotonNetwork.PlayerList[playerIndex].UserId).GetComponent<PlayerControls>().FreezePlayer(time);
                setBuffsMessage($"{PhotonNetwork.PlayerList[playerIndex].NickName} is now frozen for {time} seconds!");
                break;
            default:
                return;
        }
    }
        
    public void ModifyScore(int playerIndex, int score)
    {
        PhotonNetwork.PlayerList[playerIndex].AddScore(score);
    }

}

