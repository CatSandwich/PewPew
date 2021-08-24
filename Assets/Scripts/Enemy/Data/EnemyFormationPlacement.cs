using UnityEngine;

namespace Enemy.Data
{
    public struct EnemyFormationPlacement
    {
        public readonly EnemyBase Enemy;
        public readonly Vector2 Offset;

        public EnemyFormationPlacement(EnemyBase enemy) : this()
        {
            Enemy = enemy;
            Offset = Vector2.zero;
        }

        public EnemyFormationPlacement(EnemyBase enemy, Vector2 offset)
        {
            Enemy = enemy;
            Offset = offset;
        }
    }
}