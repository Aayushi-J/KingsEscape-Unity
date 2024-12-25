using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // This is needed to stop play mode in the Editor
#endif

public class UIManager : MonoBehaviour
{
    public Button PlayButton; // Reference to the Button component
    public Button ExitButton;
    public Button RetryButton;

    void Start()
    {
        // Add a listener to the button's onClick event
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        RetryButton.onClick.AddListener(onRetry);
    }

    // Method that gets called when the button is clicked
    void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    void OnExitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    void onRetry()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the scene by its build index
        SceneManager.LoadScene(currentScene.buildIndex - 1);
    }
}
