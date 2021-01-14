
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Quit : MonoBehaviour
{
    public void ExitGame()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}
