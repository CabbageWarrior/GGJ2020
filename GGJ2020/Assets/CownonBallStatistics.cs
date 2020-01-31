using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CownonBall", menuName = "ScriptableObjects/CownonBallStatistics", order = 1)]
public class CownonBallStatistics : ScriptableObject
{
    public Animator cownonballAnimator;

    public float shakeTime;
    public float shakeDuration;
    public float shakeIntensity;
}
