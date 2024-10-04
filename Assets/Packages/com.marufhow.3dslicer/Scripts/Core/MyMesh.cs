using System;
using System.Collections.Generic;
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
       
        
       public List<Vector3> _vertices;
       public List<Vector3> _normals;
       public List<Vector2> _uvs;
       //public List<List<int>> _subMeshIndices = new List<List<int>>();
       public List<SubMeshIndices> _listOfSubMeshIndices = new List<SubMeshIndices>();
       
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

            if (_listOfSubMeshIndices.Count < triangle.SubMeshIndex + 1)
            {
                _listOfSubMeshIndices.Add(new SubMeshIndices()); 
              //  _subMeshIndices.Add(new List<int>());
            }
           // _subMeshIndices[triangle.SubMeshIndex].AddRange(new List<int>(3){v, v+1, v+2});
           _listOfSubMeshIndices[triangle.SubMeshIndex].indices.AddRange(new List<int> { v, v + 1, v + 2 });
           
        }

        public void Clear()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _listOfSubMeshIndices = new List<SubMeshIndices>();
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
    }
}
