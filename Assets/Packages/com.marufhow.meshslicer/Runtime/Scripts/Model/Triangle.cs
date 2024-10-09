using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.marufhow.meshslicer.model
{
    public class Triangle
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }
        public int SubMeshIndex { get; set; }

        // Constructor that takes 3 vertices, 3 normals, 3 uvs, and 1 submesh index
        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3,
            Vector3 n1, Vector3 n2, Vector3 n3,
            Vector2 uv1, Vector2 uv2, Vector2 uv3,
            int subMeshIndex)
        {
            Vertices = new List<Vector3> { v1, v2, v3 };
            Normals = new List<Vector3> { n1, n2, n3 };
            UVs = new List<Vector2> { uv1, uv2, uv3 };
            SubMeshIndex = subMeshIndex;
        }
        public Triangle(TriangleVertex vertexData, TriangleNormal normalData, TriangleUVs uvData, int subMeshIndex)
        {
            // Initialize using the data from the input classes
            Vertices = vertexData.Vertices;
            Normals = normalData.Normals;
            UVs = uvData.UV;
            SubMeshIndex = subMeshIndex;
        }
    }

    public class TriangleVertex
    {
        public List<Vector3> Vertices { get; set; } = new(3) { Vector3.zero, Vector3.zero, Vector3.zero };
    }

    public class TriangleNormal
    {
        public List<Vector3> Normals { get; set; } = new(3) { Vector3.zero, Vector3.zero, Vector3.zero };
    }

    public class TriangleUVs
    {
        public List<Vector2> UV { get; set; } = new(3) { Vector2.zero, Vector2.zero, Vector2.zero };
    }
}