using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] Transform gunTurnAxis;
        [SerializeField] ShootingSettings settings;
        [SerializeField] SpriteRenderer spRend;

        private float currentLineFadeTime;
        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            FadeFireLine();
        }

        public void DrawFireLine(Vector2 startPos, Vector2 endPos)
        {
            Vector3[] linePositions = { (Vector3)startPos, (Vector3)endPos };
            lineRenderer.SetPositions(linePositions);
            currentLineFadeTime = 0;
        }

        public void RotateToTarget(PlayerState pState, Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunTurnAxis.rotation = Quaternion.Euler(0f, 0f, angle);
            spRend.flipY = pState.isFacingRight ? false : true;
        }

        private void FadeFireLine()
        {
            if (currentLineFadeTime >= settings.fireLineFadeTime)
            {
                return;
            }
            currentLineFadeTime += Time.deltaTime;
            Color lineColor = Color.Lerp(settings.fireLineStartColor, settings.fireLineEndColor, currentLineFadeTime / settings.fireLineFadeTime);
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
        }
    }
}
