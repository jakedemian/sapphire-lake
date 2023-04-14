using UnityEngine;
using UnityEngine.Tilemaps;

public class DayNightTilemapColors : MonoBehaviour {
    public SpriteRenderer waterSpriteRenderer;
    public Tilemap landTilemap;

    public DayNightTilemapColorData colorData;

    private int breakpointIndex;
    private TimeOfDay previousTime;
    private TimeOfDay nextTime;
    private Color previousWaterColor;
    private Color nextWaterColor;
    private Color previousLandColor;
    private Color nextLandColor;

    private TimeOfDay previousTimeOfDay;
    private TimeOfDay currentTimeOfDay => DayNightCycle.i.time;
    private bool isDone = false;

    private void Awake() {
        previousTime = colorData.dayStartTime;
        nextTime = colorData.times[breakpointIndex];

        previousLandColor = colorData.startingLandColor;
        nextLandColor = colorData.landColors[breakpointIndex];

        previousWaterColor = colorData.startingWaterColor;
        nextWaterColor = colorData.waterColors[breakpointIndex];

        previousTimeOfDay = currentTimeOfDay;

        waterSpriteRenderer.color = previousWaterColor;
        landTilemap.color = previousLandColor;

        EventManager.i.onTimeOfDayChange += OnTimeOfDayChange;
    }

    private void OnTimeOfDayChange(TimeOfDay time) {
        UpdateColors();
    }

    private void UpdateColors() {
        if (isDone) return;

        if (currentTimeOfDay.allMinutes >= nextTime.allMinutes) {
            breakpointIndex++;

            if (colorData.times.Length <= breakpointIndex ||
            colorData.landColors.Length <= breakpointIndex ||
            colorData.waterColors.Length <= breakpointIndex) {
                isDone = true;
                return;
            }

            UpdateLerpValues();
        }

        waterSpriteRenderer.color = Color.Lerp(
            previousWaterColor,
            nextWaterColor,
            currentTimeOfDay.GetPercentToTime(previousTime, nextTime)
        );

        landTilemap.color = Color.Lerp(
            previousLandColor,
            nextLandColor,
            currentTimeOfDay.GetPercentToTime(previousTime, nextTime)
        );

        previousTimeOfDay = currentTimeOfDay;
    }

    private void UpdateLerpValues() {
        waterSpriteRenderer.color = nextWaterColor;
        landTilemap.color = nextLandColor;

        previousTime = colorData.times[breakpointIndex - 1];
        nextTime = colorData.times[breakpointIndex];

        previousLandColor = colorData.landColors[breakpointIndex - 1];
        nextLandColor = colorData.landColors[breakpointIndex];

        previousWaterColor = colorData.waterColors[breakpointIndex - 1];
        nextWaterColor = colorData.waterColors[breakpointIndex];
    }
}

[System.Serializable]
public class DayNightTilemapColorData {
    public TimeOfDay dayStartTime;
    public Color startingWaterColor;
    public Color startingLandColor;

    public Color[] waterColors;
    public Color[] landColors;
    public TimeOfDay[] times;
}
