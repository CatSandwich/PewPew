using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu()]
    public class DoubleFileFormation : GenericFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;
        
        public override GenericEnemy[] GetEnemies() => Enemies;
        public GenericEnemy[] Enemies;
        public float Speed;
        public float Spacing;
        public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;
        public int Count;

        public float RankGap = 1f;

        private bool _initialized;
        private GenericEnemy[] _enemies;
        public bool RandomizeSpawnX;
        private Vector2 _spawnOffset;

        public override void Initialize()
        {
#if UNITY_EDITOR
            // Otherwise we only initialize once when first created,
            // so code changes to the next block won't be called properly
            _initialized = false;
#endif
            if (!_initialized)
            {
                _enemies = new GenericEnemy[Count];
                for (var i = 0; i < Count; i++)
                {
                    _enemies[i] = Enemies[i % Enemies.Length];
                }
                _initialized = true;
            }
            ResetFormation();
        }

        public override void ResetFormation()
        {
            _spawnOffset = (RandomizeSpawnX) ? new Vector2(Random.Range(WaveController.LeftBounds + (RankGap / 2f), WaveController.RightBounds - (RankGap / 2f)), 0f)
                                             : Vector2.zero;
        }

        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies() => _enemies.Select(enemy => new[] { new EnemyFormationPlacement(enemy, new Vector2(_spawnOffset.x - (RankGap / 2f), 0f)),
                                                                                                                    new EnemyFormationPlacement(enemy, new Vector2(_spawnOffset.x + (RankGap / 2f), 0f)) });
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.SingleFile;

        public override float GetSpeed() => Speed;
        public override float GetSpacing() => Spacing;
        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
    }
}