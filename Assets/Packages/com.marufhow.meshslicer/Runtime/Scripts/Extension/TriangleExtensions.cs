using com.marufhow.meshslicer.model;
using UnityEngine;

namespace com.marufhow.meshslicer.extensions
{
    public static class TriangleExtensions
    {
        /// <summary>
        /// Checks if the normal of the triangle aligns with the direction of its vertices.
        /// </summary>
        /// <param name="triangle">The triangle to check.</param>
        /// <returns>True if the normal is aligned with the direction of vertices; false otherwise.</returns>
        public static bool IsAlignWithNormal(this Triangle triangle)
        {
            // Calculate the cross product of two edges of the triangle.
            var cross = Vector3.Cross(
                triangle.Vertices[1] - triangle.Vertices[0],
                triangle.Vertices[2] - triangle.Vertices[0]);

            // Calculate the dot product between the cross product and the triangle's normal.
            var dot = Vector3.Dot(cross, triangle.Normals[0]);

            // Return true if dot product is positive, indicating the normal is aligned.
            return dot > 0; // 0 means perpendicular; positive means same direction; negative means opposite.
        }

        /// <summary>
        /// Flips the vertices, normals, and UVs of the triangle to reverse its orientation.
        /// </summary>
        /// <param name="triangle">The triangle to flip.</param>
        public static void FlipTriangle(this Triangle triangle)
        {
            // Swap the first and third vertex, normal, and UV to flip the triangle's orientation.
            (triangle.Vertices[2], triangle.Vertices[0]) = (triangle.Vertices[0], triangle.Vertices[2]);
            (triangle.Normals[2], triangle.Normals[0]) = (triangle.Normals[0], triangle.Normals[2]);
            (triangle.UVs[2], triangle.UVs[0]) = (triangle.UVs[0], triangle.UVs[2]);
        }
    }
}