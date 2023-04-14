using System.Collections.Generic;


public class GlobalValues {
    public int dayCount =
#if (UNITY_EDITOR)
    7;
#else
    1;
#endif
    public bool tutorialActive = false;
    public Dictionary<string, int> fishCollection;

    public GlobalValues() {
        fishCollection = new Dictionary<string, int>();
    }
}

public static class Globals {
    public static GlobalValues ValueInstance = new GlobalValues();
}