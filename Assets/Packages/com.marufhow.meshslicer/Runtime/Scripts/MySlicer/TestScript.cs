using System;
using UnityEngine;

namespace MySlicer
{
    public class TestScript: MonoBehaviour
    {
        public Transform _go;
        private void Update()
        {
            Transform transform1;
            var x =  (transform1 = transform).InverseTransformDirection(_go.position);
          transform1.position = x;
        }
    }
}