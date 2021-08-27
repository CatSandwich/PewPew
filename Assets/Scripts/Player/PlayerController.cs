using Attack;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        
        public PlayerUpgrades Upgrades;
        private readonly Vector3 UpRight = new Vector3(0f, 0f, 45f);
        private readonly Vector3 UpLeft = new Vector3(0f, 0f, -45f);

        private float _percentToShot = 0f;
        
        // Start is called before the first frame update
        void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Upgrades = PlayerUpgrades.Load();
        }

        // Update is called once per frame
        void Update()
        {
            var mouseX = Mathf.Clamp(Input.mousePosition.x, 0f, Screen.width);
            var x = Camera.main.ScreenToWorldPoint(Vector3.right * mouseX).x;
            transform.position = new Vector3(x, 0, 0);

            _percentToShot += Upgrades.AttackSpeed * Time.deltaTime;
            if (_percentToShot >= 1f)
            {
                _percentToShot -= 1f;
                _shoot();
            }

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
                Upgrades.TripleShot = !Upgrades.TripleShot;
            if (Input.GetKeyDown(KeyCode.H))
                Upgrades.HomingMissiles = !Upgrades.HomingMissiles;
            if (Input.GetKeyDown(KeyCode.E))
                Upgrades.ElectricField = !Upgrades.ElectricField;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RayController.Instantiate(1);
                RayController.Instantiate(-1);
            }
            #endif
        }

        private void _shoot()
        {
            var position = transform.position;
            Bullet.Instantiate(position, Vector3.up);
            
            if (Upgrades.TripleShot)
            {
                Bullet.Instantiate(position, UpRight);
                Bullet.Instantiate(position, UpLeft);
            }

            if (Upgrades.HomingMissiles)
            {
                HomingBulletController.Instantiate(position, UpRight);
                HomingBulletController.Instantiate(position, UpLeft);
            }

            if (Upgrades.ElectricField)
            {
                ElectricFieldController.Instantiate(position);
            }
        }
    }
}
