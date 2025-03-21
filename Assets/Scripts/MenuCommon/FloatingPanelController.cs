/******************************************************************************
 * File: FloatingPanelController.cs
 * Copyright (c) 2022-2023 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 ******************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class FloatingPanelController : MonoBehaviour
    {
        public bool FollowGaze = true;
        public float TargetDistance = 3f;
        public float MovementSmoothness = 0.2f;
        private Transform _arCameraTransform;
        private Camera _arCamera;
        private InteractionManager _interactionManager;
        private bool isStart;

        public void SwitchToScene(string name)
        {
            _interactionManager?.SendHapticImpulse();
            SceneManager.LoadScene(name);
        }

        private void Start()
        {
            isStart = true;
            _arCamera = OriginLocationUtility.GetOriginCamera();
            _arCameraTransform = _arCamera.transform;
            _interactionManager ??= FindObjectOfType<InteractionManager>(true);   
        }

        private void Update()
        {
            if (isStart)
            {
                Vector3 dir = _arCamera.transform.forward;
                dir.y = 0;
                Vector3.Normalize(dir);
                transform.position = _arCamera.transform.position + dir * TargetDistance;
                isStart = !isStart;
            }
            if (FollowGaze)
            {
                AdjustPanelPosition();
            }
        }

        // Adjusts the position of the Panel if the gaze moves outside of the inner rectangle of the FOV,
        //  which is half the length in both axis.
        private void AdjustPanelPosition()
        {
            var headPosition = _arCameraTransform.position;
            var gazeDirection = _arCameraTransform.forward;
            //gazeDirection.y = 0;
            //Vector3.Normalize(gazeDirection);
            var direction = (transform.position - headPosition).normalized;
            //direction.y = 0;
            //Vector3.Normalize(direction);
            var targetPosition = headPosition + (gazeDirection * TargetDistance);
            var targetDirection = (targetPosition - headPosition).normalized;
            var eulerAngles = Quaternion.LookRotation(direction).eulerAngles;
            var targetEulerAngles = Quaternion.LookRotation(targetDirection).eulerAngles;
            var verticalHalfAngle = _arCamera.fieldOfView * 0.8f;
            eulerAngles.x += GetAdjustedDelta(targetEulerAngles.x - eulerAngles.x, verticalHalfAngle);
            var horizontalHalfAngle = _arCamera.fieldOfView * 0.5f * _arCamera.aspect;
            eulerAngles.y += GetAdjustedDelta(targetEulerAngles.y - eulerAngles.y, horizontalHalfAngle);
            targetPosition = headPosition + (Quaternion.Euler(eulerAngles) * Vector3.forward * TargetDistance);
            transform.position = Vector3.Lerp(transform.position, targetPosition, MovementSmoothness);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - headPosition), MovementSmoothness);
        }

        // Returns the normalized delta to a certain threshold, if it exceeds that threshold. Otherwise return 0.
        private float GetAdjustedDelta(float angle, float threshold)
        {
            // Normalize angle to be between 0 and 360.
            angle = ((540f + angle) % 360f) - 180f;
            if (Mathf.Abs(angle) > threshold)
            {
                return -angle / Mathf.Abs(angle) * (threshold - Mathf.Abs(angle));
            }

            return 0f;
        }
    }
}
