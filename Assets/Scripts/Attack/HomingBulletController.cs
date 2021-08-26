using System.Linq;
using Enemy;
using UnityEngine;

namespace Attack
{
    public class HomingBulletController : Bullet
    {
        public override float Damage => Upgrades.HomingBulletDamage;
        protected override float Speed => Upgrades.HomingBulletSpeed;
        private static float Accuracy => Upgrades.HomingBulletAccuracy;
        private Transform _target;
        
        public new static void Instantiate(Vector3 position, Vector3 direction)
        {
            var go = Instantiate(Assets.Instance.HomingBullet);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.Euler(direction);
        }
        
        private void _retarget()
        {
            _target = WaveController.Instance.CurrentEnemies
                .OrderBy(enemy => (transform.position - enemy.transform.position).magnitude)
                .FirstOrDefault()?.transform;
        }
        
        public new void Update()
        {
            base.Update();
            _retarget();
            if (!_target) return;
            var vector = _target.position - transform.position;
            var targetAngle = _radToDeg(Mathf.Atan2(vector.y, vector.x));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), Accuracy);
        }

        private static float _radToDeg(float radians) => radians * 180f / Mathf.PI;
    }
}
