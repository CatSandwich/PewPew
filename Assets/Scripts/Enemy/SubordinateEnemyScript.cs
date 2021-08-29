using System;
using Enemy.Data;
using Singletons;

namespace Enemy
{

    public class SubordinateEnemyScript : AbstractEnemyScript
    {
        public SubordinateData Data;

        public override event Action<AbstractEnemyScript> Destroyed;

        public override void TakeDamage(float damage, bool sourceIsPlayer = true)
        {
            if (!WaveController.RunIsAlive) return;
            Health -= damage;
            if (Health > 0) return;

            WasKilled = sourceIsPlayer;

            WaveController.Instance.OnSubordinateEnemyDestroyed(this, WasKilled);
            Destroyed?.Invoke(this);
        }
    }
}
