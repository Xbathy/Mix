using UnityEngine;

namespace Mix.Navigation
{
    public class NavAgent : MonoBehaviour
    {
        private Transform _camera;
        private void Awake()
        {
            if (Camera.main is { }) _camera = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 cameraPosition = _camera.position;
            Transform trans = transform;
            Vector3 pos = new Vector3(cameraPosition.x, trans.position.y, cameraPosition.z);
            trans.position = pos;
        }
    }
}
