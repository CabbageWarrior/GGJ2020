using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance = null;
    public FadePanelsController fadePanelsController = default;
    public bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPause(bool paused)
    {
        if (paused)
        {
            isPaused = true;
        }
        else
        {
            StartCoroutine(Pause_Coroutine());

            IEnumerator Pause_Coroutine()
            {
                yield return null; // Skip a frame
                isPaused = false;
            }
        }
    }

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
