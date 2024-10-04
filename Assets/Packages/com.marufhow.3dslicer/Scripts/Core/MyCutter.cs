using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    public class MyCutter: MonoBehaviour
    {
        [SerializeField] private MyMesh leftMesh;
        
        public void Cut(GameObject cutObject, Vector3 pointToCut)
        {
            leftMesh.Clear();
            
            var mesh = cutObject.GetComponent<MeshFilter>().mesh;
            
            var subMeshCount = mesh.subMeshCount;

            for (int i = 0; i < subMeshCount; i++)
            {
                var subMesh = mesh.GetSubMesh(i);
                for (int j = 0; j <  mesh.triangles.Length-1; j += 3)
                {
                    var tris1 = mesh.triangles[j];
                    var tris2 = mesh.triangles[j+1];
                    var tris3 = mesh.triangles[j+2];

                  var triangle = new Triangle(
                        mesh.vertices[tris1], mesh.vertices[tris2], mesh.vertices[tris3],
                        mesh.normals[tris1], mesh.normals[tris2], mesh.normals[tris3],
                        mesh.uv[tris1], mesh.uv[tris2], mesh.uv[tris3],
                        i
                    );
                    
                    leftMesh.AddTriangle(triangle);
                }

             

            }



        }
    }
}