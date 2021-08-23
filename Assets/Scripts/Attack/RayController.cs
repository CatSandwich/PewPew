using Player;
using UnityEngine;

namespace Attack
{
    public class RayController : MonoBehaviour
    {
        public float Lifetime;
    
        private Transform _root;
        private float _startTime;
        private int _direction;
    
        public static void Instantiate(int direction)
        {
            var go = Instantiate(Assets.Instance.Ray);
            var controller = go.GetComponent<RayController>();
            controller._direction = direction;
            controller._root = PlayerController.Instance.transform;
            controller._startTime = Time.time;
            controller.Update();
        }
    
        void Update()
        {
            transform.position = _root.position + Vector3.up * transform.localScale.y / 2;
            transform.rotation = Quaternion.identity;
            var a = (Time.time - _startTime) / Lifetime;
            transform.RotateAround(_root.position, Vector3.forward * _direction, Mathf.Lerp(0, 90, a));
            if(a >= 1f) Destroy(gameObject);
        }
    }
}