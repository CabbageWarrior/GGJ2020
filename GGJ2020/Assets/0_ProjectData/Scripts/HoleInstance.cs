using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleInstance : MonoBehaviour
{
    public SpriteRenderer cow;
    public SpriteRenderer hole;

    public void KillHole()
    {
        StartCoroutine(KillHoleRoutine());
    }

    IEnumerator KillHoleRoutine()
    {
        float timer = 0;
        float time = 1;

        Color finalColor = Color.white;
        finalColor.a = 0;

        while(timer < time)
        {
            timer += Time.deltaTime;

            cow.color = Color.Lerp(Color.white, finalColor, timer / time);
            hole.color = Color.Lerp(Color.white, finalColor, timer / time);

            yield return null;
        }
        Destroy(this.cow.gameObject);
        Destroy(this.gameObject);
    }
}
