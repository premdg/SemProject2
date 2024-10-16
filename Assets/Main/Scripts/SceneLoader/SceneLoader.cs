using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this method when the button is pressed
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
