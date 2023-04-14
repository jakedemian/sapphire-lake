using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory i;
    public List<FishData> inventory { get; private set; }

    private void Awake() {
        i ??= this;
        inventory = new List<FishData>();
    }

    public void Add(FishData fish) {
        inventory.Add(fish);
        EventManager.i.e_InventoryCountChanged(inventory.Count);
    }
}
