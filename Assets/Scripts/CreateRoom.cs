using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Random = System.Random;

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
        Random rand = new Random();
        if (!PhotonNetwork.IsConnected || _roomName.text.Length == 0)
        {
            DateTime dateTime = DateTime.Now;
            Debug.LogWarning($"{dateTime}: Room name must be filled.");
            Console.getCurrentConsole().ShowWindow();
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        options.PublishUserId = true;
        PhotonNetwork.CreateRoom(_roomName.text + rand.Next(0, 5000), options, TypedLobby.Default);
    }
}
