using Attack.Interfaces;
using Enemy;
using Player;
using UnityEngine;

namespace Attack
{
    public abstract class AttackBase : MonoBehaviour, IAttack
    {        
        protected static GameObject Parent => _parent ??= new GameObject("Bullet Parent");
        private static GameObject _parent;
        protected static PlayerUpgrades Upgrades => PlayerController.Instance.Upgrades;
        public abstract float Damage { get; }
        public virtual bool ValidateHit(EnemyScript enemy) => true;
        public abstract void OnHit(EnemyScript enemy);
    }
}