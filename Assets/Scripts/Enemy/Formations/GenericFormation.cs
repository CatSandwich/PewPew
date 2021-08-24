using System.Collections.Generic;
using Enemy.Movement;
using UnityEngine;

namespace Enemy.Formations
{
    public abstract class GenericFormation : ScriptableObject
    {
        public GenericMovement Movement;
        public abstract GenericEnemy[] GetEnemies();
        public virtual void Initialize() { }
        public virtual void ResetFormation() { }
        public virtual IEnumerable<EnemyFormationPlacement[]> GetNextEnemies() { return null; }
        public virtual EnemyFormationWaveType GetWaveType() => EnemyFormationWaveType.Undefined;
        public virtual EnemyFormationType GetFormationType() => EnemyFormationType.Undefined;
        public virtual float GetSpeed() => 1f;
        public virtual float GetSpacing() => 1f;
        public virtual float GetDifficultyMin() => 0f;
        public virtual float GetDifficultyMax() => float.MaxValue;

    }

    public enum EnemyFormationWaveType
    {
        Undefined = -1,
        Normal,
        Bonus,
        Boss,
    }

    public enum EnemyFormationType
    {
        Undefined = -1,

        SingleUnit,
        SingleFile,
        DoubleFile,

        VFormation,
        HFormation,
    }

    public struct EnemyFormationPlacement
    {
        public readonly GenericEnemy Enemy;
        public readonly Vector2 Offset;

        public EnemyFormationPlacement(GenericEnemy enemy) : this()
        {
            Enemy = enemy;
            Offset = Vector2.zero;
        }

        public EnemyFormationPlacement(GenericEnemy enemy, Vector2 offset)
        {
            Enemy = enemy;
            Offset = offset;
        }
    }
}
