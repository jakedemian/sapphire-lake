using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {
    public Sprite buttonDisabled;
    public Sprite buttonEnabled;
    public Sprite buttonPressed;
    public Image cooldownMask;

    private SpriteRenderer sr;
    private Coroutine depressCoroutine;
    private Ability ability;
    private const float buttonPressAnimationSeconds = 0.1f;
    private Image img;
    private bool abilityOnCooldown;

    protected virtual void Awake() {
        img = GetComponent<Image>();
        ability = GetComponent<Ability>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        if (cooldownMask != null) {
            cooldownMask.fillAmount = 0f;
        }

        if (depressCoroutine != null) {
            StopCoroutine(depressCoroutine);
        }
    }

    protected virtual void SetInitalButtonState() {
        Enable();
    }

    private void Update() {
        if (ability.isOnCooldown && !abilityOnCooldown) {
            abilityOnCooldown = true;
            Press(ability.cooldownSeconds);
        } else if (!ability.isOnCooldown && abilityOnCooldown) {
            abilityOnCooldown = false;
        }
    }

    public void Enable() {
        img.sprite = buttonEnabled;
    }

    public void Disable() {
        img.sprite = buttonDisabled;
    }

    public void Press(float cooldown) {
        img.sprite = buttonPressed;
        depressCoroutine = StartCoroutine(WaitForDepress(cooldown));
    }

    private IEnumerator WaitForDepress(float cooldown) {
        yield return new WaitForSeconds(buttonPressAnimationSeconds);

        float adjustedCooldown = cooldown - buttonPressAnimationSeconds;
        bool hasMask = cooldownMask != null;
        if (adjustedCooldown >= 0f) {
            Disable();
            float timer = adjustedCooldown;
            while (timer > 0f) {
                timer -= Time.deltaTime;
                float percent = timer / cooldown;
                if (hasMask) {
                    cooldownMask.fillAmount = percent;
                }
                yield return null;
            }
            Enable();
        }

    }
}

