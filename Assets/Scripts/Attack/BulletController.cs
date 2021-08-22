using System;
using UnityEngine;

namespace Attack
{
    public class BulletController : MonoBehaviour
    {
        public float Speed;

        public static void Instantiate(Vector3 position, Vector2 direction)
        {
            var go = Instantiate(Assets.Instance.Bullet);
            go.transform.position = new Vector3(position.x, position.y, 0);
            go.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        
        // Update is called once per frame
        void Update()
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.up);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
