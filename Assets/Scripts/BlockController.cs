using UnityEngine;

public class BlockController : MonoBehaviour
{
    public int hitPoints = 1;  // How many hits needed to break the block
    
    // Array of colors for different hit points
    public Color[] blockColors = new Color[] 
    {
        new Color(1f, 0.2f, 0.2f, 1f),     // Red
        new Color(0.2f, 1f, 0.2f, 1f),     // Green
        new Color(0.2f, 0.2f, 1f, 1f),     // Blue
        new Color(1f, 1f, 0.2f, 1f),       // Yellow
        new Color(1f, 0.5f, 0.2f, 1f)      // Orange
    };

    private SpriteRenderer spriteRenderer;
    private int initialHitPoints;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        initialHitPoints = hitPoints;
        UpdateBlockColor();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeHit();
        }
    }

    void TakeHit()
    {
        hitPoints--;
        UpdateBlockColor();
        
        if (hitPoints <= 0)
        {
            // Notify BallController before destroying
            BallController ballController = FindObjectOfType<BallController>();
            if (ballController != null)
            {
                ballController.BlockDestroyed();
            }
            
            // Destroy the block
            Destroy(gameObject);
        }
    }

    void UpdateBlockColor()
    {
        if (spriteRenderer != null && blockColors.Length > 0)
        {
            // Calculate color index based on remaining hit points
            int colorIndex = Mathf.Min(hitPoints - 1, blockColors.Length - 1);
            if (colorIndex >= 0)
            {
                spriteRenderer.color = blockColors[colorIndex];
            }
        }
    }
}
