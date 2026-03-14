using System;
using UnityEngine;

namespace World
{
    public class Door : AActivatable
    {
        private Collider2D _collider2d;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _collider2d = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        public override void Activate()
        {
            _collider2d.enabled = false;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.4f);
        }

        public override void Deactivate()
        {
            _collider2d.enabled = true;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
        }
    }
}