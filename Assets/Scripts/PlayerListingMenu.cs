using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.Simple.Internal;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;

    [SerializeField] private PlayerListing _playerListing;

    [SerializeField] private Button _startGameButton;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    

    public void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
        ResetStartButtonStatus();
    }

    private void ResetStartButtonStatus()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2)
            _startGameButton.interactable = true;
        else
            _startGameButton.interactable = false;
        _listings.Find(x => x.Player.IsMasterClient).SetMasterPlayer();
    }

    public void LeaveRoom()
    {
        _content.DestoryChildren();
    }

    private void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate(_playerListing, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            if (player.IsMasterClient)
            {
                listing.SetMasterPlayer();
            }
            _listings.Add(listing);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
        ResetStartButtonStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        List<PlayerListing> list = _listings.FindAll(x => (x.Player.NickName == otherPlayer.NickName));
        foreach (PlayerListing playerListing in list)
        {
            Debug.Log($"Testing which object is destroyed: {playerListing}, {playerListing.Player.NickName}");
            if (playerListing == null) continue;
            Destroy(playerListing.gameObject);
        }

        _listings = _listings.Except(list).ToList();
        ResetStartButtonStatus();

    }

    public void OnClick_StartGame()
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        //    ;
        //    //Todo: add error msg

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }




}
