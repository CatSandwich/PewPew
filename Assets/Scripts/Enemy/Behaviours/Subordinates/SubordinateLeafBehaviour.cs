using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Subordinates
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Subordinates/LeafBehaviour")]
    public class SubordinateLeafBehaviour : SubordinateBehaviour
    {
        public override void DoBehaviour(AbstractEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;

            var t = target.LifeTime * target.Speed;
            var pos = target.SpawnPoint + new Vector3(Mathf.Cos(t), (-t * 0.25f) + (Mathf.Cos(t * 2) / 2), 0f);

            target.transform.position = pos;
        }
    }
}
