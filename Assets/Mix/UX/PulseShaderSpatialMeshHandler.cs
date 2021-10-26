using System;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI.PulseShader;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace Mix.UX
{
    /// <summary>
    /// Script for triggering the pulse shader effect on the spatial mesh.
    /// </summary>
    public class PulseShaderSpatialMeshHandler : PulseShaderHandler, IMixedRealityPointerHandler
    {
        [Space]
        [SerializeField]
        [Tooltip("Trigger the pulse shader animation on select (air-tap/pinch) of the spatial mesh. ")]
        private bool pulseOnSelect = true;

        /// <summary>
        /// Trigger the pulse shader animation on select (air-tap/pinch) of the spatial mesh. 
        /// </summary>
        public bool PulseOnSelect
        {
            get { return pulseOnSelect; }
            set
            {
                if (pulseOnSelect != value)
                {
                    pulseOnSelect = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("Set the origin of the pulse animation to the main camera location.")]
        private bool originFollowCamera = false;

        /// <summary>
        /// Set the origin of the pulse animation to the main camera location.
        /// </summary>
        public bool OriginFollowCamera
        {
            get { return originFollowCamera; }
            set
            {
                if (originFollowCamera != value)
                {
                    originFollowCamera = value;
                }
            }
        }

        public void StartPulseAnimation()
        {
            StartPulsing();
        }

        public void StopPulseAnimation()
        {
            ResetPulseMaterial();
        }
        
        protected override void Update()
        {
            if (OriginFollowCamera)
            {
                SetLocalOrigin(CameraCache.Main.transform.position);
            }
        }

        protected void TriggerAnimationOnSpatialMesh(Vector3 pulseAnimationOrigin)
        {
            SetLocalOrigin(pulseAnimationOrigin);
            PulseOnce();
        }

        private void OnDisable()
        {
            ResetPulseMaterial();
        }

        #region Pointer Handler

        public void OnPointerDown(MixedRealityPointerEventData eventData) { }
        public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
        public void OnPointerUp(MixedRealityPointerEventData eventData) { }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            if (PulseOnSelect)
            {
                Vector3 pulseAnimationOrigin = eventData.Pointer.Result.Details.Point;
                TriggerAnimationOnSpatialMesh(pulseAnimationOrigin);
            }
        }

        #endregion
    }
}
