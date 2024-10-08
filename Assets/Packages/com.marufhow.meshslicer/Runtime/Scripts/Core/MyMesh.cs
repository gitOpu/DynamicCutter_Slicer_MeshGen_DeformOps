using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    [Serializable]
    public class SubMeshIndices
    {
        public List<int> indices = new List<int>();
    }
    
    [Serializable]
    public class MyMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
       public List<Vector3> _vertices;
       public List<Vector3> _normals;
       public List<Vector2> _uvs;
       public List<SubMeshIndices> _listOfSubMeshIndices = new List<SubMeshIndices>();
       //public List<List<int>> _subMeshIndices = new List<List<int>>();
       
        /*public List<Vector3> Vertices = new List<Vector3>();
        public List<Vector3> Normals = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();
        public List<List<int>> SubMeshIndices = new List<List<int>>();*/
       
        /*public List<Vector3> Vertices { get => _vertices; set => _vertices = value; }
        public List<Vector3> Normals { get => _normals; set => _normals = value; }
        public List<Vector2> UVs { get => _uvs; set => _uvs = value; }
        public List<List<int>> SubMeshIndices { get => _subMeshIndices; set => _subMeshIndices = value; }*/
        

        // Method to add a triangle
        public void AddTriangle(Triangle triangle)
        {
            var v = _vertices.Count;
            _vertices.AddRange(triangle.Vertices); 
            _normals.AddRange(triangle.Normals);
            _uvs.AddRange(triangle.UVs);

            // if (_listOfSubMeshIndices.Count < triangle.SubMeshIndex + 1)
            // {
            //     _listOfSubMeshIndices.Add(new SubMeshIndices()); 
            //  
            // }
            // _listOfSubMeshIndices[triangle.SubMeshIndex].indices.AddRange(new List<int> { v, v + 1, v + 2 });
            if(_listOfSubMeshIndices.Count < triangle.SubMeshIndex + 1)
            {
                for (int i = _listOfSubMeshIndices.Count; i < triangle.SubMeshIndex + 1; i++)
                {
                    _listOfSubMeshIndices.Add(new SubMeshIndices());
                }
            }

            for (int i = 0; i < 3; i++)
            {
                _listOfSubMeshIndices[triangle.SubMeshIndex].indices.Add(v + i);
            }
           
        }

        public void Clear()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _listOfSubMeshIndices = new List<SubMeshIndices>();
        }

        public void GenerateMesh()
        {
            Mesh mesh = new Mesh();
            _meshFilter.mesh = mesh;

            // mesh.vertices = _vertices.ToArray();
            // mesh.normals = _normals.ToArray();
            // mesh.uv = _uvs.ToArray();
           
            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetUVs(0, _uvs);
            mesh.SetUVs(1, _uvs);
            mesh.subMeshCount = _listOfSubMeshIndices.Count;
            for (int i = 0; i < _listOfSubMeshIndices.Count; i++)
            {
                mesh.SetTriangles(_listOfSubMeshIndices[i].indices, i);
            }
            
            var collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
            
            Material[] mats = new Material[mesh.subMeshCount];
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                mats[i] = _meshRenderer.material;
            }
            _meshRenderer.materials = mats;
            
            var rightRigidbody = gameObject.AddComponent<Rigidbody>();
            rightRigidbody.AddTorque(Vector3.up * 5f, ForceMode.Impulse);
            
        }
    }

    // Triangle class definition
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

        public Triangle(Vector3[] _vertices, Vector3[] _normals, Vector2[] _uvs, int _submeshIndex)
        {
            Vertices = _vertices.ToList();
            Normals = _normals.ToList();
            UVs = _uvs.ToList();
            SubMeshIndex = _submeshIndex;
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
        public List<Vector2> UV{ get; set; } = new(3) { Vector2.zero, Vector2.zero, Vector2.zero };
    }
    
    public class Edge
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }
        public int SubMeshIndex { get; set; }
        public Edge()
        {
          
            Vertices = new List<Vector3>(2) { Vector3.zero, Vector3.zero }; // Two empty vertices
            Normals = new List<Vector3>(2) { Vector3.zero, Vector3.zero }; // Two empty normals
            UVs = new List<Vector2>(2) { Vector2.zero, Vector2.zero }; // Two empty UVs
        }
    }
}
