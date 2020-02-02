using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trump : MonoBehaviour
{
    public static Trump Instance { get; set; }
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Animator anim;
    public Sprite fallSprite;

    public Transform A;
    public Transform B;

    public float timeToReachDestination;
    public float spawnDelay;
    public float trumpFrequencySecondsMin;
    public float trumpFrequencySecondsMax;

    private bool enabled;

    public AudioSource audioSource;
    public AnimationCurve musicFadeCurve;

    Coroutine flyCo;
    private void Awake()
    {
        Instance = this;
        currentSpawnTime = Random.Range(trumpFrequencySecondsMin, trumpFrequencySecondsMax);
    }

    public void EnableTrump()
    {
        enabled = true;
    }

    private void SpawnTrump()
    {
        bool left = Random.value > 0.5f;
        sprite.flipX = !left;
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        anim.enabled = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        sprite.transform.localPosition = new Vector3(left ? 6 : -6, 0, 0);

        flyCo = StartCoroutine(JustTrumpPassingBy(left));
    }

    IEnumerator JustTrumpPassingBy(bool left)
    {
        float timer = 0;

        Vector3 initialPos = left ? A.position : B.position;
        Vector3 finalPos = left ? B.position : A.position;


        Vector3 offset = new Vector2(0, Random.Range(0, 4.5f));

        audioSource.time = 0;
        audioSource.Play();

        initialPos += offset;
        finalPos += offset;

        transform.position = initialPos;

        while(timer < timeToReachDestination)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, finalPos, timer / timeToReachDestination);
            audioSource.volume = Mathf.Lerp(0, 1, musicFadeCurve.Evaluate(timer / timeToReachDestination));
            yield return null;
        }
        audioSource.Stop();
    }

    public void HitTrump()
    {
        if(flyCo != null)
        {
            StopCoroutine(flyCo);
        }

        anim.enabled = false;
        sprite.sprite = fallSprite;

        rb.simulated = true;
        float forceX = Random.Range(-150, 150);
        float forceY = Random.Range(100, 200);

        rb.AddForce(new Vector2(forceX, forceY));
        rb.AddTorque(Random.Range(-300, 300));

        TimerManager.Instance.AddTrumpPoints();
    }

    float spawnTimer = 0;
    float currentSpawnTime;
    bool hasDoneInitialDelay;
    bool hasSpawnedFirstTrump;
    private void Update()
    {
        if (!enabled)
            return;

        spawnTimer += Time.deltaTime;

        if(!hasDoneInitialDelay)
        {
            if(spawnTimer > spawnDelay)
            {
                spawnTimer = 0;
                hasDoneInitialDelay = true;
            }
            return;
        }

        if(hasSpawnedFirstTrump)
        {
            if(spawnTimer > currentSpawnTime + timeToReachDestination)
            {
                SpawnTrump();
                spawnTimer = 0;
            }
        }
        else
        {
            if (spawnTimer > currentSpawnTime)
            {
                hasSpawnedFirstTrump = true;
                SpawnTrump();
                spawnTimer = 0;
            }
        }
    }
}
