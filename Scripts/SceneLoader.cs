using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Invoke("LoadFirstScene", 2f);
    }

    // Load the first scene
    void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
