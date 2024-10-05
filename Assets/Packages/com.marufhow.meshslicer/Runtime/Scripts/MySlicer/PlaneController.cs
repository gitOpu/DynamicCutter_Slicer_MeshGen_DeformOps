using System;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

namespace MySlicer
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        
        private Plane _plane;
        [Range(0,20)]
        [SerializeField] private float _rayRange = 1f;
        private void Start()
        {
            Debug.Log($" _meshFilter.mesh.subMeshCount { _meshFilter.mesh.subMeshCount}");
            Debug.Log($" _meshFilter.mesh.triangles.Length { _meshFilter.mesh.triangles.Length}");
            Debug.Log($" _meshFilter.mesh.GetTriangles(0) { _meshFilter.mesh.GetTriangles(0).Length}");
            
          // _plane = new Plane(Vector3.right, Vector3.zero);
           // _plane = new Plane(Vector3.up, 0);
          // Debug.Log(_plane.GetSide(Vector3.right * 1));
          // Debug.Log(_plane.GetSide(Vector3.left * 1));
           // Debug.Log( $"  _plane.distance right { _plane.ClosestPointOnPlane(Vector3.one)}");
           // Debug.Log( $"  _plane.distance left { _plane.ClosestPointOnPlane(Vector3.left)}");
        }
        
        private void Update()
        {
            // if (_plane.Raycast(new Ray(Vector3.one, Vector3.down ), out float enter))
            // {
            //     Debug.Log($" Out : {enter}");
            // }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(Vector3.right, 0.1f);
            Gizmos.DrawWireSphere(Vector3.left , 0.1f);
            Debug.DrawLine(Vector3.zero, Vector3.right, Color.red);
           // Debug.DrawLine(Vector3.one, Vector3.one + Vector3.down * _rayRange, Color.cyan);
        }
    }

}