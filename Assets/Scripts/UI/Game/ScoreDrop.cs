using System;
using System.Collections;
using System.Collections.Generic;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ScoreDrop : MonoBehaviour, IPoolable
    {
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;

        private MeshRenderer _renderer;
        private Rigidbody2D _rb;

        public void OnActivate()
        {
            _renderer = GetComponent<MeshRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));
            StartCoroutine(_life());
        }

        public void OnDeactivate() { }

        public void OnReset() { }
        private IEnumerator _life()
        {
            yield return new WaitForSeconds(5f);
            WaveController.Instance.Release(this);
        }
    }
}
