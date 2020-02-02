using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CownonBall", menuName = "ScriptableObjects/CownonBallStatistics", order = 1)]
public class CownonBallStatistics : ScriptableObject
{
    [Space]
    public float minShake;
    public float maxShake;
    public float loopTime;

    public AnimationCurve shakeCurve;

    public Vector2 rotationSpeed;
    public float initialScale;
    public float finalScale;


    [Space]
    public AnimationCurve movementCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve insertionCurve;

    [Space]
    public Sprite[] flyingSprite;
    public Sprite[] StuckSprite;
}
