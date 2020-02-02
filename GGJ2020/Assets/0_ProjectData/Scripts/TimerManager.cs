using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float gameTime = 60;
    public float trumpPoints = 2;

    public Text timer;
    public Text timeBonus;
    public Text points, endScreenPoints, endScreenScoreText;
    public Outline outline;

    public AnimationCurve pointsScaleCurve;
    public AnimationCurve pointsAlphaCurve;

    public UnityEvent OnGameOver = default;

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
        GAME_OVER = false;
        timer.text = "" + (int)gameTime;
        initialPointsColor = timeBonus.color;
        finalPointsColor = initialPointsColor;
        finalPointsColor.a = 0;

        initialOutlineColor = outline.effectColor;
        finalOutlineColor = outline.effectColor;
        finalOutlineColor.a = 0;

        outline.effectColor = finalOutlineColor;
        timeBonus.color = finalPointsColor;
        currentPoints = 0;
        UpdateScore();

    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(1);
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
        timeBonus.text = "+" + trumpPoints;

        if (co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(PointsCoroutine());
    }

    public static void AddPoint(int value = 1)
    {
        if (GAME_OVER)
            return;
        currentPoints += value;
        TimerManager.Instance.UpdateScore();
    }

    public static void RemovePoint()
    {
        currentPoints--;
        TimerManager.Instance.UpdateScore();
    }

    void UpdateScore()
    {
        points.text = currentPoints.ToString();
        endScreenPoints.text = currentPoints.ToString();
    }

    public void CheckHighScore()
    {
        if (currentPoints > PlayerPrefs.GetFloat("HighScore"))
        {
            endScreenScoreText.text = "New High Score!!!";
            SavePoints();
            GAME_OVER = false;
        }
        else
        {
            endScreenScoreText.text = "Score";
        }
    }

    //call then the last screen is up
    public void SavePoints()
    {
        PlayerPrefs.SetFloat("HighScore", currentPoints);
        PlayerPrefs.Save();
    }

    IEnumerator PointsCoroutine()
    {
        float timer = 0;
        float time = 2;

        while (timer < time)
        {
            timer += Time.deltaTime;

            timeBonus.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f,
                pointsScaleCurve.Evaluate(timer / time));

            timeBonus.color = Color.Lerp(finalPointsColor, initialPointsColor,
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

        if (gameTime < 0)
        {
            enabled = false;
            GAME_OVER = true;
            timer.text = "" + 0;
            Trump.Instance.DisableTrump();
        }
    }
}
