using System.Collections.Generic;
using Enemy.Data;
using Enemy.Movement;
using UnityEngine;

namespace Enemy.Formations
{
    public abstract class GenericFormation : ScriptableObject
    {
        public GenericMovement Movement;
        public abstract void Initialize();
        public abstract float GetDifficultyMin();
        public abstract float GetDifficultyMax();
        public abstract GenericEnemy[] GetEnemies();
        public abstract EnemyFormationType GetFormationType();
        public abstract IEnumerable<EnemyFormationPlacement[]> GetNextEnemies();
        public abstract float GetSpacing();
        public abstract float GetSpeed();
        public abstract EnemyFormationWaveType GetWaveType();
        public abstract void ResetFormation();

    }
}
