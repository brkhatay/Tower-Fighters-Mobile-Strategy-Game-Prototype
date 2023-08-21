using UnityEngine;

public class RectangleCircularMovement : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private float offset = 1.4f;
    private Vector2 rectangleSize;
    private float angle = 0.0f;

    private void Start()
    {
        rectangleSize = new Vector2(Screen.width, Screen.height) / offset;
    }

    private void Update()
    {
        angle += rotationSpeed * Time.deltaTime;

        float x = centerPoint.position.x + rectangleSize.x * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = centerPoint.position.y + rectangleSize.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        float z = centerPoint.position.z;

        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }
}