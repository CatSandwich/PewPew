using UnityEngine;

namespace Enemy.Movement
{
    [CreateAssetMenu()]
    public class DoubleSineMovement : GenericMovement
    {
        public override float GetLeftBounds() => 0f;
        public override float GetRightBounds() => 0f;

        public float SinewaveIntensity = 1f;

        public float Width = 1f;
        public override void DoMovement(EnemyScript target)
        {
            var sin = Mathf.Sin(target.LifeTime * SinewaveIntensity);
            sin = (target.WaveID % 2 == 0) ? sin : -sin;

            target.gameObject.transform.Translate(target.Speed * Time.deltaTime * Vector3.down);
            target.gameObject.transform.position = new Vector3(target.SpawnPoint.x + sin * Width, target.gameObject.transform.position.y, 0f);
        }
    }
}
