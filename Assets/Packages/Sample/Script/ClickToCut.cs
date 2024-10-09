using com.marufhow.meshslicer.core;
using UnityEngine;
using UnityEngine.Serialization;

public class ClickToCut : MonoBehaviour
{
    [FormerlySerializedAs("_myCutter")] [SerializeField]
    private MHCutter mhCutter;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                mhCutter.Cut(hit.collider.gameObject, hit.point, Vector3.right);
            }
        }
    }
}


 