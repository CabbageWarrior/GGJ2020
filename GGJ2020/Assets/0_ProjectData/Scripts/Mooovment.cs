using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mooovment : MonoBehaviour
{
    public float mooovementSpeed = 5.0f;
    public GameObject CrosshairPivot;
    bool resetCrosshairPosition = true;

    public GameObject currentProjectile;

    Vector3 cursorInitialPosition = new Vector3();

    bool isAiming = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (resetCrosshairPosition)
            {
                resetCrosshairPosition = false;
                CrosshairPivot.transform.localPosition = new Vector3();
                //CrosshairPivot.transform.rotation = Quaternion.identity;
                cursorInitialPosition = Input.mousePosition;

            }

            Vector3 resultVector = cursorInitialPosition - Input.mousePosition;
            if (resultVector.y > 0)
            {
                isAiming = true;
                CrosshairPivot.transform.localPosition = (cursorInitialPosition - Input.mousePosition) * mooovementSpeed;
            }
            else
            {
                isAiming = false;
                CrosshairPivot.transform.localPosition = new Vector3();
            }

        }

        if (!Input.GetMouseButton(0))
        {
            if (!resetCrosshairPosition)
            {
                resetCrosshairPosition = true;

                if (isAiming)
                {
                    //TODO: shoot behaviour
                    Debug.Log("Mooo");

                    isAiming = false;
                }

                CrosshairPivot.transform.localPosition = new Vector3();
                //CrosshairPivot.transform.rotation = Quaternion.identity;
            }
        }
    }
}
