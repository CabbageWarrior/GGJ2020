using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float gameTime = 60;
    public float trumpPoints = 2;

    public Text timer;
    public Text points;
    public Outline outline;

    public AnimationCurve pointsScaleCurve;
    public AnimationCurve pointsAlphaCurve;

    public static TimerManager Instance { get; set; }

    public static bool GAME_OVER = false;

    private Coroutine co;
    private bool enabled;
    private Color initialPointsColor;
    private Color finalPointsColor;

    private Color initialOutlineColor;
    private Color finalOutlineColor;

    static int currentPoints;

    private void Awake()
    {
        Instance = this;
        timer.text = "" + (int)gameTime;
        initialPointsColor = points.color;
        finalPointsColor = initialPointsColor;
        finalPointsColor.a = 0;

        initialOutlineColor = outline.effectColor;
        finalOutlineColor = outline.effectColor;
        finalOutlineColor.a = 0;

        outline.effectColor = finalOutlineColor;
        points.color = finalPointsColor;
        currentPoints = 0;
    }

    public void StartTimer()
    {
        enabled = true;
    }

    [ContextMenu("trump this bitch up")]
    public void AddTrumpPoints()
    {
        if (GAME_OVER) 
            return;

        gameTime += trumpPoints;
        points.text = "+" + trumpPoints;

        if(co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(PointsCoroutine());
    }

    public static void AddPoint(int value = 1)
    {
        currentPoints+= value;
    }

    public static void RemovePoint()
    {
        currentPoints--;
    }

    //call then the last screen is up
    public static void SavePoints()
    {
        PlayerPrefs.SetFloat("HighScore", currentPoints);
        PlayerPrefs.Save();
    }

    IEnumerator PointsCoroutine()
    {
        float timer = 0;
        float time = 2;

        while(timer < time)
        {
            timer += Time.deltaTime;

            points.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f,
                pointsScaleCurve.Evaluate(timer / time));

            points.color = Color.Lerp(finalPointsColor, initialPointsColor,
                pointsAlphaCurve.Evaluate(timer / time));

            outline.effectColor = Color.Lerp(finalOutlineColor, initialOutlineColor,
                pointsAlphaCurve.Evaluate(timer / time));

            Debug.Log("Evaluation: " + pointsAlphaCurve.Evaluate(timer / time));

            yield return null;
        }
    }


    private void Update()
    {
        if (!enabled)
            return;

        gameTime -= Time.deltaTime;

        timer.text = "" + (int)gameTime;

        if(gameTime < 0)
        {
            enabled = false;
            GAME_OVER = true;
            timer.text = "" + 0;
        }
    }
}
