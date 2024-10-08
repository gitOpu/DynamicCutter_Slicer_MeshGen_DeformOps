using UnityEditor;
using UnityEngine;
namespace com.marufhow.meshslicer.core
{
 

public class VectorOperationsVisualizer : MonoBehaviour
{
    // Vectors to operate on (set values in the Unity Inspector)
    public Vector3 vectorA = new Vector3(1, 2, 0);
    public Vector3 vectorB = new Vector3(2, 0, 1);

    // Colors for drawing the vectors and results
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public Color colorSum = Color.green;
    public Color colorDifference = Color.yellow;
    public Color colorCrossProduct = Color.magenta;

    // Scaling factor for better visualization in the scene
    public float scale = 1.0f;

    private void OnDrawGizmos()
    {
        // Draw the original vectors A and B
        DrawVector(Vector3.zero, vectorA, colorA, "A");
        DrawVector(Vector3.zero, vectorB, colorB, "B");

        /*// Draw the sum of vectors (A + B)
        Vector3 sum = vectorA + vectorB;
        DrawVector(Vector3.zero, sum, colorSum, "A + B");

       
        */
        
        // Draw the difference of vectors (A - B)
       
        Vector3 difference = vectorA - vectorB;
        DrawVector(Vector3.zero, difference, colorDifference, "A - B");
        
        // Draw the cross product (only meaningful in 3D)
        Vector3 crossProduct = Vector3.Cross(vectorA, vectorB);
        DrawVector(Vector3.zero, crossProduct, colorCrossProduct, "A x B");

        // Display the dot product as the angle between vectors A and B
        float dotProduct = Vector3.Dot(vectorA, vectorB);
       // float angle = Mathf.Acos(dotProduct / (vectorA.magnitude * vectorB.magnitude)) * Mathf.Rad2Deg;
       // Handles.Label(vectorA * 0.5f + vectorB * 0.5f, $"Dot Product: {dotProduct}\nAngle: {angle}°");

        
        // Calculate the angle using Atan2 for more intuitive angle calculation between vectorA and vectorB
        // float atan2Angle = Mathf.Atan2(vectorB.y, vectorB.x) * Mathf.Rad2Deg;
        // Handles.Label((vectorA + vectorB) * 0.5f, $"Atan2 Angle: {atan2Angle}°");
        // Handles.DrawWireArc(Vector3.zero, Vector3.forward, vectorA.normalized, atan2Angle, 0.4f);
        
        // Visualize the angle between the vectors using a wire arc
        //Gizmos.color = Color.cyan;
       // Gizmos.DrawWireSphere(Vector3.zero, 0.2f);
       // Handles.DrawWireArc(Vector3.zero, Vector3.forward, vectorA.normalized, angle, 0.5f);
    }

    // Helper method to draw a vector with Gizmos
    private void DrawVector(Vector3 origin, Vector3 vector, Color color, string label)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(origin, origin + vector * scale);
        Handles.Label(origin + vector * scale * 0.5f, $"{label}: {vector}");
    }
}

}