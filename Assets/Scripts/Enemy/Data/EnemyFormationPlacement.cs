using UnityEngine;

namespace Enemy.Data
{
    public struct EnemyFormationPlacement
    {
        public readonly GenericEnemy Enemy;
        public readonly Vector2 Offset;

        public EnemyFormationPlacement(GenericEnemy enemy) : this()
        {
            Enemy = enemy;
            Offset = Vector2.zero;
        }

        public EnemyFormationPlacement(GenericEnemy enemy, Vector2 offset)
        {
            Enemy = enemy;
            Offset = offset;
        }
    }
}