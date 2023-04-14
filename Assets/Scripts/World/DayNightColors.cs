using UnityEngine;

enum LightingState {
    LIGHTING_UP,
    FULLY_LIT,
    DIMMING_DOWN
};

public class DayNightColors : MonoBehaviour {
    public DayNightColorData colorData;

    private SpriteRenderer sr;
    private TimeOfDay currentTime => DayNightCycle.i.time;
    private TimeOfDay startOfDay => DayNightCycle.i.time.startOfDay;
    private TimeOfDay endOfDay => DayNightCycle.i.time.endOfDay;

    private Color previousColor;
    private Color nextColor;

    private TimeOfDay previousTime;
    private TimeOfDay nextTime;

    private LightingState state = LightingState.LIGHTING_UP;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();

        sr.color = colorData.darkestShade;

        previousColor = colorData.darkestShade;
        nextColor = colorData.lightestShade;

        previousTime = startOfDay;
        nextTime = colorData.peakLightTimeStart;
    }

    private void Update() {
        if (currentTime.allMinutes >= endOfDay.allMinutes) return;

        if (
            currentTime.allMinutes >= colorData.peakLightTimeStart.allMinutes &&
            state == LightingState.LIGHTING_UP
        ) {
            state = LightingState.FULLY_LIT;
            sr.color = colorData.lightestShade;
        } else if (
            currentTime.allMinutes >= colorData.peakLightTimeEnd.allMinutes &&
            state == LightingState.FULLY_LIT
        ) {
            state = LightingState.DIMMING_DOWN;
            previousColor = colorData.lightestShade;
            nextColor = colorData.darkestShade;
            previousTime = colorData.peakLightTimeEnd;
            nextTime = endOfDay;
        }

        if (state == LightingState.FULLY_LIT) return;

        sr.color = Color.Lerp(
            previousColor,
            nextColor,
            currentTime.GetPercentToTime(previousTime, nextTime)
        );
    }
}

[System.Serializable]
public class DayNightColorData {
    public Color darkestShade;
    public Color lightestShade;

    public TimeOfDay peakLightTimeStart;
    public TimeOfDay peakLightTimeEnd;
}
