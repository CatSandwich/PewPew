using UnityEngine;

namespace Singletons
{
    public class Assets : MonoBehaviour
    {
        public static Assets Instance;
        public GameObject Bullet;
        public GameObject HomingBullet;
        public GameObject Ray;
        public GameObject ElectricField;

        public GameObject ScoreDrop;
        public GameObject CoinDrop;

        public Sprite Coin1;
        public Sprite Coin5;
        public Sprite Coin10;

        private void Start()
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
