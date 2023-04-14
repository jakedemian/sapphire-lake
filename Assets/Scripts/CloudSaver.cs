using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class CloudSaver : MonoBehaviour {
    public static CloudSaver i;

    private void Awake() {
        i ??= this;
    }

    private bool useCloudSave() {
#if (UNITY_EDITOR)
        return EditorDebug.useCloudSave;
#else
        return true;
#endif
    }

    private async Task InitCloudSave() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task Save() {
        EventManager.i.e_SaveDataStart();

        if (useCloudSave()) {
            GlobalValues globals = Globals.ValueInstance;
            SettingsValues settings = Settings.ValueInstance;
            var data = new Dictionary<string, object> {
                { "globals", globals },
                { "settings", settings }
            };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);

        }
        EventManager.i.e_SaveDataComplete();
    }

    public async Task Load() {
        EventManager.i.e_LoadDataStart();

        if (useCloudSave()) {
            var query = await CloudSaveService.Instance.Data.LoadAsync(
                new HashSet<string> { "globals", "settings" }
            );

            if (query.ContainsKey("globals")) {
                GlobalValues globals = JsonConvert.DeserializeObject<GlobalValues>(
                    query["globals"]
                );
                Globals.ValueInstance = globals;
            }

            if (query.ContainsKey("settings")) {
                SettingsValues settings = JsonConvert.DeserializeObject<SettingsValues>(
                    query["settings"]
                );
                Settings.ValueInstance = settings;
            }
        }
        EventManager.i.e_LoadDataComplete();
    }
}

