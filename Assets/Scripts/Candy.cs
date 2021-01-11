using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Utilities;

public class Candy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D co)
    {
        Destroy(gameObject);
        PhotonNetwork.PlayerList[co.gameObject.GetPhotonView().Owner.UserId == PhotonNetwork.MasterClient.UserId ? 0 : 1].AddScore(1);
    }
}
