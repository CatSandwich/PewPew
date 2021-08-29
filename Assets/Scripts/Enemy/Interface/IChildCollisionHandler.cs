using UnityEngine;

namespace Enemy.Interface
{
    public interface IChildCollisionHandler
    {
        void OnChildTriggerEnter2D(Collider2D col);
    }
}