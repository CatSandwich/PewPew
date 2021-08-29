using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Attack
{
    public abstract class CooldownAttackBase : AttackBase
    {
        protected abstract float Cooldown { get; }
        protected abstract float Lifetime { get; }
        
        private readonly List<int> _cooldowns = new List<int>();

        public void Start()
        {
            StartCoroutine(_life());
        }
        
        public override bool ValidateHit(AbstractEnemyScript enemy) => !_cooldowns.Contains(enemy.GetInstanceID());
        
        public override void OnHit(AbstractEnemyScript enemy)
        {
            StartCoroutine(_cooldown(enemy.GetInstanceID(), Cooldown));
        }

        private IEnumerator _cooldown(int id, float cooldown)
        {
            _cooldowns.Add(id);
            yield return new WaitForSeconds(cooldown);
            _cooldowns.Remove(id);
        }

        private IEnumerator _life()
        {
            yield return new WaitForSeconds(Lifetime);
            Destroy(gameObject);
        }
    }
}