using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public FadePanelsController fadePanelsController = default;

    public void Replay()
    {
        fadePanelsController.OnClose = () => {
            SceneManager.LoadScene(1);
        };
        fadePanelsController.Close();
    }

    public void ReturnToMenu()
    {
        fadePanelsController.OnClose = () => {
            SceneManager.LoadScene(0);
        };
        fadePanelsController.Close();
    }
}
