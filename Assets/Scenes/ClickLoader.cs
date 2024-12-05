using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clickloader : MonoBehaviour
{
    // Start is called before the first frame update
    public void SceneLoader()
    {
        SceneManager.LoadScene("UI");
    }
}
