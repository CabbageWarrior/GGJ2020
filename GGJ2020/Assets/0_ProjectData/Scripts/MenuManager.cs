using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public FadePanelsController fadePanelsController = default;

    public void Play()
    {
        Debug.Log("Play");
        fadePanelsController.OnClose += () =>
        {
            SceneManager.LoadScene(1);
        };
        fadePanelsController.Close();
    }

    public void Credits()
    {
        Debug.Log("Credits");
    }

    public void Info()
    {
        Debug.Log("Info");
    }
}
