using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfDayModalUI : UIModal {
    public Transform uiFishList;
    public GameObject uiFishEntryPrefab;
    public TextMeshProUGUI totalCount;

    private bool isDayEnd;

    new private void Awake() {
        base.Awake();
        EventManager.i.onDayEnd += OnDayEnd;
        EventManager.i.onSaveDataComplete += OnSaveDataComplete;
    }

    private void Update() {
        if (!isDayEnd) return;
        if (Input.GetButtonDown("NextDay")) {
            base.Hide(() => {
                MusicManager.i.PauseAll(() => {
                    StartCoroutine(DoStartNewDay());
                });
            });
        } else if (Input.GetButtonDown("Quit")) {
            base.Hide(() => {
                MusicManager.i.PauseAll(() => {
                    SceneManager.LoadScene("TitleScreen");
                });
            });
        }
    }

    private void OnDayEnd() {
        Globals.ValueInstance.dayCount++;

        Dictionary<string, int> fishCount = new Dictionary<string, int>();
        List<FishData> inventory = Inventory.i.inventory;

        foreach (FishData fish in inventory) {
            if (fishCount.ContainsKey(fish.name)) {
                fishCount[fish.name] += 1;
            } else {
                fishCount.Add(fish.name, 1);
            }
        }

        Globals.ValueInstance.fishCollection = Util.MergeDictionaries(
            Globals.ValueInstance.fishCollection,
            fishCount
        );

        foreach (var fish in fishCount) {
            GameObject entry = Instantiate(uiFishEntryPrefab, transform.position, Quaternion.identity);
            entry.transform.SetParent(uiFishList.transform);

            Transform name = Util.GetChildWithTag("FishName", entry.transform);
            Transform portrait = Util.GetChildWithTag("FishPortrait", entry.transform);
            Transform count = Util.GetChildWithTag("FishCount", entry.transform);

            name.GetComponent<TextMeshProUGUI>().text = fish.Key;
            count.GetComponent<TextMeshProUGUI>().text = $"x{fish.Value.ToString()}";
            portrait.GetComponent<Image>().sprite = DataInstance.i.GetFishPortaitByName(fish.Key);
            portrait.GetComponent<Image>().SetNativeSize();
        }

        totalCount.text = $"Total Caught: {inventory.Count.ToString()}";
        CloudSaver.i.Save();
    }

    private void OnSaveDataComplete() {
        isDayEnd = true;
        base.Show();
    }

    private IEnumerator DoStartNewDay() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
