using UnityEngine;

namespace Enemy.Movement
{
    [CreateAssetMenu()]
    public class StraightMovement : GenericMovement
    {
        public override float GetLeftBounds() => WaveController.LeftBounds;
        public override float GetRightBounds() => WaveController.RightBounds;
        public override void DoMovement(EnemyScript target)
        {
            target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
        }
    }
}
