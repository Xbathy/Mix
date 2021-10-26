using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityEngine.XR.WSA;

namespace Mix.ComputerVision.Sample.Scripts
{
    public class ProjectionSample : MonoBehaviour
    {
        [SerializeField] private Material topLeftMaterial;
        [SerializeField] private Material topRightMaterial;
        [SerializeField] private Material botLeftMaterial;
        [SerializeField] private Material botRightMaterial;
        [SerializeField] private Material centerMaterial;

        private FrameResolution _resolution;
        private VideoCapture _videoCapture;
        // private IntPtr _spatialCoordinateSystemPtr;
        private byte[] _latestImageBytes;
        private bool stopVideo;
        private RaycastLaser _laser;
        private void Start()
        {
        }
    }
}