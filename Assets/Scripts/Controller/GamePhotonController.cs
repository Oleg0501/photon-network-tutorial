using Photon.Pun;
using UI;
using UnityEngine;

namespace Controller
{
    public class GamePhotonController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameView _gameView;

        private double _gameEndTime;
        
        private void Awake()
        {
            _gameView.GameButton.onClick.AddListener(OnGameButtonClicked);
        }
        
        private void Update()
        {
            var remainingTime = Mathf.Max(0f, (float)(_gameEndTime - PhotonNetwork.Time));
            _gameView.SetTimeText(remainingTime.ToString("F1"));

            if (remainingTime <= 0f)
            {
                EndGameAndLeaveRoom();
            }
        }
        
        private void OnGameButtonClicked()
        {
            var playerColor = _gameView.GetLocalPlayerColor();
            photonView.RPC(nameof(SetImageColorRPC), RpcTarget.All, playerColor.r, playerColor.g, playerColor.b);
        }

        public void EnableView(bool enable)
        {
            _gameView.gameObject.SetActive(enable);
        }

        public void InitializeView(double gameEndTime)
        {
            _gameEndTime = gameEndTime;
            _gameView.StartGame();
            SetStatusText("Game is started");
        }
        
        private void EndGameAndLeaveRoom()
        {
            // if (isLeavingRoom)
            //     return;

            // isLeavingRoom = true;
            // gameStarted = false;

            SetStatusText("Game is complete");

            _gameView.GameButton.interactable = false;
            _gameView.SetTimeText("0.0");
            
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        
        [PunRPC]
        private void SetImageColorRPC(float r, float g, float b)
        {
            _gameView.SetImageColor(new Color(r, g, b, 1f));
        }
        
        private void SetStatusText(string text)
        {
            _gameView.StatusText.text = text;
        }
    }
}