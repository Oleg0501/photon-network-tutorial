using System;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Controller
{
    public class GamePhotonController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameView _gameView;

        private void Awake()
        {
            _gameView.GameButton.onClick.AddListener(OnGameButtonClicked);
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

        public void InitializeView()
        {
            _gameView.StartGame();
        }
        
        [PunRPC]
        private void SetImageColorRPC(float r, float g, float b)
        {
            _gameView.SetImageColor(new Color(r, g, b, 1f));
        }
    }
}