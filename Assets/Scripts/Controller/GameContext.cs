using UnityEngine;

namespace Controller
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private SearchGamePhotonController _searchGamePhotonController;
        [SerializeField] private GamePhotonController _gamePhotonController;
        
        public bool IsGameStarted { get; private set; }
        public double GameEndTime { get; private set; }
        
        private void Awake()
        {
            _searchGamePhotonController.OnGameStartedRPC.AddListener(OnGameStartedRPC);
        }
        
        private void OnGameStartedRPC(double gameEndTime)
        {
            GameEndTime = gameEndTime;
            IsGameStarted = true;
            
            _searchGamePhotonController.EnableView(false);
            
            _gamePhotonController.EnableView(true);
            _gamePhotonController.InitializeView(gameEndTime);
        }
    }
}