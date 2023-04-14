using UnityEngine;

public class DebugHelper : MonoBehaviour {
    public static DebugHelper i { get; private set; }
    [SerializeField] public bool freezeTimeOfDay;
    [SerializeField] public bool freezeCameraPosition;
    [SerializeField] public int debugTutorialStep;

    private void Awake() {
        i ??= this;
    }

    private void Update() {
        if (debugTutorialStep == 0) return;

        EventManager.i.e_TutorialStepChanged(debugTutorialStep);
        debugTutorialStep = 0;
    }
}