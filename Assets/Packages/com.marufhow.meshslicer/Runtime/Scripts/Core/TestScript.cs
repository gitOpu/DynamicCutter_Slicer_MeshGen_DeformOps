using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    public class TestScript : MonoBehaviour
    {
        public Transform target;
        public Vector3 vec3 = Vector3.up;
        void Update()
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
        }
    }
}