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

        public event Action<EnemyScript> Destroyed = script => { };
        
        public EnemyWaveType WaveType;
        public GenericMovement Movement;
        public float Speed;
        public Vector3 SpawnPoint;
        public float SpawnTime;
        public int Wave;
        public int WaveID;
        public int MovementPhase;
        public int AttackPhase;
        
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

            var attack = col.GetComponent<IAttack>();
            if (attack != null && attack.ValidateHit(this))
            {
                Destroy(gameObject); // Take damage
                attack.OnHit(this);
            }
        }

        public enum EnemyWaveType
        {
            Normal,
            Bonus,
            Boss
        }
    }
}
