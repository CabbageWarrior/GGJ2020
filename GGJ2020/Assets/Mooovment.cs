using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mooovment : MonoBehaviour
{
    public float mooovementSpeed = 5.0f;
    public GameObject CrosshairPivot;
    public GameObject Crosshair;
    bool resetCrosshairPosition = true;

    public GameObject currentProjectile;


    Vector3 cursorInitialPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (resetCrosshairPosition)
            {
                resetCrosshairPosition = false;
                Crosshair.transform.localPosition = new Vector3();
                CrosshairPivot.transform.rotation = Quaternion.identity;
                cursorInitialPosition = Input.mousePosition;

            }

            Crosshair.transform.position = (cursorInitialPosition - Input.mousePosition) * mooovementSpeed;


        }

        if (!Input.GetMouseButton(0))
        {
            //TODO: shoot behaviour
           
            resetCrosshairPosition = true;
            Crosshair.transform.localPosition = new Vector3();
            CrosshairPivot.transform.rotation = Quaternion.identity;     
        }
    }
}
