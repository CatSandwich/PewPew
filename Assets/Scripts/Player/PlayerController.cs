using Attack;
using Singletons;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        
        public GameManager Manager => GameManager.Instance;
        private readonly Vector3 UpRight = new Vector3(0f, 0f, 45f);
        private readonly Vector3 UpLeft = new Vector3(0f, 0f, -45f);

        private float _percentToShot = 0f;
        private float _percentToField = 0f;
        private float _percentToRay = 0f;
        
        // Singleton
        void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            // Move player to mouse x position
            var mouseX = Mathf.Clamp(Input.mousePosition.x, 0f, Screen.width);
            var x = Camera.main.ScreenToWorldPoint(Vector3.right * mouseX).x;
            transform.position = new Vector3(x, 0, 0);

            _percentToShot += Time.deltaTime * Manager.BulletRate.Value;
            if (TrySubtract(ref _percentToShot, 1f)) _shoot();
            
            _percentToField += Time.deltaTime * Manager.ElectricFieldRate.Value;
            if (TrySubtract(ref _percentToField, 1f)) _shootField();
            
            _percentToRay += Time.deltaTime * Manager.BulletRate.Value;
            _percentToRay = Mathf.Clamp(_percentToRay, 0f, 1f);
            if (Input.GetKeyDown(KeyCode.Space) && TrySubtract(ref _percentToRay, 1f))
            {
                RayController.Instantiate(1);
                RayController.Instantiate(-1);
            }
            
            #region Debug 
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
                Manager.Multishot.Debug();
            if (Input.GetKeyDown(KeyCode.P))
                Manager.BulletPierce.Debug();
            if (Input.GetKeyDown(KeyCode.H))
                Manager.HomingMissiles.Debug();
            #endif
            #endregion
        }

        private void _shoot()
        {
            var position = transform.position;
            foreach (var angle in Manager.Multishot.Value) 
                Bullet.Instantiate(position, Vector3.forward * angle);
            foreach (var angle in Manager.HomingMissiles.Value) 
                HomingBulletController.Instantiate(position, Vector3.forward * angle);
        }

        private void _shootField()
        {
            ElectricFieldController.Instantiate(transform.position);
        }

        static bool TrySubtract(ref int num, int remove, int min = 0)
        {
            if (num - remove < min) return false;
            num -= remove;
            return true;
        }

        static bool TrySubtract(ref float num, float remove, float min = 0f)
        {
            if (num - remove < min) return false;
            num -= remove;
            return true;
        }
    }
}
