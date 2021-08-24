using UnityEngine;

namespace Enemy.Movement
{
    public class GenericMovement : ScriptableObject
    {
        public virtual float GetLeftBounds() => -50f;
        public virtual float GetRightBounds() => 50f;
        public virtual void DoMovement(EnemyScript target) { }
    }
}
