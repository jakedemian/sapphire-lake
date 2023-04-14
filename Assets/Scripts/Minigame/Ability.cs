using System.Collections;
using UnityEngine;

public class Ability : MonoBehaviour {
    public float cooldownSeconds;
    public bool isOnCooldown { get; private set; }

    private float cooldownTimer;
    private Coroutine cooldownCoroutine;

    private void OnDisable() {
        cooldownTimer = 0f;
        isOnCooldown = false;

        if (cooldownCoroutine != null) {
            StopCoroutine(cooldownCoroutine);
        }
    }

    public void Activate() {
        if (!isOnCooldown) {
            isOnCooldown = true;

            if (gameObject.activeInHierarchy) {
                cooldownCoroutine = StartCoroutine(DoCooldown());
            }
        } else {
            AudioManager.i.PlayOnCooldownSound();
        }
    }

    private IEnumerator DoCooldown() {
        cooldownTimer = cooldownSeconds;
        while (cooldownTimer > 0f) {
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isOnCooldown = false;
    }
}



