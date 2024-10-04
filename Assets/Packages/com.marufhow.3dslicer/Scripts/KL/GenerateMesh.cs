using System.Collections.Generic;
using UnityEngine;

namespace KL
{
    public class GenerateMesh : MonoBehaviour
    {
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();
        private List<List<int>> submeshIndices = new List<List<int>>();

        // Public properties with get and set (single-line)
        public List<Vector3> Vertices { get => vertices; set => vertices = value; }
        public List<Vector3> Normals { get => normals; set => normals = value; }
        public List<Vector2> UVs { get => uvs; set => uvs = value; }
        public List<List<int>> SubmeshIndices { get => submeshIndices; set => submeshIndices = value; }

        public void AddTriangle(MeshTriangle meshTriangle)
        {
            int currentVerticesCount = vertices.Count;
            
            vertices.AddRange(meshTriangle.Vertices);
            normals.AddRange(meshTriangle.Normals);
            uvs.AddRange(meshTriangle.UVs);

            if (submeshIndices.Count < meshTriangle.SubmeshIndex + 1)
            {
                for (int i = submeshIndices.Count; i < meshTriangle.SubmeshIndex + 1; i++)
                {
                    submeshIndices.Add(new List<int>());
                }
            }

            for (int i = 0; i < 3; i++)
            {
                submeshIndices[meshTriangle.SubmeshIndex].Add(currentVerticesCount + i);
            }
        }
    }
}