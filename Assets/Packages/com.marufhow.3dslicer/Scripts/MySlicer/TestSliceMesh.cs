using System;
using UnityEngine;
using UnityEngine.AI;

namespace MySlicer
{
    public class TestSliceMesh : MonoBehaviour
    {
        [SerializeField] private Plane plane;
        [SerializeField] private MeshFilter _meshFilter;

        private Mesh _mesh;
        Vector3[] _vertices  ;
        int[] _triangles  ;
        
        private void Start()
        {
            
            
            GameObject visualPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            visualPlane.transform.position = transform.position;
            visualPlane.transform.rotation = Quaternion.Euler(90,0,0);
            visualPlane.transform.localScale = new Vector3(10, 10, 1); // Adjust the size (scale it to 10x10 units)
            
            // Vector3 pointA = new Vector3(0, 5, 0); // First point
            // Vector3 pointB = new Vector3(1, 5, 0); // Second point
            // Vector3 pointC = new Vector3(0, 5, 1); // Third point
            // plane = new Plane(pointA, pointB, pointC);

            // Create a plane using a normal and a point
            Vector3 normal = new Vector3(0, 1, 0); // Example normal vector
            Vector3 point = transform.position; // new Vector3(0, 0, 0);  // Example point on the plane

            Plane plane = new Plane(normal, point);
            
            _mesh = _meshFilter.mesh;
             _vertices = _mesh.vertices;
             _triangles = _mesh.triangles;
        }

        private void Update()
        {
            
            Ray ray = new Ray(new Vector3(0, 10, 0), Vector3.down); // A ray pointing downward from above
            float enter;

            if (plane.Raycast(ray, out enter)) {
                Vector3 hitPoint = ray.GetPoint(enter);
                Debug.Log("The ray hits the plane at: " + hitPoint);
            } else {
                Debug.Log("The ray does not intersect the plane.");
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(new Vector3(0, 10, 0), Vector3.down *100);
        }

        void SliceMesh( Plane slicePlane)
        {
           

            // List<Vector3> newVertices = new List<Vector3>();
            // List<int> newTriangles = new List<int>();

            // Loop through the original triangles
            for (int i = 0; i < _triangles.Length; i += 3)
            {
                // Get the current triangle's vertices
                Vector3 v1 = _vertices[_triangles[i]];
                Vector3 v2 = _vertices[_triangles[i + 1]];
                Vector3 v3 = _vertices[_triangles[i + 2]];

                // Check where the plane intersects each edge of the triangle
                bool v1Above = slicePlane.GetSide(v1);
                bool v2Above = slicePlane.GetSide(v2);
                bool v3Above = slicePlane.GetSide(v3);
                if(v1Above) Debug.Log( $"v1Above {v1Above}");
                if(v2Above) Debug.Log( $"v2Above {v2Above}");
                if(v3Above) Debug.Log( $"v1Above {v3Above}");
                // Slice the triangle by calculating intersection points and forming new triangles
                // Pseudo-code for handling intersection:
                // - Use line-plane intersection for edges: (v1, v2), (v2, v3), (v3, v1)
                // - Create new vertices at the intersection points
                // - Rebuild new triangles using these new vertices

                // (You'll need to implement the actual intersection logic here)
            }

            // Update the mesh with new data
            // mesh.vertices = newVertices.ToArray();
            // mesh.triangles = newTriangles.ToArray();
            // mesh.RecalculateNormals();
        }
    }
}