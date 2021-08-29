using Singletons;
using UnityEngine;

namespace Enemy.Behaviours.Movement
{
    [CreateAssetMenu(menuName = "Enemies/Behaviours/Generic/DoubleSineBehaviour")]
    // ReSharper disable once UnusedMember.Global
    public class DoubleSineBehaviour : AbstractBehaviour
    {
        public override float GetLeftBounds() => 0f;
        public override float GetRightBounds() => 0f;

        public float SinewaveIntensity = 1f;
        public float Width = 1f;

        public override void PrepareBehaviour(AbstractEnemyScript target) { }
        public override void DoBehaviour(AbstractEnemyScript target)
        {
            if (!WaveController.RunIsAlive) return;
            if (!(target is WaveEnemyScript enemy)) return;
            var sin = Mathf.Sin(enemy.LifeTime * SinewaveIntensity);
            sin = (enemy.WaveId % 2 == 0) ? sin : -sin;

            target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
            target.gameObject.transform.position = new Vector3(target.SpawnPoint.x + sin * Width, target.gameObject.transform.position.y, 0f);
        }
        public override void ClearData(AbstractEnemyScript target) { }
    }
}
