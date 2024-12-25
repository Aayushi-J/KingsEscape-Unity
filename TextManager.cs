using UnityEngine;
using UnityEngine.UI;  // Required for UI elements like Text

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;  // Reference to the Text component in the UI
    private int score = 0;  // Variable to hold the current score

    void Start()
    {
        // Initialize the score text with the starting score
        UpdateScoreText();
    }
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))  // Example: increase score when space key is pressed
    //     {
    //         FindObjectOfType<ScoreManager>().IncreaseScore();
    //     }
    // }


    // This method will be called when you want to increase the score
    public void IncreaseScore()
    {
        score += 5;  // Increase score by 1
        UpdateScoreText();  // Update the displayed text
    }

    // Method to update the text display
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();  // Set the UI text to the current score
    }
}
