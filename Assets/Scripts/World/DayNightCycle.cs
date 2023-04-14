using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DayPart {
    MORNING,
    AFTERNOON,
    EVENING,
    END,
    NULL
}

[System.Serializable]
public class TimeOfDay {
    public int hours;
    public int minutes;

    public int allMinutes => (hours * 60) + minutes;
    private int displayHours => hours > 12 ? hours - 12 : hours == 0 ? 12 : hours;
    public string displayTime => $"{displayHours.ToString()}:{(minutes >= 10 ? minutes.ToString() : $"0{minutes.ToString()}")}{(hours >= 12 ? "pm" : "am")}";

    public TimeOfDay(int h, int m) {
        hours = h;
        minutes = m;
    }

    public TimeOfDay(Consts.TIMES_IN_MINUTES minutes) {
        hours = (int)minutes / 60;
        minutes = 0;
    }

    public void AddMinutes(int _minutes) {
        minutes += _minutes;
        if (minutes >= 60) {
            minutes -= 60;
            hours += 1;

            if (hours >= 24) {
                // shouldn't happen for this game, but just in case
                hours = 0;
            }
        }
    }

    public float GetPercentToTime(TimeOfDay from, TimeOfDay to) {
        return (float)(allMinutes - from.allMinutes) /
            (float)(to.allMinutes - from.allMinutes);
    }
}

public class DayCycleTime : TimeOfDay {
    public TimeOfDay startOfDay { get; private set; }
    public TimeOfDay startOfAfternoon { get; private set; }
    public TimeOfDay startOfEvening { get; private set; }
    public TimeOfDay endOfDay { get; private set; }
    public DayPart dayPart { get; private set; }

    public DayCycleTime(Consts.TIMES_IN_MINUTES _startOfDayMinutes, Consts.TIMES_IN_MINUTES _startOfAfternoonMinutes, Consts.TIMES_IN_MINUTES _startOfEveningMinutes, Consts.TIMES_IN_MINUTES _endOfDayMinutes)
    : base(_startOfDayMinutes) {

        startOfDay = new TimeOfDay(_startOfDayMinutes);
        startOfAfternoon = new TimeOfDay(_startOfAfternoonMinutes);
        startOfEvening = new TimeOfDay(_startOfEveningMinutes);
        endOfDay = new TimeOfDay(_endOfDayMinutes);
    }

    public void Tick(int _minutes) {
        AddMinutes(_minutes);
        CheckForDayPartUpdates();
    }

    private void CheckForDayPartUpdates() {
        if (allMinutes >= endOfDay.allMinutes) {
            dayPart = DayPart.END;
        } else if (allMinutes >= startOfEvening.allMinutes) {
            dayPart = DayPart.EVENING;
        } else if (allMinutes >= startOfAfternoon.allMinutes) {
            dayPart = DayPart.AFTERNOON;
        } else if (dayPart != DayPart.MORNING) {
            dayPart = DayPart.MORNING;
        }
    }
}

public class DayNightCycle : MonoBehaviour {
    public static DayNightCycle i;

    public float secondsPerTick;
    public int minuteIncrements;
    public Consts.TIMES_IN_MINUTES morningStartTime;
    public Consts.TIMES_IN_MINUTES afternoonStartTime;
    public Consts.TIMES_IN_MINUTES eveningStartTime;
    public Consts.TIMES_IN_MINUTES endOfDayTime;

    public bool timePaused { get; private set; }
    public DayCycleTime time { get; private set; }
    float timer;

    private DayPart lastDayPart = DayPart.NULL;

    private void Awake() {
        i ??= this;
        time = new DayCycleTime(morningStartTime, afternoonStartTime, eveningStartTime, endOfDayTime);

        EventManager.i.onMinigameStart += OnMinigameStart;
        EventManager.i.onSuccessModalClosed += OnSuccessModalClosed;
        EventManager.i.onBlackScreenTransparent += OnBlackScreenTransparent;
        EventManager.i.onFishEscaped += OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
    }

    private void Update() {
        if (DebugHelper.i.freezeTimeOfDay) return;
        if (timePaused || time.dayPart == DayPart.END) return;

        timer += Time.deltaTime;
        if (timer > secondsPerTick) {
            time.Tick(minuteIncrements);
            timer = 0f;
            EventManager.i.e_TimeOfDayChanged(time);
        }

        if (lastDayPart != time.dayPart) {
            lastDayPart = time.dayPart;

            if (time.dayPart == DayPart.MORNING) {
                EventManager.i.e_DayStart();
                timePaused = true; // waiting for black screen to be gone
            } else if (time.dayPart == DayPart.AFTERNOON) {
                EventManager.i.e_AfternoonStart();
            } else if (time.dayPart == DayPart.EVENING) {
                EventManager.i.e_EveningStart();
            } else if (time.dayPart == DayPart.END) {
                EventManager.i.e_DayEnd();
                timePaused = true;
                return;
            }
        }
    }

    private void OnBlackScreenTransparent() {
        SetPaused(false);
    }

    private void OnMinigameStart() {
        SetPaused(true);
    }

    private void OnSuccessModalClosed() {
        SetPaused(false);
    }

    private void OnFishEscaped() {
        SetPaused(false);
    }

    private void OnFishCaptureOutOfTime() {
        SetPaused(false);
    }

    public DayPart GetDayPart() {
        if (time == null) {
            return DayPart.MORNING;
        }
        return time.dayPart;
    }

    public void SetPaused(bool paused) {
        timePaused = paused;

        if (timePaused) {
            EventManager.i.e_GameTimePaused();
        } else {
            EventManager.i.e_GameTimeResumed();
        }
    }
}
