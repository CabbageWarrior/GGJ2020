using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CownonBallController : MonoBehaviour
{
    private CownonBallStatistics cownonBallStatistics;
    private Hole hole;

    public void ShootCow(Vector3 startingPosition, Vector3 targetPosition, 
        CownonBallStatistics stats, Hole hole)
    {
        cownonBallStatistics = stats;
        this.hole = hole;

        StartCoroutine(AnimateToPosition(startingPosition, targetPosition));
    }

    IEnumerator AnimateToPosition(Vector3 startingPosition, Vector3 targetPosition)
    {
        float timer = 0;
        float time = 1;

        while(timer < time)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(startingPosition, targetPosition,
                cownonBallStatistics.movementCurve.Evaluate(timer / time));

            transform.localScale = Vector3.Lerp(startingPosition, targetPosition,
                cownonBallStatistics.scaleCurve.Evaluate(timer / time));

            yield return null;
        }

        if(hole != null)
        {
            HolesManager.Instance.CowReachedHole(hole);
        }
        else
        {
            HolesManager.Instance.AddNewHole(targetPosition);
        }
    }
}
