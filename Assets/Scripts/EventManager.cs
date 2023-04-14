using System;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public static EventManager i;

    private void Awake() {
        i ??= this;
    }

    public event Action<Vector2> onBobberLand;
    public void e_BobberLanded(Vector2 landingPosition) => onBobberLand?.Invoke(landingPosition);

    public event Action onBobberPickedUp;
    public void e_BobberPickedUp() => onBobberPickedUp?.Invoke();

    public event Action onMinigameStart;
    public void e_MinigameStart() => onMinigameStart?.Invoke();

    public event Action onMinigameInteractive;
    public void e_MinigameInteractive() => onMinigameInteractive?.Invoke();

    // starts moving off screen
    public event Action onMinigameBeginAnimatingOffscreen;
    public void e_MinigameBeginAnimatingOffscreen() => onMinigameBeginAnimatingOffscreen?.Invoke();

    // fully off screen
    public event Action onMinigameOffscreen;
    public void e_MinigameOffscreen() => onMinigameOffscreen?.Invoke();

    public event Action<GameObject> onFishHooked;
    public void e_FishHooked(GameObject fish) => onFishHooked?.Invoke(fish);

    public event Action<int> onInventoryCountChange;
    public void e_InventoryCountChanged(int count) => onInventoryCountChange?.Invoke(count);

    public event Action<TimeOfDay> onTimeOfDayChange;
    public void e_TimeOfDayChanged(TimeOfDay tod) => onTimeOfDayChange?.Invoke(tod);

    public event Action onDayStart;
    public void e_DayStart() => onDayStart?.Invoke();

    public event Action onDayEnd;
    public void e_DayEnd() => onDayEnd?.Invoke();

    public event Action onAfternoonStart;
    public void e_AfternoonStart() => onAfternoonStart?.Invoke();

    public event Action onEveningStart;
    public void e_EveningStart() => onEveningStart?.Invoke();

    public event Action onBlackScreenTransparent;
    public void e_BlackScreenTransparent() => onBlackScreenTransparent?.Invoke();

    public event Action onBlackScreenOpaque;
    public void e_BlackScreenOpaque() => onBlackScreenOpaque?.Invoke();

    public event Action<int> onTutorialStepChange;
    public void e_TutorialStepChanged(int newStep) => onTutorialStepChange?.Invoke(newStep);

    public event Action<int> onTutorialStepHide;
    public void e_TutorialStepHide(int step) => onTutorialStepHide?.Invoke(step);

    public event Action onVolumeChange;
    public void e_VolumeChanged() => onVolumeChange?.Invoke();

    public event Action onGamePaused;
    public void e_GamePaused() => onGamePaused?.Invoke();

    public event Action onGameUnpaused;
    public void e_GameUnpaused() => onGameUnpaused?.Invoke();

    public event Action<int, string> onMenuOptionSelected;
    public void e_MenuOptionSelected(int menuInstanceId, string value) => onMenuOptionSelected?.Invoke(menuInstanceId, value);

    public event Action<TriggerId> onTriggerActivated;
    public void e_TriggerActivated(TriggerId triggerId) => onTriggerActivated?.Invoke(triggerId);

    public event Action onGameTimePaused;
    public void e_GameTimePaused() => onGameTimePaused?.Invoke();

    public event Action onGameTimeResumed;
    public void e_GameTimeResumed() => onGameTimeResumed?.Invoke();

    public event Action onFishCaptured;
    public void e_FishCaptured() => onFishCaptured?.Invoke();

    public event Action onFishCaptureOutOfTime;
    public void e_FishCaptureOutOfTime() => onFishCaptureOutOfTime?.Invoke();

    public event Action onFishEscaped;
    public void e_FishEscaped() => onFishEscaped?.Invoke();

    public event Action onFishAwaitingCapture;
    public void e_FishAwaitingCapture() => onFishAwaitingCapture?.Invoke();

    public event Action onSuccessModalShow;
    public void e_SuccessModalShow() => onSuccessModalShow?.Invoke();

    public event Action onSuccessModalClosed;
    public void e_SuccessModalClosed() => onSuccessModalClosed?.Invoke();

    public event Action onFishFatigueStart;
    public void e_FishFatigueStart() => onFishFatigueStart?.Invoke();

    public event Action onFishFatigueEnd;
    public void e_FishFatigueEnd() => onFishFatigueEnd?.Invoke();

    public event Action onFishEnergizeStart;
    public void e_FishEnergizeStart() => onFishEnergizeStart?.Invoke();

    public event Action onFishEnergizeEnd;
    public void e_FishEnergizeEnd() => onFishEnergizeEnd?.Invoke();

    public event Action onTutorialModalHidden;
    public void e_TutorialModalHidden() => onTutorialModalHidden?.Invoke();

    /////////////////////////////////////////
    // CLOUD SAVING
    public event Action onSaveDataStart;
    public void e_SaveDataStart() => onSaveDataStart?.Invoke();

    public event Action onSaveDataComplete;
    public void e_SaveDataComplete() => onSaveDataComplete?.Invoke();

    public event Action onLoadDataStart;
    public void e_LoadDataStart() => onLoadDataStart?.Invoke();

    public event Action onLoadDataComplete;
    public void e_LoadDataComplete() => onLoadDataComplete?.Invoke();
}
