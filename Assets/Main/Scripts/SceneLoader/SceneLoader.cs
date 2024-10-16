using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // The name of the scene to load
    public string sceneName;

    // Reference to the button
    public Button button;

    private void Start()
    {
        // Add a listener to the button's onClick event
        button.onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        // Load the scene asynchronously
        SceneManager.LoadSceneAsync(sceneName);
    }
}