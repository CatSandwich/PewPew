using Singletons;
using UnityEngine;

namespace UI
{
    public class ResetButton : MonoBehaviour
    {
        private static GameManager Manager => GameManager.Instance;
        public void Click()
        {
            foreach (var upgrade in Manager.Upgrades)
            {
                upgrade.ActiveIndex = 0;
            }
        }
    }
}
