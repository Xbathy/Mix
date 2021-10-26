using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Mix.ComputerVision
{
    /// <summary>
    ///  定位相机拍照
    /// </summary>
    public class LocateCamera : MonoBehaviour
    {
        private bool _isCapturing;
        private bool _isReadyToCapturePhoto;
        private PhotoCapture _photoCaptureObj;
        private FrameResolution _resolution;
        private Texture2D _texture;

        private readonly PhotoInfo _photoInfo = new PhotoInfo();
        private Matrix4x4 _cameraToWorldMatrix;
        private Matrix4x4 _projectionMatrix;
        private byte[] _imageBytes;

        private Action<PhotoInfo> _captureCompleteCallBack;

        private void Start()
        {
            PhotoCapture.CreateAsync(false, OnPhotoCaptureObjCreated);
        }

        private void OnDestroy()
        {
            _isReadyToCapturePhoto = false;
            _photoCaptureObj?.StopPhotoModeAsync(OnStopPhotoMode);
        }

        private void OnPhotoCaptureObjCreated(PhotoCapture captureObject)
        {
            _photoCaptureObj = captureObject;
            //选择分辨率最高的
            Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending(resolution => resolution.height * resolution.width).First();
            _texture = new Texture2D(resolution.width, resolution.height);
            _photoInfo.Resolution = new FrameResolution
            {
                Width = resolution.width,
                Height = resolution.height
            };

            CameraParameters cameraParameters = new CameraParameters
            {
                hologramOpacity = 0f,
                cameraResolutionHeight = resolution.height,
                cameraResolutionWidth = resolution.width,
                pixelFormat = CapturePixelFormat.BGRA32
            };
            _photoCaptureObj.StartPhotoModeAsync(cameraParameters, OnStartPhotoMode);
        }

        private void OnStartPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                _isReadyToCapturePhoto = true;
            }
            else
            {
                _isReadyToCapturePhoto = false;
                Debug.LogError("Can't start photo mode!");
            }
        }

        private void OnCapturedPhoto(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (!result.success /*|| !photoCaptureFrame.hasLocationData */ || Camera.main == null) return;
            // 处理拍摄帧的矩阵信息
            Camera main = Camera.main;
            if (!photoCaptureFrame.TryGetCameraToWorldMatrix(out _photoInfo.CameraToWorldMatrix) || !photoCaptureFrame.TryGetProjectionMatrix(main.nearClipPlane, main.farClipPlane, out _photoInfo.ProjectionMatrix)) return;
            // 获取拍摄的照片
            // TODO 只能 C++ 来获取 pointerToBuffer 的数据, C# 没有办法
            // TODO 或者将 BGRA32 转为 PNG 
            // TODO 放到子线程
            photoCaptureFrame.UploadImageDataToTexture(_texture);
            // 将图片发送到服务器
            _photoInfo.PhotoBytes = _texture.EncodeToPNG();
            _isCapturing = false;

            // 确定图片生成完成
            _captureCompleteCallBack(_photoInfo);
        }

        private void OnStopPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            _photoCaptureObj.Dispose();
            _photoCaptureObj = null;
            if (_texture != null)
            {
                Destroy(_texture);
            }
        }

        /// <summary>
        /// 拍照
        /// </summary>
        public void TakePhoto(Action<PhotoInfo> action)
        {
            if (!_isReadyToCapturePhoto || _isCapturing)
            {
                return;
            }
            _captureCompleteCallBack = action;
            _isCapturing = true;
            _photoCaptureObj.TakePhotoAsync(OnCapturedPhoto);
        }
    }
}