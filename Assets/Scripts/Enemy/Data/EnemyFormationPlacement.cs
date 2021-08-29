using UnityEngine;

namespace Enemy.Data
{
    public struct EnemyFormationPlacement
    {
        public readonly WaveEnemyData Enemy;
        public readonly Vector2 Offset;

        public EnemyFormationPlacement(WaveEnemyData enemy) : this()
        {
            Enemy = enemy;
            Offset = Vector2.zero;
        }

        public EnemyFormationPlacement(WaveEnemyData enemy, Vector2 offset)
        {
            Enemy = enemy;
            Offset = offset;
        }
    }
}