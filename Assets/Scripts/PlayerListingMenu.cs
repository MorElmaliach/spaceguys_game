using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;

    [SerializeField] private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    
    public void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
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
            _listings.Add(listing);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => Equals(x.Player, otherPlayer));

        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }




}
