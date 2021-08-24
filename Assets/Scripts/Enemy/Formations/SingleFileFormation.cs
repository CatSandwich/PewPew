using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu()]
    // ReSharper disable once UnusedMember.Global
    public class SingleFileFormation : AbstractFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;

        public EnemyBase[] Enemies;
        public float Speed;
        public float Spacing;
        public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;
        public int Count;

        private bool _initialized;
        private EnemyBase[] _enemies;
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
                _enemies = new EnemyBase[Count];
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
        public override EnemyBase[] GetEnemies() => Enemies;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.SingleFile;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies() => _enemies.Select(enemy => new[] { new EnemyFormationPlacement(enemy, _spawnOffset) });
        public override float GetSpacing() => Spacing;
        public override float GetSpeed() => Speed;
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override void ResetFormation()
        {
            _spawnOffset = new Vector2(Random.Range(WaveController.LeftBounds, WaveController.RightBounds), 0f);
        }
    }
}