using System.Collections.Generic;
using System.Linq;
using Player.Upgrades;
using UI;
using UnityEngine;
using Upgrade = Player.Upgrades.Upgrade;

namespace Singletons
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Upgrades")] 
        public Upgrade<float[]> Multishot;
        public Upgrade<float> BulletDamage;
        public Upgrade<float> BulletSpeed;
        public Upgrade<int> BulletPierce;
        public Upgrade<float> BulletRate;
        public Upgrade<float[]> HomingMissiles;
        public Upgrade<float> HomingMissileAccuracy;
        public Upgrade<float> ElectricFieldRate;
        public Upgrade<float> ElectricFieldLifetime;
        public Upgrade<float> ElectricFieldSpeed;
        public Upgrade<float> ElectricFieldDamage;
        public Upgrade<float> ElectricFieldAttackRate;
        public Upgrade<float> RayCooldown;
        public Upgrade<float> RayAttackRate;
        public Upgrade<float> RayDamage;

        public IEnumerable<UpgradeGroup> UpgradeGroups => new[]
        {
            new UpgradeGroup("Bullets & Missiles")
            {
                BulletRate,
                BulletDamage,
                BulletSpeed,
                BulletPierce,
                Multishot,
                HomingMissiles,
                HomingMissileAccuracy
            },
            new UpgradeGroup("Electric Field")
            {
                ElectricFieldRate,
                ElectricFieldDamage,
                ElectricFieldAttackRate,
                ElectricFieldSpeed,
                ElectricFieldLifetime
            },
            new UpgradeGroup("Electric Rays")
            {
                RayCooldown,
                RayDamage,
                RayAttackRate
            }
        };
        public IEnumerable<Upgrade> Upgrades => UpgradeGroups.SelectMany(group => group.Upgrades);
        
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
