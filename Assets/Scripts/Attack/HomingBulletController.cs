using System.Linq;
using Enemy;
using Singletons;
using UnityEngine;

namespace Attack
{
    public class HomingBulletController : Bullet
    {
        private static float Accuracy => Manager.HomingMissileAccuracy.Value;
        private Transform _target;
        
        public new static void Instantiate(Vector3 position, Vector3 direction)
        {
            var go = Object.Instantiate(Assets.Instance.HomingBullet, Parent.transform, true);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.Euler(direction);
        }
        
        public new void Update()
        {
            base.Update();
            _target = GetClosestEnemy();
            if (!_target) return;
            var vector = _target.position - transform.position;
            var targetAngle = _radToDeg(Mathf.Atan2(vector.y, vector.x));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), Accuracy);
        }

        private static float _radToDeg(float radians) => radians * 180f / Mathf.PI;
    }
}
