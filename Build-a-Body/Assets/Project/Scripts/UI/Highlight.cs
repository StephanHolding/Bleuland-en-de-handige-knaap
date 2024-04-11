// Author: Chrysalis shiyuchongf@gmail.com

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ProjectHKU.UI;

namespace ProjectHKU.UI
{
    public class Highlight : UIElement
    {
        public RectTransform up;
        public RectTransform left;
        public RectTransform right;
        public RectTransform down;
        public RectTransform circle;
        public RectTransform circleSurrounding;

        private Vector2 currentCenter = new Vector2(0,0);
        private float currentRadius = 1920 * 1.5f;
        public Vector2 targetCenter;
        public float targetRadius;

        public float shiftRatio = 0.1f;
        public float MinimumSpeed = 2f;

        public override void UIUpdate(){
            ShiftFocus();
            SetTransform();
            if (Application.isEditor) EditorPlay();
        }

        public void SetTarget(Vector2 target, int radius)
        {
            targetCenter = target;
            targetRadius = radius;
        }

        private void ShiftFocus()
        {
            Vector2 speed = (targetCenter - currentCenter) * shiftRatio;
            if (speed.magnitude < MinimumSpeed && speed.magnitude > shiftRatio * MinimumSpeed)
                speed = speed * (MinimumSpeed / speed.magnitude);
            currentCenter = currentCenter + speed;
            if ((targetCenter - currentCenter).magnitude < MinimumSpeed)
                currentCenter = targetCenter;

            currentRadius = (targetRadius - currentRadius) * shiftRatio + currentRadius;
            if (Mathf.Abs(targetRadius - currentRadius) < MinimumSpeed)
                currentRadius = targetRadius;
        }

        private void SetTransform()
        {
            int radius = (int) Math.Round(currentRadius);
            Vector2Int center = Vector2Int.RoundToInt(currentCenter);

            left.sizeDelta = new Vector2(center.x - currentRadius, left.sizeDelta.y);
            right.sizeDelta = new Vector2(1080 - left.rect.width - radius * 2, right.sizeDelta.y);
            up.position = new Vector2(left.sizeDelta.x, up.position.y);
            up.sizeDelta = new Vector2(radius * 2, 1920 - radius - center.y);

            down.position = new Vector2(left.sizeDelta.x, down.position.y);
            down.sizeDelta = new Vector2(radius * 2, center.y - radius);

            circle.position = (Vector2) center;
            circle.sizeDelta = new Vector2(radius * 2, radius * 2);

            circleSurrounding.position = (Vector2) center;
            circleSurrounding.sizeDelta = new Vector2(radius * 2, radius * 2);
        }

        public void Reset()
        {
            currentCenter = new Vector2(0,0);
            currentRadius = 1920 * 1.5f;
            targetCenter = currentCenter;
            targetRadius = currentRadius;
        }

        public void EditorPlay()
        {
            if (Input.GetMouseButtonDown(0))
            {
                targetCenter = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
                targetRadius = UnityEngine.Random.Range(50, 200);
            }
        }

    }
}
