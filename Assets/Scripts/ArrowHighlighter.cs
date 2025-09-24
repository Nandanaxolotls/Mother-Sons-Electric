
using UnityEngine;

public class ArrowHighlighter : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f; // degrees per second

    [Header("Bobbing Settings")]
    public float minY = 0.83f;
    public float maxY = 1f;
    public float bobSpeed = 2f; // cycles per second

    void Update()
    {
        // Continuous rotation around X axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);

        // Up/down movement between minY and maxY
        float newY = Mathf.Lerp(minY, maxY, (Mathf.Sin(Time.time * bobSpeed * Mathf.PI * 2) + 1) / 2);
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }
}
