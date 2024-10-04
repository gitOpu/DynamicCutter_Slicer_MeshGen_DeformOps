using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace KL
{
    public class MeshTriangle : MonoBehaviour
    {
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();
        private int submeshIndex;

        // Public properties with get and set (single-line)
        public List<Vector3> Vertices { get => vertices; set => vertices = value; }
        public List<Vector3> Normals { get => normals; set => normals = value; }
        public List<Vector2> UVs { get => uvs; set => uvs = value; }
        public int SubmeshIndex { get => submeshIndex; set => submeshIndex = value; }

        // Constructor
        public MeshTriangle(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, int submeshIndex)
        {
            Clear();
            this.vertices = vertices;
            this.normals = normals;
            this.uvs = uvs;
            this.submeshIndex = submeshIndex;
        }

        private void Clear()
        {
            vertices.Clear();
            normals.Clear();
            uvs.Clear();
            submeshIndex = 0;
        }
    }
}