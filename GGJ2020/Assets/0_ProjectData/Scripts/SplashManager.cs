using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public FadePanelsController fadePanelsController = default;
    public float seconds = 4f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        fadePanelsController.OnClose += () => {
            SceneManager.LoadScene(1);
        };
        yield return new WaitForSeconds(seconds);
        fadePanelsController.Close();
    }
}
