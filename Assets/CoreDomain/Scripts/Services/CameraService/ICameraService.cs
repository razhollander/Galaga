using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain
{
    public interface ICameraService
    {
        Vector3 WorldToScreenPoint(GameCameraType type, Vector3 worldPoint);
        Vector3 WorldToViewPortPoint(GameCameraType type, Vector3 worldPoint);
        Vector3 ScreenToWorldPoint(GameCameraType type, Vector3 screenPoint);
        Vector3 ScreenToViewPort(GameCameraType type, Vector3 screenPoint);
        bool ScreenPointToRay(GameCameraType type, out RaycastHit hit);
        bool ScreenPointToRay(GameCameraType type, LayerMask layerMask, out RaycastHit hit);
        bool ScreenCenterToRay(GameCameraType type, out RaycastHit hit);
        void SetCanvasCamera(GameCameraType type, UnityEngine.Canvas canvas);
        Vector2 ScreenPointToCanvasInCameraSpacePoint(GameCameraType type, RectTransform rect, Vector2 screenPoint);
        float GetAspectRatio(GameCameraType type);
        Vector3 GetCameraPosition(GameCameraType type);
        Vector3 GetCameraForward(GameCameraType type);
        Vector3 GetCameraRight(GameCameraType type);
        bool IsPointOnScreen(GameCameraType type, Vector3 point);
    }
}