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
        protected override float Lifetime => Upgrades.ElectricFieldLifetime;
        private static float Speed => Upgrades.ElectricFieldMoveSpeed;
        
        public override float Damage => 5;
        protected override float Cooldown => 0.5f;
        
        private Transform _target;

        public static void Instantiate(Vector3 position)
        {
            var go = Instantiate(Assets.Instance.ElectricField);
            go.transform.position = new Vector3(position.x, position.y, 0);
        }
        
        void Update()
        {
            _retarget();
            if (!_target) return;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
        }
        
        private void _retarget()
        {
            _target = WaveController.Instance.GetCurrentEnemies()
                .OrderBy(enemy => (transform.position - enemy.transform.position).magnitude)
                .FirstOrDefault()?.transform;
        }
    }
}
