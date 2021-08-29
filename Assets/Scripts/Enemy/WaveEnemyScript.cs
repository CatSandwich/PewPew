using System;
using Enemy.Data;
using Singletons;

namespace Enemy
{
    public class WaveEnemyScript : AbstractEnemyScript
    {
        public WaveEnemyData Data;
        /// <summary> The Wave Type of the Wave this Unit belongs to. </summary>
        public EnemyFormationWaveType WaveType;

        /// <summary> The Wave in which this Unit spawned. </summary>
        public int Wave;
        /// <summary> The Unique ID in the Wave in which this Unit spawned. </summary>
        public int WaveId;

        public override event Action<AbstractEnemyScript> Destroyed;

        public override void TakeDamage(float damage, bool sourceIsPlayer = true)
        {
            if (!WaveController.RunIsAlive) return;
            Health -= damage;
            if (Health > 0) return;

            WasKilled = sourceIsPlayer;

            switch (WaveType)
            {
                case EnemyFormationWaveType.Normal:
                    WaveController.Instance.OnNormalEnemyDestroyed(this, WasKilled);
                    break;
                case EnemyFormationWaveType.Bonus:
                    WaveController.Instance.OnBonusEnemyDestroyed(this, WasKilled);
                    break;
                case EnemyFormationWaveType.Boss:
                    WaveController.Instance.OnBossEnemyDestroyed(this, WasKilled);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroyed?.Invoke(this);
        }
    }
}
