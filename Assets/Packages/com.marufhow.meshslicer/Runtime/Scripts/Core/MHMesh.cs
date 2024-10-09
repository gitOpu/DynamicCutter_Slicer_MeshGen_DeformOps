using System;
using System.Collections.Generic;
using System.Linq;
using com.marufhow.meshslicer.model;
using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    [Serializable] public class SubMeshIndices
    {
        public List<int> indices = new();
    }
    public class MHMesh : MonoBehaviour
    {
        
        [Header("Only for observe in Inspector, not for set anything")]
        public List<Vector3> _vertices = new List<Vector3>();
        public List<Vector3> _normals = new List<Vector3>();
        public List<Vector2> _uvs = new List<Vector2>();
        public List<SubMeshIndices> _listOfSubMeshIndices = new();
        
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private List<Material> _materials;
        public List<Material> Materials => _materials;
        private void OnEnable()
        {
            // Check if the MeshFilter is attached, if not, add it.
            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
                if (_meshFilter == null)
                {
                    _meshFilter = gameObject.AddComponent<MeshFilter>();
                }
            }

            // Check if the MeshRenderer is attached, if not, add it.
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
                if (_meshRenderer == null)
                {
                    _meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
            }
            
            Collider[] allColliders = gameObject.GetComponents<Collider>();
            foreach (var collider in allColliders)
            {
                if (!(collider is MeshCollider))
                {
                    Destroy(collider);
                }
                else _meshCollider = GetComponent<MeshCollider>();
            }
            if (_meshCollider == null)
            {
                _meshCollider = gameObject.AddComponent<MeshCollider>();
            }
            _meshCollider.sharedMesh = _meshFilter.mesh;
            _meshCollider.convex = true;
            
             
            _materials = _meshRenderer.materials.ToList();
             
        }
        public void AddTriangle(Triangle triangle)
        {
            var v = _vertices.Count;
            _vertices.AddRange(triangle.Vertices);
            _normals.AddRange(triangle.Normals);
            _uvs.AddRange(triangle.UVs);

           
            if (_listOfSubMeshIndices.Count < triangle.SubMeshIndex + 1)
                for (var i = _listOfSubMeshIndices.Count; i < triangle.SubMeshIndex + 1; i++)
                {
                    _listOfSubMeshIndices.Add(new SubMeshIndices());
                }
            for (var i = 0; i < 3; i++) _listOfSubMeshIndices[triangle.SubMeshIndex].indices.Add(v + i);
        }

        public void Clear()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _listOfSubMeshIndices = new List<SubMeshIndices>();
        }

        public void GenerateMesh(List<Material> mats = null)
        {
            var mesh = new Mesh();
            _meshFilter.mesh = mesh;
            
            mesh.SetVertices(_vertices);
            mesh.SetNormals(_normals);
            mesh.SetUVs(0, _uvs);
            mesh.SetUVs(1, _uvs);
            mesh.subMeshCount = _listOfSubMeshIndices.Count;
            for (var i = 0; i < _listOfSubMeshIndices.Count; i++)
                mesh.SetTriangles(_listOfSubMeshIndices[i].indices, i);


            // Update Visual 
            if (mats != null)
            {
                _materials = mats;
            }
          
            if (_materials != null && _materials.Count < mesh.subMeshCount)
            {
                for (var i = 0; i < mesh.subMeshCount; i++)
                {
                    var materials = _meshRenderer.materials;
                    var lastMat = materials[^1];
                    if(i >= _materials.Count)
                        _materials.Add(lastMat);
                }
            }

            if (_materials != null) _meshRenderer.materials = _materials.ToArray();


            _meshCollider.sharedMesh = mesh;
            _meshCollider.convex = true;
            
           
             
        }

     
    }

    
}