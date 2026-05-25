using UnityEngine;

namespace Controller
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private GameSearchPhotonController _gameSearchPhotonController;
        [SerializeField] private GamePhotonController _gamePhotonController;
        
        public double GameEndTime { get; private set; }
        public bool IsGameStarted { get; private set; }
        
        private void Awake()
        {
            _gameSearchPhotonController.OnGameStartedRPC.AddListener(OnGameStartedRPC);
            _gameSearchPhotonController.OnLeaveRoom.AddListener(OnLeaveRoom);
            _gamePhotonController.OnLeaveRoom.AddListener(OnLeaveRoom);

            InitializeSearch();
        }

        private void OnDestroy()
        {
            _gameSearchPhotonController.OnGameStartedRPC.RemoveListener(OnGameStartedRPC);
            _gameSearchPhotonController.OnLeaveRoom.RemoveListener(OnLeaveRoom);
            _gamePhotonController.OnLeaveRoom.RemoveListener(OnLeaveRoom);
        }

        private void InitializeSearch()
        {
            _gameSearchPhotonController.EnableView(true);
            _gameSearchPhotonController.InitializeView();
            _gamePhotonController.EnableView(false);
        }
        
        private void InitializeGame()
        {
            _gameSearchPhotonController.EnableView(false);
            _gamePhotonController.EnableView(true);
            _gamePhotonController.InitializeView();
        }
        
        private void OnGameStartedRPC(double gameEndTime)
        {
            GameEndTime = gameEndTime;
            IsGameStarted = true;
            InitializeGame();
        }

        private void OnLeaveRoom()
        {
            IsGameStarted = false;
            InitializeSearch();
        }
    }
}