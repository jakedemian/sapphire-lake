using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerId {
    Null,
    CastLineTutorial,
    FatigueInformationalTutorial,
    EnergizedInformationalTutorial,
}

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour {
    [SerializeField] private TriggerId triggerId;

    private void OnTriggerEnter2D(Collider2D other) {
        EventManager.i.e_TriggerActivated(triggerId);
        Destroy(gameObject);
    }
}
