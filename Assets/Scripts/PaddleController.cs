using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boundaryOffset = 1f; // Offset from screen boundaries
    private float screenHalfWidth;
    private float paddleHalfWidth;
    private Camera mainCamera;

    void Start()
    {
        // Try to find the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            // If no main camera found
            mainCamera = FindObjectOfType<Camera>();
            if (mainCamera == null)
            {
                return;
            }
        }

        // Calculate screen boundaries in world coordinates
        float screenHalfHeightInWorld = mainCamera.orthographicSize;
        screenHalfWidth = screenHalfHeightInWorld * mainCamera.aspect;
        paddleHalfWidth = transform.localScale.x / 2;

        Debug.Log($"Screen width: {screenHalfWidth * 2}, Paddle width: {paddleHalfWidth * 2}");
    }

    void Update()
    {
        if (mainCamera == null) return;


        float horizontalInput = Input.GetAxis("Horizontal");

        float newX = transform.position.x + (horizontalInput * moveSpeed * Time.deltaTime);

        newX = Mathf.Clamp(newX, -screenHalfWidth + paddleHalfWidth + boundaryOffset,
                                screenHalfWidth - paddleHalfWidth - boundaryOffset);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
