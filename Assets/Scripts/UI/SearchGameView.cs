using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SearchGameView : MonoBehaviour
    {
        [SerializeField] private Button _searchButton;
        
        public Button SearchButton => _searchButton;
    }
}