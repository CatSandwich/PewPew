using Enemy;
using UnityEngine;

namespace Attack
{
    public class Bullet : AttackBase
    {
        public override float Damage => Upgrades.BaseBulletDamage;
        protected virtual float Speed => Upgrades.BaseBulletSpeed;
        private int StartingPierce => Upgrades.BulletPierce;
        private int _pierce;

        public static void Instantiate(Vector3 position, Vector3 rotation)
        {
            var go = Instantiate(Assets.Instance.Bullet);
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