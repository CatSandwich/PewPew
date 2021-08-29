using System;
using Attack.Interfaces;
using Enemy.Behaviours;
using Enemy.Interface;
using Pooling;
using Singletons;
using UnityEngine;

namespace Enemy
{
    public abstract class AbstractEnemyScript : MonoBehaviour, IPoolable, IChildCollisionHandler
    {

        /// <summary> The number of seconds this Enemy has been alive, in game time. </summary>
        public float LifeTime => Time.time - SpawnTime;

        public int ModelId;
        public GameObject Model;

        /// <summary> Called when this Unit is Destroyed by any means. </summary>
        public abstract event Action<AbstractEnemyScript> Destroyed;
        
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

        /// <summary> The maximum Health of this unit. </summary>
        public float MaxHealth;
        /// <summary> The current Health of this unit. </summary>
        public float Health;

        /// <summary>
        /// True if this Unit was killed by the player. <br/>
        /// Used to determine if a Unit was killed by the player, or destroyed by the environment.
        /// </summary>
        public bool WasKilled;

        private void Update()
        {
            Behaviour.DoBehaviour(this);
        }

        /// <summary>
        /// Causes this Unit to take damage, potentially killing it.
        /// </summary>
        public abstract void TakeDamage(float damage, bool sourceIsPlayer = true);
        
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

        public void OnActivate() { }
        public void OnDeactivate() { }
        public void OnReset() { }
    }
}
