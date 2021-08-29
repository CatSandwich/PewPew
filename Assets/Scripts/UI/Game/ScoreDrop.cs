using System.Collections;
using Pooling;
using Singletons;
using UnityEngine;

namespace UI.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ScoreDrop : MonoBehaviour, IPoolable
    {
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;

        private Rigidbody2D _rb;

        public void OnActivate()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));
            StartCoroutine(LifeCoroutine());
        }

        public void OnDeactivate()
        {
            StopAllCoroutines();
        }

        public void OnReset() { }
        private IEnumerator LifeCoroutine()
        {
            yield return new WaitForSeconds(5f);
            WaveController.Instance.Release(this);
        }
    }
}
