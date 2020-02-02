using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CownonBallController : MonoBehaviour
{
    public CownonBallStatistics cownonBallStatistics;
    private Hole hole;
    public LayerMask trumpLayer;
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

        var trump = Physics2D.OverlapCircle(targetPosition, 0.5f, trumpLayer);

        float trumpRadius = 1f;
        Vector2 trumpPosition = new Vector2(Trump.Instance.transform.position.x, Trump.Instance.transform.position.y);
        float cowTrumpDistance = Vector2.Distance(trumpPosition, transform.position);

        float radiusesSum = trumpRadius + cownonBallStatistics.finalScale;

        Debug.Log("Radiuses sum: " + radiusesSum + " trump rad: " + trumpRadius + 
            " cow rad: " + cownonBallStatistics.finalScale);

        if(cowTrumpDistance < radiusesSum)
        {
            this.GetComponent<Rigidbody2D>().simulated = true;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
            this.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
            this.GetComponent<SpriteRenderer>().sortingOrder = -10;
            AudioManager.Instance.PlaySfx(5);

            Trump.Instance.HitTrump();
            yield break;
        }

        if (hole != null)
        {
            if (!hole.busy)
            {
                this.GetComponent<SpriteRenderer>().sprite = cownonBallStatistics.StuckSprite[Random.Range(0, cownonBallStatistics.StuckSprite.Length)];
                hole.occowpied = this;
                hole.instance.cow = this.GetComponent<SpriteRenderer>();
                this.GetComponent<SpriteRenderer>().sortingOrder = -10;
                TimerManager.AddPoint();
            }
            else
            {
                this.GetComponent<Rigidbody2D>().simulated = true;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
                this.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
                this.GetComponent<SpriteRenderer>().sortingOrder = -10;
                hole.occowpied.GetComponent<Rigidbody2D>().simulated = true;
                hole.occowpied.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
                hole.occowpied.GetComponent<SpriteRenderer>().sprite = cownonBallStatistics.flyingSprite[Random.Range(0, cownonBallStatistics.flyingSprite.Length)];
                hole.occowpied.GetComponent<SpriteRenderer>().sortingOrder = -10;
                hole.occowpied.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
                hole.occowpied = null;
                AudioManager.Instance.PlaySfx(5);
                TimerManager.RemovePoint();
            }

            HolesManager.Instance.CowReachedHole(hole);
            timer = 0;
            time = 0.8f;
            Vector3 cachedScale = this.transform.localScale;
            while (timer < time)
            {
                timer += Time.deltaTime;

                transform.localScale = Vector3.LerpUnclamped(cachedScale, cachedScale * 1.5f,
                    cownonBallStatistics.insertionCurve.Evaluate(timer / time));               

                yield return null;
            }
        }
        else
        {
            HolesManager.Instance.AddNewHole(targetPosition);
            this.GetComponent<Rigidbody2D>().simulated = true;
            TimerManager.RemovePoint();
        }
    }
}
