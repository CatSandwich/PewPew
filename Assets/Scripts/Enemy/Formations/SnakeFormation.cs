using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu(menuName = "Enemies/Formations/SnakeFormation")]
    // ReSharper disable once UnusedMember.Global
    public class SnakeFormation : AbstractFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;

        public WaveEnemyData[] Enemies;
        public float Speed = 1f;
        public float Spacing = 1f;
        [Min(0)] public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;
        [Min(0)] public int Count;

        private bool _initialized;
        private WaveEnemyData[] _enemies;
        private Vector2 _spawnOffset;
        private EnemyFormationSpawnPosition _spawnPosition;

        public override void Initialize()
        {
#if UNITY_EDITOR
            // Otherwise we only initialize once when first created,
            // so code changes to the next block won't be called properly
            _initialized = false;
#endif
            if (!_initialized)
            {
                _enemies = new WaveEnemyData[Count];
                for (var i = 0; i < Count; i++)
                {
                    _enemies[i] = Enemies[i % Enemies.Length];
                }
                _initialized = true;
            }
            ResetFormation();
        }

        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
        public override WaveEnemyData[] GetEnemies() => Enemies;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.Snake;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies() => _enemies.Select(enemy => new[] { new EnemyFormationPlacement(enemy, _spawnOffset, _spawnPosition) });
        public override float GetSpacing() => Spacing;
        public override float GetSpeed() => Speed;
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override void ResetFormation()
        {
            _spawnOffset = new Vector2(0f, -1);
            _spawnPosition = Random.Range(0, 2) > 0
                ? EnemyFormationSpawnPosition.Left
                : EnemyFormationSpawnPosition.Right;
        }
    }
}