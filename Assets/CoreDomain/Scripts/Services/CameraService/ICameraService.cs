using UnityEngine;

namespace CoreDomain.Scripts.Services.CameraService
{
    public interface ICameraService
    {
        Vector3 WorldToScreenPoint(CameraType type, Vector3 worldPoint);
        Vector3 WorldToViewPortPoint(CameraType type, Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(CameraType type, Vector3 screenPoint);
        Vector3 ScreenToViewPort(CameraType type, Vector3 screenPoint);
        bool ScreenPointToRay(CameraType type, out RaycastHit hit);
        bool ScreenPointToRay(CameraType type, LayerMask layerMask, out RaycastHit hit);
        bool ScreenCenterToRay(CameraType type, out RaycastHit hit);
        void SetCanvasCamera(CameraType type, UnityEngine.Canvas canvas);
        Vector2 ScreenPointToCanvasInCameraSpacePoint(RectTransform rect, Vector2 screenPoint, CameraType cameraType);
        float GetAspectRatio(CameraType type);
        Vector3 GetCameraPosition(CameraType type);
        Vector3 GetCameraForward(CameraType type);
        Vector3 GetCameraRight(CameraType type);
        bool IsPointOnScreen(CameraType type, Vector3 point);
    }
}