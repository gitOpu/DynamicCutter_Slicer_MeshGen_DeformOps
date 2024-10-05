using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    public class MyCutter : MonoBehaviour
    {
        [SerializeField] private MyMesh _leftMesh;
        [SerializeField] private MyMesh _rightMesh;

        private GameObject _visualPlane;
        public List<Vector3> addedVertices;
        private void Start()
        {
            _visualPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _visualPlane.transform.position = new Vector3(100, 0, 0);
        }

        public void Cut(GameObject cutObject,Vector3 pointToCut, Vector3 cutNormal)
        {
          
            
           Plane plane = new Plane(Vector3.right, Vector3.zero);
          // Plane plane = new Plane(cutObject.transform.InverseTransformDirection( Vector3.right), cutObject.transform.InverseTransformPoint(pointToCut));
          // Plane plane = new Plane(cutObject.transform.InverseTransformDirection(-cutNormal),  cutObject.transform.InverseTransformPoint(pointToCut));
           
           // VisiblePlane(plane, pointToCut);
            
            Mesh mesh = cutObject.GetComponent<MeshFilter>().mesh;
            
            addedVertices = new List<Vector3>();
            _leftMesh.Clear();
            _rightMesh.Clear();

            for (var i = 0; i < mesh.subMeshCount; i++)
            {
                // Get triangles from a mesh : Another way
                var subMeshTriangles = mesh.GetTriangles(i); // its also return same triangles of current subMesh
                for (var j = 0; j < subMeshTriangles.Length; j += 3)
                {
                    var trisA = subMeshTriangles[j];
                    var trisB = subMeshTriangles[j + 1];
                    var trisC = subMeshTriangles[j + 2];
                  
                    Triangle triangle = GetTriangle( mesh,  trisA,  trisB,  trisC,  i); // format triangle from mesh data

                    bool isLeftSideTrisA = plane.GetSide(cutObject.transform.TransformPoint(mesh.vertices[trisA]));
                    bool isLeftSideTrisB = plane.GetSide(cutObject.transform.TransformPoint(mesh.vertices[trisB]));
                    bool isLeftSideTrisC = plane.GetSide(cutObject.transform.TransformPoint(mesh.vertices[trisC]));

 
                    if (isLeftSideTrisA == isLeftSideTrisB && isLeftSideTrisB == isLeftSideTrisC)
                    {
                        // All vertices are on the same side, either fully left or fully right
                        if (isLeftSideTrisA)
                        {
                            _leftMesh.AddTriangle(triangle);
                        }
                        else
                        {
                            _rightMesh.AddTriangle(triangle);
                        }
                    }else
                    {
                        // Triangle intersects the cutting plane, so split it
                        List<bool> isLeftSidedPoints = new List<bool>(3) { isLeftSideTrisA, isLeftSideTrisB, isLeftSideTrisC };
                        CutTriangle(plane, triangle, isLeftSidedPoints, i);
                    }
                    _leftMesh.GenerateMesh();
                    _rightMesh.GenerateMesh();
                    cutObject.SetActive(false);
                }
            }
        }
        public Triangle GetTriangle( Mesh mesh, int trisA, int trisB, int trisC, int subMeshIndex)
        {
            return new Triangle(
                mesh.vertices[trisA], mesh.vertices[trisB], mesh.vertices[trisC],
                mesh.normals[trisA], mesh.normals[trisB], mesh.normals[trisC],
                mesh.uv[trisA], mesh.uv[trisB], mesh.uv[trisC],
                subMeshIndex
            );
             
        }
        private void CutTriangle(Plane plane, Triangle triangle, List<bool> isLeftSidedPoints, int subMeshIndex)
        {
            bool isLeft = false;
            bool isRight = false;

            Edge leftEdge = new Edge();
            Edge rightEdge = new Edge();
            
            // Create four points from 3 triangle vertex
            for (int i = 0; i < isLeftSidedPoints.Count; i++)
            {
                if(isLeftSidedPoints[i]) // isLeft true
                {
                    if (!isLeft)
                    {
                        isLeft = true;
                        leftEdge.Vertices[0] = triangle.Vertices[i];
                        leftEdge.Vertices[1] = leftEdge.Vertices[0];
                        
                        leftEdge.Normals[0] = triangle.Normals[i];
                        leftEdge.Normals[1] = leftEdge.Normals[0];
                        
                        leftEdge.UVs[0] = triangle.UVs[i];
                        leftEdge.UVs[1] =  leftEdge.UVs[0];

                    } 
                    else{
                        leftEdge.Vertices[1] = triangle.Vertices[i];
                        leftEdge.Normals[1] = triangle.Normals[i];
                        leftEdge.UVs[1] =  triangle.UVs[i];

                    }
                    
                } else // isRight true
                {
                    if (!isRight)
                    {
                        isRight = true;
                        
                        rightEdge.Vertices[0] = triangle.Vertices[i];
                        rightEdge.Vertices[1] = rightEdge.Vertices[0];
                        
                        rightEdge.Normals[0] = triangle.Normals[i];
                        rightEdge.Normals[1] = rightEdge.Normals[0];
                        
                        rightEdge.UVs[0] = triangle.UVs[i];
                        rightEdge.UVs[1] =  rightEdge.UVs[0];

                    } 
                    else{
                        rightEdge.Vertices[1] = triangle.Vertices[i];
                        rightEdge.Normals[1] = triangle.Normals[i];
                        rightEdge.UVs[1] =  triangle.UVs[i];

                    }
                    
                }
            }
            
            // Find two cut points on left right 
            float distance;
            Ray rayL0R0 = new Ray(leftEdge.Vertices[0], rightEdge.Vertices[0] - leftEdge.Vertices[0]);

            plane.Raycast(rayL0R0, out distance);
             
                float normalizedDistanceL0R0 = distance / Vector3.Distance(rightEdge.Vertices[0], leftEdge.Vertices[0]);
                Vector3 vertexOnL0R0 =  Vector3.Lerp(leftEdge.Vertices[0], rightEdge.Vertices[0], normalizedDistanceL0R0);
                Vector3 normalOnL0R0 =  Vector3.Lerp(leftEdge.Normals[0], rightEdge.Normals[0], normalizedDistanceL0R0);
                Vector3 uvOnL0R0 =  Vector3.Lerp(leftEdge.UVs[0], rightEdge.UVs[0], normalizedDistanceL0R0);
                addedVertices.Add(vertexOnL0R0);
            
            
            Ray rayL1R1 = new Ray(leftEdge.Vertices[1], rightEdge.Vertices[1] - leftEdge.Vertices[1]);
            plane.Raycast(rayL1R1, out distance);
            
                float normalizedDistanceL1R1 = distance / Vector3.Distance(rightEdge.Vertices[1], leftEdge.Vertices[1]);
                Vector3 vertexOnL1R1 =  Vector3.Lerp(leftEdge.Vertices[1], rightEdge.Vertices[1], normalizedDistanceL1R1);
                Vector3 normalOnL1R1 =  Vector3.Lerp(leftEdge.Normals[1], rightEdge.Normals[1], normalizedDistanceL1R1);
                Vector3 uvOnL1R1 =  Vector3.Lerp(leftEdge.UVs[1], rightEdge.UVs[1], normalizedDistanceL1R1);
                addedVertices.Add(vertexOnL1R1);

                // Add 1st vertex to left mesh
                if (leftEdge.Vertices[0] != vertexOnL0R0 && leftEdge.Vertices[0] != vertexOnL1R1)
                {
                    Triangle tris = new Triangle(
                        leftEdge.Vertices[0], vertexOnL0R0, vertexOnL1R1,
                        leftEdge.Normals[0], normalOnL0R0, normalOnL1R1,
                        leftEdge.UVs[0], uvOnL0R0, uvOnL1R1,
                        subMeshIndex
                    );
                    if (Vector3.Dot(
                            Vector3.Cross(tris.Vertices[1] - tris.Vertices[0], tris.Vertices[2] - tris.Vertices[0]),
                            tris.Normals[0]) < 0)  FlipTriangle(tris);
                    _leftMesh.AddTriangle(tris);
                }
                // Add 2nd vertex to left mesh
                if (leftEdge.Vertices[0] != leftEdge.Vertices[1] && leftEdge.Vertices[0] != vertexOnL1R1)
                {
                    Triangle tris = new Triangle(
                        leftEdge.Vertices[0], leftEdge.Vertices[1], vertexOnL1R1,
                        leftEdge.Normals[0], leftEdge.Normals[1], normalOnL1R1,
                        leftEdge.UVs[0], leftEdge.UVs[1], uvOnL1R1,
                        
                        subMeshIndex
                    );
                    if (Vector3.Dot(
                            Vector3.Cross(tris.Vertices[1] - tris.Vertices[0], tris.Vertices[2] - tris.Vertices[0]),
                            tris.Normals[0]) < 0)  FlipTriangle(tris);
                    _leftMesh.AddTriangle(tris);
                }
                // Add 3rd vertex to right mesh
                if (rightEdge.Vertices[0] != vertexOnL0R0 && rightEdge.Vertices[0] != vertexOnL1R1)
                {
                    Triangle tris = new Triangle(
                        rightEdge.Vertices[0], vertexOnL0R0, vertexOnL1R1,
                        rightEdge.Normals[0], normalOnL0R0, normalOnL1R1,
                        rightEdge.UVs[0], uvOnL0R0, uvOnL1R1,
                        subMeshIndex
                    );
                    if (Vector3.Dot(
                            Vector3.Cross(tris.Vertices[1] - tris.Vertices[0], tris.Vertices[2] - tris.Vertices[0]),
                            tris.Normals[0]) < 0)  FlipTriangle(tris);
                    _rightMesh.AddTriangle(tris);
                }
                // Add 4th vertex to right mesh
                if (rightEdge.Vertices[0] != rightEdge.Vertices[1] && rightEdge.Vertices[0] != vertexOnL1R1)
                {
                    Triangle tris = new Triangle(
                        rightEdge.Vertices[0], rightEdge.Vertices[1], vertexOnL1R1,
                        rightEdge.Normals[0], rightEdge.Normals[1], normalOnL1R1,
                        rightEdge.UVs[0], rightEdge.UVs[1], uvOnL1R1,
                        
                        subMeshIndex
                    );
                    if (Vector3.Dot(
                            Vector3.Cross(tris.Vertices[1] - tris.Vertices[0], tris.Vertices[2] - tris.Vertices[0]),
                            tris.Normals[0]) < 0)  FlipTriangle(tris);
                    _rightMesh.AddTriangle(tris);
                }
            
        }

        private void FlipTriangle(Triangle tris)
        {
            (tris.Vertices[2], tris.Vertices[0]) = (tris.Vertices[0], tris.Vertices[2]);
            (tris.Normals[2], tris.Normals[0]) = (tris.Normals[0], tris.Normals[2]);
            (tris.UVs[2], tris.UVs[0]) = (tris.UVs[0], tris.UVs[2]);
        }


        private void VisiblePlane(Plane plane, Vector3 pointToCut)
        {
            _visualPlane.transform.position = pointToCut; //plane.normal * plane.distance;
           _visualPlane.transform.rotation = Quaternion.LookRotation(Vector3.Cross(plane.normal, Vector3.up), plane.normal);
           
        }
        /*var subMesh = mesh.GetSubMesh(i);
               for (var j = 0; j < mesh.triangles.Length - 1; j += 3)
               {
                   var tris1 = mesh.triangles[j];
                   var tris2 = mesh.triangles[j + 1];
                   var tris3 = mesh.triangles[j + 2];

                   var triangle = new Triangle(
                       mesh.vertices[tris1], mesh.vertices[tris2], mesh.vertices[tris3],
                       mesh.normals[tris1], mesh.normals[tris2], mesh.normals[tris3],
                       mesh.uv[tris1], mesh.uv[tris2], mesh.uv[tris3],
                       i
                   );

                   _leftMesh.AddTriangle(triangle);
               }*/
    }
}