using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _roomName;

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField] private byte maxPlayersPerRoom = 2;
    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected || _roomName.text.Length == 0)
        {
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
    }
}
