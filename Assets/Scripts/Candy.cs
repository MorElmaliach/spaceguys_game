using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Utilities;

public class Candy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        Destroy(gameObject);
        PhotonNetwork.PlayerList[co.gameObject.GetPhotonView().Owner.UserId == PhotonNetwork.MasterClient.UserId ? 0 : 1].AddScore(1);
    }
}
