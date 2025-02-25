using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentLives = 3;
    private int totalBlocks;
    private int remainingBlocks;

    void Start()
    {
        // Count total blocks
        totalBlocks = GameObject.FindGameObjectsWithTag("Block").Length;
        remainingBlocks = totalBlocks;
        Debug.Log($"Game Started! Lives: {currentLives}, Blocks to destroy: {totalBlocks}");
    }

    public void LoseLife()
    {
        currentLives--;
        Debug.Log($"Life lost! Remaining lives: {currentLives}");

        if (currentLives <= 0)
        {
            GameOver(false);
        }
    }

    public void BlockDestroyed()
    {
        remainingBlocks--;
        Debug.Log($"Block destroyed! Remaining blocks: {remainingBlocks}");

        if (remainingBlocks <= 0)
        {
            GameOver(true);
        }
    }

    void GameOver(bool won)
    {
        if (won)
        {
            Debug.Log("Congratulations! You Win!");
        }
        else
        {
            Debug.Log("Game Over! You Lose!");
        }

        // Disable ball movement
        BallController ball = FindObjectOfType<BallController>();
        if (ball)
        {
            ball.enabled = false;
        }

        // Return to main menu after 2 seconds
        Invoke("ReturnToMainMenu", 2f);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
