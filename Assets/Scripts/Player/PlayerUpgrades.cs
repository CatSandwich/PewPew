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
        private const string HOMING_MISSILES_KEY = "homing_missiles";

        public bool TripleShot
        {
            get => _tripleShot;
            set => SetBool(TRIPLE_SHOT_KEY, _tripleShot = value);
        }
        private bool _tripleShot;
        
        public bool HomingMissiles
        {
            get => _homingMissiles;
            set => SetBool(HOMING_MISSILES_KEY, _homingMissiles = value);
        }
        private bool _homingMissiles;
        #endregion

        public static PlayerUpgrades Load()
        {
            return new PlayerUpgrades
            {
                _tripleShot = GetBool(TRIPLE_SHOT_KEY),
                _homingMissiles = GetBool(HOMING_MISSILES_KEY)
            };
        }

        private static bool GetBool(string key) => PlayerPrefs.GetInt(key) == 1;
        private static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}
