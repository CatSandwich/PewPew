using System.Collections;
using Pooling;
using Singletons;
using UnityEngine;

namespace UI.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CoinDrop : MonoBehaviour, IPoolable
    {
        public int Value;

        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;

        private SpriteRenderer _renderer;
        private Rigidbody2D _rb;

        private Coroutine _lifeCoroutine;

        public SpriteRenderer GetRenderer() => _renderer;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!WaveController.RunIsAlive) return;
            if (col.gameObject.name.Trim() != "Player") return;
            GameManager.Instance.Coins += Value;
            WaveController.Instance.Release(this);
        }

        public void OnActivate()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));

            _lifeCoroutine = StartCoroutine(LifeCoroutine());
        }

        public void OnDeactivate()
        {
            if (_lifeCoroutine != null)
                StopCoroutine(_lifeCoroutine);
            _lifeCoroutine = null;
        }

        public void OnReset() { }
        private IEnumerator LifeCoroutine()
        {
            yield return new WaitForSeconds(5f);
            WaveController.Instance.Release(this);
            if (_lifeCoroutine != null)
                StopCoroutine(_lifeCoroutine);
            _lifeCoroutine = null;
        }
    }
}