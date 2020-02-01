using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelsController : MonoBehaviour
{
    public enum EType
    {
        Open,
        Close
    }

    public Action OnOpen = default;
    public Action OnClose = default;

    public RectTransform leftPanel = default;
    public float leftOpenViewportX = -.5f;
    public float leftCloseViewportX = 0f;
    [Space]
    public RectTransform rightPanel = default;
    public float rightOpenViewportX = .5f;
    public float rightCloseViewportX = 0f;
    [Space]
    public float movementTime = .2f;
    [Space]
    public EType positionOnSetup = EType.Close;

    Vector2 apLeft = new Vector2();
    Vector2 apRight = new Vector2();
    float currentTime = 0f;

    float leftOpenScreenX = 0f;
    float leftCloseScreenX = 0f;
    float rightOpenScreenX = 0f;
    float rightCloseScreenX = 0f;

    Camera mainCamera = default;

    private void Start()
    {
        mainCamera = Camera.main;

        leftOpenScreenX = mainCamera.ViewportToScreenPoint(new Vector3(leftOpenViewportX, 0f, 0f)).x;
        leftCloseScreenX = mainCamera.ViewportToScreenPoint(new Vector3(leftCloseViewportX, 0f, 0f)).x;
        rightOpenScreenX = mainCamera.ViewportToScreenPoint(new Vector3(rightOpenViewportX, 0f, 0f)).x;
        rightCloseScreenX = mainCamera.ViewportToScreenPoint(new Vector3(rightCloseViewportX, 0f, 0f)).x;

        apLeft = leftPanel.anchoredPosition;
        apRight = rightPanel.anchoredPosition;

        switch (positionOnSetup)
        {
            case EType.Open:
                apLeft.x = leftOpenScreenX;
                leftPanel.anchoredPosition = apLeft;

                apRight.x = rightOpenScreenX;
                rightPanel.anchoredPosition = apRight;
                break;
            case EType.Close:
                apLeft.x = leftCloseScreenX;
                leftPanel.anchoredPosition = apLeft;

                apRight.x = rightCloseScreenX;
                rightPanel.anchoredPosition = apRight;
                break;
            default:
                apLeft.x = leftOpenScreenX;
                leftPanel.anchoredPosition = apLeft;

                apRight.x = rightOpenScreenX;
                rightPanel.anchoredPosition = apRight;
                break;
        }

        Open();
    }

    public void Open()
    {
        StartCoroutine(Open_Coroutine());

        IEnumerator Open_Coroutine()
        {
            currentTime = 0f;
            while (currentTime < movementTime)
            {
                currentTime += Time.deltaTime;

                apLeft.x = Mathf.Lerp(leftCloseScreenX, leftOpenScreenX, currentTime / movementTime);
                leftPanel.anchoredPosition = apLeft;

                apRight.x = Mathf.Lerp(rightCloseScreenX, rightOpenScreenX, currentTime / movementTime);
                rightPanel.anchoredPosition = apRight;

                yield return null;
            }

            OnOpen?.Invoke();
        }
    }

    public void Close()
    {
        StartCoroutine(Close_Coroutine());

        IEnumerator Close_Coroutine()
        {
            currentTime = 0f;
            while (currentTime < movementTime)
            {
                currentTime += Time.deltaTime;

                apLeft.x = Mathf.Lerp(leftOpenScreenX, leftCloseScreenX, currentTime / movementTime);
                leftPanel.anchoredPosition = apLeft;

                apRight.x = Mathf.Lerp(rightOpenScreenX, rightCloseScreenX, currentTime / movementTime);
                rightPanel.anchoredPosition = apRight;

                yield return null;
            }

            OnClose?.Invoke();
        }
    }
}
