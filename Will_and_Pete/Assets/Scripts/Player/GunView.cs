using System;
using Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] private Transform gunTurnAxis;
        [SerializeField] private SpriteRenderer spRend;
        [SerializeField] private GameObject gunFireAnimPrefab;
        [SerializeField] private Transform gunNozzlePosition;
        [SerializeField] private SpriteRenderer gunSpriteRenderer;
        [SerializeField] private LineRenderer lineRendererFire;
        [SerializeField] private LineRenderer lineRendererAim;

        private PlayerSettings pSettings;
        private PlayerState pState;
        private GameObject gunFireAnim;
        private float currentLineFadeTime; 
        private bool isAiming;

        public Quaternion AimRotation => gunTurnAxis.rotation;
        public Vector3 GunForwards => gunTurnAxis.right;
        public void Initialize(PlayerSettings settings,PlayerState state)
        {
            lineRendererFire = GetComponent<LineRenderer>();
            gunFireAnim = Instantiate(gunFireAnimPrefab);
            pSettings = settings;
            pState = state;
        }

        private void Update()
        {
            FadeFireLine();
            lineRendererAim.enabled = isAiming;
            isAiming = false;
        }

      
        public void DrawFireLine(Vector2 endPos)
        {
            Vector3[] linePositions = { gunNozzlePosition.position, (Vector3)endPos };
            lineRendererFire.SetPositions(linePositions);
            currentLineFadeTime = 0;
            
            gunFireAnim.transform.localPosition = gunNozzlePosition.position;
            gunFireAnim.transform.rotation = gunTurnAxis.rotation;
            gunFireAnim.SetActive(true);
        }

        public void DrawAimLine(Vector2 endPos)
        {
            Vector3[] linePositions = { gunNozzlePosition.position,gunNozzlePosition.position + (Vector3)endPos };
            lineRendererAim.SetPositions(linePositions);
            isAiming = true;
        }

        public void RotateToTarget(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gunTurnAxis.rotation = Quaternion.Lerp(gunTurnAxis.rotation, Quaternion.Euler(0f, 0f, angle), Time.deltaTime * pSettings.AimControlFactor);
            // gunTurnAxis.rotation = Quaternion.Euler(0f, 0f, angle);
            spRend.flipY = !(direction.x > 0);
        }

        public void ToggleVisibility()
        {
            gunSpriteRenderer.enabled = !gunSpriteRenderer.enabled;
        }

        private void FadeFireLine()
        {
            if (currentLineFadeTime >= pSettings.FireLineFadeTime)
            {
                return;
            }
            currentLineFadeTime += Time.deltaTime;
            Color lineColor = Color.Lerp(pSettings.FireLineStartColor, pSettings.FireLineEndColor, currentLineFadeTime / pSettings.FireLineFadeTime);
            lineRendererFire.startColor = lineColor;
            lineRendererFire.endColor = lineColor;
        }
    }
}
