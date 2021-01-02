using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LeaveRoom : MonoBehaviour
{
    public void OnClick_LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }
}
