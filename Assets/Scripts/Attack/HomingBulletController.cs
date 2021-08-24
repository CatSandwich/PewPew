using System.Linq;
using Enemy;
using UnityEngine;

namespace Attack
{
    public class HomingBulletController : BulletController
    {
        private Transform _target;
        public float Accuracy;
        
        public new static void Instantiate(Vector3 position, Vector2 direction)
        {
            var go = Instantiate(Assets.Instance.HomingBullet);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        
        private void _retarget()
        {
            _target = WaveController.Instance.GetCurrentEnemies()
                .OrderBy(enemy => (transform.position - enemy.transform.position).magnitude)
                .FirstOrDefault()?.transform;
        }
        
        public new void Update()
        {
            base.Update();
            _retarget();
            if (_target == null) return;

            var vector = _target.position - transform.position;
            var targetAngle = _radToDeg(Mathf.Atan2(vector.y, vector.x));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), Accuracy);
        }

        private static float _radToDeg(float radians) => radians * 180f / Mathf.PI;
    }
}
