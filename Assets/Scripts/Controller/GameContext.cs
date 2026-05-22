using UnityEngine;

namespace Controller
{
    public class GameContext : MonoBehaviour
    {
        [SerializeField] private SearchGamePhotonController _searchGamePhotonController;
        [SerializeField] private GamePhotonController _gamePhotonController;
        
        private void Awake()
        {
            _searchGamePhotonController.OnGameStartedRPC.AddListener(OnGameStartedRPC);
        }
        
        private void OnGameStartedRPC(double gameEndTime)
        {
            _searchGamePhotonController.EnableView(false);
            
            _gamePhotonController.EnableView(true);
            _gamePhotonController.InitializeView(gameEndTime);
        }
    }
}