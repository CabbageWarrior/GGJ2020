using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CownonBallController : MonoBehaviour
{
    public CownonBallStatistics cownonBallStatistics;
    private Hole hole;

    private void Awake()
    {
        this.GetComponent<SpriteRenderer>().sprite = cownonBallStatistics.flyingSprite[Random.Range(0, cownonBallStatistics.flyingSprite.Length)];

    }

    private void Update()
    {
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
    }

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
        float rotationSpeed = Random.Range(cownonBallStatistics.rotationSpeed.x, cownonBallStatistics.rotationSpeed.y);
        while (timer < time)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.LerpUnclamped(startingPosition, targetPosition,
                cownonBallStatistics.movementCurve.Evaluate(timer / time));

            transform.localScale = Vector3.Lerp(cownonBallStatistics.initialScale * Vector3.one, cownonBallStatistics.finalScale * Vector3.one,
                cownonBallStatistics.scaleCurve.Evaluate(timer / time));

            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        if (hole != null)
        {
            if (!hole.busy)
            {
                this.GetComponent<SpriteRenderer>().sprite = cownonBallStatistics.StuckSprite[Random.Range(0, cownonBallStatistics.StuckSprite.Length)];
                hole.occowpied = this;
            }
            else
            {
                this.GetComponent<Rigidbody2D>().simulated = true;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
                this.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
                hole.occowpied.GetComponent<Rigidbody2D>().simulated = true;
                hole.occowpied.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
                hole.occowpied.GetComponent<SpriteRenderer>().sprite = cownonBallStatistics.flyingSprite[Random.Range(0, cownonBallStatistics.flyingSprite.Length)];
                hole.occowpied.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
                hole.occowpied = null;
            }
            HolesManager.Instance.CowReachedHole(hole);
        }
        else
        {
            HolesManager.Instance.AddNewHole(targetPosition);
            this.GetComponent<Rigidbody2D>().simulated = true;
        }
    }
}
