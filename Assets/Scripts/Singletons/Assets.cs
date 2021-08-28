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
