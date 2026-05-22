using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameSearchView : MonoBehaviour
    {
        [SerializeField] private Button _searchButton;
        [SerializeField] private TextMeshProUGUI _statusText;
        
        public Button SearchButton => _searchButton;
        public TextMeshProUGUI StatusText => _statusText;
    }
}