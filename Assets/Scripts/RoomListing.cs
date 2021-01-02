using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private Text _text;

    public RoomInfo RoomInfo;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void OnClick_Button()
    {
        if (PhotonNetwork.JoinRoom(RoomInfo.Name))
        {
            // Display an error for a full room
        }

    }

}
