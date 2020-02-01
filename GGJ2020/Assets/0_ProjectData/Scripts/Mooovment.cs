using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mooovment : MonoBehaviour
{
    public float mooovementSpeed = 5.0f;
    public GameObject CrosshairPivot;
    public Animation movingCow;
    public Animator gretaAnimator;

    public CownonBallController currentProjectile;
    public CownonBallStatistics currentProjectileStats;

    public static Action<Vector3> OnCowShot;
    public static Action OnCowIngropped;

    private Vector3 ClampVectorInScreen(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        float gretaVerticalViewportPosition = Camera.main.WorldToViewportPoint(transform.position).y;

        viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, gretaVerticalViewportPosition, 1);

        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        worldPosition.z = 0;

        return worldPosition;
    }

    private void Start()
    {
        OnCowIngropped?.Invoke();
    }

    void Update()
    {
        //Debug.Log("Mouse x: " + Input.GetAxis("HorizontalCursorMooovement") + 
        //    " y: " + Input.GetAxis("HorizontalCursorMooovement"));

        if (Input.GetMouseButtonDown(0) && gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            CrosshairPivot.transform.localPosition = Vector3.zero;
            gretaAnimator.Play("Aiming");

        }
        else if (Input.GetMouseButton(0) && (gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Aiming")
                || gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("CowScalcing")))
        {
            Vector3 mouseInput = new Vector3(Input.GetAxis("HorizontalCursorMooovement"),
                                             Input.GetAxis("VerticalCursorMooovement"), 0);

            Vector3 currentCrosshairPivotPosition = CrosshairPivot.transform.position;
            Vector3 targetPosition = currentCrosshairPivotPosition - mouseInput * mooovementSpeed;

            //Debug.Log("Target position: " + targetPosition + "viewport botLeft corner: " + Camera.main.ViewportToWorldPoint(Vector3.forward)
            //    + " Target to viewport pos: " + Camera.main.WorldToViewportPoint(targetPosition));

            //Debug.Log("Clamped position: " + ClampVectorInScreen(targetPosition));

            // process position, clamp between screen borderzzz, apply mucca shake!

            targetPosition = ClampVectorInScreen(targetPosition);

            CrosshairPivot.transform.position = targetPosition;
        }
        else if (Input.GetMouseButtonUp(0) && (gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Aiming")
                || gretaAnimator.GetCurrentAnimatorStateInfo(0).IsName("CowScalcing")))
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
            OnCowIngropped?.Invoke();
            movingCow.Play();

            Debug.Log("<color=yellow>Mucca is being shot! </color>");
        }
    }
}
