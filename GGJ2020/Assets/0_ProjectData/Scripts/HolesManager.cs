using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public Hole[] holes;

    public Hole currentHole;

    public static HolesManager Instance { get; set; }

    private void Awake()
    {
        Mooovment.OnCowShot += OnCowShot;
        Instance = this;
    }

    private void OnCowShot(Vector3 cursorPosition)
    {
        foreach(var hole in holes)
        {
            Vector3 holePosition = hole.position;
            float distance = Vector3.Distance(holePosition, cursorPosition);

            if(distance < hole.radius)
            {
                currentHole = hole;
                return;
            }
        }

        currentHole = null;
    }

    public void AddNewHole(Vector3 position)
    {
        Hole h = new Hole();
        h.busy = false;
        h.position = position;
        h.radius = 1; // todo
    }

    public void CowReachedHole(Hole hole)
    {
        // toggle hole
        hole.busy = !hole.busy;
    }

    private void OnDrawGizmos()
    {
        if (holes == null)
            return;

        Gizmos.color = Color.yellow;

        foreach(var hole in holes)
        {
            if (hole.position == null)
                continue;

            Gizmos.DrawWireSphere(hole.position, hole.radius);
        }
    }
}

[System.Serializable]
public class Hole
{
    public Vector3 position;
    public float radius;
    public bool busy;
}
