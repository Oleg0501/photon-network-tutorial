using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    [RequireComponent(typeof(PhotonView))]
    public class GamePhotonController : MonoBehaviourPunCallbacks
    {
        public UnityEvent OnLeaveRoom = new();
        
        [SerializeField] private GameContext _gameContext;
        
        [Header("View")]
        [SerializeField] private GameView _gameView;
        
        public void EnableView(bool enable)
        {
            _gameView.gameObject.SetActive(enable);
        }
        
        public void InitializeView()
        {
            _gameView.StartGame();
            SetStatusText("Game is started");
        }
        
        public override void OnLeftRoom()
        {
            OnLeaveRoom?.Invoke();
        }
        
        private void Awake()
        {
            _gameView.GameButton.onClick.AddListener(OnGameButtonClicked);
            _gameView.CancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnDestroy()
        {
            _gameView.GameButton.onClick.RemoveListener(OnGameButtonClicked);
            _gameView.CancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void Update()
        {
            if (_gameContext.IsGameStarted == false)
            {
                return;
            }
            
            var remainingTime = Mathf.Max(0f, (float)(_gameContext.GameEndTime - PhotonNetwork.Time));
            _gameView.SetTimeText(remainingTime.ToString("F1"));

            if (remainingTime <= 0f)
            {
                EndGameAndLeaveRoom();
            }
        }
        
        private void EndGameAndLeaveRoom()
        {
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
        
        private void OnGameButtonClicked()
        {
            var playerColor = _gameView.GetLocalPlayerColor();
            photonView.RPC(nameof(SetImageColorRPC), RpcTarget.All, playerColor.r, playerColor.g, playerColor.b);
        }

        private void OnCancelButtonClicked()
        {
            EndGameAndLeaveRoom();
        }
    }
}