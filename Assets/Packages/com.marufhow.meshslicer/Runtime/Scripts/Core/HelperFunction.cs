using System.Collections.Generic;
using UnityEngine;

namespace com.marufhow.meshslicer.core
{
    [System.Serializable]
    public enum Axis
    {
        x, y, z
    }

    public static class HelperFunction 
    {
        public static List<Vector2> ConvertToUV(Vector3 plane, List<Vector3> originalVectors)
        {
            if (originalVectors == null || originalVectors.Count == 0)
                return new List<Vector2>();

            List<Vector2> uvs = new List<Vector2>(originalVectors.Count);

            Axis normalizedAxis = GetNormalAxis(plane);
            List<Vector3> normalizedVectors = ConvertNormalizedVectors(originalVectors);

            foreach (var item in normalizedVectors)
            {
                switch (normalizedAxis)
                {
                    case Axis.x:
                        uvs.Add(new Vector2(item.z, item.y));
                        break;
                    case Axis.y:
                        uvs.Add(new Vector2(item.x, item.z));
                        break;
                    case Axis.z:
                        uvs.Add(new Vector2(item.x, item.y));
                        break;
                }
            }

            return uvs;
        }

        private static Axis GetNormalAxis(Vector3 vector)
        {
            float absX = Mathf.Abs(vector.x);
            float absY = Mathf.Abs(vector.y);
            float absZ = Mathf.Abs(vector.z);

            float largest = Mathf.Max(absX, absY, absZ);

            if (largest == absX)
                return Axis.x;
            else if (largest == absY)
                return Axis.y;
            else
                return Axis.z;
        }

        private static List<Vector3> ConvertNormalizedVectors(List<Vector3> originalVectors)
        {
            List<Vector3> normalizedVectors = new List<Vector3>(originalVectors.Count);
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;

            // Find min and max values
            foreach (var vector in originalVectors)
            {
                if (vector.x < minX) minX = vector.x;
                if (vector.x > maxX) maxX = vector.x;

                if (vector.y < minY) minY = vector.y;
                if (vector.y > maxY) maxY = vector.y;

                if (vector.z < minZ) minZ = vector.z;
                if (vector.z > maxZ) maxZ = vector.z;
            }

            // Normalize the vectors
            foreach (var vector in originalVectors)
            {
                normalizedVectors.Add(new Vector3(
                    Normalize(vector.x, minX, maxX),
                    Normalize(vector.y, minY, maxY),
                    Normalize(vector.z, minZ, maxZ)
                ));
            }

            return normalizedVectors;
        }

        private static float Normalize(float value, float minValue, float maxValue)
        {
            if (maxValue - minValue == 0)
                return 0f; // Avoid division by zero if all values are the same.
            
            return (value - minValue) / (maxValue - minValue);
        }
    }
}
