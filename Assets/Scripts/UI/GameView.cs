using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _gameButton;
        [SerializeField] private Image _gameImage;

        public Button GameButton => _gameButton;
        
        public void StartGame()
        {
            _gameButton.interactable = true;
            _gameImage.color = GetLocalPlayerColor();
        }

        public void SetImageColor(Color color)
        {
            _gameImage.color = color;
        }
        
        public Color GetLocalPlayerColor()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber == 1 ? Color.red : Color.blue;
        }
    }
}