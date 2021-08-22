using UnityEngine;

namespace Player
{
    public class PlayerUpgrades
    {
        #region Calculations
        public float AttackSpeed => 1f;
        #endregion
        
        #region Upgrades
        private const string TRIPLE_SHOT_KEY = "triple_shot";
        
        public bool TripleShot;
        #endregion

        public void Save()
        {
            SetBool(TRIPLE_SHOT_KEY, TripleShot);
        }

        public static PlayerUpgrades Load()
        {
            return new PlayerUpgrades
            {
                TripleShot = GetBool(TRIPLE_SHOT_KEY)
            };
        }

        private static bool GetBool(string key) => PlayerPrefs.GetInt(key) == 1;
        private static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}
