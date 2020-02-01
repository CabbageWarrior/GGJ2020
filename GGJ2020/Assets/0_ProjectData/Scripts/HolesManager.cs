using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public Hole[] holes;

    public Action<Vector3, bool> OnHoleFound;
    public Action<Vector3> OnHoleMissed;

    private void Awake()
    {
        Mooovment.OnCowShot += OnCowShot;
    }

    private void OnCowShot(Vector3 cursorPosition)
    {
        foreach(var hole in holes)
        {
            Vector3 holePosition = hole.transform.position;
            float distance = Vector3.Distance(holePosition, cursorPosition);

            if(distance < hole.radius)
            {
                OnHoleFound?.Invoke(holePosition, hole.busy);
                return;
            }
        }

        OnHoleMissed?.Invoke(cursorPosition);
    }

    private void OnDrawGizmos()
    {
        if (holes == null)
            return;

        Gizmos.color = Color.yellow;

        foreach(var hole in holes)
        {
            if (hole.transform == null)
                continue;

            Gizmos.DrawWireSphere(hole.transform.position, hole.radius);
        }
    }
}

[System.Serializable]
public class Hole
{
    public Transform transform;
    public float radius;
    public bool busy;
}
