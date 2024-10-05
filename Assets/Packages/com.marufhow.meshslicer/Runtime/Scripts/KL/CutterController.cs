using System.Collections.Generic;
using UnityEngine;

namespace KL
{
    public static class CutterController
    {
        public static bool currentlyCutting;
        public  static Mesh originalMesh;

        public static void Cut(GameObject _originalGameObject, Vector3 contactPoint, Vector3 direction,
            Material cutMaterial = null, bool fill = true, bool addRigidbody = false)
        {
            if(currentlyCutting) return;
            currentlyCutting = true;
            Plane plane = new Plane(
                _originalGameObject.transform.InverseTransformDirection(-direction),
                _originalGameObject.transform.InverseTransformPoint(contactPoint));
            originalMesh = _originalGameObject.GetComponent<MeshFilter>().mesh;

            List<Vector3> addVertices = new List<Vector3>();
            GenerateMesh leftMesh = new GenerateMesh();
            GenerateMesh  rightMesh = new GenerateMesh();
            int[] submeshIndices;
            int triangleIndexA, triangleIndexB, triangleIndexC;

            for (int i = 0; i < originalMesh.subMeshCount; i++)
            {
                
                submeshIndices = originalMesh.GetTriangles(i);

                for (int j = 0; j < submeshIndices.Length; j += 3)
                {
                    triangleIndexA = submeshIndices[j];
                    triangleIndexB = submeshIndices[j+1];
                    triangleIndexC = submeshIndices[j+2];

                    MeshTriangle currentTriangle =  GetTriangle(triangleIndexA, 
                        triangleIndexB, triangleIndexC, i);
                  
                    bool triangleALeftSide = plane.GetSide(originalMesh.vertices[triangleIndexA]);
                    bool triangleBLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexB]);
                    bool triangleCLeftSide = plane.GetSide(originalMesh.vertices[triangleIndexC]);

                    if (triangleALeftSide && triangleBLeftSide && triangleCLeftSide)
                    {
                      leftMesh.AddTriangle(currentTriangle);
                    }
                    else if (!triangleALeftSide && !triangleBLeftSide && !triangleCLeftSide)
                    {
                        rightMesh.AddTriangle(currentTriangle);
                    }
                }
            }
            
        }

        private static MeshTriangle GetTriangle(int triangleIndexA, int triangleIndexB, int triangleIndexC, int i)
        {
            return null;
            //   return new MeshTriangle(triangleIndexA, triangleIndexB, triangleIndexC, i);
        }
    }
}