using com.marufhow.meshslicer.core;
using UnityEngine;

namespace com.marufhow.meshslicer.demo
{
    public class ClickToCut : MonoBehaviour
    {
        [Header("Click to cut target vertically. Press SHIFT to cut horizontally")] [SerializeField]
        private MHCutter _mhCutter;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Ground")) return;

                    // Check if the Shift key is held down
                    Vector3 cutDirection = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                        ? Vector3.up
                        : Vector3.right;

                    _mhCutter.Cut(hit.collider.gameObject, hit.point, cutDirection);
                }
            }
        }



    }
}


 