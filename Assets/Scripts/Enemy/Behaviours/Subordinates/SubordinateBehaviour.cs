using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Subordinates
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Subordinates/DoNothing")]
    public class SubordinateBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds + 1f;
        public override float GetRightBounds() => WaveController.RightBounds - 1f;

        public override void PrepareBehaviour(AbstractEnemyScript target)
        {

        }

        public override void DoBehaviour(AbstractEnemyScript target)
        {

        }

        public override void ClearData(AbstractEnemyScript target)
        {

        }
    }
}
