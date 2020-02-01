﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CownonBall", menuName = "ScriptableObjects/CownonBallStatistics", order = 1)]
public class CownonBallStatistics : ScriptableObject
{
    [Space]
    public Vector2 shakeTime;
    public Vector2 shakeDuration;
    public Vector2 shakeIntensity;

    public float initialScale;
    public float finalScale;


    [Space]
    public AnimationCurve movementCurve;
    public AnimationCurve scaleCurve;

    [Space]
    public Sprite[] flyingSprite;
    public Sprite[] StuckSprite;
}
