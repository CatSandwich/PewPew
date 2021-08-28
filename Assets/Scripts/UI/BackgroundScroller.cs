using UnityEngine;

namespace UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        public Texture2D Texture;
        public float Speed;
        private SpriteRenderer _renderer;

        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        
        void Update()
        {
            _renderer.size = new Vector2(_renderer.size.x, Mathf.Lerp(20f - Texture.height * 2 / 100f, 20f, Time.time * Speed % 1f));
        }
    }
}
