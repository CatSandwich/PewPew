using System;
using System.Collections;
using System.Collections.Generic;
using Attack.Interfaces;
using Enemy.Data;
using Enemy.Movement;
using Singletons;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : MonoBehaviour, IPoolable, IChildCollisionHandler
    {

        /// <summary> The number of seconds this Enemy has been alive, in game time. </summary>
        public float LifeTime => Time.time - SpawnTime;

        public int ModelId;
        public GameObject Model;

        /// <summary> Called when this Unit is Destroyed by any means. </summary>
        public event Action<EnemyScript> Destroyed = script => { };
        
        /// <summary> The Wave Type of the Wave this Unit belongs to. </summary>
        public EnemyFormationWaveType WaveType;
        /// <summary> The Behaviour which controls this Unit. </summary>
        public AbstractBehaviour Behaviour;
        /// <summary> The Speed at which this Unit will travel. </summary>
        public float Speed;
        /// <summary>
        /// The position in World Space where this Unit spawned. <br/>
        /// This is usually off screen.
        /// </summary>
        public Vector3 SpawnPoint;
        /// <summary> The time at which this Unit spawned, in game time. </summary>
        public float SpawnTime;
        /// <summary> The time at which this Unit last preformed a non-movement Action, in game time. </summary>
        public float ActionTime;
        /// <summary> The Wave in which this Unit spawned. </summary>
        public int Wave;
        /// <summary> The Unique ID in the Wave in which this Unit spawned. </summary>
        public int WaveId;
        /// <summary> The current Movement Phase of this Unit. This is not used by all Behaviours. </summary>
        public int MovementPhase;
        /// <summary> The current Action Phase of this Unit. This is not used by all Behaviours. </summary>
        public int ActionPhase;
        /// <summary> The maximum Health of this unit. </summary>
        public float MaxHealth;
        /// <summary> The current Health of this unit. </summary>
        public float Health;

        /// <summary>
        /// True if this Unit was killed by the player. <br/>
        /// Used to determine if a Unit was killed by the player, or destroyed by the environment.
        /// </summary>
        public bool WasKilled;
        
        private void Start()
        {
            SpawnTime = Time.time;
            ActionTime = Time.time;
        }

        private void Update()
        {
            Behaviour.DoBehaviour(this);
        }

        /// <summary>
        /// Causes this Unit to take damage, potentially killing it.
        /// </summary>
        public void TakeDamage(float damage, bool sourceIsPlayer = true)
        {
            if (!WaveController.RunIsAlive) return;
            Health -= damage;
            if (Health > 0) return;

            WasKilled = sourceIsPlayer;
            
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
        
        public void OnChildTriggerEnter2D(Collider2D col)
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
                TakeDamage(attack.Damage);
                attack.OnHit(this);
            }
        }

        public void OnActivate()
        {
        }

        public void OnDeactivate()
        {
        }

        public void OnReset()
        {
        }
    }
}
