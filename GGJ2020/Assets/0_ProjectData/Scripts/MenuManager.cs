using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public FadePanelsController fadePanelsController = default;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(0);
    }

    public void Play(float delay)
    {
        StartCoroutine(Play_Coroutine(delay));

        IEnumerator Play_Coroutine(float coroutine_delay)
        {
            yield return new WaitForSeconds(coroutine_delay);
            Play();
        }
    }
    public void Play()
    {
        Debug.Log("Play");
        fadePanelsController.OnClose += () =>
        {
            SceneManager.LoadScene(2);
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
