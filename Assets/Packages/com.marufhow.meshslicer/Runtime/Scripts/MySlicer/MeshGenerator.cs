using System;
using UnityEngine;

namespace MySlicer
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshGenerator : MonoBehaviour
    {
        [SerializeField] private int _rows = 1; // Vertical value (how many y or z)
        [SerializeField] private int _columns = 1; // Horizontal value (how many x points)
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            Mesh mesh = new Mesh();
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter.mesh = mesh;

            // Calculate the number of vertices
            Vector3[] vertices = new Vector3[(_rows + 1) * (_columns + 1)];
            Vector2[] uv = new Vector2[(_rows + 1) * (_columns + 1)];
            Vector3[] normals = new Vector3[(_rows + 1) * (_columns + 1)]; // Normal array
            int[] triangles = new int[(_rows * _columns) * 6];

            // Generate vertices, UVs, and normals
            for (int i = 0, r = 0; r <= _rows; r++)
            {
                for (int c = 0; c <= _columns; c++, i++)
                {
                    vertices[i] = new Vector3(c, 0, r);
                    uv[i] = new Vector2((float)c / _columns, (float)r / _rows);
                    normals[i] = Vector3.up; // All normals point upwards (y-axis)
                }
            }

            // Generate triangles
            int triangle = 0;
            for (int r = 0, v = 0; r < _rows; r++, v++)
            {
                for (int c = 0; c < _columns; c++, v++)
                {
                    // First triangle of the quad
                    triangles[triangle] = v;
                    triangles[triangle + 1] = v + _columns + 1;
                    triangles[triangle + 2] = v + 1;

                    // Second triangle of the quad
                    triangles[triangle + 3] = v + 1;
                    triangles[triangle + 4] = v + _columns + 1;
                    triangles[triangle + 5] = v + _columns + 2;

                    triangle += 6; // Move to the next quad
                }
            }

            // Assign the vertices, triangles, UVs, and normals to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.normals = normals; // Assign the normals array

            // Recalculate bounds for proper rendering
            mesh.RecalculateBounds();

            Debug.Log($" _meshFilter { _meshFilter.mesh.triangles.Length}");
        }
    }
}
