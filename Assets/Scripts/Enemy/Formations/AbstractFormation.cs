using System.Collections.Generic;
using Enemy.Data;
using Enemy.Movement;
using UnityEngine;

namespace Enemy.Formations
{
    public abstract class AbstractFormation : ScriptableObject
    {
        public AbstractBehaviour Behaviour;
        /// <summary> Used to Initialize the given Formation before it is called. </summary>
        public abstract void Initialize();
        /// <summary> Returns the lowest difficulty at which this Formation can spawn. </summary>
        public abstract float GetDifficultyMin();
        /// <summary> Returns the highest difficulty at which this Formation can spawn. </summary>
        public abstract float GetDifficultyMax();
        /// <summary>
        /// Returns a complete list of <see cref="EnemyBase"/> prefabs which will spawn with this formation. <br/>
        /// Some formations, such as the <see cref="DoubleFileFormation"/> will spawn more units than prefabs.
        /// </summary>
        public abstract EnemyBase[] GetEnemies();
        /// <summary> Returns the Type of Formation this correlates to. </summary>
        public abstract EnemyFormationType GetFormationType();
        /// <summary> Returns a list of enemies which will spawn in this next wave. </summary>
        public abstract IEnumerable<EnemyFormationPlacement[]> GetNextEnemies();
        /// <summary> Returns the Vertical spacing, and thus Delay, at which each row will spawn. </summary>
        public abstract float GetSpacing();
        /// <summary> Returns the Speed at which this wave will travel. </summary>
        public abstract float GetSpeed();
        /// <summary> Returns the Type of Units this correlates to. </summary>
        public abstract EnemyFormationWaveType GetWaveType();
        /// <summary> Resets the Formation, allowing it to prepare to spawn again. </summary>
        public abstract void ResetFormation();

    }
}
