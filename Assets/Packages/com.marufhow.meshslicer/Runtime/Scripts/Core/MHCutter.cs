using System;
using System.Collections.Generic;
using com.marufhow.meshslicer.extensions;
using com.marufhow.meshslicer.model;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace com.marufhow.meshslicer.core
{
    public class MHCutter : MonoBehaviour
    {
        private MHMesh _leftMesh;
        private GameObject _rightSlicedGameObject;
        private MHMesh _rightMesh;
        
        private Mesh _mesh;
        private int _count;
        
        [Header("Only for observe in Inspector, not for set anything")]
        public List<Vector3> addedVertices;
        public List<Vector3> fillAreaVertex;
        public List<Vector2> UVs;
        public void Cut(GameObject cutObject,Vector3 cutPoint, Vector3 hitNormal)
        {
            Plane plane = new Plane(-hitNormal, new Vector3(cutPoint.x,cutPoint.y,0));
            
            _mesh = cutObject.GetComponent<MeshFilter>().mesh;
            addedVertices = new List<Vector3>();
            
            _leftMesh = cutObject.GetComponent<MHMesh>();
            if (_leftMesh == null)
            {
                _leftMesh = cutObject.AddComponent<MHMesh>();
            }
            
            _rightSlicedGameObject  = new GameObject($"Sliced {_count++}");
            _rightMesh = _rightSlicedGameObject.GetComponent<MHMesh>();
            if (_rightMesh == null)
            {
                _rightMesh = _rightSlicedGameObject.AddComponent<MHMesh>();
            }
            
            _leftMesh.Clear();
            _rightMesh.Clear();

            for (var i = 0; i < _mesh.subMeshCount; i++)
            {
                // Get triangles from a mesh : Another way
                var subMeshTriangles = _mesh.GetTriangles(i); // its also return same triangles of current subMesh
                for (var j = 0; j < subMeshTriangles.Length; j += 3)
                {
                    var trisA = subMeshTriangles[j];
                    var trisB = subMeshTriangles[j + 1];
                    var trisC = subMeshTriangles[j + 2];
                  
                    Triangle triangle =  _mesh.ExtractTriangle(trisA, trisB, trisC, i);
                    
                    bool isLeftSideTrisA = plane.GetSide(cutObject.transform.TransformPoint(_mesh.vertices[trisA]));
                    bool isLeftSideTrisB = plane.GetSide(cutObject.transform.TransformPoint(_mesh.vertices[trisB]));
                    bool isLeftSideTrisC = plane.GetSide(cutObject.transform.TransformPoint(_mesh.vertices[trisC]));

 
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

                   
                }
            }
            FillCuttingPlane(plane);
            
            _leftMesh.GenerateMesh(); // reform current to cutObject
            _rightMesh.GenerateMesh(_leftMesh.Materials);  // new cut object
            
            _rightSlicedGameObject.transform.position = cutObject.transform.position + Vector3.right * 0.1f;
            _rightSlicedGameObject.transform.rotation = cutObject.transform.rotation;
            _rightSlicedGameObject.transform.localScale = cutObject.transform.localScale;
            var rightRb = _rightSlicedGameObject.AddComponent<Rigidbody>();
            rightRb.AddForce( Vector3.right , ForceMode.Impulse);
        }

       
        private void FillCuttingPlane(Plane plane)
        {
            fillAreaVertex = new List<Vector3>();
            List<Vector3> visitedVertex = new List<Vector3>();
            for (int i = 0; i < addedVertices.Count; i++)
            {
                if (!visitedVertex.Contains(addedVertices[i]))
                {
                    fillAreaVertex.Clear();
                    fillAreaVertex.Add(addedVertices[i]);
                    fillAreaVertex.Add(addedVertices[i+1]);
                    
                    visitedVertex.Add(addedVertices[i]);
                    visitedVertex.Add(addedVertices[i+1]);

                    CheckPolygonForThisPairs(visitedVertex, i);
                    FillHole(plane, i);
                }
                
            }
            
        }
        private void CheckPolygonForThisPairs(List<Vector3> visitedVertex, int step)
        {
            Debug.Log($"CheckPolygonForThisPairs Steps {step} visitedVertex {visitedVertex}");
            bool stopSearch = false;
            while (!stopSearch)
            {
                stopSearch = true;
                for (int j = 0; j < addedVertices.Count; j += 2)
                {
                    if (addedVertices[j] == fillAreaVertex[^1] && !visitedVertex.Contains(addedVertices[j+1]))
                    {
                        stopSearch = false;
                        fillAreaVertex.Add(addedVertices[j+1]);
                        visitedVertex.Add(addedVertices[j+1]);
                    }else  if (addedVertices[j+1] == fillAreaVertex[^1] && !visitedVertex.Contains(addedVertices[j]))
                    {
                        stopSearch = false;
                        fillAreaVertex.Add(addedVertices[j]);
                        visitedVertex.Add(addedVertices[j]);
                    }
                }
            }
        }

       
        
        private void FillHole(Plane plane, int step)
        {
            Debug.Log($"Fill Steps {step} Polygon Vertices {fillAreaVertex}, LeftMeshCount {_leftMesh._vertices.Count} Right Mesh Count  {_rightMesh._vertices.Count}");
            Vector3 sum = Vector3.zero;
            foreach (var item in fillAreaVertex)
            {
                sum += item;
            }
            Vector3 center = sum / fillAreaVertex.Count;

            TriangleVertex vertexData = new TriangleVertex();
            TriangleNormal normalData = new TriangleNormal();
            TriangleUVs uvData = new TriangleUVs();


            UVs = HelperFunction.ConvertToUV(plane.normal, fillAreaVertex);
            
             
            for (int j = 0; j < fillAreaVertex.Count; j++)
            {
                vertexData.Vertices[0] = fillAreaVertex[j];
                vertexData.Vertices[1] = fillAreaVertex[(j+1) % fillAreaVertex.Count];
                vertexData.Vertices[2] = center;
                
                normalData.Normals[0] = - plane.normal;
                normalData.Normals[1] = - plane.normal;
                normalData.Normals[2] = - plane.normal;
                
                uvData.UV[0] = UVs[j];
                uvData.UV[1] =  UVs[(j+1) % fillAreaVertex.Count];
                uvData.UV[2] = new Vector2(0.5f, 0.5f);
                
                
                Triangle triangleL = new Triangle(vertexData, normalData, uvData, _mesh.subMeshCount);
                
                if(!triangleL.IsAlignWithNormal()) triangleL.FlipTriangle();
                _leftMesh.AddTriangle(triangleL);
                
                // Right part of the mesh
                normalData.Normals[0] = plane.normal;
                normalData.Normals[1] = plane.normal;
                normalData.Normals[2] = plane.normal;
            
                Triangle triangleR = new Triangle(vertexData, normalData, uvData, _mesh.subMeshCount);
                
                if(!triangleR.IsAlignWithNormal()) triangleR.FlipTriangle();
                _rightMesh.AddTriangle(triangleR);
                
            }
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

                    if(!tris.IsAlignWithNormal()) tris.FlipTriangle();
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

                    if(!tris.IsAlignWithNormal()) tris.FlipTriangle();
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

                    if(!tris.IsAlignWithNormal()) tris.FlipTriangle();
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
                    if(!tris.IsAlignWithNormal()) tris.FlipTriangle();
                    _rightMesh.AddTriangle(tris);
                }
            
        }
    }
}