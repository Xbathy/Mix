using UnityEngine;

namespace Mix.ComputerVision
{
   public class PhotoInfo
   {
      public Matrix4x4 CameraToWorldMatrix;
      public Matrix4x4 ProjectionMatrix;
      public byte[] PhotoBytes;
      public FrameResolution Resolution;
   }
}
