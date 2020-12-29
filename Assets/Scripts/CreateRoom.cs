using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _roomName;

    [SerializeField] private Text _mainRoomName;

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        if (PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default))
        {
            _mainRoomName.text = _roomName.text;
        }
    }
}
