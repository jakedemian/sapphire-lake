using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class Loader : MonoBehaviour {
    private void Awake() {
        EventManager.i.onLoadDataComplete += OnLoadDataComplete;
    }

    private void Start() {
        InitCloudSave();
    }

    private async void InitCloudSave() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        CloudSaver.i.Load();
    }

    private void OnLoadDataComplete() {
        SceneManager.LoadScene("TitleScreen");
    }
}
