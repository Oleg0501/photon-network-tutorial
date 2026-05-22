using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _gameButton;
        [SerializeField] private Image _gameImage;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _statusText;
        
        public Button GameButton => _gameButton;
        public TextMeshProUGUI StatusText => _statusText;
        
        public void StartGame()
        {
            _gameButton.interactable = true;
            _gameImage.color = GetLocalPlayerColor();
        }

        public void SetImageColor(Color color)
        {
            _gameImage.color = color;
        }

        public void SetTimeText(string text)
        {
            _timeText.text = text;
        }
        
        public Color GetLocalPlayerColor()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber == 1 ? Color.red : Color.blue;
        }
    }
}