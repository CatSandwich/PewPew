using Enemy.Movement;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Movement
{
    [CreateAssetMenu()]
    // ReSharper disable once UnusedMember.Global
    public class StraightBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds;
        public override float GetRightBounds() => WaveController.RightBounds;
        public override void DoBehaviour(EnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
        }
    }
}
