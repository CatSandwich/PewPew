using System.Linq;
using Attack.Interfaces;
using Enemy;
using Player;
using Singletons;
using UnityEngine;

namespace Attack
{
    public abstract class AttackBase : MonoBehaviour, IAttack
    {        
        protected static GameObject Parent => _parent ? _parent : _parent = new GameObject("Bullet Parent");
        private static GameObject _parent;
        protected static GameManager Manager => GameManager.Instance;
        public abstract float Damage { get; }
        public virtual bool ValidateHit(EnemyScript enemy) => true;
        public abstract void OnHit(EnemyScript enemy);

        public Transform GetClosestEnemy()
        {
            // This is incredibly inefficient but so far no issues
            return WaveController.Instance.GetCurrentEnemies()
                    .Where(enemy => enemy.GetComponentInChildren<SpriteRenderer>() && enemy.GetComponentInChildren<SpriteRenderer>().isVisible)
                    .OrderBy(enemy => (transform.position - enemy.transform.position).magnitude)
                    .FirstOrDefault()?.transform;
        }
    }
}