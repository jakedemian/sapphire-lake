using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour {
    public float moveDownOnHookedDistance;
    public Material hookedMaterial;
    public float hookedColorLightness;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float bobDistance;
    [SerializeField] private float bobSeconds;

    private GameObject boat;
    private LineRenderer lr;
    private Coroutine bobCoroutine;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
        boat = GameObject.FindGameObjectWithTag("Player");
        EventManager.i.onBobberLand += OnBobberLand;
        EventManager.i.onFishHooked += OnFishHooked;
    }

    private void Update() {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, boat.transform.position);
    }

    private void LateUpdate() {
        Color c = spriteRenderer.color;
        lr.startColor = c;
        lr.endColor = c;
    }

    private void OnDestroy() {
        if (bobCoroutine != null) {
            StopCoroutine(bobCoroutine);
        }

        EventManager.i.onBobberLand -= OnBobberLand;
        EventManager.i.onFishHooked -= OnFishHooked;
    }

    private void ActivateChildWithTag(string tag) {
        Transform child = Util.GetChildWithTag(tag, transform);
        child?.gameObject.SetActive(true);
    }

    private void DeactivateChildWithTag(string tag) {
        Transform child = Util.GetChildWithTag(tag, transform);
        child?.gameObject.SetActive(false);
    }

    private void OnBobberLand(Vector2 landingPosition) {
        ActivateChildWithTag("BobberReflection");
        ActivateChildWithTag("BobberRipples");
        bobCoroutine = StartCoroutine(Bob());
    }

    private void OnFishHooked(GameObject fish) {
        if (bobCoroutine != null) {
            StopCoroutine(bobCoroutine);
        }
        DeactivateChildWithTag("BobberRipples");
        ActivateChildWithTag("HookPrompt");
        ActivateChildWithTag("BobberSplashing");
        spriteRenderer.sharedMaterial = hookedMaterial;
        spriteRenderer.color = new Color(hookedColorLightness, hookedColorLightness, hookedColorLightness, 1f);
        Destroy(GetComponent<DayNightColors>());
        transform.position = new Vector2(transform.position.x, transform.position.y - moveDownOnHookedDistance);
    }

    private IEnumerator Bob() {
        bool isUp = true;
        Vector2 basePosition = transform.position;

        while (true) {
            yield return new WaitForSeconds(bobSeconds);
            isUp = !isUp;
            spriteRenderer.transform.position = basePosition + new Vector2(0, isUp ? 0 : -bobDistance);
        }
    }
}
