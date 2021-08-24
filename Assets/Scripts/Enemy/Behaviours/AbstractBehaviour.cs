using UnityEngine;

namespace Enemy.Movement
{
    public abstract class AbstractBehaviour : ScriptableObject
    {
        /// <summary>
        /// Returns the Left most Boundary this Behaviour can operate under. <br/>
        /// This is used to control Spawning placement.
        /// </summary>
        public abstract float GetLeftBounds();
        /// <summary>
        /// Returns the Right most Boundary this Behaviour can operate under. <br/>
        /// This is used to control Spawning placement.
        /// </summary>
        public abstract float GetRightBounds();
        /// <summary>
        /// Causes this Behaviour to control the given <see cref="EnemyScript"/>.
        /// </summary>
        public abstract void DoBehaviour(EnemyScript target);
    }
}
