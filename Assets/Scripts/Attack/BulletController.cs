using Attack.Interfaces;
using Singletons;
using UnityEngine;

namespace Attack
{
    public class BulletController : MonoBehaviour, IOneHitAttack
    {
        public float Damage => 5;
        public void Destroy() => Destroy(gameObject);

        public float Speed;

        public static void Instantiate(Vector3 position, Vector2 direction)
        {
            var go = Instantiate(Assets.Instance.Bullet);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        
        public void Update()
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.up);
        }
    }
}
