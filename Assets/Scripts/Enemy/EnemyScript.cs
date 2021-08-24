using System;
using System.Collections;
using System.Collections.Generic;
using Attack.Interfaces;
using Enemy.Movement;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : MonoBehaviour
    {

        /// <summary> The number of seconds this Enemy has been alive, in game time. </summary>
        public float LifeTime => Time.time - SpawnTime;

        public EnemyWaveType WaveType;
        public GenericMovement Movement;
        public float Speed;
        public Vector3 SpawnPoint;
        public float SpawnTime;
        public int Wave;
        public int WaveID;
        public int MovementPhase;
        public int AttackPhase;

        private readonly List<int> _cooldowns = new List<int>();
        
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
                case EnemyWaveType.Normal:
                    WaveController.Instance.OnNormalEnemyDestroyed(this);
                    break;
                case EnemyWaveType.Bonus:
                    WaveController.Instance.OnBonusEnemyDestroyed(this);
                    break;
                case EnemyWaveType.Boss:
                    WaveController.Instance.OnBossEnemyDestroyed(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                    Destroy(gameObject); // Remove when damage implemented
                    oneHit.Destroy();
                    break;
                }
                case ICooldownAttack cooldownAttack:
                {
                    if (_cooldowns.Contains(cooldownAttack.Id)) break;
                    Destroy(gameObject); // Remove when damage implemented
                    StartCoroutine(_cooldown(cooldownAttack.Id, cooldownAttack.Cooldown));
                    break;
                }
            }
        }

        private IEnumerator _cooldown(int id, float cooldown)
        {
            _cooldowns.Add(id);
            yield return new WaitForSeconds(cooldown);
            _cooldowns.Remove(id);
        }

        public enum EnemyWaveType
        {
            Normal,
            Bonus,
            Boss
        }
    }
}
