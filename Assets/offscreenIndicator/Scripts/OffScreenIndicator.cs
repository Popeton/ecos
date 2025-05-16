using PixelPlay.OffScreenIndicator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelPlay.OffScreenIndicator
{
    [DefaultExecutionOrder(-1)]
    public class OffScreenIndicator : MonoBehaviour
    {
        [Range(0.5f, 0.9f)]
        [Tooltip("Fraction of half-width/height that bounds the indicator.")]
        [SerializeField] private float screenBoundOffset = 0.9f;

        [Header("Arrow Settings")]
        [Tooltip("Custom position offset")]
        [SerializeField] private Vector2 arrowOffset = new Vector2(0f, 50f);

        [SerializeField] private bool updateOnOrientationChange = false;

        private Camera mainCamera;
        private RectTransform canvasRect;
        private Vector2 screenCentre;
        private Vector2 screenBounds;
        private ScreenOrientation lastOrientation;
        private float lastAspect;

        private List<Target> targets = new List<Target>();
        public static Action<Target, bool> TargetStateChanged;

        void Awake()
        {
            mainCamera = Camera.main;
            canvasRect = GetComponent<RectTransform>();
            RecalculateBounds();

            lastOrientation = Screen.orientation;
            lastAspect = (float)Screen.width / Screen.height;
            TargetStateChanged += HandleTargetStateChanged;
        }

        void OnDestroy()
        {
            TargetStateChanged -= HandleTargetStateChanged;
        }

        void LateUpdate()
        {
            if (updateOnOrientationChange && (Screen.orientation != lastOrientation || !Mathf.Approximately((float)Screen.width / Screen.height, lastAspect)))
                RecalculateBounds();

            DrawIndicators();
        }

        private void RecalculateBounds()
        {
            screenCentre = canvasRect.sizeDelta * 0.5f;
            screenBounds = screenCentre * screenBoundOffset;
            lastOrientation = Screen.orientation;
            lastAspect = (float)Screen.width / Screen.height;
        }

        void DrawIndicators()
        {
            foreach (var target in targets)
            {
                Vector3 worldPos = target.transform.position;
                Vector3 rawVP3 = mainCamera.WorldToViewportPoint(worldPos);

                Vector2 vp = new Vector2(rawVP3.x, rawVP3.y);
                bool isVisible = rawVP3.z > 0 && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f;
                float dist = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.transform.position) : float.MinValue;

                Vector2 uiPos = (vp - new Vector2(0.5f, 0.5f)) * canvasRect.sizeDelta;
                Indicator indi = null;

                if (target.NeedBoxIndicator && isVisible)
                {
                    indi = GetIndicator(target, IndicatorType.BOX);
                }
                else if (target.NeedArrowIndicator && !isVisible)
                {
                    Vector3 toTarget = target.transform.position - mainCamera.transform.position;
                    Vector3 cameraForward = mainCamera.transform.forward;
                    bool isBehind = Vector3.Dot(toTarget.normalized, cameraForward) < 0;

                    Vector2 direction;
                    if (isBehind)
                    {
                        Vector3 projected = Vector3.ProjectOnPlane(toTarget, cameraForward);
                        if (projected == Vector3.zero)
                            projected = cameraForward * -1f;

                        projected.Normalize();
                        Vector3 cameraSpaceDir = mainCamera.transform.InverseTransformDirection(projected);
                        direction = new Vector2(cameraSpaceDir.x, cameraSpaceDir.y).normalized;
                    }
                    else
                    {
                        direction = uiPos.normalized;
                    }

                    Vector2 currentDirection = direction;
                    float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
                    Vector2 clamped = currentDirection * screenBounds;

                    // Aplicar offset y ajustes finales
                    clamped += arrowOffset;
                    clamped.x = Mathf.Clamp(clamped.x, -screenBounds.x, screenBounds.x);

                    // Mantener Y libre sin clamp
                    uiPos = new Vector2(clamped.x, clamped.y);

                    indi = GetIndicator(target, IndicatorType.ARROW);
                    indi.transform.localRotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    target.indicator?.Activate(false);
                    target.indicator = null;
                }

                if (indi != null)
                {
                    indi.SetImageColor(target.TargetColor);
                    indi.SetDistanceText(dist);
                    ((RectTransform)indi.transform).anchoredPosition = uiPos;
                    indi.SetTextRotation(Quaternion.identity);
                }
            }
        }

        private void HandleTargetStateChanged(Target t, bool on)
        {
            if (on) targets.Add(t);
            else
            {
                t.indicator?.Activate(false);
                t.indicator = null;
                targets.Remove(t);
            }
        }

        private Indicator GetIndicator(Target target, IndicatorType type)
        {
            Indicator current = target.indicator;

            if (current != null && current.Type != type)
            {
                current.Activate(false);
                current = null;
            }

            if (current == null)
            {
                current = type == IndicatorType.BOX
                    ? BoxObjectPool.current.GetPooledObject()
                    : ArrowObjectPool.current.GetPooledObject();
                current.Activate(true);
                target.indicator = current;
            }
            return current;
        }
    }
}