using UnityEngine;

namespace Enemy.Data
{
    public struct EnemyFormationPlacement
    {
        public readonly WaveEnemyData Enemy;
        public readonly Vector2 Offset;
        public readonly EnemyFormationSpawnPosition SpawnPosition;

        public EnemyFormationPlacement(WaveEnemyData enemy) : this()
        {
            Enemy = enemy;
            Offset = Vector2.zero;
            SpawnPosition = EnemyFormationSpawnPosition.Top;
        }

        public EnemyFormationPlacement(WaveEnemyData enemy, Vector2 offset, EnemyFormationSpawnPosition position = EnemyFormationSpawnPosition.Top)
        {
            Enemy = enemy;
            Offset = offset;
            SpawnPosition = position;
        }
    }

    public enum EnemyFormationSpawnPosition
    {
        Top,
        Left,
        Right
    }
}