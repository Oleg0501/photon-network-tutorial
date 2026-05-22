using System;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    [RequireComponent(typeof(PhotonView))]
    public class SearchGamePhotonController : MonoBehaviourPunCallbacks
    {
        public UnityEvent<double> OnGameStartedRPC = new();
        
        [SerializeField] private SearchGameView _searchGameView;
        
        [SerializeField] private int _maxPlayers;
        [SerializeField] private float _gameDuration;

        private bool _gameStarted;
        private bool _isLeavingRoom;
        private bool _wantsToFindGame;
        
        public void EnableView(bool enable)
        {
            _searchGameView.gameObject.SetActive(enable);
        }
        
        private void Awake()
        {
            PhotonNetwork.GameVersion = "1.0";
            _searchGameView.SearchButton.onClick.AddListener(OnSearchButtonClicked);
        }

        private void OnSearchButtonClicked()
        {
            if (_isLeavingRoom)
            {
                return;
            }

            _wantsToFindGame = true;
            _searchGameView.SearchButton.interactable = false;
            SetStatusText("Trying to connect to Photon...");
            
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomRoom();
                SetStatusText("Connected to Photon. Searching for game...");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            if (!_wantsToFindGame)
            {
                return;
            }
            
            PhotonNetwork.JoinRandomRoom();
            SetStatusText("Connected to Photon. Searching for game...");
        }

        
        public override void OnJoinedRoom()
        {
            _wantsToFindGame = false;
            SetStatusText($"You entered to room. Players count: {PhotonNetwork.CurrentRoom.PlayerCount}/{_maxPlayers}");

            if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayers)
            {
                TryStartGameAsMasterClient();
            }
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            SetStatusText($"The room not founded. Creating a new room...");

            var roomOptions = new RoomOptions
            {
                MaxPlayers = _maxPlayers,
                IsOpen = true,
                IsVisible = true
            };

            var roomName = "ColorGame_" + Guid.NewGuid().ToString("N");
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            SetStatusText($"Player connected. Players count: {PhotonNetwork.CurrentRoom.PlayerCount}/{_maxPlayers}");

            if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayers)
            {
                TryStartGameAsMasterClient();
            }
        }
        
        private void TryStartGameAsMasterClient()
        {
            if (!PhotonNetwork.IsMasterClient || _gameStarted)
            {
                return;
            }
            
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            var endGameTime = PhotonNetwork.Time + _gameDuration;

            photonView.RPC(nameof(StartGameRPC), RpcTarget.AllBuffered, endGameTime);
        }
        
        [PunRPC]
        private void StartGameRPC(double endTime)
        {
            OnGameStartedRPC?.Invoke(endTime);
            
            _gameStarted = true;
            _isLeavingRoom = false;
            _searchGameView.SearchButton.interactable = false;
        }

        private void SetStatusText(string text)
        {
            _searchGameView.StatusText.text = text;
        }
    }
}