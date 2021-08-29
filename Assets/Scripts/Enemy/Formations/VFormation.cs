using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu(menuName = "Enemies/Formations/VFormation")]
    // ReSharper disable once UnusedMember.Global
    public class VFormation : AbstractFormation
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
            }
            ResetFormation();
        }

        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
        public override WaveEnemyData[] GetEnemies() => Enemies;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.VFormation;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies()
        {
            var i = 0;
            var row = 0;
            while (i < _enemies.Length)
            {
                if (i == 0)
                {
                    yield return new[] { new EnemyFormationPlacement(_enemies[i], _spawnOffset) };
                }
                else if (i == _enemies.Length - 1)
                {
                    yield return new[] { new EnemyFormationPlacement(_enemies[i], _spawnOffset - new Vector2(Spread * row, 0f)) };
                }
                else
                {
                    yield return new[]
                    {
                        new EnemyFormationPlacement(_enemies[i]    , _spawnOffset - new Vector2(Spread * row, 0f)),
                        new EnemyFormationPlacement(_enemies[(i + 1) % _enemies.Length], _spawnOffset + new Vector2(Spread * row, 0f))
                    };
                    i++;
                }
                i++;
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