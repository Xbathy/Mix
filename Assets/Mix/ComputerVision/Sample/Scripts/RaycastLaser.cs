using UnityEngine;

namespace Mix.ComputerVision.Sample
{
    public class RaycastLaser : MonoBehaviour
    {
        public float lineWidthMultiplier = 0.05f;
        public Material defaultLineMaterial;
        
        public void Shoot(Vector3 from, Vector3 direction, float length, Material mat=null)
        {
            LineRenderer laser = new GameObject().AddComponent<LineRenderer>();
            laser.widthMultiplier = lineWidthMultiplier;

            laser.material = mat == null ? defaultLineMaterial : mat;

            Ray ray = new Ray(from, direction);
            Vector3 to = from + length * direction;

            //Use this code when hit on mesh surface
            if(Physics.Raycast(ray, out RaycastHit hit, length))
                to = hit.point;

            laser.SetPosition(0, from);
            laser.SetPosition(1, to);
        }
    }
}
