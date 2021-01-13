using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private Text _text;

    private string _userID;
    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = Player.NickName;
        _userID = player.UserId;

    }

    public void SetMasterPlayer()
    {
        _text.color = Color.red;
    }

}
