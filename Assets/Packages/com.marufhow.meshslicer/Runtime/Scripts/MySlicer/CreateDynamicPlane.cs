using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateDynamicPlane : MonoBehaviour
{
    [FormerlySerializedAs("rows")] public int _rows = 5;    // Number of rows of vertices
    [FormerlySerializedAs("columns")] public int _columns = 10; // Number of columns of vertices
    public float size = 1f;  // Size of the plane

    void Start()
    {
       
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Calculate the number of vertices
        Vector3[] vertices = new Vector3[(_rows + 1) * (_columns + 1)];
        Vector2[] uv = new Vector2[(_rows + 1) * (_columns + 1)];
        int[] triangles = new int[_rows * _columns * 6]; 
        
        for (int i = 0, r = 0; r <= _rows; r++)
        {
            for (int c = 0; c <= _columns; c++, i++)
            {
                vertices[i] = new Vector3(c , 0, r );   
                uv[i] = new Vector2((float)c / _columns, (float)r / _rows);   
            }
        }

        // Generate triangles
        int tris = 0;
        for (int y = 0, vert = 0; y < _rows; y++, vert++)
        {
            for (int x = 0; x < _columns; x++, vert++)
            {
                // First triangle of the quad
                triangles[tris] = vert;
                triangles[tris + 1] = vert + _columns + 1;
                triangles[tris + 2] = vert + 1;

                // Second triangle of the quad
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + _columns + 1;
                triangles[tris + 5] = vert + _columns + 2;

                tris += 6; // Move to the next quad
            }
        }

        // Assign the vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        // Recalculate normals and bounds for proper rendering
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
