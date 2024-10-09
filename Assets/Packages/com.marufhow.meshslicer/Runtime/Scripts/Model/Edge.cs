using System.Collections.Generic;
using UnityEngine;

namespace com.marufhow.meshslicer.model
{
    public class Edge
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }
        public int SubMeshIndex { get; set; }

        public Edge()
        {
            Vertices = new List<Vector3>(2) { Vector3.zero, Vector3.zero };  
            Normals = new List<Vector3>(2) { Vector3.zero, Vector3.zero };  
            UVs = new List<Vector2>(2) { Vector2.zero, Vector2.zero };  
        }
    }
}