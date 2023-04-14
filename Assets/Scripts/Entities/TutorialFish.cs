using System.Collections;
using UnityEngine;

public class TutorialFish : Fish {
    [SerializeField] private FishData overrideFishData;

    protected override void PopulateFishData() {
        fishData = overrideFishData;
    }

    protected override IEnumerator Wander() {
        // tutorial fish should not wander
        yield return null;
    }

    protected override IEnumerator Scare(Vector2 direction) {
        // tutorial fish should not scare
        yield return null;
    }
}