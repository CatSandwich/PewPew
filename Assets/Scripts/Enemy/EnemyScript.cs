using System;
using System.Collections;
using System.Collections.Generic;
using Attack.Interfaces;
using Enemy.Data;
using Enemy.Movement;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : MonoBehaviour
    {

        /// <summary> The number of seconds this Enemy has been alive, in game time. </summary>
        public float LifeTime => Time.time - SpawnTime;

        public event Action<EnemyScript> Destroyed = script => { };
        
        public EnemyFormationWaveType WaveType;
        public GenericMovement Movement;
        public float Speed;
        public Vector3 SpawnPoint;
        public float SpawnTime;
        public int Wave;
        public int WaveID;
        public int MovementPhase;
        public int AttackPhase;

        public float MaxHealth;
        public float Health;

        public bool WasKilled;

        private readonly List<int> _cooldownList = new List<int>();
        
        private void Start()
        {
            SpawnTime = Time.time;
        }

        private void Update()
        {
            Movement.DoMovement(this);
        }

        private void OnDestroy()
        {
            if (WaveController.Instance == null) return;
            switch (WaveType)
            {
                case EnemyFormationWaveType.Normal:
                    WaveController.Instance.OnNormalEnemyDestroyed(this, WasKilled);
                    break;
                case EnemyFormationWaveType.Bonus:
                    WaveController.Instance.OnBonusEnemyDestroyed(this, WasKilled);
                    break;
                case EnemyFormationWaveType.Boss:
                    WaveController.Instance.OnBossEnemyDestroyed(this, WasKilled);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroyed(this);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!WaveController.RunIsAlive) return;
            if (col.gameObject.name.Trim() == "EndZone")
            {
                WaveController.Instance.OnEnemyHitsEndZone(this);
                return;
            }
            
            switch (col.GetComponent<IAttack>())
            {
                case null:
                {
                    break;
                }
                case IOneHitAttack oneHit:
                {
                    TakeDamage(oneHit.Damage);
                    oneHit.Destroy();
                    break;
                }
                case ICooldownAttack cooldownAttack:
                {
                    if (_cooldownList.Contains(cooldownAttack.Id)) break;
                    TakeDamage(cooldownAttack.Damage);
                    StartCoroutine(_cooldown(cooldownAttack.Id, cooldownAttack.Cooldown));
                    break;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health > 0) return;

            WasKilled = true;
            Destroy(gameObject);
        }

        private IEnumerator _cooldown(int id, float cooldown)
        {
            _cooldownList.Add(id);
            yield return new WaitForSeconds(cooldown);
            _cooldownList.Remove(id);
        }
    }
}
