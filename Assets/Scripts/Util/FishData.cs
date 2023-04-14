[System.Serializable]
public class FishData {
    public string name;
    public string portraitPath;
    public int passiveStrength;
    public float reelRecoverySeconds;
    public int reelVulnerability;
    public float startingPercentage;
    public float captureWindowSeconds;
    public float fatigueChanceOnTick;
    public float fatigueEffectLengthSeconds;
    public int yankVulnerability;
    public float yankRecoverySeconds;
    public int energizedFishStrength;
    public float energizedChanceOnTick;
    public float energizedEffectLengthSeconds;
    public float holdTime;
    public int rarityWeight;
    public string rarityType;
    public string dayPart;
    public string size;
}

public static class FishDataHelper {
    public static FishData[] GetAllFishData() {
        return Json.Loader.LoadJson<FishData>("Json/fish");
    }
}