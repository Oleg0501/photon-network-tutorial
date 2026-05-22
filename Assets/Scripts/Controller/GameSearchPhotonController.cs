using System;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    [RequireComponent(typeof(PhotonView))]
    public class GameSearchPhotonController : MonoBehaviourPunCallbacks
    {
        public UnityEvent<double> OnGameStartedRPC = new();
        
        [SerializeField] private GameContext _gameContext;
        
        [Header("View")]
        [SerializeField] private GameSearchView _gameSearchView;
        
        [Header("Configuration")]
        [SerializeField] private int _maxPlayers;
        [SerializeField] private float _gameDuration;
        
        private bool _wantsToFindGame;
        
        public void EnableView(bool enable)
        {
            _gameSearchView.gameObject.SetActive(enable);
        }
        
        #region PunCallbacks

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
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayers)
            {
                TryStartGameAsMasterClient();
            }
            
            SetStatusText($"You entered to room. Players count: {PhotonNetwork.CurrentRoom.PlayerCount}/{_maxPlayers}");
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            var roomOptions = new RoomOptions { MaxPlayers = _maxPlayers, IsOpen = true, IsVisible = true };
            var roomName = "ColorGame_" + Guid.NewGuid().ToString("N");
            
            PhotonNetwork.CreateRoom(roomName, roomOptions);
            SetStatusText($"The room not founded. Creating a new room...");
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayers)
            {
                TryStartGameAsMasterClient();
            }
            
            SetStatusText($"Player connected. Players count: {PhotonNetwork.CurrentRoom.PlayerCount}/{_maxPlayers}");
        }

        #endregion
        
        private void Awake()
        {
            _gameSearchView.SearchButton.onClick.AddListener(OnSearchButtonClicked);
        }

        private void OnDestroy()
        {
            _gameSearchView.SearchButton.onClick.RemoveListener(OnSearchButtonClicked);
        }
        
        private void TryStartGameAsMasterClient()
        {
            if (!PhotonNetwork.IsMasterClient || _gameContext.IsGameStarted)
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
            _gameSearchView.SearchButton.interactable = false;
            OnGameStartedRPC?.Invoke(endTime);
        }

        private void SetStatusText(string text)
        {
            _gameSearchView.StatusText.text = text;
        }
        
        private void OnSearchButtonClicked()
        {
            _wantsToFindGame = true;
            _gameSearchView.SearchButton.interactable = false;
            
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomRoom();
                SetStatusText("Connected to Photon. Searching for game...");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            
            SetStatusText("Trying to connect to Photon...");
        }
    }
}