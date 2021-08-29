using System.Collections.Generic;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu(menuName = "Enemies/Formations/BlockFormation")]
    // ReSharper disable once UnusedMember.Global
    public class BlockFormation : AbstractFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;

        public WaveEnemyData[] Enemies;
        public float Speed = 1;
        public float Spacing = 1;
        public float Spread = 0.5f;

        [Min(0)] public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;

        [Min(1)]
        public int Width;
        [Min(1)]
        public int Height;

        private bool _initialized;
        private WaveEnemyData[] _enemies;
        private Vector2 _spawnOffset;
        private float _halfWidth;

        public override void Initialize()
        {
#if UNITY_EDITOR
            // Otherwise we only initialize once when first created,
            // so code changes to the next block won't be called properly
            _initialized = false;
#endif
            if (!_initialized)
            {
                var count = Width * Height;
                _enemies = new WaveEnemyData[count];
                for (var i = 0; i < count; i++)
                {
                    _enemies[i] = Enemies[i % Enemies.Length];
                }
                _initialized = true;
                _halfWidth = (Width * Spread) / 2f;
            }
            ResetFormation();
        }

        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
        public override WaveEnemyData[] GetEnemies() => Enemies;
        public override EnemyFormationType GetFormationType() => EnemyFormationType.Block;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies()
        {
            for (var i = 0; i < Height; i++)
            {
                var enemies = new EnemyFormationPlacement[Width];
                for (var j = 0; j < Width; j++)
                {
                    enemies[j] = new EnemyFormationPlacement(_enemies[j % _enemies.Length], _spawnOffset + new Vector2(-_halfWidth + (j * Spread), 0f));
                }
                yield return enemies;
            }
        }

        public override float GetSpacing() => Spacing;
        public override float GetSpeed() => Speed;
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override void ResetFormation()
        {
            _spawnOffset = new Vector2(Random.Range(WaveController.LeftBounds + _halfWidth, WaveController.RightBounds - _halfWidth), 0f);
        }
    }
}