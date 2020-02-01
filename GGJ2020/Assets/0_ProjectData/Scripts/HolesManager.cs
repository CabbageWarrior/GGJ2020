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

    public GameObject holePrefab;
    public List<Hole> holes;

    public List<int> holesAmountToSpawnPerStage;
    public float delayBetweenRounds;

    public Hole currentHole;

    public static HolesManager Instance { get; set; }

    public Vector2 smallCenter;
    public Vector2 smallSize;
    public Vector2 bigCenter;
    public Vector2 bigSize;

    int currentStageIndex = 0;

    private void Awake()
    {
        Mooovment.OnCowShot += OnCowShot;
        Instance = this;

        EnholeTheHoesErrIMeanHoles(currentStageIndex++);
    }

    public void EnholeTheHoesErrIMeanHoles(int stageIndex)
    {
        stageIndex = Mathf.Clamp(stageIndex, 0, holesAmountToSpawnPerStage.Count-1);

        Debug.Log("Stage index: " + stageIndex);

        for (int i = 0; i < holesAmountToSpawnPerStage[stageIndex]; i++)
        {
            SpawnHole();
        }
    }

    public void CheckIfEveryHoleIsMucched()
    {
        bool everyHole = true;
        foreach(var hole in holes)
        {
            if(!hole.busy)
            {
                everyHole = false;
                break;
            }
        }

        if(everyHole)
        {
            SpariscTheHoles();
        }
    }

    public void SpariscTheHoles()
    {
        foreach(var hole in holes)
        {
            hole.instance.KillHole();
        }

        holes.Clear();
        StartCoroutine(StartNewHoleRoundAfterDelay());
    }

    IEnumerator StartNewHoleRoundAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenRounds);

        EnholeTheHoesErrIMeanHoles(currentStageIndex++);
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

        AddNewHole(spawnPosition);
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

            if(distance < hole.radius * 2)
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
        h.radius = smallHoleRadius;
        h.sprite = Instantiate(holePrefab, position, 
            Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
        h.sprite.transform.localScale = Vector3.one * smallHoleRadius;
        h.instance = h.sprite.GetComponent<HoleInstance>();
        holes.Add(h);
    }

    public void CowReachedHole(Hole hole)
    {
        // toggle hole
        hole.busy = !hole.busy;

        CheckIfEveryHoleIsMucched();
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
    public CownonBallController occowpied;
    public GameObject sprite;
    public HoleInstance instance;
}
