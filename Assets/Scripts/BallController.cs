using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    public Transform paddle;
    public float moveSpeed = 8f;
    public float speedIncrease = 1f;
    public float maxSpeed = 10f;
    public float minSpeed = 6f;

    private bool isLaunched = false;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private int lives = 3;
    private int remainingBlocks;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        // Configure Rigidbody2D for smooth movement
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Count initial blocks
        remainingBlocks = GameObject.FindGameObjectsWithTag("Block").Length;
        Debug.Log($"Game Started! Lives remaining: {lives}, Blocks remaining: {remainingBlocks}");
        
        ResetBall();
    }

    void Update()
    {
        if (!isLaunched)
        {
            // Follow paddle until launched
            transform.position = new Vector3(paddle.position.x, paddle.position.y + 0.5f, 0f);

            // Launch on space press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchBall();
            }
        }
    }

    void FixedUpdate()
    {
        if (isLaunched)
        {
            // Use physics-based movement in FixedUpdate for smoother motion
            rb.linearVelocity = moveDirection * moveSpeed;
        }

        // Check if ball fell below paddle
        if (transform.position.y < paddle.position.y - 2f)
        {
            LoseBall();
        }
    }

    void LaunchBall()
    {
        isLaunched = true;
        moveDirection = Vector2.up;
        moveSpeed = 8f;
        rb.linearVelocity = moveDirection * moveSpeed;
        Debug.Log("Ball launched with direction: " + moveDirection + " and speed: " + moveSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Get hit position relative to paddle center (-1 to 1)
            float hitPoint = (transform.position.x - paddle.position.x) / paddle.localScale.x;

            // Calculate bounce angle (between -60 and 60 degrees)
            float bounceAngle = hitPoint * 60f;

            // Convert angle to direction
            float angleRad = bounceAngle * Mathf.Deg2Rad;
            moveDirection = new Vector2(Mathf.Sin(angleRad), Mathf.Abs(Mathf.Cos(angleRad))).normalized;

            // Increase speed on paddle hit, but cap it
            moveSpeed = Mathf.Min(moveSpeed + speedIncrease, maxSpeed);

            // Apply new velocity immediately for responsive feel
            rb.linearVelocity = moveDirection * moveSpeed;

            Debug.Log($"Paddle hit at {hitPoint}, Bounce angle: {bounceAngle}, Direction: {moveDirection}, Speed: {moveSpeed}");
        }
        else if (collision.gameObject.CompareTag("Block"))
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;

            // Ensure we don't get stuck moving vertically
            if (Mathf.Abs(moveDirection.y) > 0.9f)
            {
                float sign = Mathf.Sign(moveDirection.y);
                moveDirection.y = sign * 0.9f;
                moveDirection.x = sign * 0.4f;
                moveDirection = moveDirection.normalized;
            }

            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    public void BlockDestroyed()
    {
        remainingBlocks--;
        Debug.Log($"Block destroyed! Blocks remaining: {remainingBlocks}");
        
        if (remainingBlocks <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log("Congratulations! You've won the game!");
        isLaunched = false;
        rb.linearVelocity = Vector2.zero;
        Invoke("ReturnToMainMenu", 2f);
    }

    void LoseBall()
    {
        lives--;
        Debug.Log($"Ball lost! Lives remaining: {lives}");

        if (lives <= 0)
        {
            Debug.Log("Game Over! No lives remaining. Returning to main menu...");
            Invoke("ReturnToMainMenu", 2f);
        }
        else
        {
            ResetBall();
        }
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetBall()
    {
        isLaunched = false;
        moveDirection = Vector2.zero;
        moveSpeed = 8f;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (paddle != null)
        {
            transform.position = new Vector3(paddle.position.x, paddle.position.y + 0.5f, 0f);
        }
    }
}
