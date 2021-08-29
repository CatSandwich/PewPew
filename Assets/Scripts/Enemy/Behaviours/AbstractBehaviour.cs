using UnityEngine;

namespace Enemy.Behaviours
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
        /// Prepares this Behaviour to handle the given <see cref="AbstractEnemyScript"/>
        /// </summary>
        /// <param name="target"></param>
        public abstract void PrepareBehaviour(AbstractEnemyScript target);
        /// <summary>
        /// Causes this Behaviour to control the given <see cref="AbstractEnemyScript"/>.
        /// </summary>
        public abstract void DoBehaviour(AbstractEnemyScript target);
        /// <summary>
        /// Clears any cached data for the given <see cref="AbstractEnemyScript"/> stored with this Behaviour.
        /// </summary>
        /// <param name="target"></param>
        public abstract void ClearData(AbstractEnemyScript target);
    }
}
