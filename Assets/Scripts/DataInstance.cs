using System.Collections.Generic;
using UnityEngine;

public class DataInstance : MonoBehaviour {
    public static DataInstance i;

    public FishData[] allFishData { get; private set; }
    private Dictionary<string, DayPart> dayPartMapper = new Dictionary<string, DayPart> {
        {"MORNING", DayPart.MORNING},
        {"AFTERNOON", DayPart.AFTERNOON},
        {"EVENING", DayPart.EVENING}
    };

    private void Awake() {
        i ??= this;
        allFishData = FishDataHelper.GetAllFishData();
    }

    public FishData GetRandomFish() {
        return allFishData[Random.Range(0, allFishData.Length)];
    }

    public FishData GetRandomFishByWeights() {
        int totalWeight = 0;

        foreach (FishData fish in allFishData) {
            if (MatchesCurrentDayPart(fish)) {
                totalWeight += fish.rarityWeight;
            }
        }

        float randomWeight = UnityEngine.Random.Range(0, totalWeight);
        float weightSum = 0;

        for (int i = 0; i < allFishData.Length; i++) {
            if (!MatchesCurrentDayPart(allFishData[i])) {
                continue;
            };

            weightSum += allFishData[i].rarityWeight;
            if (randomWeight < weightSum) {
                return allFishData[i];
            }
        }

        return allFishData[allFishData.Length - 1];
    }

    public Sprite GetFishPortaitByName(string name) {
        foreach (FishData fish in allFishData) {
            if (fish.name.Equals(name)) {
                return Resources.Load<Sprite>(fish.portraitPath);
            }
        }
        return null;
    }

    public FishData GetFishByName(string name) {
        foreach (FishData fish in allFishData) {
            if (fish.name.Equals(name)) {
                return fish;
            }
        }

        return null;
    }

    private bool MatchesCurrentDayPart(FishData fish) {
        DayPart currentDayPart = DayNightCycle.i != null ?
            DayNightCycle.i.time.dayPart :
            DayPart.MORNING;

        if ((dayPartMapper.ContainsKey(fish.dayPart) &&
                dayPartMapper[fish.dayPart] == currentDayPart
            ) ||
            fish.dayPart.Equals("ANY")) {
            return true;
        }
        return false;
    }
}
