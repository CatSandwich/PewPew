using System.Collections.Generic;
using Player.Upgrades;
using UnityEngine;

namespace Singletons
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Upgrades")] 
        public Upgrade<int> Multishot;
        public Upgrade<float> BulletDamage;
        public Upgrade<float> BulletSpeed;
        public Upgrade<int> BulletPierce;
        public Upgrade<int> HomingMissiles;
        public Upgrade<float> HomingMissileAccuracy;
        public IEnumerable<Upgrade> Upgrades => new List<Upgrade>
        {
            Multishot,
            BulletDamage,
            BulletSpeed,
            BulletPierce,
            HomingMissiles,
            HomingMissileAccuracy
        };
        public int Coins
        {
            get => PlayerPrefs.GetInt("coins", 0);
            set => PlayerPrefs.SetInt("coins", value);
        }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
