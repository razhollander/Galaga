using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreDomain.Scripts.Services.PointerInputService
{
    public class PointerInputService : IPointerInputService
    {
        public bool IsPointerOverGUI()
        {
            var isPointerOverGUI = false;
#if UNITY_ANDROID || UNITY_IOS
            foreach (Touch touch in Input.touches)
            {
                var id = touch.fingerId;
                var isTouchOverGUI = EventSystem.current.IsPointerOverGameObject(id);
                
                if (isTouchOverGUI)
                {
                    isPointerOverGUI = true;
                    break;
                }
            }
#elif UNITY_STANDALONE || UNITY_STANDALONE_OSX
            isPointerOverGUI = EventSystem.current.IsPointerOverGameObject();
#endif
            return isPointerOverGUI;
        }

        public bool IsPointerDown()
        {
            var zero = 0;
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount <= zero)
            {
                return false;
            }

            return Input.GetTouch(zero).phase == TouchPhase.Ended;
#elif UNITY_STANDALONE || UNITY_STANDALONE_OSX
            return Input.GetMouseButtonDown(zero);
#endif
        }
    }
}
