using UnityEngine;

namespace Enemy
{
    public interface IChildCollisionHandler
    {
        void OnChildTriggerEnter2D(Collider2D col);
    }
}