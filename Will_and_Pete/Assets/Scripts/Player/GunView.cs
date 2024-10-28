using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] private Transform gunTurnAxis;
        [SerializeField] private SpriteRenderer spRend;
        [SerializeField] private GameObject gunFireAnimPrefab;
        [SerializeField] private Transform gunNozzlePosition;

        private PlayerSettings pSettings;
        private PlayerState pState;
        private GameObject gunFireAnim;
        private float currentLineFadeTime;
        private LineRenderer lineRenderer;

        public void Initialize(PlayerSettings settings,PlayerState state)
        {
            lineRenderer = GetComponent<LineRenderer>();
            gunFireAnim = Instantiate(gunFireAnimPrefab);
            pSettings = settings;
            pState = state;
        }

        private void Update()
        {
            FadeFireLine();
        }

        public void DrawFireLine(Vector2 endPos)
        {
            Vector3[] linePositions = { gunNozzlePosition.position, (Vector3)endPos };
            lineRenderer.SetPositions(linePositions);
            currentLineFadeTime = 0;
            
            gunFireAnim.transform.localPosition = gunNozzlePosition.position;
            gunFireAnim.transform.rotation = gunTurnAxis.rotation;
            gunFireAnim.SetActive(true);
        }

        public void RotateToTarget(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunTurnAxis.rotation = Quaternion.Euler(0f, 0f, angle);
            spRend.flipY = pState.IsFacingRight ? false : true;
        }

        private void FadeFireLine()
        {
            if (currentLineFadeTime >= pSettings.FireLineFadeTime)
            {
                return;
            }
            currentLineFadeTime += Time.deltaTime;
            Color lineColor = Color.Lerp(pSettings.FireLineStartColor, pSettings.FireLineEndColor, currentLineFadeTime / pSettings.FireLineFadeTime);
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
        }
    }
}
