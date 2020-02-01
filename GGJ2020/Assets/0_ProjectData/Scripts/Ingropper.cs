using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingropper : MonoBehaviour
{
    [SerializeField]
    CownonBallStatistics[] CownonBallStatistics;

    [SerializeField]
    Mooovment greta;

    void InitializeVacca()
    {
        greta.currentProjectileStats = CownonBallStatistics[Random.Range(0, CownonBallStatistics.Length)];
    }

    private void Awake()
    {
        Mooovment.OnCowIngropped += InitializeVacca;
    }

    private void OnDestroy()
    {
        Mooovment.OnCowIngropped -= InitializeVacca;

    }
}
