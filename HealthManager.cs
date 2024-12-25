using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthManager : MonoBehaviour
{
    public GameObject[] hearts; // Array to hold heart images
    private int currentHearts = 3; // Starting with 3 hearts

    void Start()
    {
        UpdateHeartsUI(); // Initialize heart display
    }

    public void TakeDamage()
    {
        if (currentHearts > 0)
        {
            hearts[currentHearts - 1].SetActive(false); // Deactivate the last heart image
            currentHearts--; // Decrease current hearts
            Debug.Log("Heart removed, current hearts: " + currentHearts);
        }
        else
        {
            SceneManager.LoadScene("GameOver");// Optional: Handle player death
        }
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Enable or disable heart images based on currentHearts
            hearts[i].SetActive(i < currentHearts);
        }
    }
}
