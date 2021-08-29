using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Movement
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Generic/StraightBehaviour")]
    // ReSharper disable once UnusedMember.Global
    public class StraightBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds;
        public override float GetRightBounds() => WaveController.RightBounds;

        public override void PrepareBehaviour(AbstractEnemyScript target) { }
        public override void DoBehaviour(AbstractEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            target.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
        }
        public override void ClearData(AbstractEnemyScript target) { }
    }
}
