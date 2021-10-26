//  
// Copyright (c) 2017 Vulcan, Inc. All rights reserved.  
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
//

using UnityEngine;

namespace Mix.ComputerVision
{
    public static class Utils
    {
        /// <summary>
        /// 将像素从屏幕坐标投影到unity世界坐标系
        /// The method is based on: https://developer.microsoft.com/en-us/windows/mixed-reality/locatable_camera#pixel_to_application-specified_coordinate_system
        /// </summary>
        /// <param name="cameraToWorldMatrix">The camera to Unity world matrix.</param>
        /// <param name="projectionMatrix">Projection Matrix.</param>
        /// <param name="pixelCoordinate">The coordinate of the pixel that should be converted to world-space.</param>
        /// <param name="resolution">The resolution of the image that the pixel came from.</param>
        /// <returns>Vector3 with direction: optical center to camera world-space coordinates</returns>
        public static Vector3 PixelCoordToWorldCoord(Matrix4x4 cameraToWorldMatrix, Matrix4x4 projectionMatrix, FrameResolution resolution, Vector2 pixelCoordinate)
        {
            Vector2 pixelCoordinates = ConvertPixCoordsToScaledCoords(pixelCoordinate, resolution); // [-1, 1]

            float focalLengthX = projectionMatrix.GetColumn(0).x;
            float focalLengthY = projectionMatrix.GetColumn(1).y;
            float centerX = projectionMatrix.GetColumn(2).x;
            float centerY = projectionMatrix.GetColumn(2).y;

            float normFactor = projectionMatrix.GetColumn(2).z;
            centerX /= normFactor;
            centerY /= normFactor;

            Vector3 dirRay = new Vector3((pixelCoordinates.x - centerX) / focalLengthX, (pixelCoordinates.y - centerY) / focalLengthY, 1.0f / normFactor); //Direction is in camera space
            Vector3 direction = new Vector3(Vector3.Dot(cameraToWorldMatrix.GetRow(0), dirRay), Vector3.Dot(cameraToWorldMatrix.GetRow(1), dirRay), Vector3.Dot(cameraToWorldMatrix.GetRow(2), dirRay));

            return direction;
        }

        /// <summary>
        /// 将屏幕坐标系转换到[-1, 1]范围的 NDC(Normalized Device Coordinate) 归一化设备坐标系
        /// </summary>
        /// <param name="pixCoord"></param>
        /// <param name="resolution">像素所属的帧的分辨率</param>
        /// <returns>范围[-1, 1]的坐标</returns>
        public static Vector2 ConvertPixCoordsToScaledCoords(Vector2 pixCoord, FrameResolution resolution)
        {
            float halfWidth = resolution.Width / 2f;
            float halfHeight = resolution.Height / 2f;

            pixCoord.x -= halfWidth;
            pixCoord.y -= halfHeight;

            pixCoord = new Vector2(pixCoord.x / halfWidth, pixCoord.y / halfHeight);
            return pixCoord;
        }

        public static Vector3 GetHitPosition(Vector3 from, Vector3 direction, LayerMask layerMask,float length = 5f )
        {
            Ray ray = new Ray(from, direction);
            Vector3 to = from + length * direction;
            if (Physics.Raycast(ray, out RaycastHit hit, length, layerMask))
                to = hit.point;
            return to;
        }
        
        public static Vector3 GetNormalOfPose(Matrix4x4 pose)
        {
            return new Vector3(Vector3.Dot(Vector3.forward, pose.GetRow(0)), Vector3.Dot(Vector3.forward, pose.GetRow(1)), Vector3.Dot(Vector3.forward, pose.GetRow(2)));
        }
        
        /// <summary>
        /// 计算直线与平面的交点
        /// </summary>
        /// <param name="linePoint">直线上一点</param>
        /// <param name="lineDirection">直线方向</param>
        /// <param name="planeNormal">平面法向量</param>
        /// <param name="planePoint">平面上任意一点</param>
        /// <returns>交点的 position</returns>
        public static Vector3 GetIntersectWithLineAndPlane(Vector3 linePoint, Vector3 lineDirection, Vector3 planeNormal, Vector3 planePoint)
        {
            float d = Vector3.Dot(planePoint - linePoint, planeNormal) / Vector3.Dot(lineDirection.normalized, planeNormal);
 
            return d * lineDirection.normalized + linePoint;
        }
    }
}