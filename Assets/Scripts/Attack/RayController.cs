using Attack.Interfaces;
using Player;
using Singletons;
using UnityEngine;

namespace Attack
{
    public class RayController : CooldownAttackBase
    {
        public override float Damage => Manager.RayDamage.Value;
        protected override float Cooldown => Manager.RayCooldown.Value;
        protected override float Lifetime => 8f;

        public SpriteRenderer Renderer;
        
        private Transform _root;
        private float _startTime;
        private int _direction;
    
        public static void Instantiate(int direction)
        {
            var go = Instantiate(Assets.Instance.Ray);
            go.transform.parent = Parent.transform;
            var controller = go.GetComponent<RayController>();
            controller._direction = direction;
            controller._root = PlayerController.Instance.transform;
            controller._startTime = Time.time;
            controller.Update();
        }
    
        void Update()
        {
            transform.rotation = Quaternion.identity;
            transform.position = _root.position + Vector3.up * Renderer.bounds.size.y / 2;
            var a = (Time.time - _startTime) / Lifetime;
            transform.RotateAround(_root.position, Vector3.forward * _direction, Mathf.Lerp(0, 90, a));
            if(a >= 1f) Destroy(gameObject);
        }
    }
}
