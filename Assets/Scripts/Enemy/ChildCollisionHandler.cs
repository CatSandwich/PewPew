using UnityEngine;

namespace Enemy
{
    public class ChildCollisionHandler : MonoBehaviour
    {
        public IChildCollisionHandler Parent;

        private void OnTriggerEnter2D(Collider2D col) => Parent.OnChildTriggerEnter2D(col);
    }
}