using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;

    [SerializeField] private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnJoinedRoom()
    {
        _content.DestoryChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {

            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1 && _listings[index].gameObject != null)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index); 
                }
            }
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
                else
                {
                    //_listings.in
                }
            }
        }
    }

}
