using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Game
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        public void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void Update()
        {
            _text.text = $"Score: {ScoreKeeper.CurrentScore}";
        }
    }
}
