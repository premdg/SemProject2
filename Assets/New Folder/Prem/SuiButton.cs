using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuiButton : MonoBehaviour
{
    internal object onClick;

    void OnEnable()
    {
        SceneManager.LoadScene(4);
    }

}
