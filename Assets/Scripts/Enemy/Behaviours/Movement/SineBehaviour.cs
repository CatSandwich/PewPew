using Enemy.Movement;
using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Movement
{
    [CreateAssetMenu()]
    // ReSharper disable once UnusedMember.Global
    public class SineBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => WaveController.LeftBounds + Width;
        public override float GetRightBounds() => WaveController.RightBounds - Width;

        public float SinewaveIntensity = 1f;

        public float Width = 1f;
        public override void DoBehaviour(EnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
            target.gameObject.transform.position = new Vector3(target.SpawnPoint.x + Mathf.Sin(target.LifeTime * SinewaveIntensity) * Width, target.gameObject.transform.position.y, 0f);
        }
    }
}
