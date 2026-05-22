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
        }
        
        private void OnGameStartedRPC(double gameEndTime)
        {
            GameEndTime = gameEndTime;
            IsGameStarted = true;
            
            _gameSearchPhotonController.EnableView(false);
            _gamePhotonController.EnableView(true);
            _gamePhotonController.InitializeView();
        }
    }
}