using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Upgrade : MonoBehaviour
    {
        public Player.Upgrades.Upgrade UpgradeScriptable;
        public TextMeshProUGUI Name;
        public Button UpgradeButton;
        public TextMeshProUGUI UpgradeCost;

        void Start()
        {
            Refresh();
        }

        void Refresh()
        {
            Name.text = UpgradeScriptable.Name;
            if (!UpgradeScriptable.Next)
            {
                UpgradeButton.enabled = false;
                UpgradeCost.text = "Maxed";
            }
            else
            {
                UpgradeCost.text = UpgradeScriptable.Next.Cost.ToString();
                UpgradeButton.onClick.AddListener(TryUpgrade);
            }
        }

        void TryUpgrade()
        {
            if (GameManager.Instance.Coins < UpgradeScriptable.Next.Cost) return;
            GameManager.Instance.Coins -= UpgradeScriptable.Next.Cost;
            UpgradeScriptable.ActiveIndex++;
            UpgradeButton.onClick.RemoveAllListeners();
            Refresh();
        }
    }
}
