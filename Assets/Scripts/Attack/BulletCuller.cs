using UnityEngine;

namespace Attack
{
    public class BulletCuller : MonoBehaviour
    {
        private Renderer _renderer;
        void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!_renderer.isVisible) Destroy(gameObject);
        }
    }
}
