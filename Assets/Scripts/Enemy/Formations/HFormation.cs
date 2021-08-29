using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu(menuName = "Enemies/Formations/HFormation")]
    // ReSharper disable once UnusedMember.Global
    public class HFormation : AbstractFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;

        public WaveEnemyData[] Enemies;
        public float Speed = 1;
        public float Spacing = 1;
        public float Spread = 0.5f;

        [Min(0)] public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;
        [Min(5)] public int Count = 5;

        private bool _initialized;
        private WaveEnemyData[] _enemies;
        private Vector2 _spawnOffset;
        private int _mid;

        private void OnValidate()
        {
            if (Count % 2 == 0) Count += 1;
        }

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
                _mid = (int)(Count / 4f);
            }
            ResetFormation();
        }

        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
        public override WaveEnemyData[] GetEnemies() => Enemies;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.HFormation;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies()
        {
            var i = 0;
            var row = 0;
            while (i < _enemies.Length)
            {
                if (_mid == row)
                {
                    yield return new[]
                    {
                        new EnemyFormationPlacement(_enemies[i                        ], _spawnOffset - new Vector2(Spread, 0f)),
                        new EnemyFormationPlacement(_enemies[(i + 1) % _enemies.Length], _spawnOffset),
                        new EnemyFormationPlacement(_enemies[(i + 2) % _enemies.Length], _spawnOffset + new Vector2(Spread, 0f))
                    };
                    i += 3;
                }
                else
                {
                    yield return new[]
                    {
                        new EnemyFormationPlacement(_enemies[i                        ], _spawnOffset - new Vector2(Spread, 0f)),
                        new EnemyFormationPlacement(_enemies[(i + 1) % _enemies.Length], _spawnOffset + new Vector2(Spread, 0f))
                    };
                    i += 2;
                }
                row++;
            }
        }

        public override float GetSpacing() => Spacing;
        public override float GetSpeed() => Speed;
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override void ResetFormation()
        {
            _spawnOffset = new Vector2(Random.Range(WaveController.LeftBounds, WaveController.RightBounds), 0f);
        }
    }
}