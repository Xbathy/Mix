using UnityEngine;
using UnityEngine.Events;

namespace Mix.Utils
{
    public class KeyboardEvent : MonoBehaviour
    {
        public UnityEvent action;
        public KeyCode keyCode;
        
        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                action?.Invoke();
            }
        }
    }
}
