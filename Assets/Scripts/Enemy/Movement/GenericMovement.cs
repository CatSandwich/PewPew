using UnityEngine;

namespace Enemy.Movement
{
    public abstract class GenericMovement : ScriptableObject
    {
        public abstract float GetLeftBounds();
        public abstract float GetRightBounds();
        public abstract void DoMovement(EnemyScript target);
    }
}
