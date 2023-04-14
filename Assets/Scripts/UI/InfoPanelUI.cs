using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelUI : UIModal {
    public TextMeshProUGUI time;
    public TextMeshProUGUI fishCount;

    private string currentTime;
    private string currentFishCount;

    new private void Awake() {
        base.Awake();
        EventManager.i.onInventoryCountChange += OnInventoryCountChange;
        EventManager.i.onTimeOfDayChange += OnTimeOfDayChange;
        EventManager.i.onMinigameStart += OnMinigameStart;
        EventManager.i.onMinigameOffscreen += OnMinigameOffscreen;
        EventManager.i.onDayEnd += OnDayEnd;
        EventManager.i.onBlackScreenTransparent += OnBlackScreenGone;
    }

    private void OnInventoryCountChange(int count) {
        fishCount.text = count.ToString();
    }

    private void OnTimeOfDayChange(TimeOfDay timeOfDay) {
        time.text = timeOfDay.displayTime;
    }

    private void OnDayEnd() {
        base.Hide();
    }

    private void OnMinigameStart() {
        base.Hide();
    }

    private void OnMinigameOffscreen() {
        base.Show();
    }

    private void OnBlackScreenGone() {
        base.Show();
    }

}
