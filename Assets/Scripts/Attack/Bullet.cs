using Enemy;
using Singletons;
using UnityEngine;

namespace Attack
{
    public class Bullet : AttackBase
    {
        public override float Damage => Manager.BulletDamage.Value;
        protected virtual float Speed => Manager.BulletSpeed.Value;
        private int StartingPierce => Manager.BulletPierce.Value;
        private int _pierce;

        public static void Instantiate(Vector3 position, Vector3 rotation)
        {
            var go = Instantiate(Assets.Instance.Bullet, Parent.transform, true);
            go.transform.position = position;
            go.transform.rotation = Quaternion.Euler(rotation);
        }
        
        public void Start()
        {
            _pierce = StartingPierce;
        }

        public void Update()
        {
            transform.Translate(Time.deltaTime * Speed * Vector3.up);
        }
        
        public override void OnHit(EnemyScript enemy)
        {
            _pierce--;
            if(_pierce == 0) Destroy(gameObject);
        }
    }
}