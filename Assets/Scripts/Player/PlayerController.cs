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
        
        // Start is called before the first frame update
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
            var mouseX = Mathf.Clamp(Input.mousePosition.x, 0f, Screen.width);
            var x = Camera.main.ScreenToWorldPoint(Vector3.right * mouseX).x;
            transform.position = new Vector3(x, 0, 0);

            _percentToShot += Time.deltaTime;
            if (_percentToShot >= 1f)
            {
                _percentToShot -= 1f;
                _shoot();
            }

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
                Manager.Multishot.Debug();
            if (Input.GetKeyDown(KeyCode.P))
                Manager.BulletPierce.Debug();
            if (Input.GetKeyDown(KeyCode.H))
                Manager.HomingMissiles.Debug();
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
            
            if (Manager.Multishot.Value > 1)
            {
                Bullet.Instantiate(position, UpRight);
                Bullet.Instantiate(position, UpLeft);
            }

            if (Manager.HomingMissiles.Value > 0)
            {
                HomingBulletController.Instantiate(position, UpRight);
                HomingBulletController.Instantiate(position, UpLeft);
            }
        }
    }
}
