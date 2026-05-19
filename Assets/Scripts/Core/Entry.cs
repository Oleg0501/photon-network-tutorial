using Controller;
using UI;
using UnityEngine;

namespace Core
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private SearchGameView _searchGameView;
        [SerializeField] private GameView _gameView;
        
        private void Awake()
        {
            var searchGameController = new SearchGameController(_searchGameView);
            var gameController = new GameController(_gameView);
        }
    }
}