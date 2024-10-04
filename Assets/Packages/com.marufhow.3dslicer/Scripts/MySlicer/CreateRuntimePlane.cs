using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateRuntimePlane : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Define the vertices for the plane (a quad)
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),    // Bottom-left
            new Vector3(1, 0, 0),    // Bottom-right
            new Vector3(0, 0, 1),    // Top-left
            new Vector3(1, 0, 1)     // Top-right
        };

        // Define the triangles that make up the quad
        int[] triangles = new int[6]
        {
            // First triangle (bottom-left, top-left, bottom-right)
            0, 2, 1,
            // Second triangle (top-left, top-right, bottom-right)
            2, 3, 1
        };

        // Define the UV coordinates for texturing the quad
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),    // Bottom-left
            new Vector2(1, 0),    // Bottom-right
            new Vector2(0, 1),    // Top-left
            new Vector2(1, 1)     // Top-right
        };

        // Assign the vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // Recalculate normals and bounds for proper rendering
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}