using System.Runtime.CompilerServices;
using UnityEngine;

namespace Attack
{
    public class HomingBulletController : BulletController
    {
        public Transform Target => Assets.Instance.Target.transform;
        public float Accuracy;
        
        public new static void Instantiate(Vector3 position, Vector2 direction)
        {
            var go = Instantiate(Assets.Instance.HomingBullet);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        public new void Update()
        {
            base.Update();
            var vector = Target.position - transform.position;
            var targetAngle = _radToDeg(Mathf.Atan2(vector.y, vector.x));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle - 90), 2);
        }

        private static float _radToDeg(float radians) => radians * 180f / Mathf.PI;
    }
}
