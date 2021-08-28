using System.Collections;
using System.Linq;
using Attack.Interfaces;
using Enemy;
using Player;
using UnityEngine;
using Singletons;

namespace Attack
{
    public class ElectricFieldController : CooldownAttackBase
    {
        protected override float Lifetime => Manager.ElectricFieldLifetime.Value;
        private static float Speed => Manager.ElectricFieldSpeed.Value;
        public override float Damage => Manager.ElectricFieldDamage.Value;
        protected override float Cooldown => Manager.ElectricFieldAttackRate.Value;
        
        private Transform _target;

        public static void Instantiate(Vector3 position)
        {
            var go = Instantiate(Assets.Instance.ElectricField);
            go.transform.position = new Vector3(position.x, position.y, 0);
        }
        
        void Update()
        {
            _target = GetClosestEnemy();
            if (!_target) return;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
        }
    }
}
