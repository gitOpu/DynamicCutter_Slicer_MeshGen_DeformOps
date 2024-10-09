using com.marufhow.meshslicer.model;
using UnityEngine;

namespace com.marufhow.meshslicer.extensions
{
    public static class MeshExtensions
    {
        /// <summary>
        /// Extracts a triangle from a mesh based on given vertex indices and submesh index.
        /// </summary>
        /// <param name="mesh">The mesh to extract the triangle from.</param>
        /// <param name="trisA">Index of the first vertex.</param>
        /// <param name="trisB">Index of the second vertex.</param>
        /// <param name="trisC">Index of the third vertex.</param>
        /// <param name="subMeshIndex">The submesh index of the triangle.</param>
        /// <returns>A new Triangle object with the specified data.</returns>
        public static Triangle ExtractTriangle(this Mesh mesh, int trisA, int trisB, int trisC, int subMeshIndex)
        {
            return new Triangle(
                mesh.vertices[trisA], mesh.vertices[trisB], mesh.vertices[trisC],
                mesh.normals[trisA], mesh.normals[trisB], mesh.normals[trisC],
                mesh.uv[trisA], mesh.uv[trisB], mesh.uv[trisC],
                subMeshIndex
            );
        }
    }
}