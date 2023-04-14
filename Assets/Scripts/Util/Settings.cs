public class SettingsValues {
    public float masterVolume = 1f;
    public float musicVolume = 0.5f;
    public float ambienceVolume = 0.5f;
    public float sfxVolume = 0.5f;
}

public static class Settings {
    public static SettingsValues ValueInstance = new SettingsValues();
}