using System;
using System.Text;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Random = System.Random;

namespace SpaceGuys
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField] private GameObject _roomListingView;
        [SerializeField] private GameObject _roomsListingView;
        [SerializeField] private GameObject _createListingView;
        [SerializeField] private PlayerListingMenu _playerListingMenu;
        [SerializeField] private InputField _nicknameText;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private GameObject _console;


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            _console.GetComponent<Console>().ShowWindow();
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
                _createRoomButton.interactable = true;
            }

        }
        public override void OnJoinedRoom()
        {
            _playerListingMenu.GetCurrentRoomPlayers();
            _roomListingView.SetActive(true);
            _roomsListingView.SetActive(false);
            _createListingView.SetActive(false);
        }

        public override void OnLeftRoom()
        {
            _playerListingMenu.LeaveRoom();
        }

        public void ChangeNickname(string newNickname)
        {
            PhotonNetwork.NickName = newNickname;
        }

        private string RandomString(int Size)
        {
            StringBuilder builder = new StringBuilder();
            Random rand = new Random();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        /*
         * TODO: Delete when done testing or when discord username don't work
        */

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        }

        

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = "Player" + RandomString(5);
            _nicknameText.text = PhotonNetwork.NickName;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion


    }
}
