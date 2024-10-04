using System;
using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    public class ClickToCut: MonoBehaviour
    {
        [SerializeField] private GameObject _myPlane;
        [SerializeField] private MyCutter _myCutter;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
               if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
               {
                   _myPlane.transform.position = hit.point;
                   _myCutter.Cut(hit.collider.gameObject, hit.point);
                   Debug.Log($"Hit Game object {hit.collider.gameObject.name}");
               }
            }
        }
    }
}