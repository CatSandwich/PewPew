using Enemy.Movement;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Bosses
{
    [CreateAssetMenu()]
    // ReSharper disable once UnusedMember.Global
    public class BossBehaviour1 : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds;
        public override float GetRightBounds() => WaveController.RightBounds;
        public float GetTopBounds() => WaveController.TopBounds;

        public override void DoBehaviour(EnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;

            switch (target.MovementPhase)
            {
                case 0:
                {
                    target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
                    if (target.gameObject.transform.position.y <= GetTopBounds()) target.MovementPhase = 1;
                    break;
                }
                case 1:
                {
                    target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.left);
                    if (target.gameObject.transform.position.x <= GetLeftBounds()) target.MovementPhase = 2;
                    break;
                }
                case 2:
                {
                    target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.right);
                    if (target.gameObject.transform.position.x >= GetRightBounds()) target.MovementPhase = 1;
                    break;
                }

                default:
                    Debug.LogError($"Unknown Phase #{target.MovementPhase} for {GetType()}!");
                    break;
            }
        }
    }
}
