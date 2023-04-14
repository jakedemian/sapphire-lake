using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionMenu : MonoBehaviour {
    [SerializeField] private GameObject collectionMenu;
    [SerializeField] private GameObject collectionList;
    [SerializeField] private GameObject collectionListItemPrefab;

    public bool hasInitialized { get; private set; }

    public void OnClickedBack() {
        collectionMenu.SetActive(false);
    }

    public void Init() {
        if (hasInitialized) return;

        hasInitialized = true;

        foreach (FishData fish in DataInstance.i.allFishData) {
            GameObject listItem = Instantiate(collectionListItemPrefab, Vector2.zero, Quaternion.identity);
            int catchCount = Globals.ValueInstance.fishCollection.ContainsKey(fish.name) ?
                Globals.ValueInstance.fishCollection[fish.name] : 0;
            listItem.GetComponent<CollectionListItem>().Init(fish.name, catchCount);
            listItem.transform.SetParent(collectionList.transform);
        }
    }
}
