using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    public float moveSeconds;
    public float minPauseBetweenMovements;
    public float maxPauseBetweenMovements;
    public float minMoveDistance;
    public float maxMoveDistance;
    public AnimationCurve movementCurve;
    public float bobberScareDistance;
    public float bobberScareSeconds;
    public float bobberScareSpeed;
    public float bobberSightDistance;
    public float travelToBobberSpeed;
    public float fov;
    public float hookedDistance;
    [SerializeField] private float[] sizes = new float[3];

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Coroutine activeCoroutine;
    protected Coroutine headToBobberCoroutine;
    protected bool isHeadingToBobber;
    protected Vector2 bobberPosition;
    protected bool isHooked;
    protected FishData fishData;
    protected bool flaggedForDestroy;
    protected LayerMask worldLayerMask;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        worldLayerMask = LayerMask.GetMask("World");

        PopulateFishData();

        Dictionary<string, float> sizeMap = new Dictionary<string, float>() {
            {Consts.FISH_SIZE_SMALL, sizes[0]},
            {Consts.FISH_SIZE_MEDIUM, sizes[1]},
            {Consts.FISH_SIZE_LARGE, sizes[2]},
        };

        float size = sizeMap[fishData.size];
        transform.localScale = new Vector3(size, size, size);

        EventManager.i.onBobberLand += OnBobberLand;
        EventManager.i.onBobberPickedUp += OnBobberPickedUp;
        EventManager.i.onDayEnd += OnDayEnd;
        EventManager.i.onMinigameInteractive += OnMinigameInteractive;

        activeCoroutine = StartCoroutine(Wander());
    }

    private void OnDestroy() {
        StopActiveCoroutine();
        EventManager.i.onBobberLand -= OnBobberLand;
        EventManager.i.onBobberPickedUp -= OnBobberPickedUp;
        EventManager.i.onDayEnd -= OnDayEnd;
        EventManager.i.onMinigameInteractive -= OnMinigameInteractive;
    }

    private void Update() {
        if (bobberPosition != Vector2.zero && IsInFront(bobberPosition) && !isHeadingToBobber && HasLineOfSight()) {
            isHeadingToBobber = true;
            GetComponent<Collider2D>().enabled = false;
            StopActiveCoroutine();
            headToBobberCoroutine = StartCoroutine(HeadToBobber());
        }

        if (isHooked && Input.GetButtonDown("Hook")) {
            MinigameController.i.InitializeMinigame(fishData);
            isHooked = false;
            flaggedForDestroy = true;
        }
    }

    private bool HasLineOfSight() {
        Vector2 position = transform.position;
        Vector2 direction = (bobberPosition - position).normalized;
        float distance = Vector2.Distance(position, bobberPosition);

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, worldLayerMask);
        return hit.collider == null;
    }

    protected virtual void PopulateFishData() {
        fishData = DataInstance.i.GetRandomFishByWeights();
    }

    private void OnBobberLand(Vector2 landingPosition) {
        bobberPosition = landingPosition;
        if (Vector2.Distance(transform.position, bobberPosition) < bobberScareDistance) {
            StopActiveCoroutine();
            StartCoroutine(Scare(((Vector2)transform.position - bobberPosition).normalized));
        }
    }

    private void OnBobberPickedUp() {
        if (isHeadingToBobber && !isHooked) {
            isHeadingToBobber = false;
            rb.velocity = Vector2.zero;
            StopActiveCoroutine();
            activeCoroutine = StartCoroutine(Wander());
            GetComponent<Collider2D>().enabled = true;
        }

        bobberPosition = Vector2.zero;
    }

    private void OnMinigameInteractive() {
        if (flaggedForDestroy) {
            Destroy(gameObject);
        }
    }

    private bool IsInFront(Vector2 position) {
        Vector2 directionToTarget = position - (Vector2)transform.position;
        float angle = Vector2.Angle(transform.right, directionToTarget);

        if (angle <= fov / 2f && directionToTarget.magnitude <= bobberSightDistance) {
            return true;
        }

        return false;
    }

    protected virtual IEnumerator Wander() {
        while (true) {
            float timer = 0f;
            Vector2 startPosition = transform.position;
            float moveDistance = UnityEngine.Random.Range(minMoveDistance, maxMoveDistance);
            Vector2 currentWanderTarget = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle.normalized * moveDistance);

            const int MAX_TRIES = 5;
            for (int i = 0; i < MAX_TRIES; i++) {
                var hit = Physics2D.Raycast(currentWanderTarget, Vector2.zero);
                if (hit.collider != null) {
                    currentWanderTarget = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle.normalized * moveDistance);
                    continue;
                }

                Vector2 direction = currentWanderTarget - (Vector2)transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                while (timer < moveSeconds) {
                    timer += Time.deltaTime;
                    float t = timer / moveSeconds;
                    transform.position = Vector2.Lerp(startPosition, currentWanderTarget, movementCurve.Evaluate(t));
                    yield return null;
                }

                break;
            }

            float waitTime = UnityEngine.Random.Range(minPauseBetweenMovements, maxPauseBetweenMovements);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator HeadToBobber() {
        Vector2 directionToBobber = (bobberPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(directionToBobber.y, directionToBobber.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = directionToBobber * travelToBobberSpeed;

        while (Vector2.Distance(transform.position, bobberPosition) > hookedDistance) {
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isHooked = true;
        EventManager.i.e_BobberPickedUp();
        EventManager.i.e_FishHooked(gameObject);
        AudioManager.i.PlayHookSound();
    }

    protected virtual IEnumerator Scare(Vector2 direction) {
        rb.velocity = direction * bobberScareSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float timer = 0f;
        Color spriteColor = sr.color;
        while (timer < bobberScareSeconds) {
            timer += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - (timer / bobberScareSeconds));
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDayEnd() {
        if (headToBobberCoroutine != null && isHeadingToBobber) {
            StopCoroutine(headToBobberCoroutine);
        }

        if (isHooked) {
            isHooked = false;
        }
    }

    private void StopActiveCoroutine() {
        if (activeCoroutine != null) {
            StopCoroutine(activeCoroutine);
        }
    }
}
