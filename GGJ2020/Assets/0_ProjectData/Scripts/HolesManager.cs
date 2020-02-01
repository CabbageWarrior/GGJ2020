using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public float smallHoleRadius;
    public float bigHoleRadius;
    [Range(0,1)]
    public float smallHoleChance;
    public int initialHoleAmount = 5;


    public List<Hole> holes;

    public Hole currentHole;

    public static HolesManager Instance { get; set; }

    public Vector2 smallCenter;
    public Vector2 smallSize;
    public Vector2 bigCenter;
    public Vector2 bigSize;

    private void Awake()
    {
        Mooovment.OnCowShot += OnCowShot;
        Instance = this;

        for(int i = 0; i < initialHoleAmount; i++)
        {
            SpawnHole();
        }
    }

    private void SpawnHole()
    {
        bool small = UnityEngine.Random.value < smallHoleChance;

        float radius = small ? smallHoleRadius : bigHoleRadius;

        Vector3 min = small ? smallCenter - smallSize / 2 : bigCenter - bigSize / 2;
        Vector3 max = small ? smallCenter + smallSize / 2 : bigCenter + bigSize / 2;

        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(min.x, max.x),
                                            UnityEngine.Random.Range(min.y, max.y), 0);

        while(HoleOverlapsWithExistingHoles(spawnPosition, radius))
        {
            spawnPosition = new Vector3(UnityEngine.Random.Range(min.x, max.x),
                                        UnityEngine.Random.Range(min.y, max.y), 0);
        }

        Hole newHole = new Hole();
        newHole.radius = radius;
        newHole.position = spawnPosition;
        newHole.busy = false;

        holes.Add(newHole);
    }

    private bool HoleOverlapsWithExistingHoles(Vector3 position, float radius)
    {
        foreach(var hole in holes)
        {
            float distance = Vector3.Distance(hole.position, position);
            float radiusesSum = hole.radius + radius;
            if (distance < radiusesSum)
            {
                return true;
            }
        }
        return false;
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
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(smallCenter, smallSize);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bigCenter, bigSize);
    }
}

[System.Serializable]
public class Hole
{
    public Vector3 position;
    public float radius;
    public bool busy;
}
