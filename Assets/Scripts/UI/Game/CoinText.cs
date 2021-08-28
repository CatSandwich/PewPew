using System;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Game
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        public void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void Update()
        {
            _text.text = $"Coins: {GameManager.Instance.Coins}";
        }
    }
}
