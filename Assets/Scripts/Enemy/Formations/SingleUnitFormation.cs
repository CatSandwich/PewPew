using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using Singletons;
using UnityEngine;

namespace Enemy.Formations
{
    [CreateAssetMenu(menuName = "Enemies/Formations/SingleUnitFormation")]
    // ReSharper disable once UnusedMember.Global
    public class SingleUnitFormation : AbstractFormation
    {
        public EnemyFormationWaveType EnemyFormationWaveType;

        public WaveEnemyData Enemy;
        public float Speed = 1;
        [Min(0)] public float DifficultyMin;
        public float DifficultyMax = float.MaxValue;

        private Vector2 _spawnOffset;

        public override void Initialize() => ResetFormation();
        public override float GetDifficultyMin() => DifficultyMin;
        public override float GetDifficultyMax() => DifficultyMax;
        public override WaveEnemyData[] GetEnemies() => new [] { Enemy };
        public override EnemyFormationType GetFormationType() => EnemyFormationType.SingleUnit;
        public override IEnumerable<EnemyFormationPlacement[]> GetNextEnemies() => new [] {new [] { new EnemyFormationPlacement(Enemy, _spawnOffset) }};
        public override float GetSpacing() => 1f;
        public override float GetSpeed() => Speed;
        public override EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType;
        public override void ResetFormation()
        {
            _spawnOffset = new Vector2(Random.Range(WaveController.LeftBounds, WaveController.RightBounds), 0f);
        }
    }
}