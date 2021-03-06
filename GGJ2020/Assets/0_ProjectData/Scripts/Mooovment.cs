﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mooovment : MonoBehaviour
{
    public float mooovementSpeed = 5.0f;
    public GameObject CrosshairPivot;
    public Animation movingCow;
    public float movingCowSecondsToSkip = 1;
    public Animation scorreggiaSmoke;
    public Animator gretaAnimator;

    public CownonBallController currentProjectile;
    public CownonBallStatistics currentProjectileStats;

    public static Action<Vector3> OnCowShot;
    public static Action OnCowIngropped;

    public float secondsBeforeGameOverPanel = 2f;

    private bool isGameOverTriggered = false;

    float scorreggiaTimer;

    private Vector3 ClampVectorInScreen(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        float gretaVerticalViewportPosition = Camera.main.WorldToViewportPoint(transform.position).y;

        viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, gretaVerticalViewportPosition + 0.13f, 1);

        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        worldPosition.z = 0;

        return worldPosition;
    }

    private void Start()
    {
        OnCowIngropped?.Invoke();
    }
    private Vector3 ApplyShake(Vector3 position, float t)
    {
        float curveT = currentProjectileStats.shakeCurve.Evaluate(t);

        float shakeAmount = Mathf.Lerp(currentProjectileStats.minShake,
            currentProjectileStats.maxShake, curveT);

        Vector3 offset = UnityEngine.Random.insideUnitCircle * shakeAmount;

        if (curveT > 0.25f && !gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("ReverseAiming") && !gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("CowScalcing"))
        {
            gretaAnimator.Play("ReverseAiming");
        }

        return position + offset;
    }

    float shakeTimer = 0;

    void Update()
    {
        //Debug.Log("Mouse x: " + Input.GetAxis("HorizontalCursorMooovement") + 
        //    " y: " + Input.GetAxis("HorizontalCursorMooovement"));

        if (!PauseController.Instance.isPaused && !TimerManager.GAME_OVER)
        {
            AnimatorStateInfo asi = gretaAnimator.GetCurrentAnimatorStateInfo(0);

            if (asi.IsName("Throwing") || asi.IsName("GrabAnimation"))
            {
                if (CrosshairPivot.activeSelf) CrosshairPivot.SetActive(false);
            }
            else
            {
                if (!CrosshairPivot.activeSelf) CrosshairPivot.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) && asi.IsName("Idle"))
            {
                CrosshairPivot.transform.localPosition = Vector3.zero;
                gretaAnimator.Play("Aiming");
                //AudioManager.Instance.PlaySfx(1);
                shakeTimer = 0;
                scorreggiaTimer = 0;
                if (!TimerManager.GAME_OVER)
                {
                    Trump.Instance.EnableTrump();
                    TimerManager.Instance.StartTimer();
                }
            }
            else if (Input.GetMouseButton(0) && (
                   asi.IsName("Aiming")
                || asi.IsName("CowScalcing")
                || asi.IsName("ReverseAiming")
            ) && scorreggiaTimer < currentProjectileStats.timeToScorreggia)
            {
                Vector3 mouseInput = new Vector3(Input.GetAxis("HorizontalCursorMooovement"),
                                                 Input.GetAxis("VerticalCursorMooovement"), 0);

                Vector3 currentCrosshairPivotPosition = CrosshairPivot.transform.position;
                Vector3 targetPosition = currentCrosshairPivotPosition - mouseInput * mooovementSpeed;

                //Debug.Log("Target position: " + targetPosition + "viewport botLeft corner: " + Camera.main.ViewportToWorldPoint(Vector3.forward)
                //    + " Target to viewport pos: " + Camera.main.WorldToViewportPoint(targetPosition));

                //Debug.Log("Clamped position: " + ClampVectorInScreen(targetPosition));

                // process position, clamp between screen borderzzz, apply mucca shake!
                shakeTimer += Time.deltaTime;
                scorreggiaTimer += Time.deltaTime;


                targetPosition = ApplyShake(targetPosition, (shakeTimer / currentProjectileStats.loopTime) % 1);

                targetPosition = ClampVectorInScreen(targetPosition);

                CrosshairPivot.transform.position = targetPosition;
            }
            else if ((Input.GetMouseButtonUp(0) && (
                   asi.IsName("Aiming")
                || asi.IsName("CowScalcing")
                || asi.IsName("ReverseAiming")
            )) || scorreggiaTimer >= currentProjectileStats.timeToScorreggia)
            {
                // shoot cow lol
                OnCowShot?.Invoke(CrosshairPivot.transform.position);
                CownonBallController cowThrown = Instantiate(currentProjectile);

                if (HolesManager.Instance.currentHole != null)
                {
                    cowThrown.ShootCow(this.transform.position, HolesManager.Instance.currentHole.position,
                        currentProjectileStats, HolesManager.Instance.currentHole);
                }
                else
                {
                    cowThrown.ShootCow(this.transform.position, CrosshairPivot.transform.position,
                        currentProjectileStats, HolesManager.Instance.currentHole);
                }


                CrosshairPivot.transform.localPosition = Vector3.zero;

                gretaAnimator.Play("Throwing");
                AudioManager.Instance.PlaySfx(2);
                AudioManager.Instance.PlaySfx(9, 0.2f);
                OnCowIngropped?.Invoke();
                if (scorreggiaTimer >= currentProjectileStats.timeToScorreggia)
                {
                    AudioManager.Instance.PlaySfx(3);
                    scorreggiaTimer = 0;
                    scorreggiaSmoke.Play();
                }
                Debug.Log("<color=yellow>Mucca is being shot! </color>");

                StartCoroutine(PlayVacca(movingCowSecondsToSkip));

                IEnumerator PlayVacca(float secToSkip)
                {
                    yield return new WaitForSeconds(secToSkip);
                    movingCow.Play();
                    yield return null;
                }

            }
        }
        else // Is Paused or Game Over
        {
            CrosshairPivot.transform.localPosition = Vector3.zero;
            if (!gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                gretaAnimator.Play("Idle");

            if (TimerManager.GAME_OVER && !isGameOverTriggered)
            {
                isGameOverTriggered = true;


                StartCoroutine(CallGameOverCoroutine(secondsBeforeGameOverPanel));

                IEnumerator CallGameOverCoroutine(float secBeforeEvent)
                {
                    yield return new WaitForSeconds(secBeforeEvent);
                    TimerManager.Instance.OnGameOver?.Invoke();
                    yield return null;
                }
            }
        }
    }
}
